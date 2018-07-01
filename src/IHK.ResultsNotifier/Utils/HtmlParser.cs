using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;


namespace IHK.ResultsNotifier.Utils
{
    public class HtmlParser : IDisposable
    {
        private const int ROWS    = 6;
        private const int COLUMNS = 4;

        private const int HTML_FIRST_RESULT_COLUMN_IDX  = 3;
        private const int HTML_MAX_TABLE_ROWS           = 8;
        private const int HTML_MAX_TABLE_COLUMNS        = 6;


        public string GetUsername(string content, string xpath)
        {
            HtmlNode userNode = GetNode(content, xpath);

            if (userNode.Attributes.Count < 2)
                return String.Empty;

            string[] data = userNode.Attributes[1].Value.Split(new[] {"\n"}, 
                StringSplitOptions.RemoveEmptyEntries);

            if (data.Length < 2)
                return String.Empty;

            string[] firstLastName = data[1].Split(new[] {" "}, 
                StringSplitOptions.RemoveEmptyEntries);

            string username = String.Join(" ", firstLastName);

            return username;
        }

        public TableData<string> GetExamResultsTable(string content, string xpath)
        {
            HtmlNode node = GetNode(content, xpath);
            HtmlNode tableNode = FilterNodesByTag(node, "div");

            TableData<string> table = new TableData<string>(ROWS, COLUMNS);
            table.InitAllWith("-");

            for (int i = 1; i < HTML_MAX_TABLE_ROWS; i++)
            {
                HtmlNode rowNode = FilterNodesByTag(tableNode.ChildNodes[i], "div");

                bool hasData = rowNode.ChildNodes.Count > HTML_FIRST_RESULT_COLUMN_IDX;
                if (!hasData) continue;

                for (int j = HTML_FIRST_RESULT_COLUMN_IDX - 1; j < HTML_MAX_TABLE_COLUMNS; j++)
                {
                    string rowValue = rowNode.ChildNodes[j].InnerHtml;
                    table[i - 1, j - 2] = rowValue;
                }
            }

            return table;
        }

        public static HtmlNode GetNode(string content, string xpath)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(content);

            HtmlNode node = document.DocumentNode.SelectSingleNode(xpath);

            return node;
        }

        /// <summary>
        /// HtmlAgilityPack sometimes randomly picks up spaces as #text empty node, therefore this method
        /// made to filter a node by specific tags. Does not do deep filtering.
        /// </summary>
        /// <param name="srcNode">The node to filter and to be modified.</param>
        /// <param name="tags">The tags to be keept for the specific node.</param>
        /// <returns></returns>
        public static HtmlNode FilterNodesByTag(HtmlNode srcNode, params string[] tags)
        {
            List<HtmlNode> nodesToRemove = srcNode.ChildNodes.Where(c => !tags.Contains(c.Name)).ToList();
            nodesToRemove.ForEach(node => srcNode.RemoveChild(node));

            return srcNode;
        }

        public void Dispose()
        {
        }

    }
}

using System;
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


        public HtmlNode GetHtmlNode(string content, string xpath)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(content);

            return document.DocumentNode.SelectNodes(xpath)?.First();
        }

        public TableData<string> ParseHtmlTableData(HtmlNode tableNode)
        {
            TableData<string> table = new TableData<string>(ROWS, COLUMNS);
            table.InitAllWith("-");

            for (int i = 1; i < HTML_MAX_TABLE_ROWS; i++)
            {
                HtmlNode rowNode = tableNode.ChildNodes[i];
                bool hasData = rowNode.ChildNodes.Count > HTML_FIRST_RESULT_COLUMN_IDX;
                if (!hasData) continue;

                for (int j = HTML_FIRST_RESULT_COLUMN_IDX - 1; j < HTML_MAX_TABLE_COLUMNS; j++)
                {
                    string rowValue = rowNode.ChildNodes[j].InnerHtml;
                    if (String.IsNullOrEmpty(rowValue)) continue;

                    table[i - 1, j - 2] = rowValue;
                }
            }

            return table;
        }

        public void Dispose()
        {
        }

    }
}

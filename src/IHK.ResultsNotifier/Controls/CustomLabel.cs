using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IHK.ResultsNotifier.Controls
{
    public sealed class CustomLabel : Label
    {
        public CustomLabel()
        {
            Text      = "-";
            AutoSize  = true;
            Anchor    = AnchorStyles.None;
            Font      = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.FloralWhite;
        }
    }
}

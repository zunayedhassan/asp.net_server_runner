using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ASP.Net_Development_Server_Runner
{
    class CustomizedTooltip : ToolTip
    {
        public CustomizedTooltip(string tip)
        {
            this.Content = new TextBlock()
                               {
                                   Text = tip
                               };
        }
    }
}

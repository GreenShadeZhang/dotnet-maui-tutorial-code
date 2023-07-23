using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verdure.Common
{
    public class MsgModel
    {
        public string EventName { get; set; }=string.Empty;

        public string Content { get; set; } = string.Empty;

        public string TtsText { get; set; } = string.Empty;

        public string ClockName { get; set; } = string.Empty;

        public int FontColorR { get; set; }
        public int FontColorG { get; set; }
        public int FontColorB { get; set; }
    }
}

using System.Collections.Generic;

namespace FinanceSharp.Helpers {
    /// <summary>
    /// 	 Global Market Short Codes and their full versions: (used in tick objects)
    /// </summary>
    public static class MarketCodes {
        /// 	 US Market Codes
        public static Dictionary<string, string> US = new Dictionary<string, string> {
            {"A", "American Stock Exchange"},
            {"B", "Boston Stock Exchange"},
            {"C", "National Stock Exchange"},
            {"D", "FINRA ADF"},
            {"I", "International Securities Exchange"},
            {"J", "Direct Edge A"},
            {"K", "Direct Edge X"},
            {"M", "Chicago Stock Exchange"},
            {"N", "New York Stock Exchange"},
            {"P", "Nyse Arca Exchange"},
            {"Q", "NASDAQ OMX"},
            {"T", "NASDAQ OMX"},
            {"U", "OTC Bulletin Board"},
            {"u", "Over-the-Counter trade in Non-NASDAQ issue"},
            {"W", "Chicago Board Options Exchange"},
            {"X", "Philadelphia Stock Exchange"},
            {"Y", "BATS Y-Exchange, Inc"},
            {"Z", "BATS Exchange, Inc"},
            {"IEX", "Investors Exchange"}
        };

        /// 	 Canada Market Short Codes:
        public static Dictionary<string, string> Canada = new Dictionary<string, string> {
            {"T", "Toronto"},
            {"V", "Venture"}
        };
    }
}
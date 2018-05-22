using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopFinstatApiTester.Model
{
    public enum ResponseType
    {
        XML,
        JSON
    }
    public class Settings
    {
        public ResponseType ResponseType { get; set; } = ResponseType.XML;
        public ApiKeys ApiKeys { get; set; } = new ApiKeys();
        public string StationName { get; set; } = "Api Tester Desktop";
        public string StationID { get; set; } = "api-tester";
        public int TimeOut { get; set; } = 3000;

        public string FinStatApiUrl { get; set; } = "https://www.finstat.sk/api";
        public string FinStatApiUrlCZ { get; set; } = "https://cz.finstat.sk/api";
    }
}

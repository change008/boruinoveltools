using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Tiexue.Framework.Net 
{

    [NetworkPerformanceCounterCategoryAttribute("TiexueFramework")]
    internal class NetworkPerformanceCounter
    {
        public readonly static NetworkPerformanceCounter Instance = PerformanceFactory.Create<NetworkPerformanceCounter>();

        [NetworkPerformanceCounterAttribute("#Connected Total.", "", PerformanceCounterType.NumberOfItems32)]
        public PerformanceCounter NumberOfConnected;

        [NetworkPerformanceCounterAttribute("#DisConnected Total.", "", PerformanceCounterType.NumberOfItems32)]
        public PerformanceCounter NumberOfDisConnected;

        [NetworkPerformanceCounterAttribute("#SocketRec Total.", "", PerformanceCounterType.NumberOfItems32)]
        public PerformanceCounter NumberOfSocketRec;

        [NetworkPerformanceCounterAttribute("#ParserError Total.", "", PerformanceCounterType.NumberOfItems32)]
        public PerformanceCounter NumberOfParserError;

        [NetworkPerformanceCounterAttribute("#ParserSuccess Total.", "", PerformanceCounterType.NumberOfItems32)]
        public PerformanceCounter NumberOfParserSucc;


        [NetworkPerformanceCounterAttribute("Connected /Sec.", "", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerformanceCounter RateOfConnected;

        [NetworkPerformanceCounterAttribute("DisConnected /Sec.", "", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerformanceCounter RateOfDisConnected;
      

    }
}

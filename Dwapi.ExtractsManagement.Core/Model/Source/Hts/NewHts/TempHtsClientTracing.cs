﻿using System; 

namespace Dwapi.ExtractsManagement.Core.Model.Source.Hts.NewHts
{
    public  class TempHtsClientTracing : TempHtsExtract
    {
        public  DateTime? TracingType { get; set; }
        public  DateTime? TracingDate { get; set; }
        public  string TracingOutcome { get; set; }
    }
}

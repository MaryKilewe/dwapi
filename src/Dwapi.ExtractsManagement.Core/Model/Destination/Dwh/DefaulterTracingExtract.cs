using System;
using Dwapi.Contracts.Ct;

namespace Dwapi.ExtractsManagement.Core.Model.Destination.Dwh
{
    public class DefaulterTracingExtract : ClientExtract,IDefaulterTracing
    {
        public string FacilityName { get; set; }
        public int? VisitID { get; set; }
        public DateTime? VisitDate { get; set; }
        public int? EncounterId { get; set; }
        public string TracingType { get; set; }
        public string TracingOutcome { get; set; }
        public int? AttemptNumber { get; set; }
        public string IsFinalTrace { get; set; }
        public string TrueStatus { get; set; }
        public string CauseOfDeath { get; set; }
        public string Comments { get; set; }
        public DateTime? BookingDate { get; set; }
    }
}

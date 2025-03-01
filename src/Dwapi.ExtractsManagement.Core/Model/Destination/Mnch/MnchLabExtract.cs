using System;
using Dwapi.Contracts.Mnch;
using Dwapi.ExtractsManagement.Core.Model.Source.Dwh;

namespace Dwapi.ExtractsManagement.Core.Model.Destination.Mnch
{
    public class MnchLabExtract : MnchClientExtract, IMnchLab
    {
        public string PatientMNCH_ID { get; set; }
        public string FacilityName { get; set; }
        public string SatelliteName { get; set; }
        public int? VisitID { get; set; }
        public DateTime? OrderedbyDate { get; set; }
        public DateTime? ReportedbyDate { get; set; }
        public string TestName { get; set; }
        public string TestResult { get; set; }
        public string LabReason { get; set; }
        public DateTime? Date_Created { get; set; }
        public DateTime? Date_Last_Modified { get; set; }
    }
}

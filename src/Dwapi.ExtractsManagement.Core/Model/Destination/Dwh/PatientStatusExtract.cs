using System;
using Dwapi.Contracts.Ct;

namespace Dwapi.ExtractsManagement.Core.Model.Destination.Dwh
{

    public class PatientStatusExtract : ClientExtract,IStatus
    {
        public string FacilityName { get; set; }
        public string ExitDescription { get; set; }
        public DateTime? ExitDate { get; set; }
        public string ExitReason { get; set; }
        public string TOVerified { get; set; }
        public DateTime? TOVerifiedDate { get; set; }
        public DateTime? ReEnrollmentDate { get; set; }


        public string ReasonForDeath { get; set; }
        public string SpecificDeathReason { get; set; }
        public DateTime? DeathDate { get; set; }
    }
}

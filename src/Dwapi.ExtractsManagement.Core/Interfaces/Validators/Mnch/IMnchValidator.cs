﻿using System.Threading.Tasks;

namespace Dwapi.ExtractsManagement.Core.Interfaces.Validators.Mnch
{
    public interface IMnchValidator
    {
        Task<bool> Validate(string sourceTable);
    }
}

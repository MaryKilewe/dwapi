﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dwapi.ExtractsManagement.Core.Commands.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Extratcors.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Loaders.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Utilities;
using Dwapi.ExtractsManagement.Core.Interfaces.Validators;
using Dwapi.ExtractsManagement.Core.Model.Destination.Dwh;
using Dwapi.ExtractsManagement.Core.Model.Source.Dwh;
using Dwapi.ExtractsManagement.Core.Notifications;
using Dwapi.SharedKernel.Enum;
using Dwapi.SharedKernel.Events;
using Dwapi.SharedKernel.Model;
using MediatR;

namespace Dwapi.ExtractsManagement.Core.ComandHandlers.Dwh
{
    public class ExtractPatientHandler :IRequestHandler<ExtractPatient,bool>
    {
        private readonly IPatientSourceExtractor _patientSourceExtractor;
        private readonly IExtractValidator _extractValidator;
        private readonly IPatientLoader _patientLoader;
        private readonly IClearDwhExtracts _clearDwhExtracts;
        private readonly ITempPatientExtractRepository _tempPatientExtractRepository;
        private readonly IExtractHistoryRepository _extractHistoryRepository;

        public ExtractPatientHandler(IPatientSourceExtractor patientSourceExtractor, IExtractValidator extractValidator, IPatientLoader patientLoader, IClearDwhExtracts clearDwhExtracts, ITempPatientExtractRepository tempPatientExtractRepository, IExtractHistoryRepository extractHistoryRepository)
        {
            _patientSourceExtractor = patientSourceExtractor;
            _extractValidator = extractValidator;
            _patientLoader = patientLoader;
            _clearDwhExtracts = clearDwhExtracts;
            _tempPatientExtractRepository = tempPatientExtractRepository;
            _extractHistoryRepository = extractHistoryRepository;
        }

        public async Task<bool> Handle(ExtractPatient request, CancellationToken cancellationToken)
        {

            //Extract
            int found = await _patientSourceExtractor.Extract(request.Extract, request.DatabaseProtocol);

            //Check for duplicate patients
            var patientKeys = _tempPatientExtractRepository.GetAll().Select(k => k.PatientPK);
            var distinct = new HashSet<int?>();
            var duplicates = new HashSet<int?>();
            foreach (var key in patientKeys)
            {
                if (!distinct.Add(key))
                    duplicates.Add(key);
            }

            if (duplicates.Any())
            {
                var readDuplicates = string.Join(", ", duplicates.ToArray());
                throw new DuplicatePatientException($"Duplicate patient(s) with PatientPK(s) {readDuplicates} found");
            }

            //Validate
            await _extractValidator.Validate(request.Extract.Id, found, nameof(PatientExtract), $"{nameof(TempPatientExtract)}s");

            //Load
            int loaded = await _patientLoader.Load(request.Extract.Id, found, request.DatabaseProtocol.SupportsDifferential);

            int rejected =
                _extractHistoryRepository.ProcessRejected(request.Extract.Id, found - loaded, request.Extract);


           // _extractHistoryRepository.ProcessExcluded(request.Extract.Id, rejected,0);
           _extractHistoryRepository.ProcessExcluded(request.Extract.Id, rejected,request.Extract);

            //notify loaded
            DomainEvents.Dispatch(
                new ExtractActivityNotification(request.Extract.Id, new DwhProgress(
                    nameof(PatientExtract),
                    nameof(ExtractStatus.Loaded),
                    found, loaded, rejected, loaded, 0)));

            return true;
        }
    }

    public class DuplicatePatientException : Exception
    {
        public DuplicatePatientException(string msg) : base(msg) { }
    }
}

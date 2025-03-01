﻿using System.Threading;
using System.Threading.Tasks;
using Dwapi.ExtractsManagement.Core.Commands.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Extratcors.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Loaders.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository;
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
    public class ExtractContactListingHandler :IRequestHandler<ExtractContactListing,bool>
    {
        private readonly IContactListingSourceExtractor _ContactListingSourceExtractor;
        private readonly IExtractValidator _extractValidator;
        private readonly IContactListingLoader _ContactListingLoader;
        private readonly IClearDwhExtracts _clearDwhExtracts;
        private readonly IExtractHistoryRepository _extractHistoryRepository;

        public ExtractContactListingHandler(IContactListingSourceExtractor ContactListingSourceExtractor, IExtractValidator extractValidator, IContactListingLoader ContactListingLoader, IClearDwhExtracts clearDwhExtracts, IExtractHistoryRepository extractHistoryRepository)
        {
            _ContactListingSourceExtractor = ContactListingSourceExtractor;
            _extractValidator = extractValidator;
            _ContactListingLoader = ContactListingLoader;
            _clearDwhExtracts = clearDwhExtracts;
            _extractHistoryRepository = extractHistoryRepository;
        }

        public async Task<bool> Handle(ExtractContactListing request, CancellationToken cancellationToken)
        {
            //Extract
            int found = await _ContactListingSourceExtractor.Extract(request.Extract, request.DatabaseProtocol);

            //Validate
            await _extractValidator.Validate(request.Extract.Id, found, nameof(ContactListingExtract), $"{nameof(TempContactListingExtract)}s");

            //Load
            int loaded = await _ContactListingLoader.Load(request.Extract.Id, found, request.DatabaseProtocol.SupportsDifferential);

            int rejected =
                _extractHistoryRepository.ProcessRejected(request.Extract.Id, found - loaded, request.Extract);


            _extractHistoryRepository.ProcessExcluded(request.Extract.Id, rejected, request.Extract);

            //notify loaded
            DomainEvents.Dispatch(
                new ExtractActivityNotification(request.Extract.Id, new DwhProgress(
                    nameof(ContactListingExtract),
                    nameof(ExtractStatus.Loaded),
                    found, loaded, rejected, loaded, 0)));

            return true;
        }
    }
}

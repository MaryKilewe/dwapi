using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.Data;
using Dwapi.Custom;
using Dwapi.ExtractsManagement.Core.Cleaner.Cbs;
using Dwapi.ExtractsManagement.Core.Cleaner.Dwh;
using Dwapi.ExtractsManagement.Core.Cleaner.Hts;
using Dwapi.ExtractsManagement.Core.Cleaner.Mgs;
using Dwapi.ExtractsManagement.Core.Cleaner.Mnch;
using Dwapi.ExtractsManagement.Core.Cleaner.Mts;
using Dwapi.ExtractsManagement.Core.Extractors.Cbs;
using Dwapi.ExtractsManagement.Core.Extractors.Dwh;
using Dwapi.ExtractsManagement.Core.Extractors.Hts;
using Dwapi.ExtractsManagement.Core.Extractors.Mgs;
using Dwapi.ExtractsManagement.Core.Extractors.Mnch;
using Dwapi.ExtractsManagement.Core.Extractors.Mts;
using Dwapi.ExtractsManagement.Core.Interfaces.Cleaner.Cbs;
using Dwapi.ExtractsManagement.Core.Interfaces.Cleaner.Hts;
using Dwapi.ExtractsManagement.Core.Interfaces.Cleaner.Mgs;
using Dwapi.ExtractsManagement.Core.Interfaces.Cleaner.Mnch;
using Dwapi.ExtractsManagement.Core.Interfaces.Cleaner.Mts;
using Dwapi.ExtractsManagement.Core.Interfaces.Extratcors.Cbs;
using Dwapi.ExtractsManagement.Core.Interfaces.Extratcors.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Extratcors.Hts;
using Dwapi.ExtractsManagement.Core.Interfaces.Extratcors.Mgs;
using Dwapi.ExtractsManagement.Core.Interfaces.Extratcors.Mnch;
using Dwapi.ExtractsManagement.Core.Interfaces.Extratcors.Mts;
using Dwapi.ExtractsManagement.Core.Interfaces.Loaders.Cbs;
using Dwapi.ExtractsManagement.Core.Interfaces.Loaders.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Loaders.Hts;
using Dwapi.ExtractsManagement.Core.Interfaces.Loaders.Mgs;
using Dwapi.ExtractsManagement.Core.Interfaces.Loaders.Mnch;
using Dwapi.ExtractsManagement.Core.Interfaces.Loaders.Mts;
using Dwapi.ExtractsManagement.Core.Interfaces.Reader;
using Dwapi.ExtractsManagement.Core.Interfaces.Reader.Cbs;
using Dwapi.ExtractsManagement.Core.Interfaces.Reader.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Reader.Hts;
using Dwapi.ExtractsManagement.Core.Interfaces.Reader.Mgs;
using Dwapi.ExtractsManagement.Core.Interfaces.Reader.Mnch;
using Dwapi.ExtractsManagement.Core.Interfaces.Reader.Mts;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository.Cbs;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository.Diff;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository.Dwh;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository.Hts;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository.Mgs;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository.Mnch;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository.Mts;
using Dwapi.ExtractsManagement.Core.Interfaces.Services;
using Dwapi.ExtractsManagement.Core.Interfaces.Utilities;
using Dwapi.ExtractsManagement.Core.Interfaces.Validators;
using Dwapi.ExtractsManagement.Core.Interfaces.Validators.Cbs;
using Dwapi.ExtractsManagement.Core.Interfaces.Validators.Hts;
using Dwapi.ExtractsManagement.Core.Interfaces.Validators.Mgs;
using Dwapi.ExtractsManagement.Core.Interfaces.Validators.Mnch;
using Dwapi.ExtractsManagement.Core.Loader.Cbs;
using Dwapi.ExtractsManagement.Core.Loader.Dwh;
using Dwapi.ExtractsManagement.Core.Loader.Hts;
using Dwapi.ExtractsManagement.Core.Loader.Mgs;
using Dwapi.ExtractsManagement.Core.Loader.Mnch;
using Dwapi.ExtractsManagement.Core.Loader.Mts;
using Dwapi.ExtractsManagement.Core.Profiles;
using Dwapi.ExtractsManagement.Core.Profiles.Cbs;
using Dwapi.ExtractsManagement.Core.Profiles.Dwh;
using Dwapi.ExtractsManagement.Core.Profiles.Hts;
using Dwapi.ExtractsManagement.Core.Profiles.Mgs;
using Dwapi.ExtractsManagement.Core.Profiles.Mts;
using Dwapi.ExtractsManagement.Core.Services;
using Dwapi.ExtractsManagement.Infrastructure;
using Dwapi.ExtractsManagement.Infrastructure.Reader.Cbs;
using Dwapi.ExtractsManagement.Infrastructure.Reader.Dwh;
using Dwapi.ExtractsManagement.Infrastructure.Reader.Hts;
using Dwapi.ExtractsManagement.Infrastructure.Reader.Mgs;
using Dwapi.ExtractsManagement.Infrastructure.Reader.Mnch;
using Dwapi.ExtractsManagement.Infrastructure.Reader.Mts;
using Dwapi.ExtractsManagement.Infrastructure.Reader.SmartCard;
using Dwapi.ExtractsManagement.Infrastructure.Repository;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Cbs;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Diff;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Dwh.Extracts;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Dwh.TempExtracts;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Dwh.Validations;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Hts.Extracts;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Hts.TempExtracts;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Hts.Validations;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Mgs.Extracts;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Mgs.TempExtracts;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Mgs.Validations;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Mnch.Extracts;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Mnch.TempExtracts;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Mnch.Validations;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Mts.Extracts;
using Dwapi.ExtractsManagement.Infrastructure.Repository.Mts.TempExtracts;
using Dwapi.ExtractsManagement.Infrastructure.Validators.Cbs;
using Dwapi.ExtractsManagement.Infrastructure.Validators.Dwh;
using Dwapi.ExtractsManagement.Infrastructure.Validators.Hts;
using Dwapi.ExtractsManagement.Infrastructure.Validators.Mgs;
using Dwapi.ExtractsManagement.Infrastructure.Validators.Mnch;
using Dwapi.Hubs.Cbs;
using Dwapi.Hubs.Dwh;
using Dwapi.Hubs.Hts;
using Dwapi.Hubs.Mgs;
using Dwapi.Hubs.Mnch;
using Dwapi.SettingsManagement.Core.Interfaces;
using Dwapi.SettingsManagement.Core.Interfaces.Repositories;
using Dwapi.SettingsManagement.Core.Interfaces.Services;
using Dwapi.SettingsManagement.Core.Model;
using Dwapi.SettingsManagement.Core.Services;
using Dwapi.SettingsManagement.Infrastructure;
using Dwapi.SettingsManagement.Infrastructure.Repository;
using Dwapi.SharedKernel.Enum;
using Dwapi.SharedKernel.Events;
using Dwapi.SharedKernel.Infrastructure;
using Dwapi.UploadManagement.Core.Interfaces.Packager.Cbs;
using Dwapi.UploadManagement.Core.Interfaces.Packager.Dwh;
using Dwapi.UploadManagement.Core.Interfaces.Packager.Hts;
using Dwapi.UploadManagement.Core.Interfaces.Packager.Mgs;
using Dwapi.UploadManagement.Core.Interfaces.Packager.Mnch;
using Dwapi.UploadManagement.Core.Interfaces.Reader;
using Dwapi.UploadManagement.Core.Interfaces.Reader.Cbs;
using Dwapi.UploadManagement.Core.Interfaces.Reader.Dwh;
using Dwapi.UploadManagement.Core.Interfaces.Reader.Hts;
using Dwapi.UploadManagement.Core.Interfaces.Reader.Mgs;
using Dwapi.UploadManagement.Core.Interfaces.Reader.Mnch;
using Dwapi.UploadManagement.Core.Interfaces.Reader.Mts;
using Dwapi.UploadManagement.Core.Interfaces.Services;
using Dwapi.UploadManagement.Core.Interfaces.Services.Cbs;
using Dwapi.UploadManagement.Core.Interfaces.Services.Dwh;
using Dwapi.UploadManagement.Core.Interfaces.Services.Hts;
using Dwapi.UploadManagement.Core.Interfaces.Services.Mgs;
using Dwapi.UploadManagement.Core.Interfaces.Services.Mnch;
using Dwapi.UploadManagement.Core.Interfaces.Services.Mts;
using Dwapi.UploadManagement.Core.Packager.Cbs;
using Dwapi.UploadManagement.Core.Packager.Dwh;
using Dwapi.UploadManagement.Core.Packager.Hts;
using Dwapi.UploadManagement.Core.Packager.Mgs;
using Dwapi.UploadManagement.Core.Packager.Mnch;
using Dwapi.UploadManagement.Core.Packager.Mts;
using Dwapi.UploadManagement.Core.Profiles;
using Dwapi.UploadManagement.Core.Services.Cbs;
using Dwapi.UploadManagement.Core.Services.Dwh;
using Dwapi.UploadManagement.Core.Services.Hts;
using Dwapi.UploadManagement.Core.Services.Mgs;
using Dwapi.UploadManagement.Core.Services.Mnch;
using Dwapi.UploadManagement.Core.Services.Mts;
using Dwapi.UploadManagement.Core.Services.Psmart;
using Dwapi.UploadManagement.Infrastructure.Data;
using Dwapi.UploadManagement.Infrastructure.Reader;
using Dwapi.UploadManagement.Infrastructure.Reader.Cbs;
using Dwapi.UploadManagement.Infrastructure.Reader.Dwh;
using Dwapi.UploadManagement.Infrastructure.Reader.Hts;
using Dwapi.UploadManagement.Infrastructure.Reader.Mgs;
using Dwapi.UploadManagement.Infrastructure.Reader.Mnch;
using Dwapi.UploadManagement.Infrastructure.Reader.Mts;
using Hangfire;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StructureMap;
using Swashbuckle.AspNetCore.Swagger;
using Z.Dapper.Plus;

namespace Dwapi
{
    public class Startup
    {
        public static IConfiguration Configuration;
        public IServiceCollection Service;
        public static IServiceProvider ServiceProvider;
        public static IHubContext<ExtractActivity> HubContext;
        public static IHubContext<CbsActivity> CbsHubContext;
        public static IHubContext<CbsSendActivity> CbsSendHubContext;
        public static IHubContext<HtsSendActivity> HtsSendHubContext;
        public static IHubContext<HtsActivity> HtsHubContext;
        public static IHubContext<MgsSendActivity> MgsSendHubContext;
        public static IHubContext<MgsActivity> MgsHubContext;
        public static IHubContext<MnchActivity> MnchHubContext;
        public static AppFeature AppFeature;
        private IHostingEnvironment CurrrentEnv;
        public static List<string> StartupErrors = new List<string>();

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("implementation.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"implementation.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            CurrrentEnv = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var assemblyNames = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            List<Assembly> assemblies = new List<Assembly>();
            foreach (var assemblyName in assemblyNames)
            {
                assemblies.Add(Assembly.Load(assemblyName));
            }

            services.AddMediatR(assemblies);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Concat(
                        new[]
                        {
                            "application/xhtml+xml",
                            "application/atom+xml",
                            "image/svg+xml",
                        });
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
                options.Level = CompressionLevel.Fastest);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Dwapi API Test",
                    Version = "v1",
                    Description = "Dwapi API. Exposes endpoints used in datawarehouse operations"
                });
            });

            services.AddSignalR();

            services.AddHangfire(_ => _.UseMemoryStorage());
            JobStorage.Current = new MemoryStorage();
            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()))
                .AddJsonOptions(o =>
                    o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.ConfigureWritable<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            var connectionString = Startup.Configuration["ConnectionStrings:DwapiConnection"];
            DatabaseProvider provider = (DatabaseProvider) Convert.ToInt32(Configuration["ConnectionStrings:Provider"]);

            try
            {
                if (provider == DatabaseProvider.MySql)
                {
                    services.AddDbContext<SettingsContext>(o =>
                        {
                            o.UseMySql(connectionString,
                                x => x.MigrationsAssembly(typeof(SettingsContext).GetTypeInfo().Assembly.GetName()
                                    .Name));
                            o.EnableSensitiveDataLogging(CurrrentEnv.IsDevelopment());
                        }
                    );
                    services.AddDbContext<ExtractsContext>(o =>
                    {
                        o.UseMySql(connectionString,
                            x => x.MigrationsAssembly(typeof(ExtractsContext).GetTypeInfo().Assembly.GetName().Name));
                        o.EnableSensitiveDataLogging(CurrrentEnv.IsDevelopment());
                    });
                    services.AddDbContext<UploadContext>(o =>
                    {
                        o.UseMySql(connectionString,
                            x => x.MigrationsAssembly(typeof(UploadContext).GetTypeInfo().Assembly.GetName().Name));
                        o.EnableSensitiveDataLogging(CurrrentEnv.IsDevelopment());
                    });
                }

                if (provider == DatabaseProvider.MsSql)
                {
                    services.AddDbContext<SettingsContext>(
                        o =>
                        {
                            o.UseSqlServer(connectionString,
                                x => x.MigrationsAssembly(typeof(SettingsContext).GetTypeInfo().Assembly.GetName()
                                    .Name));
                            o.EnableSensitiveDataLogging(CurrrentEnv.IsDevelopment());
                        }
                    );
                    services.AddDbContext<ExtractsContext>(o =>
                    {
                        o.UseSqlServer(connectionString,
                            x => x.MigrationsAssembly(typeof(ExtractsContext).GetTypeInfo().Assembly.GetName().Name));
                        o.EnableSensitiveDataLogging(CurrrentEnv.IsDevelopment());
                    });
                    services.AddDbContext<UploadContext>(o =>
                    {
                        o.UseSqlServer(connectionString,
                            x => x.MigrationsAssembly(typeof(UploadContext).GetTypeInfo().Assembly.GetName().Name));
                        o.EnableSensitiveDataLogging(CurrrentEnv.IsDevelopment());
                    });
                }
            }
            catch (Exception e)
            {
                var error = "Connections not Initialized";
                Log.Error(e, error);
                StartupErrors.Add(error);
            }

            try
            {
                var name = Configuration["Features:PKV:Name"];
                var description = Configuration["Features:PKV:Description"];
                var key = Configuration["Features:PKV:Key"];
                AppFeature = AppFeature.Load(name, description, key, CurrrentEnv.IsDevelopment());
            }
            catch (Exception e)
            {
                string error = "Features not Initialized";
                Log.Error(e,error );
                StartupErrors.Add(error);
            }

            services.AddTransient<ExtractsContext>();
            services.AddScoped<ICentralRegistryRepository, CentralRegistryRepository>();
            services.AddScoped<IEmrSystemRepository, EmrSystemRepository>();
            services.AddScoped<IDocketRepository, DocketRepository>();
            services.AddScoped<IDatabaseProtocolRepository, DatabaseProtocolRepository>();
            services.AddScoped<IRestProtocolRepository, RestProtocolRepository>();
            services.AddScoped<IExtractRepository, ExtractRepository>();
            services.AddScoped<IPsmartStageRepository, PsmartStageRepository>();
            services.AddTransient<IExtractHistoryRepository, ExtractHistoryRepository>();
            services.AddTransient<IDiffLogRepository, DiffLogRepository>();
            services.AddScoped<ITempPatientExtractRepository, TempPatientExtractRepository>();
            services.AddScoped<ITempPatientArtExtractRepository, TempPatientArtExtractRepository>();
            services.AddScoped<ITempPatientBaselinesExtractRepository, TempPatientBaselinesExtractRepository>();
            services.AddScoped<ITempPatientLaboratoryExtractRepository, TempPatientLaboratoryExtractRepository>();
            services.AddScoped<ITempPatientPharmacyExtractRepository, TempPatientPharmacyExtractRepository>();
            services.AddScoped<ITempPatientStatusExtractRepository, TempPatientStatusExtractRepository>();
            services.AddScoped<ITempPatientVisitExtractRepository, TempPatientVisitExtractRepository>();
            services.AddScoped<ITempPatientAdverseEventExtractRepository, TempPatientAdverseEventExtractRepository>();
            services.AddScoped<IValidatorRepository, ValidatorRepository>();
            services.AddScoped<IPatientExtractRepository, PatientExtractRepository>();
            services.AddScoped<IPatientArtExtractRepository, PatientArtExtractRepository>();
            services.AddScoped<IPatientBaselinesExtractRepository, PatientBaselinesExtractRepository>();
            services.AddScoped<IPatientLaboratoryExtractRepository, PatientLaboratoryExtractRepository>();
            services.AddScoped<IPatientPharmacyExtractRepository, PatientPharmacyExtractRepository>();
            services.AddScoped<IPatientStatusExtractRepository, PatientStatusExtractRepository>();
            services.AddScoped<IPatientVisitExtractRepository, PatientVisitExtractRepository>();
            services.AddScoped<IPatientAdverseEventExtractRepository, PatientAdverseEventExtractRepository>();
            services.AddScoped<ITempPatientExtractErrorSummaryRepository, TempPatientExtractErrorSummaryRepository>();
            services
                .AddScoped<ITempPatientArtExtractErrorSummaryRepository, TempPatientArtExtractErrorSummaryRepository>();
            services
                .AddScoped<ITempPatientBaselinesExtractErrorSummaryRepository,
                    TempPatientBaselinesExtractErrorSummaryRepository>();
            services
                .AddScoped<ITempPatientLaboratoryExtractErrorSummaryRepository,
                    TempPatientLaboratoryExtractErrorSummaryRepository>();
            services
                .AddScoped<ITempPatientPharmacyExtractErrorSummaryRepository,
                    TempPatientPharmacyExtractErrorSummaryRepository>();
            services
                .AddScoped<ITempPatientStatusExtractErrorSummaryRepository,
                    TempPatientStatusExtractErrorSummaryRepository>();
            services
                .AddScoped<ITempPatientVisitExtractErrorSummaryRepository, TempPatientVisitExtractErrorSummaryRepository
                >();
            services
                .AddScoped<ITempPatientAdverseEventExtractErrorSummaryRepository,
                    TempPatientAdverseEventExtractErrorSummaryRepository>();

            services.AddScoped<IMasterPatientIndexRepository, MasterPatientIndexRepository>();
            services.AddScoped<ITempMasterPatientIndexRepository, TempMasterPatientIndexRepository>();

            services.AddScoped<IDatabaseManager, DatabaseManager>();
            services.AddScoped<IRegistryManagerService, RegistryManagerService>();
            services.AddScoped<IEmrManagerService, EmrManagerService>();
            services.AddScoped<IExtractManagerService, ExtractManagerService>();

            services.AddScoped<IPsmartExtractService, PsmartExtractService>();
            services.AddScoped<IExtractStatusService, ExtractStatusService>();
            services.AddScoped<IPsmartSourceReader, PsmartSourceReader>();
            services.AddScoped<IPsmartSendService, PsmartSendService>();

            services.AddScoped<IDwhExtractSourceReader, DwhExtractSourceReader>();
            services.AddScoped<IPatientSourceExtractor, PatientSourceExtractor>();
            services.AddScoped<IPatientArtSourceExtractor, PatientArtSourceExtractor>();
            services.AddScoped<IPatientBaselinesSourceExtractor, PatientBaselinesSourceExtractor>();
            services.AddScoped<IPatientLaboratorySourceExtractor, PatientLaboratorySourceExtractor>();
            services.AddScoped<IPatientPharmacySourceExtractor, PatientPharmacySourceExtractor>();
            services.AddScoped<IPatientStatusSourceExtractor, PatientStatusSourceExtractor>();
            services.AddScoped<IPatientVisitSourceExtractor, PatientVisitSourceExtractor>();
            services.AddScoped<IPatientAdverseEventSourceExtractor, PatientAdverseEventSourceExtractor>();
            services.AddScoped<IExtractValidator, ExtractValidator>();
            services.AddScoped<IPatientLoader, PatientLoader>();
            services.AddScoped<IPatientArtLoader, PatientArtLoader>();
            services.AddScoped<IPatientBaselinesLoader, PatientBaselinesLoader>();
            services.AddScoped<IPatientLaboratoryLoader, PatientLaboratoryLoader>();
            services.AddScoped<IPatientPharmacyLoader, PatientPharmacyLoader>();
            services.AddScoped<IPatientStatusLoader, PatientStatusLoader>();
            services.AddScoped<IPatientVisitLoader, PatientVisitLoader>();
            services.AddScoped<IPatientAdverseEventLoader, PatientAdverseEventLoader>();
            services.AddScoped<IClearDwhExtracts, ClearDwhExtracts>();

            services.AddScoped<IMasterPatientIndexReader, MasterPatientIndexReader>();
            services.AddScoped<IMasterPatientIndexSourceExtractor, MasterPatientIndexSourceExtractor>();
            services.AddScoped<IMasterPatientIndexValidator, MasterPatientIndexValidator>();
            services.AddScoped<IMasterPatientIndexLoader, MasterPatientIndexLoader>();
            services.AddScoped<IClearCbsExtracts, ClearCbsExtracts>();
            services.AddScoped<ICbsExtractReader, CbsExtractReader>();
            services.AddScoped<ICbsSendService, CbsSendService>();
            services.AddScoped<ICbsPackager, CbsPackager>();
            services.AddScoped<IMpiSearchService, MpiSearchService>();

            services.AddScoped<IDwhExtractReader, DwhExtractReader>();
            services.AddScoped<IDwhPackager, DwhPackager>();
            services.AddScoped<IDwhSendService, DwhSendService>();
            services.AddScoped<ICTSendService, CTSendService>();
            services.AddScoped<IDwhExtractSentServcie, DwhExtractSentServcie>();
            services.AddScoped<IMnchExtractSentServcie, MnchExtractSentServcie>();

            services.AddScoped<ITempHTSClientExtractRepository, TempHTSClientExtractRepository>();
            services.AddScoped<ITempHTSClientLinkageExtractRepository, TempHTSClientLinkageExtractRepository>();
            services.AddScoped<ITempHTSClientPartnerExtractRepository, TempHTSClientPartnerExtractRepository>();

            services.AddScoped<IHTSClientExtractRepository, HTSClientExtractRepository>();
            services.AddScoped<IHTSClientLinkageExtractRepository, HTSClientLinkageExtractRepository>();
            services.AddScoped<IHTSClientPartnerExtractRepository, HTSClientPartnerExtractRepository>();

            services.AddScoped<ITempHtsClientsExtractRepository, TempHtsClientsExtractRepository>();
            services.AddScoped<ITempHtsClientsLinkageExtractRepository, TempHtsClientsLinkageExtractRepository>();
            services.AddScoped<ITempHtsClientTestsExtractRepository, TempHtsClientTestsExtractRepository>();
            services.AddScoped<ITempHtsTestKitsExtractRepository, TempHtsTestKitsExtractRepository>();
            services.AddScoped<ITempHtsClientTracingExtractRepository, TempHtsClientTracingExtractRepository>();
            services.AddScoped<ITempHtsPartnerTracingExtractRepository, TempHtsPartnerTracingExtractRepository>();
            services
                .AddScoped<ITempHtsPartnerNotificationServicesExtractRepository,
                    TempHtsPartnerNotificationServicesExtractRepository>();

            services.AddScoped<IHtsClientsExtractRepository, HtsClientsExtractRepository>();
            services.AddScoped<IHtsClientsLinkageExtractRepository, HtsClientsLinkageExtractRepository>();
            services.AddScoped<IHtsClientTestsExtractRepository, HtsClientTestsExtractRepository>();
            services.AddScoped<IHtsTestKitsExtractRepository, HtsTestKitsExtractRepository>();
            services.AddScoped<IHtsClientTracingExtractRepository, HtsClientTracingExtractRepository>();
            services.AddScoped<IHtsPartnerTracingExtractRepository, HtsPartnerTracingExtractRepository>();
            services
                .AddScoped<IHtsPartnerNotificationServicesExtractRepository,
                    HtsPartnerNotificationServicesExtractRepository>();

            services
                .AddScoped<ITempHTSClientExtractErrorSummaryRepository, TempHTSClientExtractErrorSummaryRepository>();
            services
                .AddScoped<ITempHTSClientLinkageExtractErrorSummaryRepository,
                    TempHTSClientLinkageExtractErrorSummaryRepository>();
            services
                .AddScoped<ITempHTSClientPartnerExtractErrorSummaryRepository,
                    TempHTSClientPartnerExtractErrorSummaryRepository>();

            services
                .AddScoped<ITempHtsClientsExtractErrorSummaryRepository, TempHtsClientsExtractErrorSummaryRepository>();
            services
                .AddScoped<ITempHtsClientLinkageErrorSummaryRepository,
                    TempHtsClientsLinkageExtractErrorSummaryRepository>();
            services
                .AddScoped<ITempHtsClientTestsErrorSummaryRepository, TempHtsClientTestsExtractErrorSummaryRepository
                >();
            services.AddScoped<ITempHtsTestKitsErrorSummaryRepository, TempHtsTestKitsExtractErrorSummaryRepository>();
            services
                .AddScoped<ITempHtsClientTracingErrorSummaryRepository,
                    TempHtsClientTracingExtractErrorSummaryRepository>();
            services
                .AddScoped<ITempHtsPartnerTracingErrorSummaryRepository,
                    TempHtsPartnerTracingExtractErrorSummaryRepository>();
            services
                .AddScoped<ITempHtsPartnerNotificationServicesErrorSummaryRepository,
                    TempHtsPartnerNotificationServicesExtractErrorSummaryRepository>();

            //services.AddScoped<ICleanHtsExtracts, CleanHtsExtracts>();
            services.AddScoped<IClearHtsExtracts, ClearHtsExtracts>();

            services.AddScoped<IHTSExtractSourceReader, HTSExtractSourceReader>();

            /*services.AddScoped<IHTSClientSourceExtractor, HTSClientSourceExtractor>();
            services.AddScoped<IHTSClientLinkageSourceExtractor, HTSClientLinkageSourceExtractor>();
            services.AddScoped<IHTSClientPartnerSourceExtractor, HTSClientPartnerSourceExtractor>();*/

            services.AddScoped<IHtsClientsSourceExtractor, HtsClientsSourceExtractor>();
            services.AddScoped<IHtsClientTestsSourceExtractor, HtsClientTestsSourceExtractor>();
            services.AddScoped<IHtsClientsLinkageSourceExtractor, HtsClientsLinkageSourceExtractor>();
            services.AddScoped<IHtsTestKitsSourceExtractor, HtsTestKitsSourceExtractor>();
            services.AddScoped<IHtsClientTracingSourceExtractor, HtsClientTracingSourceExtractor>();
            services.AddScoped<IHtsPartnerTracingSourceExtractor, HtsPartnerTracingSourceExtractor>();
            services
                .AddScoped<IHtsPartnerNotificationServicesSourceExtractor, HtsPartnerNotificationServicesSourceExtractor
                >();

            /*services.AddScoped<IHTSClientLoader, HTSClientLoader>();
            services.AddScoped<IHTSClientLinkageLoader, HTSClientLinkageLoader>();
            services.AddScoped<IHTSClientPartnerLoader, HTSClientPartnerLoader>();*/

            services.AddScoped<IHtsClientsLoader, HtsClientsLoader>();
            services.AddScoped<IHtsClientTestsLoader, HtsClientTestsLoader>();
            services.AddScoped<IHtsClientsLinkageLoader, HtsClientsLinkageLoader>();
            services.AddScoped<IHtsTestKitsLoader, HtsTestKitsLoader>();
            services.AddScoped<IHtsClientTracingLoader, HtsClientTracingLoader>();
            services.AddScoped<IHtsPartnerTracingLoader, HtsPartnerTracingLoader>();
            services.AddScoped<IHtsPartnerNotificationServicesLoader, HtsPartnerNotificationServicesLoader>();

            services.AddScoped<IHtsPackager, HtsPackager>();
            services.AddScoped<IHtsSendService, HtsSendService>();
            services.AddScoped<IHtsExtractValidator, HtsExtractValidator>();
            services.AddScoped<IHtsSendService, HtsSendService>();
            services.AddScoped<IHtsExtractReader, HtsExtractReader>();


            services.AddScoped<ITempMetricMigrationExtractRepository, TempMetricMigrationExtractRepository>();
            services.AddScoped<IMetricMigrationExtractRepository, MetricMigrationExtractRepository>();
            services
                .AddScoped<ITempMetricMigrationExtractErrorSummaryRepository,
                    TempMetricMigrationExtractErrorSummaryRepository>();
            services.AddScoped<IClearMgsExtracts, ClearMgsExtracts>();


            services.AddScoped<IMgsExtractSourceReader, MgsExtractSourceReader>();
            services.AddScoped<IMetricMigrationSourceExtractor, MetricMigrationSourceExtractor>();
            services.AddScoped<IMgsExtractReader, MgsExtractReader>();
            services.AddScoped<IMetricMigrationLoader, MetricMigrationLoader>();
            services.AddScoped<IMgsPackager, MgsPackager>();
            services.AddScoped<IMgsSendService, MgsSendService>();
            services.AddScoped<IMetricExtractValidator, MetricExtractValidator>();


            services.AddScoped<IHtsSendService, HtsSendService>();

            services.AddScoped<IAppDatabaseManager, AppDatabaseManager>();

            services.AddScoped<IEmrMetricRepository, EmrMetricRepository>();
            services.AddScoped<IEmrMetricsService, EmrMetricsService>();
            services.AddScoped<IEmrMetricReader, EmrMetricReader>();
            services.AddScoped<IDiffLogReader, DiffLogReader>();

            services.AddScoped<IAppMetricRepository, AppMetricRepository>();

            #region newCT
            services.AddTransient<IAllergiesChronicIllnessExtractRepository, AllergiesChronicIllnessExtractRepository>();
            services.AddTransient<IContactListingExtractRepository, ContactListingExtractRepository>();
            services.AddTransient<IDepressionScreeningExtractRepository, DepressionScreeningExtractRepository>();
            services.AddTransient<IDrugAlcoholScreeningExtractRepository, DrugAlcoholScreeningExtractRepository>();
            services.AddTransient<IEnhancedAdherenceCounsellingExtractRepository, EnhancedAdherenceCounsellingExtractRepository>();
            services.AddTransient<IGbvScreeningExtractRepository, GbvScreeningExtractRepository>();
            services.AddTransient<IIptExtractRepository, IptExtractRepository>();
            services.AddTransient<IOtzExtractRepository, OtzExtractRepository>();
            services.AddTransient<IOvcExtractRepository, OvcExtractRepository>();

            //DefaulterTracing
            services.AddTransient<ICovidExtractRepository, CovidExtractRepository>();
            services.AddTransient<IDefaulterTracingExtractRepository, DefaulterTracingExtractRepository>();

            services.AddTransient<ITempAllergiesChronicIllnessExtractRepository, TempAllergiesChronicIllnessExtractRepository>();
            services.AddTransient<ITempContactListingExtractRepository, TempContactListingExtractRepository>();
            services.AddTransient<ITempDepressionScreeningExtractRepository, TempDepressionScreeningExtractRepository>();
            services.AddTransient<ITempDrugAlcoholScreeningExtractRepository, TempDrugAlcoholScreeningExtractRepository>();
            services.AddTransient<ITempEnhancedAdherenceCounsellingExtractRepository, TempEnhancedAdherenceCounsellingExtractRepository>();
            services.AddTransient<ITempGbvScreeningExtractRepository, TempGbvScreeningExtractRepository>();
            services.AddTransient<ITempIptExtractRepository, TempIptExtractRepository>();
            services.AddTransient<ITempOtzExtractRepository, TempOtzExtractRepository>();
            services.AddTransient<ITempOvcExtractRepository, TempOvcExtractRepository>();

            //DefaulterTracing
            services.AddTransient<ITempDefaulterTracingExtractRepository, TempDefaulterTracingExtractRepository>();
            services.AddTransient<ITempCovidExtractRepository, TempCovidExtractRepository>();

            services.AddTransient<ITempAllergiesChronicIllnessExtractErrorSummaryRepository, TempAllergiesChronicIllnessExtractErrorSummaryRepository>();
            services.AddTransient<ITempContactListingExtractErrorSummaryRepository, TempContactListingExtractErrorSummaryRepository>();
            services.AddTransient<ITempDepressionScreeningExtractErrorSummaryRepository, TempDepressionScreeningExtractErrorSummaryRepository>();
            services.AddTransient<ITempDrugAlcoholScreeningExtractErrorSummaryRepository, TempDrugAlcoholScreeningExtractErrorSummaryRepository>();
            services.AddTransient<ITempEnhancedAdherenceCounsellingExtractErrorSummaryRepository, TempEnhancedAdherenceCounsellingExtractErrorSummaryRepository>();
            services.AddTransient<ITempGbvScreeningExtractErrorSummaryRepository, TempGbvScreeningExtractErrorSummaryRepository>();
            services.AddTransient<ITempIptExtractErrorSummaryRepository, TempIptExtractErrorSummaryRepository>();
            services.AddTransient<ITempOtzExtractErrorSummaryRepository, TempOtzExtractErrorSummaryRepository>();
            services.AddTransient<ITempOvcExtractErrorSummaryRepository, TempOvcExtractErrorSummaryRepository>();


            //DefaulterTracing
            services.AddTransient<ITempCovidExtractErrorSummaryRepository, TempCovidExtractErrorSummaryRepository>();
            services.AddTransient<ITempDefaulterTracingExtractErrorSummaryRepository, TempDefaulterTracingExtractErrorSummaryRepository>();


            services.AddScoped<IAllergiesChronicIllnessSourceExtractor, AllergiesChronicIllnessSourceExtractor>();
            services.AddScoped<IContactListingSourceExtractor, ContactListingSourceExtractor>();
            services.AddScoped<IDepressionScreeningSourceExtractor, DepressionScreeningSourceExtractor>();
            services.AddScoped<IDrugAlcoholScreeningSourceExtractor, DrugAlcoholScreeningSourceExtractor>();
            services.AddScoped<IEnhancedAdherenceCounsellingSourceExtractor, EnhancedAdherenceCounsellingSourceExtractor>();
            services.AddScoped<IGbvScreeningSourceExtractor, GbvScreeningSourceExtractor>();
            services.AddScoped<IIptSourceExtractor, IptSourceExtractor>();
            services.AddScoped<IOtzSourceExtractor, OtzSourceExtractor>();
            services.AddScoped<IOvcSourceExtractor, OvcSourceExtractor>();

            //DefaulterTracing
            services.AddScoped<ICovidSourceExtractor, CovidSourceExtractor>();
            services.AddScoped<IDefaulterTracingSourceExtractor, DefaulterTracingSourceExtractor>();

            services.AddScoped<IAllergiesChronicIllnessLoader, AllergiesChronicIllnessLoader>();
            services.AddScoped<IContactListingLoader, ContactListingLoader>();
            services.AddScoped<IDepressionScreeningLoader, DepressionScreeningLoader>();
            services.AddScoped<IDrugAlcoholScreeningLoader, DrugAlcoholScreeningLoader>();
            services.AddScoped<IEnhancedAdherenceCounsellingLoader, EnhancedAdherenceCounsellingLoader>();
            services.AddScoped<IGbvScreeningLoader, GbvScreeningLoader>();
            services.AddScoped<IIptLoader, IptLoader>();
            services.AddScoped<IOtzLoader, OtzLoader>();
            services.AddScoped<IOvcLoader, OvcLoader>();

            //DefaulterTracing
            services.AddScoped<ICovidLoader, CovidLoader>();
            services.AddScoped<IDefaulterTracingLoader, DefaulterTracingLoader>();

            #endregion



            services.AddScoped<ITempIndicatorExtractRepository, TempIndicatorExtractRepository>();
            services.AddScoped<IIndicatorExtractRepository, IndicatorExtractRepository>();
            services.AddScoped<IClearMtsExtracts, ClearMtsExtracts>();

            services.AddScoped<IMtsExtractSourceReader, MtsExtractSourceReader>();
            services.AddScoped<IIndicatorSourceExtractor, IndicatorSourceExtractor>();
            services.AddScoped<IMtsMigrationLoader, MtsMigrationLoader>();

            services.AddScoped<IMtsExtractReader, MtsExtractReader>();
            services.AddScoped<IMtsPackager, MtsPackager>();
            services.AddScoped<IMtsSendService, MtsSendService>();

            services.AddScoped<IIntegrityCheckRepository, IntegrityCheckRepository>();

             #region Extracts

             services.AddTransient<ITempPatientMnchExtractRepository, TempPatientMnchExtractRepository>();
             services.AddTransient<ITempMnchEnrolmentExtractRepository, TempMnchEnrolmentExtractRepository>();
             services.AddTransient<ITempMnchArtExtractRepository, TempMnchArtExtractRepository>();
             services.AddTransient<ITempAncVisitExtractRepository, TempAncVisitExtractRepository>();
             services.AddTransient<ITempMatVisitExtractRepository, TempMatVisitExtractRepository>();
             services.AddTransient<ITempPncVisitExtractRepository, TempPncVisitExtractRepository>();
             services.AddTransient<ITempMotherBabyPairExtractRepository, TempMotherBabyPairExtractRepository>();
             services.AddTransient<ITempCwcEnrolmentExtractRepository, TempCwcEnrolmentExtractRepository>();
             services.AddTransient<ITempCwcVisitExtractRepository, TempCwcVisitExtractRepository>();
             services.AddTransient<ITempHeiExtractRepository, TempHeiExtractRepository>();
             services.AddTransient<ITempMnchLabExtractRepository, TempMnchLabExtractRepository>();

             services.AddTransient<ITempPatientMnchExtractErrorSummaryRepository, TempPatientMnchExtractErrorSummaryRepository>();
             services.AddTransient<ITempMnchEnrolmentExtractErrorSummaryRepository, TempMnchEnrolmentExtractErrorSummaryRepository>();
             services.AddTransient<ITempMnchArtExtractErrorSummaryRepository, TempMnchArtExtractErrorSummaryRepository>();
             services.AddTransient<ITempAncVisitExtractErrorSummaryRepository, TempAncVisitExtractErrorSummaryRepository>();
             services.AddTransient<ITempMatVisitExtractErrorSummaryRepository, TempMatVisitExtractErrorSummaryRepository>();
             services.AddTransient<ITempPncVisitExtractErrorSummaryRepository, TempPncVisitExtractErrorSummaryRepository>();
             services.AddTransient<ITempMotherBabyPairExtractErrorSummaryRepository, TempMotherBabyPairExtractErrorSummaryRepository>();
             services.AddTransient<ITempCwcEnrolmentExtractErrorSummaryRepository, TempCwcEnrolmentExtractErrorSummaryRepository>();
             services.AddTransient<ITempCwcVisitExtractErrorSummaryRepository, TempCwcVisitExtractErrorSummaryRepository>();
             services.AddTransient<ITempHeiExtractErrorSummaryRepository, TempHeiExtractErrorSummaryRepository>();
             services.AddTransient<ITempMnchLabExtractErrorSummaryRepository, TempMnchLabExtractErrorSummaryRepository>();

            services.AddTransient<IPatientMnchExtractRepository, PatientMnchExtractRepository>();
            services.AddTransient<IMnchEnrolmentExtractRepository, MnchEnrolmentExtractRepository>();
            services.AddTransient<IMnchArtExtractRepository, MnchArtExtractRepository>();
            services.AddTransient<IAncVisitExtractRepository, AncVisitExtractRepository>();
            services.AddTransient<IMatVisitExtractRepository, MatVisitExtractRepository>();
            services.AddTransient<IPncVisitExtractRepository, PncVisitExtractRepository>();
            services.AddTransient<IMotherBabyPairExtractRepository, MotherBabyPairExtractRepository>();
            services.AddTransient<ICwcEnrolmentExtractRepository, CwcEnrolmentExtractRepository>();
            services.AddTransient<ICwcVisitExtractRepository, CwcVisitExtractRepository>();
            services.AddTransient<IHeiExtractRepository, HeiExtractRepository>();
            services.AddTransient<IMnchLabExtractRepository, MnchLabExtractRepository>();

            #endregion
            services.AddScoped<IMnchExtractSourceReader, MnchExtractSourceReader>();
            services.AddScoped<IMnchExtractValidator, MnchExtractValidator>();
            services.AddScoped<IClearMnchExtracts, ClearMnchExtracts>();

            services.AddScoped<IPatientMnchSourceExtractor, PatientMnchSourceExtractor>();
            services.AddScoped<IMnchEnrolmentSourceExtractor, MnchEnrolmentSourceExtractor>();
            services.AddScoped<IMnchArtSourceExtractor, MnchArtSourceExtractor>();
            services.AddScoped<IAncVisitSourceExtractor, AncVisitSourceExtractor>();
            services.AddScoped<IMatVisitSourceExtractor, MatVisitSourceExtractor>();
            services.AddScoped<IPncVisitSourceExtractor, PncVisitSourceExtractor>();
            services.AddScoped<IMotherBabyPairSourceExtractor, MotherBabyPairSourceExtractor>();
            services.AddScoped<ICwcEnrolmentSourceExtractor, CwcEnrolmentSourceExtractor>();
            services.AddScoped<ICwcVisitSourceExtractor, CwcVisitSourceExtractor>();
            services.AddScoped<IHeiSourceExtractor, HeiSourceExtractor>();
            services.AddScoped<IMnchLabSourceExtractor, MnchLabSourceExtractor>();

            services.AddScoped<IPatientMnchLoader, PatientMnchLoader>();
            services.AddScoped<IMnchEnrolmentLoader, MnchEnrolmentLoader>();
            services.AddScoped<IMnchArtLoader, MnchArtLoader>();
            services.AddScoped<IAncVisitLoader, AncVisitLoader>();
            services.AddScoped<IMatVisitLoader, MatVisitLoader>();
            services.AddScoped<IPncVisitLoader, PncVisitLoader>();
            services.AddScoped<IMotherBabyPairLoader, MotherBabyPairLoader>();
            services.AddScoped<ICwcEnrolmentLoader, CwcEnrolmentLoader>();
            services.AddScoped<ICwcVisitLoader, CwcVisitLoader>();
            services.AddScoped<IHeiLoader, HeiLoader>();
            services.AddScoped<IMnchLabLoader, MnchLabLoader>();

            services.AddScoped<IMnchExtractReader, MnchExtractReader>();
            services.AddScoped<IMnchPackager, MnchPackager>();
            services.AddScoped<IMnchSendService, MnchSendService>();

            var container = new Container();
            container.Populate(services);
            ServiceProvider = container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            Stopwatch stopWatch = Stopwatch.StartNew();

            //ServiceProvider = serviceProvider;
            app.UseResponseCompression();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(
                    builder => builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
                app.UseCors(
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStaticFiles();
            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });


            app.UseMvcWithDefaultRoute();
            app.UseDefaultFiles();

            app.UseStaticFiles()
                .UseSwaggerUi();

            var hfServerOptions = new BackgroundJobServerOptions()
            {
                ServerName = $"dwapi",
                // WorkerCount =
                WorkerCount = Environment.ProcessorCount * 5,
                //Queues = new string[] {"mpi", "default"}
            };
            /*
            app.UseHangfireDashboard("/api/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
            });
            */
            app.UseHangfireServer(hfServerOptions);
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute() {Attempts = 3});
            GlobalConfiguration.Configuration.UseBatches();

            try
            {
                DapperPlusManager.AddLicense("1755;700-ThePalladiumGroup", "218460a6-02d0-c26b-9add-e6b8d13ccbf4");
                if (!Z.Dapper.Plus.DapperPlusManager.ValidateLicense(out var licenseErrorMessage))
                {
                    throw new Exception(licenseErrorMessage);
                }
            }
            catch (Exception e)
            {
                var error = "DapperPlus Initialization Error";
                Log.Error($"{e}");
                StartupErrors.Add(error);
                throw;
            }

            Log.Debug(@"initializing Database...");

            EnsureMigrationOfContext<SettingsContext>(serviceProvider);
            EnsureMigrationOfContext<ExtractsContext>(serviceProvider);

            app.UseSignalR(
                routes =>
                {
                    routes.MapHub<ExtractActivity>($"/{nameof(ExtractActivity).ToLower()}");
                    routes.MapHub<CbsActivity>($"/{nameof(CbsActivity).ToLower()}");
                    routes.MapHub<HtsActivity>($"/{nameof(HtsActivity).ToLower()}");
                    routes.MapHub<MgsActivity>($"/{nameof(MgsActivity).ToLower()}");
                    routes.MapHub<DwhSendActivity>($"/{nameof(DwhSendActivity).ToLower()}");
                    routes.MapHub<CbsSendActivity>($"/{nameof(CbsSendActivity).ToLower()}");
                    routes.MapHub<HtsSendActivity>($"/{nameof(HtsSendActivity).ToLower()}");
                    routes.MapHub<MgsSendActivity>($"/{nameof(MgsSendActivity).ToLower()}");
                    routes.MapHub<MnchActivity>($"/{nameof(MnchActivity).ToLower()}");
                    routes.MapHub<MnchSendActivity>($"/{nameof(MnchSendActivity).ToLower()}");
                }
            );

            // Mapper.Initialize(cfg =>
            //     {
            //         cfg.AddDataReaderMapping();
            //         // cfg.AddProfile<TempExtractProfile>();
            //         cfg.AddProfile<TempMasterPatientIndexProfile>();
            //         cfg.AddProfile<EmrProfiles>();
            //         cfg.AddProfile<TempHtsExtractProfile>();
            //         if (null != AppFeature && AppFeature.PKV.IsValid)
            //         {
            //             cfg.AddProfile<MasterPatientIndexProfileResearch>();
            //         }
            //         else
            //         {
            //             cfg.AddProfile<MasterPatientIndexProfile>();
            //         }
            //         cfg.AddProfile<TempMetricExtractProfile>();
            //     }
            // );

            DomainEvents.Init();

            stopWatch.Stop();

            Log.Debug(@"initializing Database [Complete]");
            if (null != AppFeature)
            {
                Log.Debug(new string('=', 50));
                Log.Debug("Features");
                Log.Debug($"    {AppFeature.PKV}");
                Log.Debug(new string('=', 50));
            }

            Log.Debug(
                @"---------------------------------------------------------------------------------------------------");
            Log.Debug
            (@"

                                          _____                      _
                                         |  __ \                    (_)
                                         | |  | |_      ____ _ _ __  _
                                         | |  | \ \ /\ / / _` | '_ \| |
                                         | |__| |\ V  V / (_| | |_) | |
                                         |_____/  \_/\_/ \__,_| .__/|_|
                                                              | |
                                                              |_|
");
            Log.Debug(
                @"---------------------------------------------------------------------------------------------------");

            if (StartupErrors.Any())
            {
                Log.Error(new string('*',60));
                Log.Error($"Dwapi startup FAILED, took {stopWatch.ElapsedMilliseconds} ms");
                Log.Error($"Startup Failed Due to the follwing {StartupErrors.Count} ERROR(s)");
                Log.Error(new string('-',60));
                StartupErrors.ForEach(Log.Error);
                Log.Error(new string('-',60));
                Log.Error(new string('*',60));
            }
            else
            {
                Log.Debug($"Dwapi started in {stopWatch.ElapsedMilliseconds} ms");
            }
        }

        public static void EnsureMigrationOfContext<T>(IServiceProvider app) where T : BaseContext
        {
            var contextName = typeof(T).Name;
            Log.Debug($"initializing Database context: {contextName}");
            var context = app.GetService<T>();
            try
            {
                context.Database.Migrate();
                context.EnsureSeeded();
                Log.Debug($"initializing Database context: {contextName} [OK]");
            }
            catch (Exception e)
            {
                var error = $"initializing Database context: {contextName} Error";
                Log.Error(error);
                Log.Error($"{e}");
                StartupErrors.Add($"{error} {e}");
            }
        }
    }
}

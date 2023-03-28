using Microsoft.Extensions.DependencyInjection;
using rapid_docs_services.Services.Account;
using rapid_docs_services.Services.Blob;
using rapid_docs_services.Services.CustomerAccount;
using rapid_docs_services.Services.Email;
using rapid_docs_services.Services.Signer;
using rapid_docs_services.Services.SigningDocuments;
using rapid_docs_services.Services.SigningService;
using rapid_docs_services.Services.SurveyService;
using rapid_docs_services.Services.Survyeee;
using rapid_docs_services.Services.System;
using rapid_docs_services.Services.TextControl;

namespace rapid_docs_api.Core
{
    public static class DependencyInjectionServicesHelper
    {
        public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped<ISigningService, SigningService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISigningDocumentService, SigningDocumentService>();
            services.AddScoped<ISignerService, SignerService>();
            services.AddScoped<IThumbnailService, ThumbnailService>();
            services.AddScoped<ICustomerAccountService, CustomerAccountService>();
            services.AddScoped<ISurveyService, SurveyService>();
            services.AddScoped<ISurveyeeService, SurveyeeService>();
            services.AddScoped<ISystemService, SystemService>();

            return services;
        }
    }
}

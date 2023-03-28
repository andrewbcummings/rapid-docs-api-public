using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rapid_docs_models.DataAccess;
using AutoMapper;
using rapid_docs_viewmodels;
using rapid_docs_api.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using rapid_docs_api.Middleware;
using rapid_docs_core.ApplicationSettings;
using Microsoft.AspNetCore.Identity;
using rapid_docs_models.DbModels;

namespace rapid_docs_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // CORS Policy
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => { builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
            });

            services.AddHttpContextAccessor();

            // JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Auth0:Domain"];
                    options.Audience = Configuration["Auth0:Audience"];
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier,
                        ValidateLifetime = false,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                    };
                });

            // DB Context
            services.AddDbContext<VidaDocsDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("VidadocsDb"))
                .EnableSensitiveDataLogging().UseQueryTrackingBehavior( QueryTrackingBehavior.TrackAll)               
                );
            services.AddIdentityCore<User>().AddEntityFrameworkStores<VidaDocsDbContext>()
                .AddDefaultTokenProviders().AddVidaDocsTokenProvider();
            services.AddScoped<VidaDocsDbContext>();

            // DI for services
            services.AddDependencyInjectionServices();

            // App Settings and Secrets
            services.Configure<AuthSettings>(Configuration.GetSection("Auth0"));
            services.Configure<BlobStorageSettings>(Configuration.GetSection("Blob"));
            services.Configure<EmailOptions>(Configuration.GetSection("EmailOptions"));
            services.Configure<TextControlSettings>(Configuration.GetSection("TextControl"));
            services.Configure<SiteSettings>(Configuration.GetSection("SiteSettings"));

            services.AddVidaDocsContextFactory();

            // AutoMapper Configuration
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperConfiguration());
            });
            services.AddSingleton(mapperConfiguration.CreateMapper());
            services.AddControllers();

            // Swagger Configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "rapid_docs_api", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference { Type=ReferenceType.SecurityScheme, Id="Bearer" }
                        },
                        new string[] {}
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "rapid_docs_api v1"));
                app.ConfigureExceptionHandler();
            //}
            app.UseMiddleware<JWTMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(opt => opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

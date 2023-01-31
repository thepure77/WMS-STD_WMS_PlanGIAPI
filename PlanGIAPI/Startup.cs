using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FluentScheduler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using APIJWTAndRSA.Class;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace PlanGIAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //JobManager.Initialize(new DashboardRegistry());
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //  .AddJwtBearer(options =>
            //  {
            //       ////   var signingKey = Convert.FromBase64String(Configuration["Jwt:SigningSecret"]);
            //       //var signingKey = Configuration["Jwt:SigningSecret"]; 
            //       //options.TokenValidationParameters = new TokenValidationParameters
            //       //{
            //       //    ValidateIssuer = false,
            //       //    ValidateAudience = false,
            //       //    ValidateIssuerSigningKey = true,
            //       //    IssuerSigningKey = new SymmetricSecurityKey(signingKey)
            //       //};


            //       var publicXmlKey = Encoding.UTF8.GetString(Convert.FromBase64String(Configuration["Jwt:SigningSecret"]));

            //       //   var publicXmlKey = RSAToXmlString.getPublicTagXml();
            //       //  var rsa = RSA.Create();

            //       var rsa = new RSACryptoServiceProvider();

            //       //rsa.FromXmlString(publicXmlKey);


            //       RSAToXmlString.FromXmlString(ref rsa, publicXmlKey);

            //      var issuerSigningKey = new RsaSecurityKey(rsa);

            //      options.SaveToken = true;
            //      options.TokenValidationParameters = new TokenValidationParameters
            //      {
            //          ValidateAudience = false,
            //          ValidateIssuer = false,
            //          ValidateIssuerSigningKey = true,
            //          IssuerSigningKey = issuerSigningKey
            //      };
            //  });

            //#region บังคับให้ใส่token ทั้ง project
            //var authorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            //services.AddMvc(config => { config.Filters.Add(new AuthorizeFilter(authorizePolicy)); }
            //)
            //.AddJsonOptions(options =>
            //{
            //    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            //    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //});
            //#endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());

            app.UseHttpsRedirection();
            //app.UseAuthentication();  // Use Authen Jwt
            app.UseMvc();
        }
    }
}

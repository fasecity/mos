using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MosqueService;

namespace MosqueService
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

            //new---way check appsetting json file
            services
                .AddDbContext<ApiContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("Sqlite")));

            //add cors service
            services.AddCors(options => options.AddPolicy("Cors",
                builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                }));

            //-------------------------------------AUTH--------------------------------------------------------------------//

            //sign in key-- is literally what it says but encrypeted//
             var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is the secret phrase"));
          


            //add auth service + options JWT bearer auth + configurare jwt
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg => {

                cfg.RequireHttpsMetadata = false;//--make true in prod req httpsz
                cfg.SaveToken = true;//------------- saves token in db after authorizefdg

                cfg.TokenValidationParameters = new TokenValidationParameters()//--gets/sets the params to validate token
                {
                    //in prod set to true and set them in the Jwt
                    IssuerSigningKey = signInKey,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,//---not sure but seems like time
                    ValidateIssuerSigningKey = true
                    


                };
            });
            //-------------------------------------AUTH--------------------------------------------------------------------//

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //authentication added 
            app.UseAuthentication();

            app.UseCors("Cors");
            app.UseMvc();
        }
    }
}

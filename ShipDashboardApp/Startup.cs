using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShipDashboardApp.Data;
using ShipDashboardApp.Data.Impl;
using Microsoft.AspNetCore.Components.Authorization;
using ShipDashboardApp.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using EmailService;
using LoginComponent;

namespace ShipDashboardApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier);

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            services.AddSingleton<IShipdashboardsService, ShipdashboardService>();
            services.AddScoped<IUserService, InMemoryUserService>();
            services.AddHttpContextAccessor();
            services.AddAuthorization(options => {
                options.AddPolicy("MustBeVIA", a =>
                   a.RequireAuthenticatedUser().RequireClaim("Domain", "via.dk"));

                options.AddPolicy("IsAdmin", a =>
                   a.RequireAuthenticatedUser().RequireClaim("Role", "Admin"));

                options.AddPolicy("IsUser", a =>
                   a.RequireAuthenticatedUser().RequireClaim("Role", "User"));

                options.AddPolicy("SecurityLevel2", policy =>
                    policy.RequireAuthenticatedUser().RequireAssertion(context => {
                        Claim levelClaim = context.User.FindFirst(claim => claim.Type.Equals("Level"));
                        if (levelClaim == null) return false;
                        return int.Parse(levelClaim.Value) >= 2;
                    }));

                options.AddPolicy("SecurityLevel1", policy =>
                    policy.RequireAuthenticatedUser().RequireAssertion(context => {
                        Claim levelClaim = context.User.FindFirst(claim => claim.Type.Equals("Level"));
                        if (levelClaim == null) return false;
                        return int.Parse(levelClaim.Value) >= 1;
                    }));
            });

            var emailConfig = Configuration.GetSection("EmailConfiguration")
                    .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddControllers();
            services.AddScoped<UserExchange>();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}

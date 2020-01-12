using System;
using apigw.ExternalServices.BeerCalculator;
using apigw.ExternalServices.RecipeService;
using apigw.Recipes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;

namespace apigw
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddMvc(option => option.EnableEndpointRouting = false);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = _configuration["Auth0:Domain"];
                    options.Audience = _configuration["Auth0:ApiIdentifier"];
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sahtivahti API", Version = "v0.0.1" });
            });

            services.AddHttpClient("RecipeService", client =>
            {
                client.BaseAddress = new System.Uri(_configuration["RecipeService:BaseUri"]);
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new []
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            }));

            services.AddHttpClient("BeerCalculator", client =>
            {
                client.BaseAddress = new System.Uri(_configuration["BeerCalculator:BaseUri"]);
            });

            services.AddTransient<IRecipeServiceClient, RecipeServiceHttpClient>();
            services.AddTransient<IRecipeService, RecipeService>();
            services.AddTransient<IBeerCalculator, HttpBeerCalculator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sahtivahti API");
                options.RoutePrefix = "doc";
            });

            app.UseMvc();
        }
    }
}

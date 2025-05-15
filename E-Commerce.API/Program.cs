
using Domain.Contracts;
using E_Commerce.API.Extensions;
using E_Commerce.API.Factories;
using E_Commerce.API.MiddleWares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presistence.Data;
using Presistence.Repositories;
using Services;
using Services.Abstraction;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //presentation
           builder.Services.AddPresentationServices();


          

            //core
           builder.Services.AddCoreServices(builder.Configuration);


            //infrastructure
            builder.Services.AddInfrastructureService(builder.Configuration);
           
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseCustomExceptionMiddleware();

            await app.SeedDbAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseCors("CORSPolicy");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();


            app.Run();

            
        }
    }
}

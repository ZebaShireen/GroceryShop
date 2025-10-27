using FluentValidation;
using GroceryShop.Api;
using GroceryShop.Api.Middleware;
using GroceryShop.Application.CQRS.Commands.CreateOrder;
using GroceryShop.Application.CQRS.Queries;
using GroceryShop.Application.Services;
using GroceryShop.Infrastructure.Persistence;
using GroceryShop.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Threading.Tasks;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Infrastructure Services
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICheckoutService, CheckoutService>();

        // Application Services
        services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();

        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilter>();
        });


        // Add CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp",
                builder => builder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        // Add MediatR
        services.AddMediatR(typeof(GetProductsQuery).Assembly);
        Console.WriteLine("Scanning assembly for MediatR: " + typeof(GetProductsQuery).Assembly.FullName);

        // Add Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Grocery Shop API", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Seed database
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedData.Initialize(context);
        }
        // Gloabal ExcpetionHandling Middleware
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Grocery Shop API v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("AllowReactApp");

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            // Redirect root to Swagger UI
            endpoints.MapGet("/", context =>
            {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });
        });

    }
}
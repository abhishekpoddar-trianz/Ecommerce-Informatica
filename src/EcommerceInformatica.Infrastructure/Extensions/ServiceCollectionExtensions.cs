using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Infrastructure.Data;
using EcommerceInformatica.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceInformatica.Infrastructure.Extensions;

/// <summary>
/// Extension methods for registering infrastructure layer services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all infrastructure layer services to the DI container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                sqlOptions.CommandTimeout(120);
            });

            // Enable sensitive data logging in development
            if (bool.TryParse(configuration["Logging:EnableSensitiveDataLogging"], out var enableSensitiveLogging) && enableSensitiveLogging)
            {
                options.EnableSensitiveDataLogging();
            }

            // Enable detailed errors in development
            if (bool.TryParse(configuration["Logging:EnableDetailedErrors"], out var enableDetailedErrors) && enableDetailedErrors)
            {
                options.EnableDetailedErrors();
            }
        });

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProvinceRepository, ProvinceRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IInvoiceDetailRepository, InvoiceDetailRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();

        // Register generic repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }

    /// <summary>
    /// Alternative method for registering infrastructure services with custom connection string
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="connectionString">The database connection string</param>
    /// <param name="enableSensitiveDataLogging">Enable sensitive data logging</param>
    /// <param name="enableDetailedErrors">Enable detailed errors</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        string connectionString,
        bool enableSensitiveDataLogging = false,
        bool enableDetailedErrors = false)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                sqlOptions.CommandTimeout(120);
            });

            if (enableSensitiveDataLogging)
            {
                options.EnableSensitiveDataLogging();
            }

            if (enableDetailedErrors)
            {
                options.EnableDetailedErrors();
            }
        });

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProvinceRepository, ProvinceRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IInvoiceDetailRepository, InvoiceDetailRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();

        // Register generic repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}

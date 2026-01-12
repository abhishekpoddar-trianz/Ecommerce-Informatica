using EcommerceInformatica.Application.Mappings;
using EcommerceInformatica.Application.Services;
using EcommerceInformatica.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceInformatica.Application.Extensions;

/// <summary>
/// Extension methods for registering application layer services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all application layer services to the DI container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // Register all services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProvinceService, ProvinceService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IInvoiceDetailService, InvoiceDetailService>();
        services.AddScoped<IPaymentMethodService, PaymentMethodService>();

        return services;
    }
}

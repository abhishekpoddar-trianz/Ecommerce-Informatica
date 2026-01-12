using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EcommerceInformatica.Application.Services;

public class CategoryService : BaseService<Category, CategoryService>, ICategoryService
{
    public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger)
        : base(unitOfWork, logger) { }

    protected override IRepository<Category> Repository => UnitOfWork.Categories;

    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting category by name: {Name}", name);
            return await UnitOfWork.Categories.GetByNameAsync(name, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting category by name: {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting active categories");
            return await UnitOfWork.Categories.GetActiveCategoriesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting active categories");
            throw;
        }
    }

    public async Task<IEnumerable<Category>> GetCategoriesWithProductsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting categories with products");
            return await UnitOfWork.Categories.GetCategoriesWithProductsAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting categories with products");
            throw;
        }
    }
}

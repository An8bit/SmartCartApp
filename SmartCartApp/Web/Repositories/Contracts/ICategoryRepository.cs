
using Web.Models.Domain;
using Web.Models.DTO.CategoryDTOs;
using Web.Models.DTO.ProductDTOs;
using Web.Repositories.Interfaces;

namespace Web.Repositories.Contracts
{
    public interface ICategoryRepository: IRepository<Category>
    {
        Task<Category> GetByIdWithRelatedDataAsync(int id);

       

       

       
    }
}

using AutoMapper;
using Web.Models.Domain;
using Web.Models.DTO.ProductDTOs;
using Web.Repositories.Contracts;
using Web.Repositories.Interfaces.Service;

namespace Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            if (productDto.Variants != null && productDto.Variants.Any())
            {
                product.Variants = _mapper.Map<List<ProductVariant>>(productDto.Variants);
            }
            var createdProduct = await _productRepository.AddAsync(product);
            return _mapper.Map<ProductDto>(createdProduct);
        }

        public  async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            await _productRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
        //chưa code
        public async Task<PagedResultDto<ProductDto>> GetFilteredProductsAsync(ProductFilterOptions options)
        {
            var products = await _productRepository.GetFilteredProductsAsync(options);
            if (products == null)
            {
                throw new KeyNotFoundException($"Products with the given filter options not found.");
            }

            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products.Items);
            return new PagedResultDto<ProductDto>
            {
                Items = productDtos.ToList(),
                TotalCount = products.TotalCount,
                Page = products.Page,
                PageSize = products.PageSize
            };
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        //chưa code
        public Task<PagedResultDto<ProductDto>> SearchProductsAsync(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProductAsync(int id, UpdateProductDto productDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");
            if (productDto.Variants != null && productDto.Variants.Any())
            {
                existingProduct.Variants = _mapper.Map<List<ProductVariant>>(productDto.Variants);
            }
            _mapper.Map(productDto, existingProduct);
            existingProduct.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(existingProduct);
        }
    }
}

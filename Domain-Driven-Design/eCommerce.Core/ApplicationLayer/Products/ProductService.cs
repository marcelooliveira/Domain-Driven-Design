using eCommerce.Helpers.Repository;
using eCommerce.DomainModelLayer.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace eCommerce.ApplicationLayer.Products
{
    public class ProductService : IProductService
    {
        readonly IRepository<Product> productRepository;
        readonly IRepository<ProductCode> productCodeRepository;
        readonly IUnitOfWork unitOfWork;
        private IMapper mapper { get; set; }

        public ProductService(IRepository<Product> productRepository, 
            IRepository<ProductCode> productCodeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.productRepository = productRepository;
            this.productCodeRepository = productCodeRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public ProductDto Get(Guid productId)
        {
            Product product = this.productRepository.FindById(productId);
            return mapper.Map<Product, ProductDto>(product);
        }

        public ProductDto Add(ProductDto productDto)
        {
            ProductCode productCode =
                this.productCodeRepository.FindById(productDto.ProductCodeId);

            if(productCode == null)
                throw new Exception("Product Code Is Not Valid");

            Product product = 
                Product.Create(productDto.Name, productDto.Quantity, productDto.Cost, productCode);

            this.productRepository.Add(product);

            return mapper.Map<Product, ProductDto>(product);
        }
    }
}

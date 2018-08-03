using AutoMapper;
using eCommerce.ApplicationLayer.Carts;
using eCommerce.ApplicationLayer.Customers;
using eCommerce.ApplicationLayer.History;
using eCommerce.ApplicationLayer.Products;
using eCommerce.DomainModelLayer.Carts;
using eCommerce.DomainModelLayer.Customers;
using eCommerce.DomainModelLayer.Products;
using eCommerce.DomainModelLayer.Purchases;
using eCommerce.Helpers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eCommerce.ApplicationLayer
{
    public class Map : Profile
    {
        public Map()
        {
            CreateMap<Cart, CartDto>();
            CreateMap<CartProduct, CartProductDto>();

            CreateMap<Purchase, CheckOutResultDto>()
                .ForMember(x => x.PurchaseId, options => options.MapFrom(x => x.Id));

            CreateMap<CreditCard, CreditCardDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<CustomerPurchaseHistoryReadModel, CustomerPurchaseHistoryDto>();
            CreateMap<DomainEventRecord, EventDto>();
        }
    }
}

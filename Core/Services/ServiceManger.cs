using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Services.Abstraction;

namespace Services
{
    public class ServiceManger : IServiceManger
    {
        private readonly Lazy<IProductService> _productService;
        public ServiceManger(IUnitOfWork _unitOfWork , IMapper _mapper)
        {
            _productService = new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper));
        }
        public IProductService ProductService => _productService.Value;
    }
}

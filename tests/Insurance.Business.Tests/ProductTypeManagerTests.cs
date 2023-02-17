using Insurance.Business.Concrete;
using Insurance.Core.Logging;
using Insurance.DataAccess.Abstract;
using Insurance.Entities.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Tests
{
    public class ProductTypeManagerTests
    {
        private readonly Mock<IProductTypeApi> _productTypeApiMock;
        private readonly Mock<ILogBuilder> _logBuilderMock;

        private readonly ProductTypeManager _productTypeManager;

        public ProductTypeManagerTests()
        {
            _productTypeApiMock = new Mock<IProductTypeApi>();
            _logBuilderMock = new Mock<ILogBuilder>();

            _productTypeManager = new ProductTypeManager(_productTypeApiMock.Object, _logBuilderMock.Object);
        }

        [Fact]
        public async Task GetAllProductTypes_GivenQuery_ShouldReturnListOfProductTypes()
        {
            List<ProductType> products = new List<ProductType> {
                new ProductType{ Id = 1, Name = "Laptops" },
                new ProductType{ Id = 2, Name = "Smartphones"}
            };

            _productTypeApiMock.Setup(x => x.GetList(It.IsAny<string>()))
                .Returns(Task.FromResult(products));

            var result = await _productTypeManager.GetAllProductTypes();

            Assert.Equal(result.Count, products.Count);
        }


        [Fact]
        public async Task GetProductTypeById_GivenProductId_ShouldReturnCorrespondingProductType()
        {
            ProductType productType = new ProductType { Id = 1, Name = "Laptop", CanBeInsured = true };

            _productTypeApiMock.Setup(x => x.Get("/product_types/1"))
                .Returns(Task.FromResult(productType));

            var result = await _productTypeManager.GetProductTypeById(productType.Id);

            Assert.Same(productType, result);
        }
    }
}

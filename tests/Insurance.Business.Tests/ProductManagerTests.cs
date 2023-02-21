using Insurance.Business.Concrete;
using Insurance.Core.Logging;
using Insurance.DataAccess.Abstract;
using Insurance.Entities.Concrete;
using Moq;

namespace Insurance.Business.Tests
{
    public class ProductManagerTests
    {
        private readonly Mock<IProductApi> _productApiMock;
        private readonly Mock<ILogBuilder> _logBuilderMock;

        private readonly ProductManager _productManager;

        public ProductManagerTests()
        {
            _productApiMock = new Mock<IProductApi>();
            _logBuilderMock = new Mock<ILogBuilder>();

            _productManager = new ProductManager(_productApiMock.Object, _logBuilderMock.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_GivenQuery_ShouldReturnListOfProducts()
        {
            List<Product> products = new List<Product> {
                new Product { Id = 1, Name = "Samsung"},
                new Product { Id = 2, Name = "Apple" },
                new Product { Id = 3, Name = "Oppo" }
            };

            _productApiMock.Setup(x => x.GetListAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(products));

            var result = await _productManager.GetAllProductsAsync();

            Assert.Equal(result.Count, products.Count);
        }

        [Fact]
        public async Task GetProductByIdAsync_GivenProductId_ShouldReturnCorrespondingProduct()
        {
            Product product = new Product { Id = 1, Name = "Laptop", ProductTypeId = 1, SalesPrice = 100 };

            _productApiMock.Setup(x => x.GetAsync("/products/1"))
                .Returns(Task.FromResult(product));

            var result = await _productManager.GetProductByIdAsync(product.Id);

            Assert.Same(product, result);
        }
    }
}
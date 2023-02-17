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
        public async Task GetAllProducts_GivenQuery_ShouldReturnListOfProducts()
        {
            List<Product> products = new List<Product> {
                new Product { Id = 1, Name = "Samsung"},
                new Product { Id = 2, Name = "Apple" },
                new Product { Id = 3, Name = "Oppo" }
            };

            _productApiMock.Setup(x => x.GetList(It.IsAny<string>()))
                .Returns(Task.FromResult(products));

            var result = await _productManager.GetAllProducts();

            Assert.Equal(result.Count, products.Count);
        }

        [Fact]
        public async Task GetAllProducts_WhenErrorThrown_ShouldReturnDefaultList()
        {
            _productApiMock.Setup(x => x.GetList(It.IsAny<string>()))
                .Throws(new NullReferenceException());

            var result = await _productManager.GetAllProducts();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProductById_GivenProductId_ShouldReturnCorrespondingProduct()
        {
            Product product = new Product { Id = 1, Name = "Laptop", ProductTypeId = 1, SalesPrice = 100 };

            _productApiMock.Setup(x => x.Get("/products/1"))
                .Returns(Task.FromResult(product));

            var result = await _productManager.GetProductById(product.Id);

            Assert.Same(product, result);
        }

        [Fact]
        public async Task GetProductById_WhenErrorThrown_ShouldReturnNull()
        {
            _productApiMock.Setup(x => x.Get("/products/1"))
                .Throws(new NullReferenceException());

            var result = await _productManager.GetProductById(1);

            Assert.Null(result);
        }
    }
}
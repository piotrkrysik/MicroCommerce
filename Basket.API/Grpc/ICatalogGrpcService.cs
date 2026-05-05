using Catalog.API.Protos;

namespace Basket.API.Grpc
{
    public interface ICatalogGrpcService
    {
        Task<ProductModel> GetProduct(string productId);
    }
}
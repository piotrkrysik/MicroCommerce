using Catalog.API.Protos;

namespace Basket.API.Grpc
{
    public class BasketCatalogGrpcService : ICatalogGrpcService
    {
        private readonly CatalogProtoService.CatalogProtoServiceClient _grpcClient;

        public BasketCatalogGrpcService(CatalogProtoService.CatalogProtoServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        public async Task<ProductModel> GetProduct(string productId)
        {
            var request = new GetProductRequest { Id = productId };
            return await _grpcClient.GetProductAsync(request);
        }
    }
}

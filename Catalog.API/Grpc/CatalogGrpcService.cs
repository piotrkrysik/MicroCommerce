using Catalog.API.Protos;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Repositories;
using Grpc.Core;
using Microsoft.AspNetCore.Http;

namespace Catalog.API.Grpc
{
    // Dziedziczymy po klasie wygenerowanej automatycznie z pliku .proto
    public class CatalogGrpcService : CatalogProtoService.CatalogProtoServiceBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogGrpcService> _logger;

        public CatalogGrpcService(IProductRepository repository, ILogger<CatalogGrpcService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<ProductModel> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            _logger.LogInformation("==> gRPC GetProduct wywołane dla ID: {Id}", request.Id);

            var product = await _repository.GetProduct(Guid.Parse(request.Id));

            if (product == null)
            {
                _logger.LogWarning("==> gRPC Produkt o ID: {Id} nie został znaleziony.", request.Id);
                throw new RpcException(new Status(StatusCode.NotFound, $"Produkt o ID {request.Id} nie istnieje."));
            }

            return new ProductModel
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Price = (double)product.Price // gRPC używa double zamiast decimal
            };
        }
    }
}
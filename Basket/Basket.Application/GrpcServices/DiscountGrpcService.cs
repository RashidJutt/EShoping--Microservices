using DiscountAPI;

namespace Basket.Application.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _protoServiceClient;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient protoServiceClient)
        {
            _protoServiceClient = protoServiceClient;
        }

        public async Task<CouponModel> GetDiscountAsync(string productName)
        {
            var request = new GetDiscountRequest { ProductName = productName };
            return await _protoServiceClient.GetDiscountAsync(request);
        }
    }
}
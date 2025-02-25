using System.Net;
using System.Threading.Tasks;
using TakepaymentsGateway;

namespace MegwayParcel.Common.TakePaymentGateway
{
    public interface IGatewayHelper
    {
        Task<HttpStatusCode> PostDataAsync<T>(string path, T data);
        Task<HostedPaymentForm> Hosted(MakeOrderDTO dto);
    }
}

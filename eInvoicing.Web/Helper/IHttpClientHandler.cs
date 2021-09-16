using eInvoicing.DTO;

namespace eInvoicing.Web.Helper
{
    public interface IHttpClientHandler
    {
        genericResponse GET(string url);
        genericResponse POST(string url, object Content);
    }
}
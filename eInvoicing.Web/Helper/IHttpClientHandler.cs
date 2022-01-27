using eInvoicing.DTO;

namespace eInvoicing.Web.Helper
{
    public interface IHttpClientHandler
    {
        genericResponse GET(string url, string Media = null);
        genericResponse POST(string url, object Content);
    }
}
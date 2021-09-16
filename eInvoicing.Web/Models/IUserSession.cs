using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Web.Models
{
    public interface IUserSession
    {
        string Username { get; }
        string BearerToken { get; }
        string URL { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.API.Models
{
    public interface IUserSession
    {
        string url { get; set; }
        string loginUrl { get; set; }
        string submitServiceUrl { get; set; }
        string submissionurl { get; set; }
        string client_id { get; set; }
        string client_secret { get; set; }
        string InternalAPIURL { get; set; }
        string pin { get; set; }
        string SRN { get; set; }
        void GetBusinessGroupId(string BusinessGroupId);
        void SetBusinessGroup(string BusinessGroup);
        bool IsDBSync(string BusinessGroup);
    }
}

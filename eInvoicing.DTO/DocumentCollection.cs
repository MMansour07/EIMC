using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace eInvoicing.DTO
{
    public class DocumentCollection : List<DocumentDetail>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                  new SqlMetaData("DocumentId", SqlDbType.NVarChar, 50));
            foreach (DocumentDetail doc in this)
            {
                sqlRow.SetString(0, doc.DocumentId);

                yield return sqlRow;
            }
        }
    }
}

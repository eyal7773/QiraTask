using CsvHelper.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Maps
{
    public sealed class InvoiceMap : ClassMap<Invoice>
    {
        public InvoiceMap()
        {
            Map(m => m.CreatedAt);
            Map(m => m.Id);
            Map(m => m.Status);
            Map(m => m.Amount);
            Map(m => m.PaymentMethod);
        }
    }
}

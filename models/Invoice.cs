using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public InvoiceStatuses Status { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethods PaymentMethod { get; set; }

    }
}

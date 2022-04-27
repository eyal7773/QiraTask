using Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO
{
    public class InvoiceParams
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Invoice number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invoice number must be greater than 0")]
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public InvoiceStatuses Status { get; set; }
        
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Amount is required")]
        [RegularExpression("[1-9][0-9]*", ErrorMessage = "Amount must be a valid number")]
        public decimal Amount { get; set; }
        public PaymentMethods PaymentMethod { get; set; }

        public static Invoice ToInvoice(InvoiceParams invoiceToAdd)
        {
            return new Invoice
            {
                Id = invoiceToAdd.Id,
                CreatedAt = invoiceToAdd.CreatedAt,
                UpdatedAt = invoiceToAdd.UpdatedAt,
                Status = invoiceToAdd.Status,
                Amount = invoiceToAdd.Amount,
                PaymentMethod = invoiceToAdd.PaymentMethod
            };
        }
    }
}

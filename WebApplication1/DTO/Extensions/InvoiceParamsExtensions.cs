using Models;

namespace WebApi.DTO.Extensions
{
    public static class InvoiceParamsExtensions
    {
        public static Invoice ToInvoice(this InvoiceParams invoiceToAdd)
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

using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Invoice
    {
        [Key]
        public required string Id { get; set; }
        public required string CustomerId { get; set; }
        public required string Status { get; set; }
        public required string Url { get; set; }
        public DateTime? InvoiceDate { get; set; }
    }
}

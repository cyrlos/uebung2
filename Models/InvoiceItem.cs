namespace Lagerverwaltung.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public Invoice? Invoice { get; set; }

        public string Description { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal VatRate { get; set; }

        public decimal NetAmount { get; set; }

        public decimal VatAmount { get; set; }

        public decimal GrossAmount { get; set; }
    }
}
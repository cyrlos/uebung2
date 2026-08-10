namespace Lagerverwaltung.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime ServiceDate { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }

        public string CustomerPostalCode { get; set; }

        public string CustomerCity { get; set; }

        public string CustomerCountry { get; set; }

        public string CustomerVatNumber { get; set; }

        public string PaymentTerms { get; set; }

        public List<InvoiceItem> Items { get; set; } = new();

        public decimal NetTotal { get; set; }

        public decimal VatTotal { get; set; }

        public decimal GrossTotal { get; set; }
    }
}
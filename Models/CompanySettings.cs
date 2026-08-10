namespace Lagerverwaltung.Models
{
    public class CompanySettings
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string VatNumber { get; set; }

        public string Iban { get; set; }

        public string Bic { get; set; }
    }
}
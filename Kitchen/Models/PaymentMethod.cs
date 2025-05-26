namespace Kitchen.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        public string? TypeAr { get; set; }
        public string? TypeEn { get; set; }

        public string? AccountNumber { get; set; }
        public string? IBAN { get; set; }
        public string? InstapayEmail { get; set; }
    }


}

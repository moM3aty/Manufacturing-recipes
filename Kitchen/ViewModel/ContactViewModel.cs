using Kitchen.Migrations;
using Kitchen.Models;

namespace Kitchen.ViewModel
{
    public class ContactViewModel
    {
        public List<ContactInfo> contacts { get; set; }
        public List<PaymentMethod> paymentMethods { get; set; }
    }

}

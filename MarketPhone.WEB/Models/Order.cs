using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MarketPhone.WEB.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public decimal Sum { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }

        public int PhoneId { get; set; }
        public Phone Phone { get; set; }
    }
}

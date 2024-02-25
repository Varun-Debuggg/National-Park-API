using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NationalParkWebApp_3.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Adult { get; set; }
        public string Child { get; set; }

        public string BookingStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string TransactionId { get; set; }
        public int NationalParkId { get; set; }
        [ForeignKey("NationalParkId")]
        public NationalPark NationalPark { get; set; }
    }
}

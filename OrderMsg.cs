namespace NationalParkWebApp_3
{
    public class OrderMsg
    {
        public string ConfirmMessage(string name, string bookingId, string date, string address, string totalAmount)
        {
            return "Dear " + name + ",\n\n" +
           "Your booking has been successfully received and confirmed.\n" +
           "Booking Details:\n" +
           "Booking ID: [" + bookingId + "]\n" +
           "Date of Booking: [" + date + "]\n" +
           "Address: [" + address + "]\n\n" +
           "Payment Information:\n" +
           "Total Amount: [" + totalAmount + "]\n" +
           "Payment Method: [Online]\n\n" +
           "Status: Confirmed\n\n";
        }
        public string ErrorMessage(string name, string bookingId, string date, string address, string totalAmount)
        {
            return "Dear " + name + ",\n\n" +
                "We regret to inform you that your payment failed, and your booking could not be confirmed.\n" +
                "Booking Details:\n" +
                "Booking ID: [" + bookingId + "]\n" +
                "Date of Booking: [" + date + "]\n" +
                "Address: [" + address + "]\n\n" +
                "Payment Information:\n" +
                "Total Amount: [" + totalAmount + "]\n" +
                "Payment Method: [Online]\n\n" +
                "Status: Not Confirmed\n\n" +
                "Please review your payment information and try again. If you continue to experience issues, please contact our customer support.";
        }
    }
}

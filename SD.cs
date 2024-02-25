

namespace NationalParkWebApp_3
{
    public static class SD
    {
        public static string APIBaseUrl = "https://localhost:7224/";
        public static string NationalParkAPIPath = APIBaseUrl + "api/NationalPark";
        public static string TrailAPIPath = APIBaseUrl + "api/Trail";
        public static string BookingAPIPath = APIBaseUrl + "api/Booking";


        

        // status
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefund = "Refunded";

        // Payment status
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayPayment = "Payment Status Delay";
        public const string PaymentStatusRejected = "Rejected";




    }
}

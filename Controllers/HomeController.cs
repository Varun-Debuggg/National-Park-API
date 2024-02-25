using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using NationalParkWebApp_3.Models;
using NationalParkWebApp_3.Models.ViewModels;
using NationalParkWebApp_3.Repository.IRepository;
using Stripe;
using System.Diagnostics;
using System.Security.Claims;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace NationalParkWebApp_3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly TwilioSettings _twilio;
        private readonly IEmailSender _emailSender;
        public HomeController(ILogger<HomeController> logger, ITrailRepository trailRepository, INationalParkRepository nationalParkRepository, IBookingRepository bookingRepository ,IOptions<TwilioSettings> twilio, IEmailSender emailSender)
        {
            _logger = logger;
            _trailRepository = trailRepository;
            _nationalParkRepository = nationalParkRepository;
            _bookingRepository = bookingRepository;
            _twilio = twilio.Value;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM IndexVM = new IndexVM()
            {
                NationalParksList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath),
                /*TrailList = await _trailRepository.GetAllAsync(SD.TrailAPIPath)*/
                TrailList = await _trailRepository.GetAllAsync(SD.TrailAPIPath)
            };
            return View(IndexVM);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Booking(int id)
        {
            Booking booking = new Booking();
            if (id == 0) return View();
            booking.NationalPark = await _nationalParkRepository.GetAsync(SD.NationalParkAPIPath, id);
            if (booking.NationalPark == null) return NotFound();
            booking.NationalParkId = booking.NationalPark.Id;
            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Booking(Booking booking)
        {
            if (booking == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();
            booking.NationalParkId = booking.Id;
            booking.Id = 0;
            booking.BookingStatus = SD.StatusPending;
            booking.PaymentStatus = SD.PaymentStatusPending;
            await _bookingRepository.CreateAsync(SD.BookingAPIPath, booking);
            booking = await _bookingRepository.GetLastestAsync(SD.BookingAPIPath, booking.Email);
            return RedirectToAction("BookingConfirm", "Home", new { bookingId = booking.Id, nationalParkId = booking.NationalParkId });

        }
        [HttpGet]
        public async Task<IActionResult> BookingConfirm(int bookingId, int nationalParkId)
        {
            if (bookingId == 0 && nationalParkId == 0)
                return BadRequest();
            var bookingInDb = await _bookingRepository.GetAsync(SD.BookingAPIPath, bookingId, nationalParkId);
            if (bookingInDb == null)
                return NotFound();
            bookingInDb.NationalPark = await _nationalParkRepository.GetAsync(SD.NationalParkAPIPath, nationalParkId);
            if (bookingInDb.NationalPark == null)
                return NotFound();
            return View(bookingInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("BookingConfirm")]
        public async Task<IActionResult> BookingConfirmPost(string stripeToken, int bookingId, int nationalParkId)
        {
            if (bookingId == 0 && nationalParkId == 0)
                return BadRequest();
            var bookingDetailInDb = await _bookingRepository.GetAsync(SD.BookingAPIPath, bookingId, nationalParkId);
            if (bookingDetailInDb == null)
                return NotFound();
            #region Stripe
            if (stripeToken == null)
            {
                bookingDetailInDb.PaymentStatus = SD.PaymentStatusDelayPayment;
                bookingDetailInDb.BookingStatus = SD.StatusApproved;
            }
            else
            {
                var amount = (int.Parse(bookingDetailInDb.Adult) * 100) + (int.Parse(bookingDetailInDb.Child) * 50);
                var options = new ChargeCreateOptions()
                {
                    Amount = Convert.ToInt32(amount),
                    Currency = "USD",
                    Description = "Booking Id:" + bookingDetailInDb.Id,
                    Source = stripeToken
                };
                // Payment
                var service = new ChargeService();
                Charge charge = service.Create(options);
                if (charge.BalanceTransactionId == null)
                    bookingDetailInDb.PaymentStatus = SD.PaymentStatusRejected;
                else
                    bookingDetailInDb.TransactionId = charge.BalanceTransactionId;
                if (charge.Status.ToLower() == "succeeded")
                {
                    bookingDetailInDb.PaymentStatus = SD.PaymentStatusApproved;
                    bookingDetailInDb.BookingStatus = SD.StatusApproved;
                    bookingDetailInDb.Date = DateTime.Now;
                }
                await _bookingRepository.UpdateAsync(SD.BookingAPIPath, bookingDetailInDb);
                OrderMsg orderMsg = new OrderMsg();
                var address = bookingDetailInDb.Address;
                TwilioClient.Init(_twilio.AccountSID, _twilio.AuthToken);
                var message = MessageResource.Create(
                       to: new PhoneNumber("+917009447051"),
                       from: new PhoneNumber(_twilio.PhoneNumber),
                       body: charge.Status.ToLower() == "succeeded" ? orderMsg.ConfirmMessage(bookingDetailInDb.Name, bookingDetailInDb.Id.ToString(), bookingDetailInDb.Date.ToString(), address, amount.ToString()) :
                       orderMsg.ErrorMessage(bookingDetailInDb.Name, bookingDetailInDb.Id.ToString(), bookingDetailInDb.Date.ToString(), address, amount.ToString())
                );
                await _emailSender.SendEmailAsync("nittinkum.dnn@gmail.com", charge.Status.ToLower() == "succeeded" ? "Booking Status : Confirmed - Thank You for Your Booking!" : "Booking Status : Not Confirmed", charge.Status.ToLower() == "succeeded" ? "Successfully done" : "Failed"); // send email to user who booking and message send to user according to payment is successded and failed
            }
            #endregion
            return RedirectToAction("BookingConfirmation", "Home", new { id = bookingDetailInDb.Id });
        }

        public IActionResult BookingConfirmation(int id)
        {
            return View(id);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
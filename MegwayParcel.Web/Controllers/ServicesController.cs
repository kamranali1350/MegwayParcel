
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MegwayParcel.Common;
using MegwayParcel.Common.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using LogisticERP.Web.Models.Landmark;
using System;
using MegwayParcel.Common.Data;
using MegwayParcel.Common.CommonServices;

namespace MegwayParcel.Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IShippingService _shippingService;
        private readonly ILogger<ServicesController> _logger;
        private readonly LogisticERPContext db;
        public ServicesController(IShippingService shippingService, ILogger<ServicesController> logger, LogisticERPContext context)
        {
            this._shippingService = shippingService;
            _logger = logger;
            db = context;
        }
        public IActionResult ParcelDelivery(string id)
        {
            
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer != null)
            {
                //var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.CustomerId == customer.CustomerId && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0)?.Count();
                //ViewBag.basketCount = basketCount ?? 0;

                var guestEmail = customer.GuestUser?.Email;
                var customerId = customer.CustomerId;
                var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0
                                && (guestEmail != null ? x.OrderEmail == guestEmail : x.CustomerId == customerId)).Count();
                ViewBag.basketCount = basketCount;
            }
            var result = db.Pages.FirstOrDefault(x => x.Prefix == id);
            return View(result);
        }
        public async Task TrackShipment()
        {
            // Create the request object
            var trackRequest = new TrackRequest
            {
                Login = new Login
                {
                    Username = "Megway_Logistics_API",
                    Password = "Kotli1977@pakistan"
                },
                Test = false,              // Set to true if you are testing
                ClientID = 2208,           // Example ClientID, replace with your actual ClientID
                TrackingNumber = "93557954540",  // Replace with actual tracking number
                RetrievalType = "Historical"   // Example retrieval type; adjust as per your requirements

//                < TrackingNumber > 93557954540 </ TrackingNumber >

//< LandmarkTrackingNumber > LTN_test38752547N1 </ LandmarkTrackingNumber >

//< PackageID > xxx1 </ PackageID >

//< BarcodeData > xxx492104000000412001 </ BarcodeData >

//< PackageReference > 98233312 </ PackageReference >


            };

            try
            {
                // Call the generic SendRequestAsync method
                var trackResponse = await _shippingService.SendRequestAsync<TrackRequest, TrackResponse>(trackRequest, "/v2/Track.php");

                // Process the response
                if (trackResponse != null && trackResponse.TrackResult != null)
                {
                    _logger.LogInformation("Tracking successful.");
                    foreach (var package in trackResponse.TrackResult.Packages)
                    {
                        _logger.LogInformation("Package Tracking Number: {0}", package.TrackingNumber);
                        foreach (var ev in package.Events)
                        {
                            _logger.LogInformation("Event: {0}, DateTime: {1}, Location: {2}", ev.Status, ev.DateTime, ev.Location);
                        }
                    }
                }
                else if (trackResponse?.Errors != null)
                {
                    // Log any errors returned in the response
                    foreach (var error in trackResponse.Errors)
                    {
                        _logger.LogError("Error: {0} - {1}", error.ErrorCode, error.ErrorMessage);
                    }
                }
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "External service error occurred while tracking the shipment.");
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error occurred while tracking the shipment.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while tracking the shipment.");
            }
        }
    }
}


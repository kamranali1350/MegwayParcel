using LogisticERP.Web.Models;
using LogisticERP.Web.Models.Landmark;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using MegwayParcel.Common.Services;
using System.Xml.Serialization;
using System.IO;
using Microsoft.EntityFrameworkCore;
using LogisticERP.Web.Models.Megway;
using System.Net.Http;
using MegwayParcel.Common.CommonServices;
using MegwayParcel.Common.Data;

namespace MegwayParcel.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IShippingService _shippingService;
        private readonly ILogger<HomeController> _logger;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly CourierApiService _courierApiService;
        public string apiKey = "4h4Gy4QpKo";
        public string apiPassword = "USzh0o4o&A";
        private readonly ApiClient _apiClientMegway;
        private readonly ApiClient _apiClientLandmark;
        private readonly LogisticERPContext db;

        public HomeController(ILogger<HomeController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment env
            , IShippingService shippingService, CourierApiService courierApiService, LogisticERPContext context)
        {
            db = context;
            _logger = logger;
            _hostingEnvironment = env;
            _shippingService = shippingService;
            _courierApiService = courierApiService;
            // Initialize ApiClient instances with different base URLs
            _apiClientMegway = new ApiClient("https://production.courierapi.co.uk/", "Megway","tqlvkmzcsdgfhrbo" );
            _apiClientLandmark = new ApiClient("http://services3.transglobalexpress.co.uk/", apiUser: null, apiToken: null );
        }
        public async Task<IActionResult> Index()
        {
            
            CountryRootInput root = new CountryRootInput();
            root.Credentials = new Credentials();
            root.Credentials.APIKey = apiKey;
            root.Credentials.Password = apiPassword;
            //LandmarkShipRequest shipRequest = new();
            //var xmlresponse = await _shippingService.SendShipRequestAsync(shipRequest);
            //var landmarkresponse = JsonConvert.DeserializeObject(xmlresponse);
            //ViewBag.Response = shipRequest;

            var result = await _apiClientLandmark.PostAsync<CountryRootOutput>("Country/V2/GetCountries", root);
            var countries = result.Countries.ToList();
            var ukCountries = countries.Where(x => x.CountryCode == "GB").ToList();
            countries.RemoveAll(x => x.CountryCode == "GB");

            ukCountries.AddRange(countries);
            ViewBag.contries = ukCountries;

            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");

            //var pages = db.Pages.Select(x => new Page { PageId = x.PageId, Title = x.Title, ParentId = x.ParentId, Prefix = x.Prefix, SortOrder = x.SortOrder }).ToList();
            if (customer != null)
            {
                
                var guestEmail = customer.GuestUser?.Email;
                var customerId = customer.CustomerId;
                var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0
                                && (guestEmail != null ? x.OrderEmail == guestEmail : x.CustomerId == customerId)).Count();
                ViewBag.basketCount = basketCount;
            }
            else
                ViewBag.basketCount = "";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> GetQuotes([FromForm] string data)
        {
            try
            {
                var newServiceResult1 = new ServiceResult();
                if (string.IsNullOrEmpty(data))
                {
                    return BadRequest("Model data is missing.");
                }
                Shipment model = JsonConvert.DeserializeObject<Shipment>(data);
                var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
                CompanySetting setting;
                decimal packageweightt = 0;
                decimal packageWidth = 0;
                decimal packageid = 0;
                decimal packageHeight = 0;
                decimal packageLength = 0;
                double newWeight = 0;
                double actualWeight = 0;
                List<TotalCost> landmarkPrices = new List<TotalCost>();
                List<TotalCost> prices = new List<TotalCost>();
                
                foreach (var item in model.Consignment.Packages)
                {
                    packageweightt = Convert.ToDecimal(item.Weight) + packageweightt;
                    packageweightt = Math.Ceiling(packageweightt);
                    packageWidth = Convert.ToDecimal(item.Width);
                    packageHeight = Convert.ToDecimal(item.Height);
                    packageLength = Convert.ToDecimal(item.Length);
                    newWeight = ((double)(packageWidth * packageHeight * packageLength) / 6000) + newWeight;
                    // Round newWeight to the next 0.05 interval
                    newWeight = Math.Ceiling(newWeight);
                    packageid++;

                }
                int countryid = model.DeliveryAddress.Country.CountryID;
                var eucountries = db.Countries.FirstOrDefault(x => x.CountryId == countryid);
                //var eucountries = db.Countries.Where(x=>x.CountryId== model.DeliveryAddress.Country.CountryID).FirstOrDefault();
                if (eucountries != null && eucountries.IsEU == true)
                {
                    prices = db.LandMarkPrices
                    .Where(x => x.CountryId == model.DeliveryAddress.Country.CountryID
                                && x.Weight == Convert.ToDouble(packageweightt)) // Use the greater of the actual or dimensional weight
                    .Select(x => new TotalCost
                    {
                        TotalCostGrossWithoutCollection = (double)x.Price
                    }).ToList();
                    actualWeight = (double)packageweightt;
                }
                else
                {
                    prices = db.LandMarkPrices
                  .Where(x => x.CountryId == model.DeliveryAddress.Country.CountryID
                              && x.Weight == Math.Max(Convert.ToDouble(packageweightt), newWeight)) // Use the greater of the actual or dimensional weight
                  .Select(x => new TotalCost
                  {
                      TotalCostGrossWithoutCollection = (double)x.Price
                  }).ToList();
                    actualWeight = (double)newWeight;
                }
                landmarkPrices.AddRange(prices);
                CreateLabelRequest createLabelRequest = new();
                GetQuoteInput_DTO root = new GetQuoteInput_DTO()
                {
                    Credentials = new Credentials()
                };
                root.Credentials.APIKey = apiKey;
                root.Credentials.Password = apiPassword;
                root.Shipment = model;
                //it was made but giving error
                // var response = await _apiClientMegway.PostAsync<CreateLabelResponse>("api/couriers/v1/MoovParcel/create-label", createLabelRequest);
                var result = await _apiClientLandmark.PostAsync<GetQuoteOutput_DTO>("Quote/V2/GetQuoteMinimal", root);
                #region For calling Move Parcel API
                string countryname = model.DeliveryAddress.CountryName;
                var validCountries = new List<string>
                    {
                        "United Kingdom (Mainland)",
                        "United Kingdom (Scottish Highlands)",
                        "United Kingdom (Offshore Isles)",
                        "United Kingdom (Northern Ireland)"
                    };

                if (validCountries.Contains(countryname?.Trim(), StringComparer.OrdinalIgnoreCase))
                {
                    var request = new CreateLabelRequest
                    {
                        //auth_company = "Megway",
                        //request_id = "efcf0fcf6c20f9e17a07030eac4d1a09",
                        auth_company = "8ack",
                        format_address_default = true,
                        request_id = "efcf0fcf6c20f9e17a07030eac4d1a09",
                        Shipment = new CreateLabelRequestShipment
                        {
                            label_size = "6x4",
                            label_format = "pdf",
                            generate_invoice = false,
                            generate_packing_slip = false,
                            courier = new CreateLabelRequestCourier { auth_company = "Moov Master" },
                            collection_date = DateTime.Now,
                            dc_service_id = "DPD-12",
                            reference = "megwaytest",
                            ship_from = new CreateLabelRequestShipFrom
                            {
                                name = "Ross",
                                phone = "01111111111",
                                email = "ross@saas-ecommerce.com",
                                company_name = "Nino Logistics",
                                address_1 = "2 Infirmary Street",
                                city = "Leeds",
                                postcode = "LS1 2JP",
                                country_iso = "GB",
                                company_id = "00000000",
                                tax_id = "GB123456789",
                                eori_id = "GB123456789000"
                            },
                            ship_to = new CreateLabelRequestShipTo
                            {
                                name = "Ross Jermy",
                                email = "ross@saas-ecommerce.com",
                                address_1 = "9 Mellor Meadows",
                                city = "Oswestry",
                                postcode = "SY11 4FN",
                                country_iso = "GB"
                            },
                     parcels = new List<CreateLabelRequestParcel>
                    {
                        new CreateLabelRequestParcel
                        {
                            dim_width = (int)packageWidth,
                            dim_height = (int)packageHeight,
                            dim_length = (int)packageLength,
                            dim_unit = "cm",
                            items = new List<CreateLabelRequestItem>
                            {
                                new CreateLabelRequestItem
                                {
                                    description = "Test Item",
                                    origin_country = "GB",
                                    quantity = 1,
                                    value_currency = "GBP",
                                    weight = (double)packageweightt,
                                    weight_unit = "KG",
                                    sku = "TestSKU",
                                    hs_code = "50000000",
                                    value = "0.00",
                                    extended_description = "Test Item"
                                }
                            }
                        }
                    }
                        }
                    };
                    var MoveParcelquoteResponse = await _courierApiService.GetQuoteAsync(request);
                    var servicesresult = JsonConvert.DeserializeObject<List<CourierService>>(MoveParcelquoteResponse);
					if (MoveParcelquoteResponse != null && servicesresult.Count > 0)
                    {
                        int sid = 501;
                        foreach (var MoveParcelresult in servicesresult)
                        {
                            // Filter services that start with "Evri" and contain specific keywords
                            if (MoveParcelresult.service_name.StartsWith("Evri") &&
                                (MoveParcelresult.service_name.Contains("48 Postable C2C") ||
                                 MoveParcelresult.service_name.Contains("48 Non POD C2C") ||
                                 MoveParcelresult.service_name.Contains("24 Non POD C2C")))
                            {
                                // Calculate the cost once for each MoveParcelresult
                                var newcost = MoveParcelresult.Price.Total * 1.35m;

                                var totalCost = new TotalCost
                                {
                                    TotalCostGrossWithoutCollection = (double)newcost
                                };

                                // Create a single ServiceResult per MoveParcelresult
                                var newServiceResult = new ServiceResult
                                {
                                    ServiceID = sid,
                                    ServiceName = MoveParcelresult.service_name,
                                    CarrierName = MoveParcelresult.courier,
                                    ChargeableWeight = actualWeight,
                                    TransitTimeEstimate = "2",
                                    IsWarehouseService = false,
                                    TotalCost = totalCost,
                                    ServicePriceBreakdown = null,
                                    OptionalExtras = null,
                                    ServiceCode = MoveParcelresult.service_code,
                                    SignatureRequiredAvailable = false,
                                    ExpectedLabels = null,
                                    CollectionOptions = null,
                                    ServiceType = null,
                                    SameDayCollectionCutOffTime = null
                                };

                                // Add the new service result to the result.ServiceResults
                                result.ServiceResults.Add(newServiceResult);
                                sid++;
                            }
                        }
                    }
                    //var serviceDetails = result.ServiceResults.Select(s => new
                    //{
                    //    id=s.ServiceID,
                    //    ServiceName = s.ServiceName,
                    //    TotalPrice = s.TotalCost.TotalCostGrossWithoutCollection
                    //}).ToList();
                }
                #endregion
                //var result = await Common.CommonObjects.Instance.ApiClient.PostAsync<GetQuoteOutput_DTO>("Quote/V2/GetQuoteMinimal", root);
                if (landmarkPrices.Count != 0 && packageid ==1)
                {
                    foreach (var serviceResult in result.ServiceResults.ToList())
                    {
                        newServiceResult1 = new ServiceResult
                        {
                            ServiceID = 1200,
                            ServiceName = "Landmark Global DDP/IOSS",
                            CarrierName = "Landmark Global DDP/IOSS",
                            ChargeableWeight = actualWeight,
                            TransitTimeEstimate = "8",
                            IsWarehouseService = serviceResult.IsWarehouseService,
                            TotalCost = landmarkPrices[0],
                            ServicePriceBreakdown = serviceResult.ServicePriceBreakdown,
                            OptionalExtras = serviceResult.OptionalExtras,
                            ServiceCode =null,
                            SignatureRequiredAvailable = serviceResult.SignatureRequiredAvailable,
                            ExpectedLabels = serviceResult.ExpectedLabels,
                            CollectionOptions = serviceResult.CollectionOptions,
                            ServiceType = serviceResult.ServiceType,
                            SameDayCollectionCutOffTime = serviceResult.SameDayCollectionCutOffTime
                        };
                    }
                    result.ServiceResults.Add(newServiceResult1);
                }
                int serviceid = 0;
                // Sort the ServiceResults by TotalCost.TotalCostGrossWithoutCollection
                foreach (var serid in result.ServiceResults)
                {

                    if (serid.ServiceID == 1200)
                    {
                        serviceid = serid.ServiceID;
                        break; // Exit the loop once the service ID 200 is found
                    }
                }
                if (serviceid== 1200)
                {
                    setting = db.CompanySettings.FirstOrDefault(x => x.SettingName=="Landmark");
                }
                else
                {
                    setting = db.CompanySettings.FirstOrDefault(x => customer != null && x.CompanySettingId == customer.CompanySettingId);
                }
                if (setting == null)
                {
                    setting = db.CompanySettings.FirstOrDefault();
                }
                HttpContext.Session.SetObjectAsJson("Setting", setting);

                if (string.IsNullOrEmpty(model.DeliveryAddress.Postcode))
                {
                    model.DeliveryAddress.Postcode = "1";
                }
                if (string.IsNullOrEmpty(model.CollectionAddress.Postcode))
                {
                    model.CollectionAddress.Postcode = "1";
                }

                ViewBag.LogoPath = _hostingEnvironment.WebRootPath + "\\image\\shipment";
				if (result.Status == "SUCCESS")
				{
					result.ServiceResults = result.ServiceResults
					.OrderBy(sr => sr.TotalCost.TotalCostGrossWithoutCollection)
					.ToList();
					var json = JsonConvert.SerializeObject(model);
					ViewBag.requestQuoteStr = json;
					return View("RequestQuote", result);
				}
				else
				{
					return View("Error", result);
				}
			}
            catch (Exception ex)
            {
                // Consider logging the exception or adding specific error handling
                throw;
            }
        }
        [HttpPost]
        public IActionResult SaveOrder(Order model)
        {
            try
            {
                var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
                if (customer == null)
                {
                    return Ok("login");
                }
                if (customer.GuestUser != null)
                {
                    model.OrderEmail = customer.GuestUser.Email;
                }
                

                //var setting = HttpContext.Session.GetObjectFromJson<CompanySetting>("Setting");
                var currentDate = DateTime.Now;
                string month = currentDate.ToString("MM");
                string year = currentDate.ToString("yy");
                /*
                var RefNo = db.Orders.Where(x => (x.RefrenceNo.Substring(3, 2)) == year && (x.RefrenceNo.Substring(5, 2)) == month).Select(o => o.RefrenceNo).FirstOrDefault();

                if (!string.IsNullOrEmpty(RefNo))
                {
                    var val = db.Orders.Where(x => (x.RefrenceNo.Substring(3, 2)) == year && (x.RefrenceNo.Substring(5, 2)) == month).Select(o => o.RefrenceNo)
                          .Max(b => b == null ? 0 : Convert.ToInt32((b.Substring(7, 5))));

                    var MaxNo = $"0000{(val + 1)}".GetLast(5);
                    model.RefrenceNo = $"MP-{year}{month}{MaxNo}";
                }
                else
                {
                    //model.RefrenceNo = $"MP-{year}{month}00001";
                    model.RefrenceNo = $"MP-0000001";

                }*/
                var RefNo = db.Orders
                    .Where(x => x.RefrenceNo.StartsWith("MP-"))
                    .OrderByDescending(o => o.OrderId)
                    .Select(o => o.RefrenceNo)
                    .FirstOrDefault();

                //var RefNo = db.Orders.Where(x => x.RefrenceNo.StartsWith("MP-")).Select(o => o.RefrenceNo).FirstOrDefault();
                if (!string.IsNullOrEmpty(RefNo))
                {
                    var val = db.Orders
                    .Where(x => x.RefrenceNo.StartsWith("MP-"))
                    .OrderByDescending(o => o.OrderId)
                    .Select(o => o.RefrenceNo)
                    .FirstOrDefault();
                    //long numericPart = long.Parse(val.Substring(3)) + 1;
                    //var MaxNo = $"000000{(numericPart + 1)}".GetLast(7);
                    // Replace the first four digits if the reference number starts with "24"
                    //if (MaxNo.StartsWith("24"))
                    //{
                    //    MaxNo = "0000" + MaxNo.Substring(4);
                    //}
                   // model.RefrenceNo = $"MP-{MaxNo}";

                    // Extract the numeric part and increment it
                    long numericPart = long.Parse(val.Substring(3)) + 1;

                    // Format the new number with leading zeros, ensuring it's at least 7 digits long
                    var MaxNo = numericPart.ToString("D7");

                    // Set the new reference number
                    model.RefrenceNo = $"MP-{MaxNo}";

                }
                else
                {
                    model.RefrenceNo = "MP-0000001";
                }

                
                var shipment = JsonConvert.DeserializeObject<Shipment>(model.RequestQuote);
                if (shipment.DeliveryAddress.Country.CountryID == 247)
                    model.IsSameCountry = false;
                else
                model.IsSameCountry = shipment.IsSameCountry;
                model.CustomerId = customer.CustomerId;
                model.ServiceCode = model.ServiceCode;
                db.Orders.Add(model);
                db.SaveChanges();
                return Ok(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }
        public IActionResult Index1()
        {
            return View();
        }
        public IActionResult Tracking()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            
            var order = new Order(); //db.Orders.Where(x => x.OrderId == 15).FirstOrDefault();
            return View(order);
        }
        public IActionResult TermAndCondition()
        {
            return View();
        }

        public IActionResult Page(int id)
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
            var result = db.Pages.FirstOrDefault(x => x.PageId == id);
            return View(result);
        }
    }
}

using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using MegwayParcel.Common;
using LogisticERP.Web.Models;
using LogisticERP.Web.Models.Landmark;
using LogisticERP.Web.Models.Megway;
using MegwayParcel.Common.Services;
using MegwayParcel.Common.TakePaymentGateway;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Xml.Serialization;
using XAct.IO;
using MegwayParcel.Common.CommonServices;
using MegwayParcel.Common.Services;
using MegwayParcel.Common.Data;

namespace MegwayParcel.Web.Controllers
{
    public class AddressController : Controller
    {
        public string apiKey = "4h4Gy4QpKo";
        public string apiPassword = "USzh0o4o&A";
        public string Lmusername = "Megway_Logistics_API";
        public string LmPassword = "Kotli1977@pakistan";
        public readonly IGatewayHelper _gatewayHelper;
		private readonly IShippingService shippingService;
        [Obsolete]
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly ApiClient _apiClientMegway;
        private readonly ApiClient _apiClientLandmark;
        private readonly CourierApiService _courierApiService;
        private readonly LogisticERPContext db;

        [Obsolete]
        public AddressController(Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IGatewayHelper gatewayHelper,
            IShippingService shippingService, CourierApiService courierApiService, LogisticERPContext context)
        {
            db = context;
            _courierApiService = courierApiService;
            _gatewayHelper = gatewayHelper;
			this.shippingService = shippingService;
			_hostingEnvironment = env;
            _apiClientMegway = new ApiClient("https://production.courierapi.co.uk/", "Megway", "tqlvkmzcsdgfhrbo");
            _apiClientLandmark = new ApiClient("http://services3.transglobalexpress.co.uk/", apiUser: null, apiToken: null);
        }
        private string GetUrl()
        {
            var request = HttpContext.Request;
            return $"{request.Path.Value}{request.QueryString.Value}";
        }
        public IActionResult Index(int orderId, string returnUrl = "")
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return RedirectToAction("Login", "Account", new { path = GetUrl() });
            }
            
            var order = db.Orders.Find(orderId);
            ViewBag.IsSameCountry = order.IsSameCountry;
            var address = db.Addresses.Where(x => x.OrderId == orderId && x.TypeId == 1).FirstOrDefault();
            if (address == null)
            {
                address = new Address();
                address.Forename = customer.DefForename;
                address.Surname = customer.DefSurname;
                address.EmailAddress = customer.DefEmailAddress;
                address.Country = customer.DefTelephoneNumber;
                address.MobileNumber = customer.DefMobileNumber;
                address.CompanyName = customer.DefCompanyName;
                address.AddressLineOne = customer.DefAddressLineOne;
                address.AddressLineTwo = customer.DefAddressLineTwo;
                address.City = customer.DefCity;
                address.County = customer.DefCounty;
                address.Postcode = customer.DefPostcode;
                var quote = JsonConvert.DeserializeObject<Shipment>(order.RequestQuote);
                address.Postcode = quote.CollectionAddress.Postcode;
                address.Country = quote.CollectionAddress.CountryName;
            }
            ViewBag.returnUrl = returnUrl;
            address.OrderId = orderId;
            address.TypeId = 1;
            var guestEmail = customer.GuestUser?.Email;
            var customerId = customer.CustomerId;
            var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0
                            && (guestEmail != null ? x.OrderEmail == guestEmail : x.CustomerId == customerId)).Count();
            ViewBag.basketCount = basketCount;


            return View(address);
        }
        public IActionResult Save(Address model)
        {
            
            if (model.AddressId > 0)
            {
                db.Addresses.Update(model);
            }
            else
            {
                db.Addresses.Add(model);
            }
            db.SaveChanges();

            return Ok(model);
        }
        public IActionResult Exporter(int orderId, string returnUrl = "")
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return RedirectToAction("Login", "Account", new { path = GetUrl() });
            }
            
            var order = db.Orders.Where(x => x.OrderId == orderId).Include(inc => inc.Addresses).Include(inc => inc.ExporterDetails).FirstOrDefault();
            ViewBag.order = order;
            var or = new Order();
            or.OrderId = order.OrderId;
            or.Addresses = order.Addresses.Where(x => x.TypeId == 2).ToList();
            if (order.ExporterDetails == null || order.ExporterDetails.Count == 0)
            {
                ExporterDetail expo = new ExporterDetail
                {
                    OrderId = order.OrderId,
                    ExporterEorino = customer.Eorinumber,
                    ExporterVatno = customer.MyVatNumber,
                    IossNumber = customer.MyVatNumber,
                };
                or.ExporterDetails = new List<ExporterDetail> { expo };
            }
            else
                or.ExporterDetails = order.ExporterDetails;

            var guestEmail = customer.GuestUser?.Email;
            var customerId = customer.CustomerId;
            var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0
                            && (guestEmail != null ? x.OrderEmail == guestEmail : x.CustomerId == customerId)).Count();
            ViewBag.basketCount = basketCount;

            ViewBag.returnUrl = returnUrl;
            return View(or);
        }
        public IActionResult SaveExporter(Order order)
        {
            

            if (order.ExporterDetails?.Count > 0)
            {
                order.ExporterDetails.FirstOrDefault().OrderId = order.OrderId;
                if (order.ExporterDetails.FirstOrDefault()?.ExporterDetailId > 0)
                {
                    db.ExporterDetails.Update(order.ExporterDetails.FirstOrDefault());
                }
                else
                {
                    db.ExporterDetails.Add(order.ExporterDetails.FirstOrDefault());
                }

                if (order.Addresses?.Count > 0 && order.ExporterDetails.FirstOrDefault().IsExporterAddressAsCollectionAddress == false)
                {
                    order.Addresses.FirstOrDefault().TypeId = 2;
                    if (order.Addresses.FirstOrDefault().AddressId > 0)
                    {
                        db.Addresses.Update(order.Addresses.FirstOrDefault());
                    }
                    else
                    {
                        db.Addresses.Add(order.Addresses.FirstOrDefault());
                    }
                }
            }

            db.SaveChanges();
            return Ok();
        }
        public IActionResult Delivery(int orderId)
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return RedirectToAction("Login", "Account", new { path = GetUrl() });
            }
            
            var order = db.Orders.Where(x => x.OrderId == orderId).Include(inc => inc.Addresses).Include(inc => inc.DeliveryDetails).FirstOrDefault();
            ViewBag.order = order;
            var or = new Order();
            or.OrderId = order.OrderId;
            or.Addresses = order.Addresses.Where(x => x.TypeId == 3).ToList();
            if (or.Addresses == null || or.Addresses.Count == 0)
            {
                var quote = JsonConvert.DeserializeObject<Shipment>(order.RequestQuote);
                var add = new Address
                {
                    Postcode = quote.DeliveryAddress.Postcode,
                    Country = quote.DeliveryAddress.CountryName,
                    TypeId = 3
                };
                or.Addresses = new List<Address> { add };
            }
            or.DeliveryDetails = order.DeliveryDetails;
            or.SelectQuote = order.SelectQuote;

            var guestEmail = customer.GuestUser?.Email;
            var customerId = customer.CustomerId;
            var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0
                            && (guestEmail != null ? x.OrderEmail == guestEmail : x.CustomerId == customerId)).Count();
            ViewBag.basketCount = basketCount;

            return View(or);
        }
        public IActionResult SaveDelivery(Order order)
        {
            

            if (order.DeliveryDetails?.Count > 0)
            {
                order.DeliveryDetails.FirstOrDefault().OrderId = order.OrderId;
                if (order.DeliveryDetails.FirstOrDefault()?.DeliveryDetailId > 0)
                {
                    db.DeliveryDetails.Update(order.DeliveryDetails.FirstOrDefault());
                }
                else
                {
                    db.DeliveryDetails.Add(order.DeliveryDetails.FirstOrDefault());
                }
                Order or = db.Orders.Find(order.OrderId);
                or.SignaturePrice = order.SignaturePrice;
                or.ResidentialSurcharge = order.ResidentialSurcharge;
                db.Orders.Update(or);
                //if (order.DeliveryDetails.FirstOrDefault().IsSignatureRequired || order.DeliveryDetails.FirstOrDefault().IsResidentialAddress )
                //{
                //}
            }
            if (order.Addresses?.Count > 0)
            {
                order.Addresses.FirstOrDefault().TypeId = 3;
                if (order.Addresses.FirstOrDefault().AddressId > 0)
                {
                    db.Addresses.Update(order.Addresses.FirstOrDefault());
                }
                else
                {
                    db.Addresses.Add(order.Addresses.FirstOrDefault());
                }
            }

            db.SaveChanges();
            return Ok();
        }
        public async Task<IActionResult> Declaration(int orderId)
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return RedirectToAction("Login", "Account", new { path = GetUrl() });
            }
            
            var order = db.Orders.Where(x => x.OrderId == orderId).Include(inc => inc.Addresses).Include(a => a.ConsignmentSummaries).Include(x => x.CustomerInvoices).FirstOrDefault();

            var quote = JsonConvert.DeserializeObject<Shipment>(order.RequestQuote);

            if (quote.Consignment.ItemType == "Document" && (order.ConsignmentSummaries == null || order.ConsignmentSummaries.Count == 0))
            {
                var sumary = PrepareConsignmentSummary();
                sumary.OrderId = orderId;
                db.ConsignmentSummaries.Add(sumary);
                if (!order.IsSameCountry)
                {
                    var custInv = PrepareCustomerInvoice(quote, orderId);
                    db.CustomerInvoices.AddRange(custInv);
                }
                db.SaveChanges();

                return RedirectToAction("Detail", "Address", new { orderId = order.OrderId });
            }
            if (quote.Consignment.ItemType == "Document")
            {
                order.ShippingCharges = 0;
            }


            CountryRootInput root = new CountryRootInput();
            root.Credentials = new Credentials();
            root.Credentials.APIKey = apiKey;
            root.Credentials.Password = apiPassword;
            var result = await _apiClientLandmark.PostAsync<CountryRootOutput>("Country/V2/GetCountries", root);
            //var result = await Common.CommonObjects.Instance.ApiClient.PostAsync<CountryRootOutput>("Country/V2/GetCountries", root);
            ViewBag.contries = result.Countries;
            var res = JsonConvert.DeserializeObject<ServiceResult>(order.SelectQuote);
            var subTotal = res.ServicePriceBreakdown?.Where(x => x.Code == "FRT").FirstOrDefault().Cost;
            ViewBag.ShippingCharges = subTotal;



            var guestEmail = customer.GuestUser?.Email;
            var customerId = customer.CustomerId;
            var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0
                            && (guestEmail != null ? x.OrderEmail == guestEmail : x.CustomerId == customerId)).Count();
            ViewBag.basketCount = basketCount;

            return View(order);
        }
        public ConsignmentSummary PrepareConsignmentSummary()
        {
            var summary = new ConsignmentSummary()
            {
                ContentSummary = "Non-valued Document",
                ReasonForShipment = "Other",
                TotalGoodsValue = 1,
                ShippingCharges = 0,
                TotalValueForCustoms = 1,

            };
            return summary;
        }
        public List<CustomerInvoice> PrepareCustomerInvoice(Shipment ship, int orderId)
        {
            var invoices = new List<CustomerInvoice>();
            foreach (var item in ship.Consignment.Packages)
            {
                var inv = new CustomerInvoice()
                {
                    CommodityCode = "4901100020",
                    ProductDescription = "Non-valued Document",
                    CountryId = ship.CollectionAddress.Country.CountryID,
                    NoOfItems = 1,
                    PerItemValue = 1,
                    ValueForCustoms = 1,
                    OrderId = orderId,
                    ShippingCharges = 0,
                    TotalGoodsValue = 0,
                    TotalValueForCustoms = 1,
                };
                inv.PerItemWeight = Convert.ToDecimal(item.Weight);
                inv.TotalGoodsValue = ship.Consignment.Packages.Count;
                inv.TotalValueForCustoms = ship.Consignment.Packages.Count;
                invoices.Add(inv);
            }

            return invoices;
        }
        public IActionResult SaveDeclaration(Order order)
        {
            

            if (order.ConsignmentSummaries?.Count > 0)
            {
                order.ConsignmentSummaries.FirstOrDefault().OrderId = order.OrderId;
                if (order.ConsignmentSummaries.FirstOrDefault().ConsignmentSummaryId > 0)
                {
                    db.ConsignmentSummaries.Update(order.ConsignmentSummaries.FirstOrDefault());
                }
                else
                {
                    db.ConsignmentSummaries.Add(order.ConsignmentSummaries.FirstOrDefault());
                }
            }

            var inv = db.CustomerInvoices.Where(x => x.OrderId == order.OrderId).ToList();
            if (inv != null && inv.Count > 0)
            {
                db.CustomerInvoices.RemoveRange(inv);
            }
            if (order.CustomerInvoices?.Count > 0)
            {
                foreach (var item in order.CustomerInvoices)
                {
                    item.OrderId = order.OrderId;
                    db.CustomerInvoices.Add(item);
                }
            }

            db.SaveChanges();
            return Ok();

        }
        public IActionResult Detail(int orderId)
        {
            try
            {
                var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
                if (customer == null)
                {
                    return RedirectToAction("Login", "Account", new { path = GetUrl() });
                }
                
                var order = db.Orders.Where(x => x.OrderId == orderId)
                    .Include(inc => inc.Addresses).Include(inc => inc.ConsignmentSummaries)
                    .Include(inc => inc.CustomerInvoices)
                    .Include(inc => inc.DeliveryDetails)
                    .Include(inc => inc.ExporterDetails)
                    .FirstOrDefault();

                // have to add here the price of new api

                var res = JsonConvert.DeserializeObject<ServiceResult>(order.SelectQuote);
                ViewBag.shipmentResult = res;

                var guestEmail = customer.GuestUser?.Email;
                var customerId = customer.CustomerId;
                var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0
                                && (guestEmail != null ? x.OrderEmail == guestEmail : x.CustomerId == customerId)).Count();
                ViewBag.basketCount = basketCount;

                return View(order);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult Basket()
        {
            try
            {
                var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
                if (customer == null)
                {
                    return RedirectToAction("Login", "Account", new { path = GetUrl() });
                }
                
                var order = new List<Order>();
                if (customer.GuestUser != null)
                {
                    order = db.Orders.Where(x => x.OrderEmail == customer.GuestUser.Email && (x.IsRemoveFromBasket ?? false) == false && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0).ToList();
                    var basketCount = order?.Count;
                    ViewBag.basketCount = basketCount ?? 0;
                }
                else
                {
                    order = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.CustomerId == customer.CustomerId && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0).ToList();
                    var basketCount = order?.Count;
                    ViewBag.basketCount = basketCount ?? 0;
                }
                ViewBag.customer = customer;
                return View(order);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult OrderConfirm(int orderId, bool isRemove)
        {
            
            var order = db.Orders.Where(x => x.OrderId == orderId).FirstOrDefault();

            if (isRemove)
            {
                order.IsRemoveFromBasket = true;
            }
            else
            {
                order.IsAddCart = true;
            }
            db.Orders.Update(order);
            db.SaveChanges();
            return Ok();
        }
        public IActionResult Collection(int orderId)
        {
            try
            {
                var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
                if (customer == null)
                {
                    return RedirectToAction("Login", "Account", new { url = GetUrl() });
                }
                
                var order = db.Orders.Where(x => x.OrderId == orderId)
                    .Include(inc => inc.Addresses).Include(inc => inc.ConsignmentSummaries)
                    .Include(inc => inc.CustomerInvoices)
                    .Include(inc => inc.DeliveryDetails)
                    .Include(inc => inc.ExporterDetails)
                    .FirstOrDefault();

                var guestEmail = customer.GuestUser?.Email;
                var customerId = customer.CustomerId;
                var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0
                                && (guestEmail != null ? x.OrderEmail == guestEmail : x.CustomerId == customerId)).Count();
                ViewBag.basketCount = basketCount;

                return View(order);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult SaveCollection(Order model)
        {
            
            var order = db.Orders.Where(x => x.OrderId == model.OrderId).FirstOrDefault();
            order.CollectionDate = model.CollectionDate;
            order.CollectionTime = model.CollectionTime;
            order.CollectionOptionId = model.CollectionOptionId;
            order.CollectionCharges = model.CollectionCharges;

            db.Orders.Update(order);
            db.SaveChanges();
            return Ok();
        }
        public IActionResult Insurance(int orderId)
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return RedirectToAction("Login", "Account", new { path = GetUrl() });
            }
            
            var order = db.Orders.Where(x => x.OrderId == orderId).Include(inc => inc.ConsignmentSummaries).FirstOrDefault();

            var guestEmail = customer.GuestUser?.Email;
            var customerId = customer.CustomerId;
            var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0
                            && (guestEmail != null ? x.OrderEmail == guestEmail : x.CustomerId == customerId)).Count();
            ViewBag.basketCount = basketCount;

            return View(order);
        }
        public IActionResult SaveInsurance(Order model)
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return RedirectToAction("Login", "Account", new { path = GetUrl() });
            }
            
            var order = db.Orders.Find(model.OrderId);

            order.InsuranceValue = model.InsuranceValue;
            order.InsuranceCharges = model.InsuranceCharges;
            var total = order.InsuranceCharges + order.ShippingCharges + order.RemoteSurcharges + order.Surcharges + order.CollectionCharges + order.SignaturePrice + (order.ResidentialSurcharge ?? 0);

            //decimal vatPercent = 0;
            //var setting = HttpContext.Session.GetObjectFromJson<CompanySetting>("Setting");
            //vatPercent = setting?.VatchargePercent ?? 0;

            //order.Vatcharges = total * (vatPercent / 100);

            order.TotalCharges = total;// + order.Vatcharges;

            db.Orders.Update(order);
            db.SaveChanges();
            if (order.IsPaymentReceived)
                return RedirectToAction("OrderDetail", "Address", new { orderId = order.OrderId });
            else
                return RedirectToAction("Checkout", "Address", new { orderId = order.OrderId });
        }
        public IActionResult Checkout(int orderId)
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return RedirectToAction("Login", "Account", new { path = GetUrl() });
            }
            
            var order = db.Orders.Where(x => x.OrderId == orderId)
                .Include(inc => inc.Addresses).Include(inc => inc.ConsignmentSummaries)
                .Include(inc => inc.CustomerInvoices)
                .Include(inc => inc.DeliveryDetails)
                .Include(inc => inc.ExporterDetails)
                .FirstOrDefault();

            var guestEmail = customer.GuestUser?.Email;
            var customerId = customer.CustomerId;
            var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0
                            && (guestEmail != null ? x.OrderEmail == guestEmail : x.CustomerId == customerId)).Count();
            ViewBag.basketCount = basketCount;

            return View(order);
        }
        private async Task<IActionResult> BookShipment(int orderId)
        {
            try
            {
                string airWaybillReference = "";
                
                var order = db.Orders.Where(x => x.OrderId == orderId)
                    .Include(inc => inc.Addresses)
                    .Include(inc => inc.ConsignmentSummaries)
                    .Include(inc => inc.CustomerInvoices)
                    .Include(inc => inc.DeliveryDetails)
                    .Include(inc => inc.ExporterDetails)
                    .FirstOrDefault();
                if (order.OrderStatusId == 5)
                {
                    return Ok();
                }
                //here need to change the new api

                var service = JsonConvert.DeserializeObject<ServiceResult>(order.SelectQuote);

                var quote = JsonConvert.DeserializeObject<Shipment>(order.RequestQuote);
                var collectionAddress = order.Addresses.Where(x => x.TypeId == 1).FirstOrDefault();
                var exporterAddress = order.Addresses.Where(x => x.TypeId == 2).FirstOrDefault();
                var deliveryAddress = order.Addresses.Where(x => x.TypeId == 3).FirstOrDefault();

                var exporter = order.ExporterDetails.FirstOrDefault();
                var delivery = order.DeliveryDetails.FirstOrDefault();
                var custom = order.CustomerInvoices.ToList();
                var consignmentSummary = order.ConsignmentSummaries.FirstOrDefault();
                var serviceId = db.Orders
                        .Where(x => x.ServiceId.HasValue && x.OrderId == orderId)
                        .Select(x => x.ServiceId.Value)
                        .FirstOrDefault();
                var serviceCode = db.Orders
                        .Where(x => x.ServiceId.HasValue && x.OrderId == orderId)
                        .Select(x => x.ServiceCode)
                        .FirstOrDefault();
                string _deliveryAddressLineTwo = deliveryAddress.AddressLineTwo ?? "";
                string _collectionAddressLineOne = collectionAddress.AddressLineTwo ?? "";

                // Check if the serviceId is 200 and if so, send the shipping request
                if (serviceId == 1200)
                {
                    #region Landmark API Call
                    LandmarkShipRequest landmarkShipRequest = new();

                    landmarkShipRequest.Login = new Login();
                    landmarkShipRequest.Login.Username = "Megway_Logistics_API";
                    landmarkShipRequest.Login.Password = "Kotli1977@pakistan";

                    landmarkShipRequest.ShippingLane = new ShippingLane();
                    landmarkShipRequest.ShippingLane.Region = "Client UK";

                    landmarkShipRequest.IsCommercialShipment = false;
                    landmarkShipRequest.Test = false;
                    landmarkShipRequest.ClientID = "2208";
                    landmarkShipRequest.AccountNumber = "L2208A";
                    landmarkShipRequest.ShipMethod = "LGINTSTD";
                    landmarkShipRequest.ItemsCurrency = "USD";
                    landmarkShipRequest.LabelFormat = "PDF";
                    landmarkShipRequest.LabelEncoding = "LINKS";
                    landmarkShipRequest.Reference = $"{order.RefrenceNo}";
                    //landmarkShipRequest.VendorInformation = new VendorInformation();
                    //Vendor information start
                    var vendrinfo = new VendorInformation
                    {
                        VendorAddress1 = collectionAddress.AddressLineOne,
                        VendorAddress2 = _collectionAddressLineOne,
                        VendorCity = collectionAddress.City,
                        //VendorCountry = collectionAddress.Country,
                        VendorCountry = "GB",
                        VendorEmail = collectionAddress.EmailAddress,
                        VendorEORINumber = collectionAddress.Eorinumber,
                        VendorName = collectionAddress.Surname + collectionAddress.Forename,
                        VendorPhone = collectionAddress.MobileNumber,
                        VendorPostalCode = collectionAddress.Postcode,
                        VendorBusinessNumber = "",
                        VendorIOSSNumber = collectionAddress.MobileNumber,
                        VendorState = collectionAddress.MobileNumber,
                        VendorRGRNumber = ""
                    };
                    landmarkShipRequest.VendorInformation = vendrinfo;

                    //vendor info ended
                    //delivery address for landmark start

                    landmarkShipRequest.ShipTo = new ShipTo();
                    var shipto = new ShipTo
                    {
                        Name = deliveryAddress.Surname + deliveryAddress.Forename,
                        Attention = "",
                        Address1 = deliveryAddress.AddressLineOne,
                        Address2 = _deliveryAddressLineTwo,
                        Address3 = "",
                        City = deliveryAddress.City,
                        State = "",
                        PostalCode = deliveryAddress.Postcode,
                        //Country = quote.DeliveryAddress.CountryName,
                        Country = deliveryAddress.Country,
                        Phone = deliveryAddress.MobileNumber,
                        Email = deliveryAddress.EmailAddress,
                        ConsigneeTaxID = null

                    };
                    landmarkShipRequest.ShipTo = shipto;
                    //delivery address for landmark ended
                    // packages start

                    landmarkShipRequest.Packages = new List<ShipPackage>();
                    int packCount = 1;
                    foreach (var item in quote?.Consignment?.Packages)
                    {
                        var package = new ShipPackage
                        {
                            WeightUnit = "LB",
                            Weight = (decimal)Convert.ToDouble(item.Weight),
                            DimensionsUnit = "IN",
                            Length = (decimal)Convert.ToDouble(item.Length),
                            Width = (decimal)Convert.ToDouble(item.Width),
                            Height = (decimal)Convert.ToDouble(item.Height),
                            PackageReference = $"{order.RefrenceNo}",
                        };
                        packCount++;
                        landmarkShipRequest.Packages.Add(package);
                    }
                    landmarkShipRequest.Items = new List<Item>();
                    int cCount = 1;
                    foreach (var cus in custom)
                    {
                        if (quote?.Consignment?.Packages?.Count == 1)
                        {
                            var det = new Item
                            {
                                Sku = "7224059",
                                Quantity = (int)cus.NoOfItems,
                                UnitPrice = (decimal)cus.PerItemValue,
                                Description = cus.ProductDescription,
                                HSCode = cus.CommodityCode,
                                CountryOfOrigin = "CN"
                            };
                            landmarkShipRequest.Items.Add(det);
                        }
                        cCount++;
                    }
                    //packages end
                    #endregion
                    var shipResponse = await shippingService.SendRequestAsync<LandmarkShipRequest, LandmarkShipResponse>(landmarkShipRequest, "/v2/Ship.php");
                    if (!shipResponse.Result.Success)
                    {
                        //_logger.LogWarning("Landmark API responded with failure. Error message: {ResultMessage}", shipResponse.Result.ResultMessage);
                        order.OrderStatusId = 1; // Assuming 1 is the failure status
                        order.ErrorResponse = JsonConvert.SerializeObject(shipResponse);
                        db.Orders.Update(order);
                        db.SaveChanges();
                        return Ok(shipResponse);
                    }
                    order.OrderResponse = JsonConvert.SerializeObject(shipResponse);
                    string invoiceBody = "";
                    if (shipResponse.Result.ShipmentLabelLink != null)
                    {
                        foreach (var item in shipResponse.Result.Packages)
                        {
                            
                            if (string.IsNullOrEmpty(airWaybillReference))
                            {
                                airWaybillReference = item.PackageReference;
                            }
                        }
                    }
                }
                else if (serviceId > 500 )
                {
                    #region MoveParcel API
                    try
                    {
                        int quantity = 1;
                        CreateLabelRequest createLabelRequest = new();

                        createLabelRequest.auth_company = "Megway";
                        createLabelRequest.request_id = "efcf0fcf6c20f9e17a07030eac4d1a09";
                        createLabelRequest.format_address_default = true;

                        var shipment = new CreateLabelRequestShipment
                        {
                            label_size = "6x4",
                            label_format = "pdf",
                            generate_invoice = false,
                            generate_packing_slip = false,
                            courier = new CreateLabelRequestCourier 
                            {
                                auth_company = "Moov Master" 
                            },
                            collection_date = DateTime.Now,
                            dc_service_id = serviceCode,
                            reference = $"{order.RefrenceNo}",
                            ship_from = new CreateLabelRequestShipFrom
                            {
                                name = $"{collectionAddress.Surname} {collectionAddress.Forename}",
                                phone = collectionAddress.MobileNumber,
                                email = collectionAddress.EmailAddress,
                                company_name = collectionAddress.CompanyName,
                                address_1 = $"{collectionAddress.AddressLineOne} {_collectionAddressLineOne}",
                                city = collectionAddress.City,
                                postcode = collectionAddress.Postcode,
                                country_iso = "GB",
                                company_id = "00000000",
                                tax_id = "GB123456789",
                                eori_id = collectionAddress.Eorinumber,
                            },
                            ship_to = new CreateLabelRequestShipTo
                            {
                                name = $"{deliveryAddress.Forename} {deliveryAddress.Surname}",
                                email = deliveryAddress.EmailAddress,
                                address_1 = $"{deliveryAddress.AddressLineOne} {_deliveryAddressLineTwo}",
                                city = deliveryAddress.City,
                                postcode = deliveryAddress.Postcode,
                                country_iso = "GB",
                            },
                        };
                        createLabelRequest.Shipment = shipment;
                        foreach (var pkg in quote?.Consignment?.Packages)
                        {
                            // Initialize a new package
                            var package = new CreateLabelRequestParcel
                            {
                                dim_width = Convert.ToInt32(pkg.Width),
                                dim_height = Convert.ToInt32(pkg.Height),
                                dim_length = Convert.ToInt32(pkg.Length),
                                dim_unit = "cm",
                                items = new List<CreateLabelRequestItem>
                                    {
                                        new CreateLabelRequestItem
                                    {
                                        description = "Test Item",
                                        origin_country = "GB",
                                        quantity = quantity,
                                        value_currency = "GBP",
                                        weight = Convert.ToInt32(pkg.Weight),
                                        weight_unit = "KG",
                                        sku = "TestSKU",
                                        hs_code = "50000000",
                                        value = "0.00",
                                        extended_description = "Test Item"
                                    }
                                }
                            };

                            // Ensure parcels list is initialized
                            createLabelRequest.Shipment.parcels ??= new List<CreateLabelRequestParcel>();
                            createLabelRequest.Shipment.parcels.Add(package);
                            quantity++;
                        }
                        
                        // Call API
                        var moveResponse = await _courierApiService.CreateLabelAsync<object, CreateLabelResponse>(createLabelRequest);

                        // Serialize and handle response
                        order.OrderResponse = System.Text.Json.JsonSerializer.Serialize(moveResponse, new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        if (moveResponse.TrackingRequestId == null || moveResponse.TrackingRequestId == 0)
                        {
                            order.OrderStatusId = 1;
                            order.ErrorResponse = System.Text.Json.JsonSerializer.Serialize(moveResponse);
                            db.Orders.Update(order);
                            db.SaveChanges();
                            return Ok(moveResponse);
                        }

                        string invoiceBody = "";
                        if(moveResponse != null && moveResponse.TrackingCodes != null)
                        {
                            // Convert the list of TrackingCodes into a single string
                            airWaybillReference = string.Join(",", moveResponse.TrackingCodes);

                            // Print the new string containing all tracking codes
                            Console.WriteLine($"Tracking Codes: {airWaybillReference}");
                        }
                        //airWaybillReference = moveResponse.TrackingRequestId.ToString();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    #endregion
                }

                else
                {
                    BookShipmentInput_DTO root = new BookShipmentInput_DTO();
                    root.Credentials = new BookCredentials();
                    root.Credentials.APIKey = apiKey;
                    root.Credentials.Password = apiPassword;

                    var details = new BookDetails();
                    details.ServiceID = service.ServiceID;
                    details.YourReference = $"{order.RefrenceNo}";
                    details.VATNumber = exporter?.ExporterVatno;
                    details.EORINumber = exporter?.ExporterEorino;
                    details.IMEINumber = "";


                    if (order.CollectionOptionId > 0 || (service.IsWarehouseService == false && order.CollectionOptionId == -9))
                    {
                        details.Collection = new Collection();
                        details.Collection.CollectionDate = Convert.ToDateTime(order.CollectionDate).ToString("yyyy-MM-dd");
                        details.Collection.ReadyFrom = Convert.ToString(order.CollectionTime);
                        details.Collection.CollectionOptionID = order.CollectionOptionId ?? 0;
                    }

                    details.BookAccessories = new List<BookAccessory>();
                    if (service.SignatureRequiredAvailable && delivery.IsSignatureRequired)
                    {
                        details.BookAccessories.Add(new BookAccessory
                        {
                            Code = "SIG"
                        });
                    }

                    decimal insuranceCoverValue = 50;
                    //Insurance start
                    if (order.SelectedCompanyLogo.ToUpper() == "EVRI")
                    {
                        insuranceCoverValue = 25;
                    }
                    if (consignmentSummary.TotalGoodsValue > insuranceCoverValue)
                    {
                        var ins = new Insurance
                        {
                            CoverValue = (double)order.InsuranceValue,
                            ExcessValue = (double)order.InsuranceCharges,
                            GoodsAreNew = true
                        };
                        details.Insurance = ins;
                    }

                    //Insurance end

                    root.BookDetails = details;

                    root.Shipment = new BookShipment();
                    ///Consigment Start
                    BookConsignment consign = new()
                    {
                        ItemType = quote.Consignment.ItemType,
                        ItemsAreStackable = true,
                        ConsignmentSummary = consignmentSummary.ContentSummary,
                        ConsignmentValue = (double)consignmentSummary.TotalValueForCustoms,
                        Packages = new List<BookPackage>()
                    };

                    int packageCount = 1;
                    foreach (var item in quote?.Consignment?.Packages)
                    {
                        var package = new BookPackage
                        {
                            Height = Convert.ToDouble(item.Height),
                            Width = Convert.ToDouble(item.Width),
                            Length = Convert.ToDouble(item.Length),
                            Weight = Convert.ToDouble(item.Weight)
                        };
                        package.CommodityDetails = new List<CommodityDetail>();

                        int cusCount = 1;
                        foreach (var cus in custom)
                        {
                            if (cusCount == packageCount || quote?.Consignment?.Packages?.Count == 1)
                            {
                                var det = new CommodityDetail
                                {
                                    CommodityCode = cus.CommodityCode,
                                    CommodityDescription = cus.ProductDescription,
                                    NumberOfUnits = (int)cus.NoOfItems,
                                    UnitValue = (double)cus.PerItemValue,
                                    UnitWeight = (double)cus.PerItemWeight,
                                };
                                det.CountryOfOrigin = new CountryOfOrigin();
                                det.CountryOfOrigin.CountryID = cus.CountryId;
                                package.CommodityDetails.Add(det);
                            }
                            cusCount++;
                        }
                        consign.Packages.Add(package);
                    }
                    root.Shipment.Consignment = consign;

                    ///Consignement End


                    //collection start
                    var coll = new BookCollectionAddress
                    {
                        AddressLineOne = collectionAddress.AddressLineOne,
                        AddressLineTwo = _collectionAddressLineOne,
                        City = collectionAddress.City,
                        CompanyName = collectionAddress.CompanyName,
                        County = collectionAddress.County,
                        EmailAddress = collectionAddress.EmailAddress,
                        EORINumber = collectionAddress.Eorinumber,
                        Forename = collectionAddress.Forename,
                        IsAddressResidential = collectionAddress.IsAddressResidential,
                        MobileNumber = collectionAddress.MobileNumber,
                        Postcode = collectionAddress.Postcode,
                        Surname = collectionAddress.Surname,
                        TelephoneNumber = collectionAddress.MobileNumber,
                        Country = new Country()
                    };
                    coll.Country.CountryID = quote.CollectionAddress.Country.CountryID;
                    root.Shipment.CollectionAddress = coll;
                    //collection end

                    //Delivery start
                    
                    var deli = new BookDeliveryAddress
                    {
                        AddressLineOne = deliveryAddress.AddressLineOne,
                        AddressLineTwo = _deliveryAddressLineTwo,
                        City = deliveryAddress.City,
                        CompanyName = deliveryAddress.CompanyName,
                        County = deliveryAddress.County,
                        EmailAddress = deliveryAddress.EmailAddress,
                        EORINumber = deliveryAddress.Eorinumber,
                        Forename = deliveryAddress.Forename,
                        IsAddressResidential = deliveryAddress.IsAddressResidential,
                        MobileNumber = deliveryAddress.MobileNumber,
                        Postcode = deliveryAddress.Postcode,
                        Surname = deliveryAddress.Surname,
                        TelephoneNumber = deliveryAddress.MobileNumber,
                        Country = new BookCountry()
                    };
                    deli.Country.CountryID = quote.DeliveryAddress.Country.CountryID;
                    root.Shipment.DeliveryAddress = deli;
                    //Delivery end
                    root.Credentials = new BookCredentials();
                    root.Credentials.APIKey = apiKey;
                    root.Credentials.Password = apiPassword;
                    

                    var result = await _apiClientLandmark.PostAsync<BookShipmentOutput_DTO>("Book/V2/BookShipment", root);
                    order.OrderResponse = JsonConvert.SerializeObject(result);
                    if (result.Status == "FAIL")
                    {
                        order.OrderStatusId = 1;
                        order.ErrorResponse = JsonConvert.SerializeObject(result);
                        db.Orders.Update(order);
                        db.SaveChanges();
                        return Ok(result);
                    }

                    if (result.Documents != null && result.Documents?.Count > 0)
                    {
                        foreach (var item in result?.Documents)
                        {
                            item.Content = "";
                        }
                    }
                    string invoiceBody = "";

                    if (result.Labels != null && result.Labels?.Count > 0)
                    {
                        foreach (var item in result.Labels)
                        {
                            item.LabelContent = "";
                            if (string.IsNullOrEmpty(airWaybillReference))
                            {
                                airWaybillReference = item.AirWaybillReference;
                            }
                        }
                    }
                }
                order.AirWaybillReference = airWaybillReference;
                order.OrderStatusId = 5;
                order.OrderDate = DateTime.Now;
                db.Orders.Update(order);
                db.SaveChanges();
                try
                {
                    string emailBodyColl = EmailBody(order, airWaybillReference, true);
                    string collBody = $"<p><strong>YOUR PARCEL HAS BEEN BOOKED!</strong></p><p>Hi {collectionAddress.Forename} {collectionAddress.Surname},</p>" + emailBodyColl;
                    ResponseVM collEmail = await GenerateInvoice(order, collectionAddress.EmailAddress, "Thank you for choosing Megway Parcels", collBody, "");
                    if (!string.IsNullOrEmpty(collEmail.Code) && collEmail.Code == "200")
                    {
                        order.IsCollectionEmailSent = true;
                    }
                    else
                    {
                        order.IsCollectionEmailSent = false;
                        order.CollectionEmailError = collEmail.Message;
                    }
                    string emailBodyDeliver = EmailBody(order, airWaybillReference, false);
                    string deliveryBody = $"<p><strong>YOUR PARCEL HAS BEEN BOOKED!</strong></p><p>Hi {deliveryAddress.Forename} {deliveryAddress.Surname},</p>" + emailBodyDeliver;
                    ResponseVM deliveryEmail = await GenerateInvoice(order, deliveryAddress.EmailAddress, "Thank you for choosing Megway Parcels", deliveryBody, "deliver");
                    if (!string.IsNullOrEmpty(deliveryEmail.Code) && deliveryEmail.Code == "200")
                    {
                        order.IsDeliveryEmailSent = true;
                    }
                    else
                    {
                        order.IsDeliveryEmailSent = false;
                        order.DeliveryEmailError = deliveryEmail.Message;
                    }
                    db.Orders.Update(order);
                    db.SaveChanges();
                }
                catch (Exception em)
                {
                }
                return RedirectToAction("Login", "Account", new { path = GetUrl() });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult Order()
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return RedirectToAction("Login", "Account", new { path = GetUrl() });
            }
            
            var orders = new List<Order>();
            if (customer.GuestUser != null)
            {
                orders = db.Orders.Where(x => x.OrderEmail == customer.GuestUser.Email && (x.IsRemoveFromBasket ?? false) == false && x.ConsignmentSummaries.Count > 0).OrderByDescending(x => x.OrderId).ToList();
                var basketCount = db.Orders.Where(x => x.OrderEmail == customer.GuestUser.Email && (x.IsRemoveFromBasket ?? false) == false && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0)?.Count();
                ViewBag.basketCount = basketCount ?? 0;

            }
            else
            {
                orders = db.Orders.Where(x => (x.CustomerId == customer.CustomerId || customer.Email == "admin@megwayparcels.co.uk") && (x.IsRemoveFromBasket ?? false) == false && x.ConsignmentSummaries.Count > 0).Include(inc => inc.Addresses).OrderByDescending(x => x.OrderId).ToList();
                var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.CustomerId == customer.CustomerId && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0)?.Count();
                ViewBag.basketCount = basketCount ?? 0;
            }
            ViewBag.customer = customer;
            return View(orders);
        }
        public IActionResult OrderDetail(int orderId)
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return RedirectToAction("Login", "Account", new { path = GetUrl() });
            }
            

            var order = db.Orders.Where(x => x.OrderId == orderId)
                .Include(inc => inc.Addresses).Include(inc => inc.ConsignmentSummaries)
                .Include(inc => inc.CustomerInvoices)
                .Include(inc => inc.DeliveryDetails)
                .Include(inc => inc.ExporterDetails)
                .FirstOrDefault();

            var guestEmail = customer.GuestUser?.Email;
            var customerId = customer.CustomerId;
            var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0
                            && (guestEmail != null ? x.OrderEmail == guestEmail : x.CustomerId == customerId)).Count();
            ViewBag.basketCount = basketCount;

            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> ProceedToPay(int OrderId)
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return RedirectToAction("Login", "Account", new { path = GetUrl() });
            }
            if (customer.Email == "admin@megwayparcels.co.uk" || customer.IsWithoutPayment == true)
            {
                await BookShipment(OrderId);
                return Ok(new { status = "success", orderId = OrderId });
            }
            else
            {
                
                var order = db.Orders.Where(x => x.OrderId == OrderId)
                    .Include(inc => inc.Addresses)
                    .Include(inc => inc.ConsignmentSummaries)
                    .Include(inc => inc.CustomerInvoices)
                    .Include(inc => inc.DeliveryDetails)
                    .Include(inc => inc.ExporterDetails)
                    .FirstOrDefault();
                //var service = JsonConvert.DeserializeObject<ServiceResult>(order.SelectQuote);


                //paymet gateway redierct
                MakeOrderDTO makeOrder = new MakeOrderDTO
                {
                    Amount = (double)order.TotalCharges * 100,
                    OrderRef = order.RefrenceNo,
                    UniqueTransectionId = Guid.NewGuid().ToString()
                };
                var paymentDetails = await _gatewayHelper.Hosted(makeOrder);
                order.PaymentRefrenceNo = makeOrder.UniqueTransectionId;
                db.Orders.Update(order);
                db.SaveChanges();
                return PartialView("_ProceedToPayment", paymentDetails);
            }
        }

        [HttpPost("/PaymentResponse/{RefranceNo}/{TransectionId:Guid}")]
        public async Task<IActionResult> TransectionResponse(string RefranceNo, Guid TransectionId, TransactionResult model)
        {
            
            var order = db.Orders.Where(x => x.RefrenceNo == RefranceNo && x.PaymentRefrenceNo == TransectionId.ToString()).FirstOrDefault();
            if (order == null)
            {
                return RedirectToAction("Login", "Account", new { path = GetUrl() });
            }
            if (model.responseCode == "0")
            {
                order.IsPaymentReceived = true;
                order.PaymentResponse = JsonConvert.SerializeObject(model);
                db.Orders.Update(order);
                db.SaveChanges();
                await BookShipment(order.OrderId);
                PaymentResponse respons = new PaymentResponse
                {
                    OrderId = order.OrderId,
                    TransectionMessage = model.responseMessage,
                    StatusId = 1

                };
                ViewBag.response = respons;
                //return RedirectToAction("PaymentFailed", respons);
                return RedirectToAction("OrderDetail", "Address", new { orderId = order.OrderId });
            }
            else
            {
                PaymentResponse respons = new PaymentResponse
                {
                    OrderId = order.OrderId,
                    TransectionMessage = model.responseMessage,
                    StatusId = 0
                };
                //return RedirectToAction("PaymentFailed", respons);
                ViewBag.response = respons;
                return RedirectToAction("OrderDetail", "Address", new { orderId = order.OrderId });

            }
            return null;

        }
        public IActionResult PaymentFailed(PaymentResponse model)
        {
            ViewBag.basketCount = "";
            return View("PaymentResponse", model);
        }
        public async Task<IActionResult> ReBookOrder(int orderId)
        {

            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return RedirectToAction("Login", "Account", new { path = GetUrl() });
            }
            
            var order = db.Orders.Find(orderId);

            if (order != null && (order.IsPaymentReceived || (customer != null && customer?.Email == "admin@megwayparcels.co.uk")) && order.OrderStatusId != 5)
            {
                await BookShipment(orderId);
                return RedirectToAction("OrderDetail", "Address", new { orderId = orderId });
            }

            return RedirectToAction("Order", "Address");
        }
        public async Task<ResponseVM> GenerateInvoice(Order order, string email, string subject, string body, string source)
        {
            ResponseVM inv = new ResponseVM();
            string logoSrc = $"https://megwayparcels.co.uk/image/logo.JPG";
            if (!string.IsNullOrEmpty(order.RefrenceNo))
            {
                subject = $"{subject}+({order.RefrenceNo})";
            }
            body = $"<div style='text-align:center;'>\r\n<img src='{logoSrc}' alt='Logo' style='max-width: 100%;' />\r\n</div>" + body;
            if (source != "deliver")
            {
                string viewFromAnotherController = await this.RenderViewToStringAsync("/Views/Shared/_Invoice.cshtml", order);
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    string imageFilePath = _hostingEnvironment.WebRootPath + "\\image\\letterhead_new.jpg";
                    var jpg = iTextSharp.text.Image.GetInstance(imageFilePath);

                    //Resize image depend upon your need
                    //For give the size to image
                    jpg.ScaleToFit(3000, 770);

                    //If you want to choose image as background then,
                    jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
                    //If you want to give absolute/specified fix position to image.
                    jpg.SetAbsolutePosition(7, 69);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    pdfDoc.Open();
                    pdfDoc.Add(jpg);
                    var textReader = new StringReader(viewFromAnotherController);
                    htmlparser.Parse(textReader);
                    pdfDoc.Close();

                    byte[] bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    inv = EmailHelper.SendMail(email, subject, body, bytes, true);
                    //System.IO.File.WriteAllBytes("D:\\hello.pdf", bytes);
                }
            }
            else
            {
                inv = EmailHelper.SendMail(email, subject, body, null, true);
            }
            return inv;
        }
        private string EmailBody(Order order, string airWaybillReference, bool isCollection)
        {
            var collection = order.Addresses.Where(x => x.TypeId == 1).FirstOrDefault();
            var delivery = order.Addresses.Where(x => x.TypeId == 3).FirstOrDefault();
            ConsignmentSummary consignment = order.ConsignmentSummaries.FirstOrDefault();
            var resposne = JsonConvert.DeserializeObject<BookShipmentOutput_DTO>(order.OrderResponse);
            string lableHtml = "";
            if (resposne != null && resposne.Labels != null && resposne.Labels?.Count > 0 && isCollection == true)
            {
                string str = "<b>Print your shipping labels below:</b><br/>";
                lableHtml = str + "<table style=\"height: 13px;border-collapse: collapse;\" border=\"1\" width=\"600\"><thead><tr style=\"height: 13px;\"><th style=\"width: 282.438px; height: 13px; text-align: left;\">Type</th><th style=\"width: 196.891px; height: 13px; text-align: left;\">Format</th><th style=\"width: 272.672px; height: 13px; text-align: left;\">Label Size</th><th style=\"width: 270px; height: 13px; text-align: left;\">Download</th></tr></thead><tbody>";
                foreach (var item in resposne.Labels)
                {
                    item.DownloadURL = item.DownloadURL.Replace("http://", "https://");
                    lableHtml += $"<tr style=\"height: 13px;\"><td style=\"width: 282.438px; height: 22px;\">{item.LabelRole}</td><td style=\"width: 196.891px; height: 22px;\">{item.LabelFormat}</td><td style=\"width: 272.672px; height: 22px;\">{item.LabelSize}</td><td style=\"width: 270px; height: 22px;\"><a class=\"lable-download\" href=\"{item.DownloadURL}\" target=\"_blank\" download=\"label.pdf\">Print your label</a></td></tr>";
                }
                lableHtml += "</tbody></table>";
            }
            string res = "";

            var service = JsonConvert.DeserializeObject<ServiceResult>(order.SelectQuote);
            if (isCollection == true)
            {
                res = $"<p>&nbsp;</p><p>Thank you for choosing our parcel booking service! We are delighted to confirm that your parcel has been successfully booked for delivery. Here are the details of your booking: </p>{lableHtml}<p>&nbsp;</p><p>You will find your order reference number on this email. You will be asked for this number for all correspondence with this order.</p><p>&nbsp;</p><p><strong><strong>Receiver&rsquo;s Details:</strong></strong></p><p>Recipient: {delivery.Forename} {delivery.Surname}</p><p>Content: {consignment.ContentSummary}</p><p>Tracking Number: {airWaybillReference}</p><p>Service Name: {service.ServiceName}</p><p>Delivery Address: {delivery.AddressLineOne} {delivery.AddressLineTwo}, {delivery.County}, {delivery.Postcode}</p><p>&nbsp;</p><p>Collection Point:</p><p>{collection.Forename} {collection.Surname}</p><p>{order.RefrenceNo}</p><p>Address: {collection.AddressLineOne} {collection.AddressLineTwo}, {collection.County}, {collection.Postcode}</p><p>&nbsp;</p><p><strong><strong>Your Customs Invoice (Packing List)</strong></strong></p><p>&nbsp;</p><p>Please print off three copies of your Packing List and attach them to your parcel, preferably in an envelope or &ldquo;document enclosed&rdquo; wallet for protection.</p><p>&nbsp;</p><p><strong><strong>Your Shipping Label (Air Waybill)</strong></strong></p><p>&nbsp;</p><p>Please find your Shipping Label for the above order attached. Please use the thermal labels attachment if you are using a thermal label printer.</p><p>&nbsp;</p><p>Please print your Shipping Label and securely attach it to your parcel.</p><p>&nbsp;</p><p>&nbsp;</p><p>&nbsp;</p><p>By placing this order, you have confirmed that you have read and agreed to our Terms and Conditions. The carrier reserves the right to refuse collection of any goods they feel are inadequately or incorrectly packaged.</p><p>This email was sent by Megway Parcels Ltd (Registered No.14245581) registered in the United Kingdom as private limited company and whose Registered Office is at: 1 Abington Square, Northampton, NN1 4AE</p>";
            }
            else
            {
                res = $"<p>&nbsp;</p><p>We are delighted to confirm that your parcel has been successfully booked for delivery. Here are the details of your booking: </p><p>&nbsp;</p><p>You will find your order reference number on this email. You will be asked for this number for all correspondence with this order.</p><p>&nbsp;</p><p><strong><strong>Receiver&rsquo;s Details:</strong></strong></p><p>Recipient: {delivery.Forename} {delivery.Surname}</p><p>Content: {consignment.ContentSummary}</p><p>Tracking Number: {airWaybillReference}</p><p>Service Name: {service.ServiceName}</p><p>Delivery Address: {delivery.AddressLineOne} {delivery.AddressLineTwo}, {delivery.County}, {delivery.Postcode}</p><p>&nbsp;</p><p>This email was sent by Megway Parcels Ltd (Registered No.14245581) registered in the United Kingdom as private limited company and whose Registered Office is at: 1 Abington Square, Northampton, NN1 4AE</p>";
            }
            return res;
        }
        public async Task<IActionResult> ResendEmail(int orderId, string type)
        {
            //LoginVM model = new();


            //model.Email = "megwaylogistics@gmail.com";
            //model.Password = "123";
            //var customer = db.Customers.Where(x => x.Email == model.Email && x.Password == model.Password).FirstOrDefault();
            //if (customer?.CustomerId > 0)
            //{
            //    HttpContext.Session.SetObjectAsJson("LoginObject", customer);
            //}
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return RedirectToAction("Login", "Account", new { path = GetUrl() });
            }
            
            var order = db.Orders.Where(x => x.OrderId == orderId)
                    .Include(inc => inc.Addresses)
                    .Include(inc => inc.ConsignmentSummaries)
                    .Include(inc => inc.CustomerInvoices)
                    .Include(inc => inc.DeliveryDetails)
                    .Include(inc => inc.ExporterDetails)
                    .FirstOrDefault();
            var collectionAddress = order.Addresses.Where(x => x.TypeId == 1).FirstOrDefault();
            var exporterAddress = order.Addresses.Where(x => x.TypeId == 2).FirstOrDefault();
            var deliveryAddress = order.Addresses.Where(x => x.TypeId == 3).FirstOrDefault();

            string airWaybillReference = order.AirWaybillReference;



            if (type == "coll")
            {
                string emailBody = EmailBody(order, airWaybillReference, true);
                string collBody = $"<p><strong>YOUR PARCEL HAS BEEN BOOKED!</strong></p><p>Hi {collectionAddress.Forename} {collectionAddress.Surname},</p>" + emailBody;
                ResponseVM collEmail = await GenerateInvoice(order, collectionAddress.EmailAddress, "Thank you for choosing Megway Parcels", collBody, "");
                if (!string.IsNullOrEmpty(collEmail.Code) && collEmail.Code == "200")
                {
                    order.IsCollectionEmailSent = true;
                }
                else
                {
                    order.IsCollectionEmailSent = false;
                    order.CollectionEmailError = collEmail.Message;
                }
                db.Orders.Update(order);
                db.SaveChanges();
            }
            else if (type == "deli")
            {
                string emailBody = EmailBody(order, airWaybillReference, false);
                string deliveryBody = $"<p><strong>YOUR PARCEL HAS BEEN BOOKED!</strong></p><p>Hi {deliveryAddress.Forename} {deliveryAddress.Surname},</p>" + emailBody;
                ResponseVM deliveryEmail = await GenerateInvoice(order, deliveryAddress.EmailAddress, "Thank you for choosing Megway Parcels", deliveryBody, "deliver");
                if (!string.IsNullOrEmpty(deliveryEmail.Code) && deliveryEmail.Code == "200")
                {
                    order.IsDeliveryEmailSent = true;
                }
                else
                {
                    order.IsDeliveryEmailSent = false;
                    order.DeliveryEmailError = deliveryEmail.Message;
                }
                db.Orders.Update(order);
                db.SaveChanges();
            }
            else if (type == "inv" && !order.IsSameCountry)
            {
                var customerEmail = db.Customers.Where(x => x.CustomerId == order.CustomerId).Select(x => x.Email).FirstOrDefault();
                customerEmail ??= customer?.GuestUser?.Email;
                ResponseVM inv = await GenerateInvoice(order, customerEmail, "Packing List", "", "deliver");
                if (!string.IsNullOrEmpty(inv.Code) && inv.Code == "200")
                {
                    order.IsInvoiceEmailSent = true;
                }
                else
                {
                    order.IsInvoiceEmailSent = false;
                    order.InvoiceEmailError = inv.Message;
                }
                db.Orders.Update(order);
                db.SaveChanges();

            }
            return RedirectToAction("OrderDetail", "Address", new { orderId = order.OrderId });
        }
        //public static string Serialize<T>(T obj)
        //{
        //    var xmlSerializer = new XmlSerializer(typeof(T));
        //    using (var stringWriter = new StringWriterWithEncoding(Encoding.UTF8))
        //    {
        //        xmlSerializer.Serialize(stringWriter, obj);
        //        return stringWriter.ToString();
        //    }
        //}
    }

}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MegwayParcel.Common.Data;
using MegwayParcel.Common.CommonServices;

namespace MegwayParcel.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly LogisticERPContext db;

        public AccountController(LogisticERPContext context)
        {
            db = context;
        }
        public IActionResult Register()
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer != null && customer.Email == "admin@megwayparcels.co.uk")
            {
                ViewBag.IsAdminUser = 1;
            }
            return View();
        }
        public IActionResult Profile(string type = "")
        {

            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return View("Register");
            }
            ViewBag.type = type;
            
            var cust = db.Customers.Find(customer.CustomerId);
            return View(cust);

        }
        public IActionResult Save(Customer model)
        {
            
            var email = db.Customers.FirstOrDefault(x => x.Email == model.Email.Trim());
            if (email != null)
            {
                return Ok(new {status=201, message = "You already have an account." });
            }
            if (model.CustomerId > 0)
            {
                var customer = db.Customers.Find(model.CustomerId);
                customer.AcountName= model.AcountName;
                customer.Title = model.Title;
                customer.ForeName = model.ForeName;
                customer.SurName = model.SurName;
                customer.TelephoneNumber = model.TelephoneNumber;
                customer.MobileNo = model.MobileNo;
                customer.AddTitle = model.AddTitle;
                customer.AddForeName = model.AddForeName;
                customer.AddSurName = model.AddSurName;
                customer.AddLine1 = model.AddLine1;
                customer.AddLine2 = model.AddLine2;
                customer.City = model.City;
                customer.State = model.State;
                customer.IsResidentialAddress = model.IsResidentialAddress;
                customer.Postcode = model.Postcode;
                customer.IsVatRegistered = model.IsVatRegistered;
                customer.MyVatNumber = model.MyVatNumber;
                customer.Eorinumber = model.Eorinumber;

                //Default Address
                customer.DefForename = model.DefForename;
                customer.DefSurname = model.DefSurname;
                customer.DefEmailAddress = model.DefEmailAddress;
                customer.DefTelephoneNumber = model.DefTelephoneNumber;
                customer.DefMobileNumber = model.DefMobileNumber;
                customer.DefCompanyName = model.DefCompanyName;
                customer.DefAddressLineOne = model.DefAddressLineOne;
                customer.DefAddressLineTwo = model.DefAddressLineTwo;
                customer.DefCity = model.DefCity;
                customer.DefCounty = model.DefCounty;
                customer.DefPostcode = model.DefPostcode;
                customer.DefTitle = model.DefTitle;
                db.Customers.Update(customer);
            }
            else
                db.Customers.Add(model);
            db.SaveChanges();
            HttpContext.Session.SetObjectAsJson("LoginObject", model); CompanySetting setting;
            if (model.CompanySettingId > 0)
            {
                setting = db.CompanySettings.Where(x => x.CompanySettingId == model.CompanySettingId).FirstOrDefault();
            }
            else
            {
                setting = db.CompanySettings.Where(x => x.IsActive).FirstOrDefault();
            }
            if (setting != null)
            {
                HttpContext.Session.SetObjectAsJson("Setting", setting);
            }
            return Ok(new {status=200, message = "Account created successfully" });

        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM model, string url = "")
        {
            
            var customer = db.Customers.Where(x => x.Email == model.Email).FirstOrDefault();

            if (customer != null && customer.Password == model.Password && customer?.CustomerId > 0)
            {
                HttpContext.Session.SetObjectAsJson("LoginObject", customer);
                CompanySetting setting;
                if (customer.CompanySettingId > 0)
                {
                    setting = db.CompanySettings.Where(x => x.CompanySettingId == customer.CompanySettingId).FirstOrDefault();

                }
                else
                {
                    setting = db.CompanySettings.Where(x => x.IsActive).FirstOrDefault();

                }
                if (setting != null)
                {
                    HttpContext.Session.SetObjectAsJson("Setting", setting);
                }
                return Ok(new { status = 200, url = url });
            }
            else
            {
                return Ok(new { status = 404 });
            }
        }
        public IActionResult GetLoginObject()
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return Ok(new { status = 404 });
            }
            return Ok(new { status = 200, customer = customer });
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.SetObjectAsJson("LoginObject", null);
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Customer()
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null || customer.Email != "admin@megwayparcels.co.uk")
            {
                return View("Index", "Home");
            }
            ContactCount();
            
            var cust = db.Customers.Include(inc => inc.CompanySetting).ToList();
            ViewBag.settings = db.CompanySettings.ToList();
            return View(cust);
        }
        public IActionResult Order(int id = 0)
        {

            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null || customer.Email != "admin@megwayparcels.co.uk")
            {
                return View("Index", "Home");
            }
            ContactCount();
            
            var order = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && (x.CustomerId == 0 || x.CustomerId == id) && x.ConsignmentSummaries.Count > 0).ToList();

            var custIds = order.Select(x => x.CustomerId).ToArray();
            ViewBag.customer = null;
            if (custIds != null && custIds.Length > 0)
            {
                ViewBag.customer = db.Customers.Where(x => custIds.Contains(x.CustomerId)).Select(m => new { m.CustomerId, CustomerName = $"{customer.ForeName} {customer.SurName}" }).ToList();
            }
            return View(order);
        }
        public IActionResult AssignSetting(int customerId, int? settingId = null)
        {
            if (!(settingId > 0))
            {
                settingId = null;
            }
            
            var customer = db.Customers.Find(customerId);
            customer.CompanySettingId = settingId;
            db.Customers.Update(customer);
            db.SaveChanges();
            return Ok();
        }


        public IActionResult ResetPassword()
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            var model = new LoginVM();
            if (customer != null)
            {
                model.Email = customer.Email;
            }
            return View(model);
        }

        public IActionResult SendResetPassword(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                

                var customer = db.Customers.FirstOrDefault(x => x.Email == email);
                if (customer != null)
                {
                    Random generator = new Random();
                    string r = generator.Next(0, 1000000).ToString("D6");

                    string body = $"<p></p><p>Hi Mr. {customer.ForeName} {customer.SurName},</p><br/>Your Reset Password code is: <strong>{r}</strong>";
                    body += "<p>Kindest Regards,</p><p>&nbsp;</p><p>Megway Parcels Ltd</p><p>&nbsp;</p><p>&nbsp;</p><p>&nbsp;</p><p>By placing this order, you have confirmed that you have read and agreed to our Terms and Conditions. The carrier reserves the right to refuse collection of any goods they feel are inadequately or incorrectly packaged.</p><p>This email was sent by Megway Parcels Ltd (Registered No.14245581) registered in the United Kingdom as private limited company and whose Registered Office is at: 1 Abington Square, Northampton, NN1 4AE</p>";

                    var result = EmailHelper.SendMail(email, "Reset Password Code", body, null, true);
                    if (result.Code == "200")
                    {
                        customer.EmailCode = r;
                        customer.EmailExpiry = DateTime.Now.AddMinutes(10);
                        db.Customers.Update(customer);
                        db.SaveChanges();

                        return Ok(new { status = 200, customer = customer });
                    }
                }
            }
            return Ok(new { status = 404, email, message = "Incorrect Email" });
        }

        public IActionResult ResetPasswordVerifyCode(int id, string email)
        {
            LoginVM model = new LoginVM();
            model.Id = id;
            model.Email = email;
            return View(model);
        }

        public IActionResult SaveResetPasswordVerifyCode(int id, string code)
        {
            
            var customer = db.Customers.FirstOrDefault(x => x.CustomerId == id);
            if (customer != null)
            {
                if (customer.EmailCode == code && DateTime.Now < customer.EmailExpiry)
                {
                    customer.IsEmailConfirmed = true;
                    db.Customers.Update(customer);
                    db.SaveChanges();
                    return Ok(new { status = 200, customer, message = "Code verified", id });
                }
            }
            return Ok(new { status = 404, message = "Incorrect code" });
        }
        public IActionResult SetNewPassword(int id)
        {
            
            var customer = db.Customers.FirstOrDefault(x => x.CustomerId == id);
            if (customer != null && customer.IsEmailConfirmed == true)
            {
                return View(id);
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult SaveNewPassword(int id, string password)
        {
            
            var customer = db.Customers.FirstOrDefault(x => x.CustomerId == id);
            if (customer != null && customer.IsEmailConfirmed == true)
            {
                customer.Password = password;
                customer.ConfirmPassword = password;
                customer.EmailCode = null;
                customer.EmailExpiry = null;
                customer.IsEmailConfirmed = false;
                db.Customers.Update(customer);
                db.SaveChanges();
                return Ok(new { status = 200, message = "New password updated" });

            }
            return Ok(new { status = 404, message = "Incorrect code" });

        }

        [HttpPost]
        public IActionResult VerifyPassword(LoginVM model)
        {
            
            var customer = db.Customers.Where(x => x.CustomerId == model.Id && x.Password == model.Password).FirstOrDefault();
            if (customer?.CustomerId > 0)
            {
                return Ok(new { status = 200 });
            }
            else
            {
                return Ok(new { status = 404 });
            }
        }
        private void ContactCount()
        {
            
            var contactCount = db.ContactUs.Where(x => x.IsRead == false)?.Count();
            ViewBag.contactCount = contactCount ?? 0;
        }

        [HttpPost]
        public IActionResult GuestLogin_old(LoginVM model)
        {
            
            Random generator = new Random();
            string r = generator.Next(0, 1000000).ToString("D6");
            var guest = db.GuestUsers.FirstOrDefault(x => x.Email == model.Email);
            if (guest != null)
            {
                guest.EmailCode = r;
                guest.EmailExpiry = DateTime.Now.AddMinutes(10);
                db.GuestUsers.Update(guest);
            }
            else
            {
                guest = new GuestUser();
                guest.Email = model.Email;
                guest.EmailCode = r;
                guest.EmailExpiry = DateTime.Now.AddMinutes(10);
                db.GuestUsers.Add(guest);
            }
            db.SaveChanges();

            string body = $"<p></p><p>Hi ,</p><br/>Your Reset Password code is: <strong>{r}</strong>";
            body += "<p>Kindest Regards,</p><p>&nbsp;</p><p>Megway Parcels Ltd</p><p>&nbsp;</p><p>&nbsp;</p><p>&nbsp;</p><p>This is your Email verification code.</p><p>This email was sent by Megway Parcels Ltd (Registered No.14245581) registered in the United Kingdom as private limited company and whose Registered Office is at: 1 Abington Square, Northampton, NN1 4AE</p>";

            var result = EmailHelper.SendMail(guest.Email, "Email Verification Code", body, null, true);
            if (result.Code == "200")
            {
                return Ok(new { status = 200, guest = guest });
            }
            return Ok(new { status = 404, guest, message = "Incorrect Email" });
        }


        [HttpPost]
        public IActionResult GuestLogin(LoginVM model)
        {
            

            var guest = db.GuestUsers.FirstOrDefault(x => x.Email == model.Email);
            
            if(guest==null)
            {
                guest = new GuestUser();
                guest.Email = model.Email;
                db.GuestUsers.Add(guest);
            }
            db.SaveChanges();
            var currentDate = DateTime.Now;
            var customer = db.Customers.Where(x => x.AcountName == "Guest User").FirstOrDefault();
            if (customer != null && customer?.CustomerId > 0)
            {
                customer.GuestUser = guest;
                HttpContext.Session.SetObjectAsJson("LoginObject", customer);
                CompanySetting setting;
                if (customer.CompanySettingId > 0)
                {
                    setting = db.CompanySettings.Where(x => x.CompanySettingId == customer.CompanySettingId).FirstOrDefault();
                }
                else
                {
                    setting = db.CompanySettings.Where(x => x.IsActive).FirstOrDefault();
                }
                if (setting != null)
                {
                    HttpContext.Session.SetObjectAsJson("Setting", setting);
                }
                return Ok(new { status = 200 });
            }
            else
            {
                return Ok(new { status = 404 });
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace MegwayParcel.Admin.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
		public static ResponseVM SendMail(string ToEmail, string Subject, string Body, byte[] attachment = null, bool IsHtmlBody = false)
		{
			ResponseVM response = new ResponseVM();
			try
			{

				using (MailMessage mm = new MailMessage("no-reply@megwayparcels.co.uk", ToEmail))
				{
					mm.Subject = Subject;
					mm.Body = Body;
					mm.IsBodyHtml = IsHtmlBody;
					mm.Bcc.Add("megwayparcels.co.uk+7dec114693@invite.trustpilot.com");
					if (attachment != null)
					{
						mm.Attachments.Add(new Attachment(new MemoryStream(attachment), "Invoice.pdf", "application/pdf"));
						//Attachment file = new Attachment(ms, "PdfAttachment.pdf", "application/pdf");
					}

					using (SmtpClient smtp = new SmtpClient())
					{
						smtp.Host = "smtp.office365.com";
						smtp.EnableSsl = true;

						NetworkCredential cred = new NetworkCredential("no-reply@megwayparcels.co.uk", "@ut0web!123");
						smtp.UseDefaultCredentials = false;
						smtp.Credentials = cred;
						smtp.Port = 587;
						smtp.Send(mm);
						response.Code = "200";
						return response;
					}
				}
			}
			catch (Exception ex)
			{
				response.Code = "404";
				response.Message = ex.Message;
				if (ex?.InnerException?.InnerException?.Message != null)
				{
					response.Message = ex?.InnerException?.InnerException?.Message;
				}
				return response;
			}

		}

	}
	public class ResponseVM
	{
		public string Code { get; set; }
		public string Message { get; set; }
		public object Data { get; set; }

	}
}

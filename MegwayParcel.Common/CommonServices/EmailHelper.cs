
using MegwayParcel.Common.Data;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace MegwayParcel.Common.CommonServices
{
    public static class EmailHelper
    {

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
}

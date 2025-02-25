namespace MegwayParcel.Common.TakePaymentGateway
{
    public class TransactionResult
    {
        public string merchantID { get; set; }

        public string transactionID { get; set; }

        public string xref { get; set; }

        public string orderRef { get; set; }

        public string state { get; set; }

        public string transactionUnique { get; set; }

        public string requestID { get; set; }

        public string cv2Check { get; set; }

        public string addressCheck { get; set; }

        public string postcodeCheck { get; set; }

        public string authorisationCode { get; set; }

        public string responseStatus { get; set; }

        public string responseMessage { get; set; }

        public string avscv2ResponseCode { get; set; }

        public string avscv2ResponseMessage { get; set; }

        public string threeDSResponseCode { get; set; }

        public string threeDSResponseMessage { get; set; }

        public string threeDSAuthenticated { get; set; }

        public string cardNumberMask { get; set; }

        public string cardTypeCode { get; set; }

        public string cardExpiryDate { get; set; }

        public string cardExpiryMonth { get; set; }

        public string cardExpiryYear { get; set; }

        public string responseCode { get; set; }

        public string response
        {
            get
            {
                string text = "Unknown response code";
                return responseCode switch
                {
                    "0" => "Successful / authorised transaction.",
                    "1" => "Card referred - Refer to card issuer.",
                    "2" => "Card referred - Special condition.",
                    "4" => "Card declined - Keep card.",
                    "5" => "Card declined.",
                    _ => "Unknown status code",
                };
            }
        }
    }
}

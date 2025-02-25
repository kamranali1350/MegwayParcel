namespace MegwayParcel.Common.TakePaymentGateway
{
    public class MakeOrderDTO
    {
        public double Amount { get; set; }
        public string OrderRef { get; set; }
        public string UniqueTransectionId { get; set; }
    }
}

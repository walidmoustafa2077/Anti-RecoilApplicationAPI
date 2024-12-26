namespace Anti_RecoilApplicationAPI.DTOs
{
    public class PaymentRequestDTO
    {
        public decimal Price { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}

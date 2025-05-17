namespace ECommerce512.Models
{
    public class ApplicationUserOTP
    {
        public int Id { get; set; }
        public int OTP { get; set; }
        public DateTime ReleaseData { get; set; }
        public DateTime ExpirationData { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}

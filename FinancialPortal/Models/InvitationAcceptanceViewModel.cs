namespace FinancialPortal.Models
{
    public class InvitationAcceptanceViewModel
    {
        public int Id { get; set; }
        public string InviteeEmailAddress { get; set; }
        public bool HasAccount { get; set; }
    }
}
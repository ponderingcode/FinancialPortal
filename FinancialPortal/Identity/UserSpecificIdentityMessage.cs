using Microsoft.AspNet.Identity;

namespace FinancialPortal.Identity
{
    public class UserSpecificIdentityMessage : IdentityMessage
    {
        //
        // Summary:
        //     Origin, i.e. From email, or SMS phone number
        public virtual string Origin { get; set; }
    }
}
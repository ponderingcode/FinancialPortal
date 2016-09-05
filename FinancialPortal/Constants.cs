namespace FinancialPortal
{
    class ApplicationRole
    {
        public const string ADMINISTRATOR = "Administrator";
        public const string PRIVILEGED_USER = "PrivilegedUser";
        public const string STANDARD_USER = "StandardUser";
    }

    class AccountRole
    {
        public const string ACCOUNT_HOLDER = "AccountHolder";
        public const string JOINT_ACCOUNT_HOLDER = "Joint" + ACCOUNT_HOLDER;
        public const string PRINCIPAL = "Principal";
        public const string SIGNATORY = "Signatory";
    }

    class AccountType
    {
        public const string JOINT_TENANCY = "JointTenancy";
        public const string CONVENIENCE = "Convenience";
    }

    class HouseholdRole
    {
        public const string HEAD_OF_HOUSEHOLD = "HeadOfHousehold";
        public const string MEMBER = "Member";
    }
}
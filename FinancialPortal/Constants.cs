namespace FinancialPortal
{
    public static class Common
    {
        public const string ACTIVE = "active";
        public const string ID = "Id";
        public const string TITLE = "Title";
        public const string DESCRIPTION = "Description";
        public const string NAME = "Name";
        public const string UPDATED = "Updated";
        public const string USER_NAME = "UserName";
        public const string MULTIPART_FORM_DATA = "Multipart/form-data";
    }

    public static class ActionName
    {
        public const string ARCHIVE = "Archive";
        public const string ATTACHMENT_DETAILS = "AttachmentDetails";
        public const string CHANGE_PROFILE_INFO = "ChangeProfileInfo";
        public const string CHANGE_PASSWORD = "ChangePassword";
        public const string CREATE = "Create";
        public const string DELETE = "Delete";
        public const string DELETE_ATTACHMENT = "DeleteAttachment";
        public const string DETAILS = "Details";
        public const string EDIT = "Edit";
        public const string INDEX = "Index";
        public const string INVITATION_ACCEPTANCE = "InvitationAcceptance";
        public const string INVITE = "Invite";
        public const string LOGIN = "Login";
        public const string LOGIN_AS_HEAD_OF_HOUSEHOLD = "LoginAsHeadOfHousehold";
        public const string LOGIN_AS_JILL = "LoginAsJill";
        public const string LOGIN_AS_ERIC = "LoginAsEric";
        public const string LOGIN_AS_LINDA = "LoginAsLinda";
        public const string LOG_OFF = "LogOff";
        public const string SEND_INVITATION = "SendInvitation";
        public const string SET_PASSWORD = "SetPassword";
    }

    public static class ControllerName
    {
        public const string ACCOUNT = "Account";
        public const string HOME = "Home";
        public const string MANAGE = "Manage";
        public const string HOUSEHOLDS = "Households";
        public const string ROLES = "Roles";
    }

    public static class PartialViewName
    {
    }

    public static class AccountRoleName
    {
        public const string ACCOUNT_HOLDER = "AccountHolder";
        public const string JOINT_ACCOUNT_HOLDER = "Joint" + ACCOUNT_HOLDER;
        public const string PRINCIPAL = "Principal";
        public const string SIGNATORY = "Signatory";
    }

    public static class AccountTypeName
    {
        public const string JOINT_TENANCY = "JointTenancy";
        public const string CONVENIENCE = "Convenience";
    }

    public static class HouseholdRoleName
    {
        public const string HEAD_OF_HOUSEHOLD = "HeadOfHousehold";
        public const string MEMBER = "Member";
        public const string NONE = "None";
    }
}
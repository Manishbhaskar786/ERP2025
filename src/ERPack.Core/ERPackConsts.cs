using ERPack.Debugging;

namespace ERPack
{
    public static class ERPackConsts
    {
        public const string LocalizationSourceName = "ERPack";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;

        public const string BaseDirectory = @"wwwroot\uploads\";

        public const string Enquiry = "Enquiry";

        #region Status
        public const string Design = "Design";
        public const string Estimate = "Estimate";
        public const string SentForApproval = "Sent For Approval";
        public const string InProgress = "In Progress";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
        public const string Production = "Production";
        public const string Clarification = "Clarification";
        public const string Checklist = "Checklist";
        public const string StockRequested = "StockRequested";
        public const string StockReceived = "StockReceived";
        public const string Completed = "Completed";
        #endregion


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "5844f5bcc4fa4f89973b7e443e64ba87";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utilities
{
    public static class SD
    {
        public const string Role_User_Customer = "Customer";
        public const string Role_User_Company = "Company";
        public const string Role_User_Admin = "Admin";
        public const string Role_User_Employee = "Employee ";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatisShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDeleayedPayment = "ApprovedForDelayedPayment";
        public const string PaymentStatusRejected = "Rejected";


    }
}

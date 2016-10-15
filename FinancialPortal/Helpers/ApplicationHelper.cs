using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinancialPortal.Models;

namespace FinancialPortal.Helpers
{
    public static class ApplicationHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static decimal bankAccountsAggregateBalance { get; set; }
        private static bool householdExists
        {
            get
            {
                return 0 < db.Households.ToList().Count;
            }
        }

        public static int HouseholdId
        {
            get
            {
                return householdExists ? db.Households.ToList().FirstOrDefault().Id : -1;
            }
        }

        public static int BudgetId
        {
            get
            {
                return db.Budgets.ToList().FirstOrDefault().Id;
            }
        }

        public static void AssociateAllBankAccountsWithHousehold()
        {
            if (0 < db.BankAccounts.ToList().Count)
            {
                foreach (BankAccount bankAccount in db.BankAccounts.ToList())
                {
                    bankAccount.HouseholdId = HouseholdId;
                }
                db.SaveChanges();
            }
        }

        public static decimal BankAccountsAggregateBalance
        {
            get
            {
                bankAccountsAggregateBalance = 0;
                if (0 < db.BankAccounts.ToList().Count)
                {
                    foreach (BankAccount bankAccount in db.BankAccounts.ToList())
                    {
                        bankAccountsAggregateBalance += bankAccount.Balance;
                    }
                }
                return bankAccountsAggregateBalance;
            }
        }

        public static void AssociateAllBudgetsWithHousehold()
        {
            if (0 < db.Budgets.ToList().Count)
            {
                foreach (Budget budget in db.Budgets.ToList())
                {
                    budget.HouseholdId = HouseholdId;
                }
                db.SaveChanges();
            }
        }
    }
}
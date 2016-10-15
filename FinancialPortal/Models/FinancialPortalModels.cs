using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialPortal.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        [Display(Name = "Reconcile Balance")]
        public decimal ReconcileBalance { get; set; }
    }

    public class Budget
    {
        public Budget()
        {
            BudgetItems = new HashSet<BudgetItem>();
        }

        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string Name { get; set; }
        [Display(Name = "Annual Limit")]
        public decimal LimitAnnual
        {
            get
            {
                return LimitMonthly * 12;
            }
        }
        [Display(Name = "Monthly Limit")]
        public decimal LimitMonthly { get; set; }
        [Display(Name = "Pay Period Limit")]
        public decimal LimitPerPaycheck
        {
            get
            {
                return LimitMonthly / 2;
            }
        }
        [Display(Name = "Weekly Limit")]
        public decimal LimitWeekly
        {
            get
            {
                return LimitMonthly * 12 / 52;
            }
        }
        [Display(Name = "Daily Limit")]
        public decimal LimitDaily
        {
            get
            {
                return LimitMonthly * 12 / 365;
            }
        }
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
    }

    public class BudgetItem
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; } 
    }

    //public class Category
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}

    public class Category
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string Name { get; set; }
    }

    public class CategoryHousehold
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
    }

    public class Household
    {
        public Household()
        {
            Members = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Annual Income")]
        public decimal IncomeAnnual { get; set; }
        [Display(Name = "Monthly Income")]
        public decimal IncomeMonthly
        {
            get
            {
                return IncomeAnnual / 12;
            }
        }
        [Display(Name = "Paycheck Amount")]
        public decimal IncomePerPayPeriod
        {
            get
            {
                return IncomeAnnual / 24;
            }
        }
        [Display(Name = "Weekly Income")]
        public decimal IncomeWeekly
        {
            get
            {
                return IncomeAnnual / 52;
            }
        }
        [Display(Name = "Daily Income")]
        public decimal IncomeDaily
        {
            get
            {
                return IncomeAnnual / 365;
            }
        }
        public ApplicationUser HeadOfHousehold { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
    }

    public class Transaction
    {
        public int Id { get; set; }
        [Display(Name = "Bank Account")]
        public int BankAccountId { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Entered By")]
        public string EnteredById { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        [Display(Name = "Reconciled Amount")]
        public decimal ReconciledAmount { get; set; }
        [Display(Name = "Reconciled?")]
        public bool Reconciled { get; set; }
    }
}
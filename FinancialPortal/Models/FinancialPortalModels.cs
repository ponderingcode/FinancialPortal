using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialPortal.Models
{
    public class Budget
    {
        public Budget()
        {
            BudgetItems = new HashSet<BudgetItem>();
        }

        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string Name { get; set; }
        public decimal LimitAmount { get; set; }
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
    }

    public class BudgetItem
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; } 
    }

    public class Household
    {
        public Household()
        {
            Members = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ApplicationUser HeadOfHousehold { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CategoryHousehold
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
    }


    public class Account
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal ReconcileBalance { get; set; }
    }

    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public string EnteredById { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public decimal ReconciledAmount { get; set; }
        public bool Reconciled { get; set; }
    }
}
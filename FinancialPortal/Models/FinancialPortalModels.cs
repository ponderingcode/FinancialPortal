using System;

namespace FinancialPortal.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string Name { get; set; }
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
        public int Id { get; set; }
        public string Name { get; set; }
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
        public int EnteredById { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public decimal ReconciledAmount { get; set; }
        public bool Reconciled { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FinancialPortal.Models
{
    public class TransactionViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Bank Account")]
        public string SelectedBankAccount { get; set; }
        [Display(Name = "Category")]
        public string SelectedCategory { get; set; }
        public SelectList BankAccounts { get; set; }
        public SelectList Categories { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        [Display(Name = "Reconciled Amount")]
        public decimal ReconciledAmount { get; set; }
        public bool Reconciled { get; set; }
}
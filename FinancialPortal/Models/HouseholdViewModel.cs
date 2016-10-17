using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FinancialPortal.Models
{
    public class HouseholdViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Head of Household")]
        public string HeadOfHousehold { get; set; }
        [Display(Name = "Annual Income")]
        public decimal IncomeAnnual { get; set; }
        public MultiSelectList Members { get; set; }
        public List<string> SelectedMembers { get; set; }
    }
}
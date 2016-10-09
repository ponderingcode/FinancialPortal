using System.Collections.Generic;
using System.Web.Mvc;

namespace FinancialPortal.Models
{
    public class HouseholdViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MultiSelectList Members { get; set; }
        public List<string> SelectedMembers { get; set; }
    }
}
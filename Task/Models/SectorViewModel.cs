using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HTask.Models
{
    public class SectorViewModel
    {

        [StringLength(60, MinimumLength = 2)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Required]
        public string Name { get; set;}

        [Display(Name="Agree to terms")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Must be checked")]
        public bool TermsAndConditions { get; set;}
        [Display(Name="Sectors:")]
        public List<CheckBoxListItem> Sectors { get; set;}
    }
}

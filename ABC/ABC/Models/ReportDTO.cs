using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABC.Models
{
    public class ReportDTO : IValidatableObject
    {
        [Required]
        [Display(Name = "Report Name")]
        public string name { get; set; }
        [Required]
        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        public DateTime fromDate { get; set; }

        [Required]
        [Display(Name = "To Date")]
        [DataType(DataType.Date)]

        public DateTime toDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();
            if (toDate < fromDate)
            {
                ValidationResult mss = new ValidationResult(
                                    errorMessage: "EndDate must be greater than StartDate",
                                    memberNames: new[] { "toDate" }
                               );
                res.Add(mss);
            }
            return res;
        }
    }

}
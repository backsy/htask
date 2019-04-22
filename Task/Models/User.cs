using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HTask.Models
{
    public class User
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 2)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Required]
        public string UserName { get; set; }

        [Required]
        public bool AgreeToTerms { get; set;}

        [Required]
        public string SessionID { get; set; }

        public ICollection<UserSector> UserSectors { get; set;}
    }
}

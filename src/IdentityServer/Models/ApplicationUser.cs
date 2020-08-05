using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string ArabicFirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string ArabicMiddleName { get; set; }

        [StringLength(50)]
        public string ArabicGrandName { get; set; }

        [Required]
        [StringLength(50)]
        public string ArabicFamilyName { get; set; }

        [Required]
        [StringLength(50)]
        public string EnglishFirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string EnglishMiddleName { get; set; }

        [StringLength(50)]
        public string EnglishGrandName { get; set; }

        [Required]
        [StringLength(50)]
        public string EnglishFamilyName { get; set; }

        [Required]
        public bool IsMale { get; set; }

        [StringLength(200)]
        public string Nationality { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        public string ProfileImage { get; set; }
        //public bool IsAdmin { get; internal set; }
    }
}

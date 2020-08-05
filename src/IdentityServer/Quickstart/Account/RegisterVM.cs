using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerHost.Quickstart.UI
{
    public class RegisterVM
    {
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }

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
        [DataType(DataType.DateTime)]
        public DateTime Birthdate { get; set; }

        public string ProfileImage { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}

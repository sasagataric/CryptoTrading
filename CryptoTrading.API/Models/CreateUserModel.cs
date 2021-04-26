using CryptoTrading.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTrading.API.Models
{
    public class CreateUserModel
    {
        [Required]
        [StringLength(50, ErrorMessage = Messages.USER_USERNAME_REQUIRED)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = Messages.USER_NAME_REQUIRED)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = Messages.USER_LASTNAME_REQUIRED)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = Messages.USER_EMAIL_REQUIRED)]
        public string Email { get; set; }


    }
}

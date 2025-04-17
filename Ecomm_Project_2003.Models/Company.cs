using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_Project_2003.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name ="Street Addresss")] 
        public string StreetAddress { get; set; }
        public string City { get; set; }    
        public String State { get; set; }
        [Display(Name ="Postal Code")]
        public string PostalCode { get; set; }
        [Display( Name="Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Is Authorized Company")]
        public bool IsAuthorizedCompany { get; set; } = false;
    }
}

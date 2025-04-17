using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_Project_2003.Models.ViewModels
{
    public class ShoopingCartVM
    {
        public IEnumerable<ShoopingCart> ListCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
        }

    }


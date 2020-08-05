using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TextGallery.Client
{
    public class OrderAddressViewModel
    {
        public string Address { get; set; }= string.Empty;

        public OrderAddressViewModel( string address)
        {
            Address = address;
        } 


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelAPI.Models
{
    public class Hotel
    {
        public String Id { get; set; }

        public String Name { get; set; }

        public String Address { get; set; }

        public String GeoCode { get; set; }
    }
}
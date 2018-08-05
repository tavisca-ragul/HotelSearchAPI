using HotelAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HotelAPI.Controllers
{
    public class HotelController : ApiController
    {
        private String _address, _lat, _lng;
        private dynamic _response;
        private static List<Hotel> _hotels = new List<Hotel>();

        public HotelController()
        {
            _address = "";
            _response = "";
        }

        public dynamic sendRequest(String uri)
        {
            using (var client = new WebClient())
            {
                return JsonConvert.DeserializeObject(client.DownloadString(uri).ToString());
            }
        }

        public IHttpActionResult GetHotelsSearch(String address)
        { 
            var client = new WebClient();
            var hotelAddress = "";
            _response = sendRequest("https://autocomplete.geocoder.cit.api.here.com/6.2/suggest.json?query="+address+"&&app_id=xWyJAJbwxCsixyUso1gA&app_code=Oxd1esiMMAd_ghiCAOewRQ");
            
            try
            {
                _address = _response.suggestions[0].label;
            } catch(Exception ex)
            {
                return Content(HttpStatusCode.NotFound, "Unable find the address");
            }
            _response = sendRequest("https://api.opencagedata.com/geocode/v1/json?q="+_address+"&key=39572c8b627b4c9e907b69941859c62c&limit=1");
            _lat = _response.results[0].geometry.lat;
            _lng = _response.results[0].geometry.lng;
            _response = sendRequest("https://api.foursquare.com/v2/venues/search?ll="+_lat+","+_lng+"&query=hotel&client_id=IDAD2DPCRKFK53XT5SNUX4BXZKSDKPMG2AEMKVME5B4WPCS2&client_secret=IN220TUS0CLYL3ILVCWP452POBAUB13HWEAMMLYTXU4OXYCE&v=20180805&radius=10000");
            foreach (var venue in _response.response.venues)
            {
                foreach (var addressLine in venue.location.formattedAddress)
                    hotelAddress += addressLine+", ";
                _hotels.Add(
                    new Hotel()
                    {
                        Id = venue.id,
                        Name = venue.name,
                        Address = hotelAddress,
                        GeoCode = venue.location.lat+","+venue.location.lng
                    });
            }
            return Ok(_hotels);
        }
    }
}
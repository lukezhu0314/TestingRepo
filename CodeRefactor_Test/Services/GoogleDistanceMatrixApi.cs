using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using Newtonsoft.Json;
using CodeRefactor_Test.Models.DistanceModel;
using CodeRefactor_Test.Models.HospitalModel;    

namespace CodeRefactor_Test.Services
{
    public class GoogleDistanceMatrixApi
    {
         public const string DISTANCE_MATRIX_URL = "https://maps.googleapis.com/maps/api/distancematrix/json";

        //-----Alan's key
        //public const string DISTANCE_MATRIX_KEY = "AIzaSyArYjfVBTSnc0YKHg1iZgglUdINSMmD6a0";
        //-----Michael's key
        //public const string DISTANCE_MATRIX_KEY = "AIzaSyDIHfQu3-FcuXH47lRRVgmrzmthIFijUDQ";
        //-----DESTINE key
        public const string DISTANCE_MATRIX_KEY = "AIzaSyAMhRYs7DX9NENrnYMAfd8PfQJ3F4EY7eY";

        private string apiKey { get; set; }
        private string apiUrl { get; set; }
        private string url { get; set; }

        private IEnumerable<string> originAddresses { get; set; }
        private IEnumerable<string> destinationAddresses { get; set; }

        public GoogleDistanceMatrixApi(GeoCoordinate origin, List<string> destinationAddresses)
        {
            SetupDefaultKeys();
            SetupApi(new List<string> { origin.ToString() }, destinationAddresses);
        }

        public GoogleDistanceMatrixApi(Hospital origin, Hospital destination)
        {
            SetupDefaultKeys();
            SetupApi(new List<string> { origin.latitude.ToString() + ',' + origin.longitude.ToString() },
                     new List<string> { destination.latitude.ToString() + ',' + destination.longitude.ToString() });
        }

        private void SetupDefaultKeys()
        {
            // Fetch url/key from app settings
            this.apiKey = GoogleDistanceMatrixApi.DISTANCE_MATRIX_KEY;
            this.apiUrl = GoogleDistanceMatrixApi.DISTANCE_MATRIX_URL;
        }

        private void SetupApi(IEnumerable<string> originAddresses, IEnumerable<string> destinationAddresses)
        {
            this.originAddresses = originAddresses;
            this.destinationAddresses = destinationAddresses;
        }

        public async Task<ResponseDistanceMatrix> GetResponse()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(GetRequestUrl());
                
                HttpResponseMessage response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("GoogleDistanceMatrixApi failed with status code: " + response.StatusCode);
                }

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseDistanceMatrix>(content);                
            }
        }

        private string GetRequestUrl()
        {
            var originsUrl = string.Join("|", originAddresses.Select(WebUtility.UrlEncode));
            var destinationsUrl = string.Join("|", destinationAddresses.Select(WebUtility.UrlEncode));
            var departureTimeUrl = ((47 * 365 * 24 * 60 * 60) + (11 * 24 * 60 * 60) + (181 * 24 * 60 * 60) + (5 * 60 * 60) + (8 * 7 * 24 * 60 * 60)).ToString();
            string uri = $"{apiUrl}?departure_time=now&traffic_model=optimistic&origins={originsUrl}&destinations={destinationsUrl}&key={apiKey}";
            return uri;
            //&key={apiKey}"
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeRefactor_Test.Models.DistanceModel;
using CodeRefactor_Test.Models.HospitalModel;
using CodeRefactor_Test.Models.ProbabilityModel;
using GeoCoordinatePortable;

namespace CodeRefactor_Test.Services
{
    public class TimeService
    {
        /**********************************************************/
        /*                     Data Structures                    */
        /**********************************************************/
        public enum Method { UND = 1, DS, MS, DNE }

        public enum MatrixType { ORIGIN_TO_HOSPITALS, PSC_TO_CSC }

        public bool UND = false;

        /**********************************************************/
        /*                  Instance Variables                    */
        /**********************************************************/
        private IEnumerable<Hospital> cscHospitals { get; set; }
        private IEnumerable<Hospital> pscHospitals { get; set; }

        public TimeService(List<Hospital> hospitals)
        {
            // Split up the CSC and PSC hospitals
            this.cscHospitals = hospitals.Where((hospital) => hospital.type == HospitalType.CSC);
            this.pscHospitals = hospitals.Where((hospital) => hospital.type == HospitalType.PSC);
        }

        public List<string> GenerateResponseFeature(Centroid centroid)
        {
            List<string> ClosestHospitals = new List<string>();
            
            // Generate origin coordinates
            var originCoordinates = new GeoCoordinate(centroid.Lat, centroid.Long);

            //find closest PSC
            var closestPSC = GetClosestHospital(originCoordinates, HospitalType.PSC);

            //find closest CSC
            var closestCSC = GetClosestHospital(originCoordinates, HospitalType.CSC);

            // Calculate probabilities of good outcome for each method and return the best method with its probability
            //ResponseFeature best_method = await GetProbability(closestPSC, closestCSC, originCoordinates);

            ClosestHospitals.Add(closestPSC.name);
            ClosestHospitals.Add(closestCSC.name);

            return ClosestHospitals;
        }
        
        public async Task<Dictionary<MatrixType, ResponseDistanceMatrix>> GetTravelTimes(List<string>HospitalNames, GeoCoordinate point)
        {
            /*
            List<string> dnsHospitals = new List<string>();
            dnsHospitals.Add(cscHospital.name);
            dnsHospitals.Add(pscHospital.name);
            */

            // get travel times for origin to CSC, origin to PSC, and PSC to CSC
            GoogleDistanceMatrixApi originToHospitalAPI = new GoogleDistanceMatrixApi(point, HospitalNames);
            //GoogleDistanceMatrixApi pscToCscAPI = new GoogleDistanceMatrixApi(pscHospital, cscHospital);

            // get travel times from origin to CSC and from origin to PSC
            ResponseDistanceMatrix originToHospitalTimes = await originToHospitalAPI.GetResponse();

            // get travel times from PSC to CSC
            //ResponseDistanceMatrix pscToCscTimes = await pscToCscAPI.GetResponse();

            return new Dictionary<MatrixType, ResponseDistanceMatrix>
            {
                { MatrixType.ORIGIN_TO_HOSPITALS, originToHospitalTimes },
                //{ MatrixType.PSC_TO_CSC, pscToCscTimes }
            };
        }

        public Hospital GetClosestHospital(GeoCoordinate point, HospitalType type)
        {
            // Decide hospitals to use
            IEnumerable<Hospital> hospitals = null;
            switch (type)
            {
                case HospitalType.CSC:
                    hospitals = this.cscHospitals;
                    break;
                
                case HospitalType.PSC:
                    hospitals = this.pscHospitals;
                    break;
                
                default:
                    throw new ArgumentException("This is not a valid hospital type");
            }

            Hospital closestHospital = null;
            double closestDistance = -1;

            foreach (var hospital in hospitals)
            {
                // If first, set this hospital as closest
                if (closestHospital == null)
                {
                    SetClosestHospital(ref closestHospital, ref closestDistance, hospital, GetHospitalDistance(hospital, point));
                    continue;
                }

                // Get distance of hospital from origin
                var distance = GetHospitalDistance(hospital, point);

                if (distance < closestDistance)
                {
                    SetClosestHospital(ref closestHospital, ref closestDistance, hospital, distance);
                }
            }

            return closestHospital;
        }

        private void SetClosestHospital(ref Hospital closestHospital, ref double closestDistance, Hospital targetHospital, double targetDistance)
        {
            closestHospital = targetHospital;
            closestDistance = targetDistance;
        }

        private double GetHospitalDistance(Hospital hospital, GeoCoordinate point)
        {
            return point.GetDistanceTo(new GeoCoordinate(hospital.latitude, hospital.longitude));
        }        
    }
}
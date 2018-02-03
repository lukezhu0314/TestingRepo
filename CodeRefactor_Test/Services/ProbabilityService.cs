using System;
using CodeRefactor_Test.Models.DistanceModel;
using CodeRefactor_Test.Models.ProbabilityModel;
using CodeRefactor_Test.Models.TimeModel;
using static CodeRefactor_Test.Services.TimeService;

namespace CodeRefactor_Test.Services
{
    public class ProbabilityService
    {

        /**********************************************************/
        /*                     Data Structures                    */
        /**********************************************************/
        public enum Method { UND = 1, DS, MS, DNE }

        public bool UND = false;

        /**********************************************************/
        /*                     Constants                          */
        /**********************************************************/

        /** Mothership Door-to-Reperfusion time (DTRP) in minutes */
        private double msDoorToReperfusion = 115;

        /** Mothership Door-to-Needle time (DTN) in minutes */
        // TODO const
        private double msDoorToNeedle = 30;

        /** Drip n' Ship DTRP in minutes */
        private double dnsDtrp = 115;

        /** Drip n' Ship DTN in minutes  */
        private double dnsDoorToNeedle = 60;

        /** Drip n' Ship Needle-to-Door time (NTD) in minutes */
        private double dnsNeedleToDoor = 60;

        /** Onset-to-First-Medical-Response time (OFMR) in minutes */
        private double onsetFirstMedical = 60;

        /** Scene-Time (ST) in minutes */
        private double sceneTime = 25;

        public ResponseFeature GetProbability(TravelTime travelTime)
        {
            ResponseFeature response;
            UND = false;

            if (travelTime.DnS_PscTime == null || travelTime.MS_CscTime == null || travelTime.DnS_Psc_CscTime == null)
            {
                response = new ResponseFeature
                {
                    probability = 0,
                    method = Method.DNE.ToString(),
                };
                return response;
            }

            // Determines which method is better then return that method and its probability
            double travel_PSC = (double)travelTime.DnS_PscTime / 60;  // travel time from origin to CSC
            double travel_CSC = (double)travelTime.MS_CscTime / 60;  // travel time from origin to PSC
            double travel_PSC_CSC = (double)travelTime.DnS_Psc_CscTime/60;  //travel time from PSC to CSC

            double msProbability; // Mothership's probability of good outcome
            double dnsProbability; // Drip n' Ship's probability of good outcome
            Method method; // Determined best method
            double probability;   // Determined best method's probability of good outcome

            // Mothership Good Outcome Probability Calculations
            double t1 = onsetFirstMedical + sceneTime + travel_CSC + msDoorToNeedle; // Time to drug treatment
            double t2 = onsetFirstMedical + sceneTime + travel_CSC + msDoorToReperfusion;  // Time to reperfusion

            if (t1 < 4.5 * 60)
            {
                if (ProbEVT(t2) < 0)
                    msProbability = ProbTPA(t1);
                else
                    msProbability = ProbTPA(t1) + (1 - ProbTPA(t1)) * ProbEVT(t2);
            }
            else
            {
                if (ProbEVT(t2) < 0)
                {
                    msProbability = 0;
                }
                else
                {
                    msProbability = ProbEVT(t2);
                }
            }

            // Drip n' Ship Good Outcome Probability Calculations
            t1 = onsetFirstMedical + sceneTime + travel_PSC + dnsDoorToNeedle;   // Time to drug treatment
            t2 = t1 + dnsNeedleToDoor + travel_PSC_CSC + dnsDtrp; //Time to reperfusion

            if (t1 < 4.5 * 60)
            {
                if (ProbEVT(t2) < 0)
                {
                    dnsProbability = ProbTPA(t1);
                }
                else
                {
                    dnsProbability = ProbTPA(t1) + (1 - ProbTPA(t1)) * ProbEVT(t2);
                }
            }
            else
            {
                if (ProbEVT(t2) < 0)
                {
                    dnsProbability = 0;
                }
                else
                {
                    dnsProbability = ProbEVT(t2);
                }
            }

            // Setting probability of good outcome to minimum value if lower than it
            if (dnsProbability < 0.1578)
            {
                dnsProbability = 0.1578;
            }
            if (msProbability < 0.1578)
            {
                msProbability = 0.1578;
            }

            // Difference between good outcome probabilities of both approaches
            double difference = Math.Abs(msProbability - dnsProbability);

            // Determine if area in the undefined zone
            if (difference <= 0.01 && !(travel_CSC == 0 && travel_PSC == 0 && travel_PSC_CSC == 0))
            {
                UND = true;
                probability = (dnsProbability + msProbability) / 2;
            }
            // Assign best method
            if (dnsProbability > msProbability)
            {
                method = Method.DS;
                probability = dnsProbability;
            }
            else if (msProbability > dnsProbability)
            {
                method = Method.MS;
                probability = msProbability;
            }
            else if (travel_CSC == 0 && travel_PSC == 0 && travel_PSC_CSC == 0)
            {
                method = Method.DNE;
                probability = 0.1578;
            }
            else if (msProbability == 0.1578 && dnsProbability == 0.1578)
            {
                if (travel_CSC < travel_PSC)
                    method = Method.MS;
                else
                    method = Method.DS;
                probability = 0.1578;
            }
            else
            {
                method = Method.DNE;
                probability = 0.1578;
            }

            response = new ResponseFeature
            {
                undefined = UND,
                probability = probability,
                method = method.ToString(),
            };


            return response;
        }

        public double ProbEVT(double time) 
        {
            // Calculates probability of good outcome for EVT as a function of time (uses old formulae)
            return (Math.Exp(-0.0019 * time + 0.7239)) / (1 + Math.Exp(-0.0019 * time + 0.7239));
        }

        public double ProbTPA(double time)
        {
            // Calculates probability of good outcome for TPa as a function of time (uses old formulae)
            return (0.0703 * time + 41.02) / (time + 113.7);
        }
    }
}
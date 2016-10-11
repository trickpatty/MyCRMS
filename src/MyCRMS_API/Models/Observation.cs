using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCRMS_API.Models
{
    public class Observation
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public string ObservedRed { get; set; }
        public int ObservedNumber { get; set; }
        public Letter ObservedLetter { get; set; }
        public int Multiplier { get; set; }
        public DateTime Date { get; set; }
        public bool Interc { get; set; }
        
        public bool IsPeakType()
        {
            if (ObservedNumber == 10 ||
                ObservedLetter == Letter.K ||
                ObservedLetter == Letter.L)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsNonPeakMucus()
        {
            if(ObservedNumber == 6 || ObservedNumber == 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

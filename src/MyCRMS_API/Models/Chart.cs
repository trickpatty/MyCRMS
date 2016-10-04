using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCRMS_API.Models
{
    public class Chart
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }
        public virtual ICollection<Observation> Observations { get; set; }
        public ICollection<YellowBabyRule> YellowBabyRules { get; set; }

        public Observation GetMostFertileOnDate(DateTime date)
        {
            var observationsOnDate = Observations
                .Where(obs => obs.Date.Date == date.Date)
                .OrderBy(obs => obs.ObservedNumber);
            if (observationsOnDate.Any())
            {
                var observationsClearOrLubracitive = observationsOnDate
                    .Where(obs => obs.ObservedLetter == (Letter.K) || obs.ObservedLetter == Letter.L);
                if (observationsClearOrLubracitive.Any())
                {
                    return observationsClearOrLubracitive.First();
                }
                else
                {
                    return observationsOnDate.First();
                }
            }
            else
            {
                return null;
            }
        }

        public Stamp CalculateStamp(DateTime date)
        {
            Observation obs = GetMostFertileOnDate(date);
            
            Color stampColor = CalculateColor(obs);
            bool stampBaby = CalculateBaby(obs);
            int stampPCount = CalculatePCount(obs);
            return new Stamp(stampColor, stampBaby, stampPCount);
        }

        private int CalculatePCount(Observation obs)
        {
            Observation tomorrowsObs = GetMostFertileOnDate(obs.Date.AddDays(1));

            // Without data from the day before, we cannot compute the count
            if (tomorrowsObs == null)
            {
                return -1;
            }

            // D4: 3 or more days of non-Peak mucus pre-Peak - plus count 3
            if(GetPreviousObservations(obs, 3).All(prevObs => prevObs.IsNonPeakMucus()) && !tomorrowsObs.IsNonPeakMucus())
            {
                return 0;
            }

            // If today is a peak day, check to see if it's THE peak day
            // D5 - Any single day of Peak mucus - plus 3 count
            if (obs.IsPeakType() && !tomorrowsObs.IsPeakType())
            {
                return 0;
            }
            



            return -1;
        }

        private bool CalculateBaby(Observation obs)
        {
            Color color = CalculateColor(obs);
            switch (color)
            {
                case Color.Green:
                    // if PCount > 0 then true
                    // else false
                    return false;
                case Color.Red:
                    return false;
                case Color.White:
                    return true;
                case Color.Yellow:
                    return false;
                default:
                    return true;
            }
        }

        private Color CalculateColor(Observation obs)
        {
            //TODO: Include calculation for matching YBR
            if (obs.ObservedRed != "")
            {
                return Color.Red;
            }
            else if (obs.ObservedNumber == 10 || obs.ObservedLetter == Letter.K || obs.ObservedLetter == Letter.L)
            {
                return Color.White;
            }
            else if (obs.ObservedNumber <= 4)
            {
                return Color.Green;
            }
            else
            {
                return Color.White;
            }
        }

        private IEnumerable<Observation> GetPreviousObservations(Observation obs, int previous)
        {
            if(previous == 0 || obs == null)
            {
                return new List<Observation>() { obs };
            }
            Observation previousObs = GetMostFertileOnDate(obs.Date.AddDays(-1));
            List<Observation> myList = new List<Observation>();
            myList.Add(previousObs);
            return myList.Concat(GetPreviousObservations(previousObs, previous - 1));
        }
    }
}

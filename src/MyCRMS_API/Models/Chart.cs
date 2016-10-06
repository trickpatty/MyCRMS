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
            if (obs != null)
            {
                Observation tomorrow = GetMostFertileOnDate(obs.Date.AddDays(1));
                Observation yesterday = GetMostFertileOnDate(obs.Date.AddDays(-1));
                int yesterdayPCount = CalculatePCount(yesterday);

                // Without data from the day before, we cannot compute the count
                if (yesterday == null)
                {
                    return -1;
                }


                if (
                    // D4: 3 or more days of non-Peak mucus pre-Peak - plus count 3
                    (GetPreviousObservations(obs, 3).All(prevObs => prevObs.IsNonPeakMucus()) && !tomorrow.IsNonPeakMucus()) ||
                    // D5: Any single day of Peak mucus - plus count 3
                    (obs.IsPeakType() && !tomorrow.IsPeakType()) ||
                    // D6: Any unusual bleeding - plus count 3
                    (GetPreviousObservations(obs, 2).All(prevObs => prevObs.ObservedRed != "") && obs.ObservedRed == "" && IsPostPeak(obs))
                    )
                {
                    return 0;
                }

                // If we don't match a "P" day, but yesterday has a count, then coutinue the count
                if (yesterdayPCount >= 0 && yesterdayPCount < 3)
                {
                    return yesterdayPCount + 1;
                }
            }

            // No conditions met, so today is not a counting day
            return -1;
        }

        private bool IsPostPeak(Observation obs)
        {
            Observation start = GetMostFertileOnDate(GetCycleStart(obs.Date));
            Observation peak = Observations.Where(prevObs => prevObs.Date > start.Date && prevObs.Date < obs.Date && prevObs.IsPeakType() == true).First();

            if (obs.Date > peak.Date)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DateTime GetCycleStart(DateTime date)
        {
            DateTime firstDate = Observations.OrderBy(obs => obs.Date).First().Date;
            DateTime lastDate = Observations.OrderBy(obs => obs.Date).Last().Date;
            if (firstDate > date)
            {
                date = firstDate;
            }

            if(lastDate < date)
            {
                date = lastDate;
            }

            Observation today = GetMostFertileOnDate(date);
            Observation yesterday = GetMostFertileOnDate(today.Date.AddDays(-1));

            if(yesterday == null)
            {
                return today.Date;
            }

            DateTime redDate = Observations.OrderByDescending(prevObs => prevObs.Date).Where(prevObs => (prevObs.ObservedRed == "H" || prevObs.ObservedRed == "M") && prevObs.Date <= today.Date).First().Date;

            Observation red0 = GetMostFertileOnDate(redDate);
            Observation red1 = GetMostFertileOnDate(redDate.AddDays(-1));

            if(red1 == null || (red1.ObservedRed != "H" && red1.ObservedRed != "M"))
            {
                return red0.Date;
            }
            else
            {
                return GetCycleStart(red1.Date);
            }
        }

        private bool CalculateBaby(Observation obs)
        {
            Color color = CalculateColor(obs);
            switch (color)
            {
                case Color.Green:
                    // if PCount >= 0 then true
                    if ( (obs.IsNonPeakMucus()) || (CalculatePCount(obs) >= 0) )
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
            if (obs != null)
            {
                Color? ybrColor = GetYellowBabyRuleColor(obs);
                if (ybrColor != null)
                {
                    return (Color)ybrColor;
                }
                else if (obs.ObservedRed != "")
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
            else
            {
                return Color.White;
            }
        }


        private Color? GetYellowBabyRuleColor(Observation obs)
        {
            if (YellowBabyRules != null)
            {
                foreach (var ybr in YellowBabyRules.Where(ybr => ybr.EffectiveDate <= obs.Date).OrderByDescending(ybr => ybr.EffectiveDate))
                {
                    if ((obs.ObservedNumber.ToString() + obs.ObservedLetter.ToString()).Contains(ybr.Rule))
                    {
                        bool postPeak = IsPostPeak(obs);
                        if (
                            (ybr.Timeframe == YBRTimeframe.PrePeak && !postPeak) ||
                            (ybr.Timeframe == YBRTimeframe.PostPeak && postPeak)
                            )
                        {
                            return ybr.ChangeToColor;
                        }

                    }
                }
            }

            return null;
        }

        private IEnumerable<Observation> GetPreviousObservations(Observation obs, int previous)
        {
            if (previous == 0 || obs == null)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCRMS_API.Models
{
    public class YellowBabyRule
    {
        public int ID { get; set; }
        //public int UserId { get; set; }
        public string Rule { get; set; }
        public YBRTimeframe Timeframe { get; set; }
        public Color ChangeToColor { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    public enum YBRTimeframe
    {
        PrePeak,
        PostPeak
    }
}

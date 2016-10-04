using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCRMS_API.Models
{
    public class Stamp
    {
        public Stamp()
        {
            Color = Color.White;
            Baby = true;
            PCount = 1;
        }
        public Stamp(Color color, bool baby, int pCount)
        {
            Color = color;
            Baby = baby;
            PCount = pCount;
        }
        public Color Color { get; set; }
        public bool Baby { get; set; }
        public int PCount { get; set; }
    }
}

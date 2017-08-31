using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessTracker.DTOs
{
    public class MeasureDto
    {
        public string Label { get; set; }
        public double Equivalent { get; set; }
        public string EquivUnit { get; set; }
        public double Quantity { get; set; }
        public double Value { get; set; }
    }
}
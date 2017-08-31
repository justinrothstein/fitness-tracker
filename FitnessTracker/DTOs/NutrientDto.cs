using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessTracker.DTOs
{
    public class NutrientDto
    {
        public int NutrientId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public double Value { get; set; }
        public List<MeasureDto> Measures { get; set; }
    }
}
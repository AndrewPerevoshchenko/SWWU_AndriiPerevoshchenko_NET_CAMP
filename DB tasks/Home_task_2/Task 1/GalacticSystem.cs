using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal class GalacticSystem
    {
        public uint ID { get; }
        public string MorphologicalClass { get; set; }
        public string BotzMorganType { get; set; }
        public GalacticSystem(string MorphologicalClass, string BotzMorganType)
        {
            this.MorphologicalClass = MorphologicalClass;
            this.BotzMorganType = BotzMorganType;
        }
        public override string ToString()
        {
            return $"{ID} {MorphologicalClass} {BotzMorganType}";
        }
    }
}

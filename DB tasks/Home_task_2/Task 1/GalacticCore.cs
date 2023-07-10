using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal class GalacticCore
    {
        public uint ID { get; }
        public string FormationMechanism { get; set; }
        public uint Age { get; set; }
        public ulong BlackHoleMass { get; set; }
        public GalacticCore(string FormationMechanism, uint Age, ulong BlackHoleMass)
        {
            this.FormationMechanism = FormationMechanism;
            this.Age = Age;
            this.BlackHoleMass = BlackHoleMass;
        }
        public override string ToString()
        {
            return $"{ID} {FormationMechanism} {Age} {BlackHoleMass}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal class Galaxy
    {
        public uint ID { get; }
        public uint GalacticCoreID { get; set;}
        public uint GalacticSystemID { get; set; }
        public string GalaxyType { get; set; }
        public ulong FormationDate { get; set; }
        public double Diameter { get; set; }
        public double StellarDiskThickness { get; set; }
        public ulong Mass { get; set; }
        public Galaxy(uint GalaticCoreID, uint GalacticSystemID, string GalaxyType, ulong FormationDate, double Diameter, double StellarDiskThickness, ulong Mass)
        {
            this.GalacticCoreID = GalaticCoreID;
            this.GalacticSystemID = GalacticSystemID;
            this.GalaxyType = GalaxyType;
            this.FormationDate = FormationDate;
            this.Diameter = Diameter > 0 ? Diameter : 0;
            this.StellarDiskThickness = StellarDiskThickness > 0 ? StellarDiskThickness : 0;
            this.Mass = Mass;
        }
        public override string ToString()
        {
            return $"{ID} {GalacticCoreID} {GalacticSystemID} {GalaxyType} {FormationDate} {Diameter} {StellarDiskThickness} {Mass}";
        }
    }
}

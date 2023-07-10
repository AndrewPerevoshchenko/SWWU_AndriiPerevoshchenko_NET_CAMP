using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Reflection;

namespace Task_1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Galaxy testGalaxy = new Galaxy(1, 1, "Spiral", 10000, 100000, 1000, 1500000000000);
            Galaxy testGalaxySecond = new Galaxy(2, 1, "Elliptical", 5000, 50000, 500, 700000000000);
            DbContext<Galaxy>.AddData(testGalaxy);
            DbContext<Galaxy>.AddData(testGalaxySecond);
            GalacticCore testCore = new GalacticCore("Star rest", 10000, 43000000);
            DbContext<GalacticCore>.AddData(testCore);
            DbContext<GalacticCore>.UpdateData("BlackHoleMass = 4299000", "FormationMechanism = 'Star rest'");
            DbContext<Galaxy>.RemoveData("GalaxyType = 'Spiral'");
            Console.WriteLine(DbContext<Galaxy>.GetStringData());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task_1
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Pump pump = new Pump(10);
            User user = new User(20);
            int maxLevel = 50;
            int receivedSpeed = 5;
            Simulator simulator = new Simulator(maxLevel, receivedSpeed, pump, user);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SimulatorForm(simulator));
        }
    }
}

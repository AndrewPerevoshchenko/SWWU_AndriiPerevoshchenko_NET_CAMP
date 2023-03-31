using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace Task_1
{
    internal partial class SimulatorForm : Form
    {
        private static double _percentage = 0;
        private static Simulator _simulator;
        //private static System.Windows.Forms.Timer _timer;
        //private int _timer_i;
        public SimulatorForm(Simulator simulator)
        {
            if (simulator != null)
            {
                _simulator = new Simulator(simulator);
            }
            InitializeComponent();
            //_timer = new System.Windows.Forms.Timer()
            //{
            //    Enabled = false,
            //    Interval = 1000
            //};
            //_timer.Tick += _timer_Tick;

        }
        //private void _timer_Tick(object sender, EventArgs e)
        //{
        //    textBox1.Text = _timer_i.ToString();
        //    _timer_i++;
        //    if (_timer_i >= 10)
        //    {
        //        _timer.Stop();
        //    }
        //}

        //private void _timer_Tick(object sender, EventArgs e)
        //{
          
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    _timer.Stop();
        //    _timer_i = 0;
        //    _timer.Start();
        //}
        private void SimulatorForm_Load(object sender, EventArgs e)
        {

        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
        public static void checkSystem()
        {
            //_timer.Stop();
            _percentage = _simulator.WaterTower.CurrentLevel / _simulator.WaterTower.MaxWaterLevel * elementHost1.Height;
            tower1.value = _percentage;
            tower1.ChangeValue();
            //_timer.Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            _simulator.MeetNeeds();
           
        }
    }
}

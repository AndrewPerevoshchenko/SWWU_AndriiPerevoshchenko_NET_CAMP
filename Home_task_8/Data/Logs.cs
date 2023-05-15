using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_7
{
    internal class Logs : ICloneable //Клас для роботи з результуючими стрічками
    {
        private List<string> _logs;
        public Logs() 
        {
            _logs = new();
        }
        public void AddLogs(string logString)
        {
            if (logString.Length > 0)
            {
                _logs.Add(logString);
            }         
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string logString in _logs)
            {
                sb.Append(logString + "\n\n");
            }
            return sb.ToString();
        }
        public object Clone()
        {
            Logs copy = new Logs();
            copy._logs = new List<string>(_logs);
            return copy;
        }
    }
}
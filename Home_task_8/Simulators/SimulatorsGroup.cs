using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_7
{
    internal class SimulatorsGroup
    {  
        private Dictionary<int, CrossroadsSimulator> _simulators; //Айді симулятора - симулятор
        private Dictionary<int, Logs> _simulatorsLogs; //Айді симулятора - його логи
        public SimulatorsGroup()
        {
            _simulators = new Dictionary<int, CrossroadsSimulator>();
            _simulatorsLogs = new Dictionary<int, Logs>();
        }
        public void AddSimulator(CrossroadsSimulator simulator) //Додаємо симулятор
        {
            _simulators.Add(_simulators.Count, (CrossroadsSimulator)simulator.Clone());
        }
        public void StartSimulator(int ID, double timeCeil) //Запускаємо симулятор через айді
        {
            if (ID < 0 || ID >= _simulators.Count)
            {
                throw new IndexOutOfRangeException("Incorrect ID of the simulator");
            }
            _simulators[ID].ColourChanged += _simulators[ID].ToString; //Підписка на подію, використовуємо ToString() симулятора
            _simulators[ID].Start(timeCeil); //Запуск
            if (_simulatorsLogs.ContainsKey(ID)) //Переписуємо логи, якщо вже існує ключ
            {
                _simulatorsLogs[ID] = _simulators[ID].Logs;
                return;
            }
            _simulatorsLogs.Add(ID, _simulators[ID].Logs); //Ні - додали новий ключ
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var item in _simulatorsLogs)
            {
                sb.Append(">> Simulator ID: " + item.Key + "\n");
                sb.Append(item.Value.ToString() + "\n");
            }
            return sb.ToString();
        }
    }
}
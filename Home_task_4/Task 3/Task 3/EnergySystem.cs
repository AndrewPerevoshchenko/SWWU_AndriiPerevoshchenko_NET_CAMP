using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Task_3
{
    internal class EnergySystem //Загальна енергетична система
    {
        private const double KVT_PRICE = 8.50d; //Ціна за кілоВатт
        private const string CULTURE_PRICE = "uk-UA";
        private List<PersonalEnergy> _users;
        private uint _flatAmount;
        private uint _quarter;
        public List<PersonalEnergy> Users
        {
            get { return _users; }
        }
        public uint FlatAmount
        {
            get { return _flatAmount; }
        }
        public EnergySystem(string fileURL)
        {           
            if (fileURL != null)
            {
                string[] temp = File.ReadAllLines(fileURL);
                string[] tempFirst = temp[0].Split(' ');
                uint.TryParse(tempFirst[0], out _flatAmount);
                uint.TryParse(tempFirst[1], out _quarter);
                _users = new List<PersonalEnergy>((int)_flatAmount);
                if (_flatAmount <= temp.Length - 1)
                {
                    for (int i = 1; i <= _flatAmount; ++i)
                    {
                        string[] tempOthers = temp[i].Split(" | ");
                        List<ElectricityMeter> electricityTemp = new List<ElectricityMeter>((int)PersonalEnergy.QUARTER_AMOUNT);
                        for (int j = 3; j < tempOthers.Length; j += 2)
                        {
                            string[] date = tempOthers[j + 1].Split('.');
                            electricityTemp.Add(new ElectricityMeter(DateTime.Parse(tempOthers[j + 1],
                                new CultureInfo("uk-UA")), uint.Parse(tempOthers[j])));
                        }
                        _users.Add(new PersonalEnergy(electricityTemp, uint.Parse(tempOthers[0]), tempOthers[1], tempOthers[2]));
                    }
                }
            }
            else
            {
                _users = new List<PersonalEnergy>();
                _flatAmount = 0;
                _quarter = 0;
            }
        }
        public Dictionary<(uint, string), string> CountExpenses(double kvtPrice = KVT_PRICE) //Розрахунок витрат (квартира, адреса, ціна)
        {
            Dictionary<(uint, string), string> expences = new Dictionary<(uint, string), string>();
            foreach(PersonalEnergy item in _users)
            {
                double price = kvtPrice * (item.Measurements.Last().Counter - item.Measurements.First().Counter);
                expences.Add((item.Flat, item.Address), price.ToString("C2", CultureInfo.CreateSpecificCulture("ua-UA")));
            }
            return expences;
        } 
        public List<uint> FindLifelessFlat() //Пошук квартир, у яких не змінилися показники
        {
            List<uint> ecoFlats = new List<uint>();
            foreach(PersonalEnergy item in _users)
            {
                if (item.Measurements.Last().Counter == item.Measurements.First().Counter)
                {
                    ecoFlats.Add(item.Flat);
                }
            }
            return ecoFlats;
        }
        public List<(uint, DateTime, uint)> CountDifferenceBetweenDates(DateTime dateTime) //Різниця з указаною датою
        {
            List<(uint, DateTime, uint)> differences = new List<(uint, DateTime, uint)>();
            foreach(PersonalEnergy item in _users)
            {
                TimeSpan diff = dateTime.Subtract(item.Measurements.Last().RemovalDate);
                differences.Add((item.Flat, item.Measurements.Last().RemovalDate, (uint)diff.Days));
            }
            return differences; 
        }
        public string FindMaxDebtor() //Розрахунок боржника (по суті того, хто більше всіх витратив)
        {
            uint maxDebt = 0;
            int index = 0;
            for(int i = 0; i < _flatAmount; ++i)
            {
                uint temp = _users[i].Measurements.Last().Counter - _users[i].Measurements.First().Counter;
                if (temp > maxDebt)
                {
                    maxDebt = temp;
                    index = i;
                }
            }
            return _users[index].LastName;
        }
        public override string ToString() //Звіт
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"QUARTER >> {_quarter}\n");
            sb.Append($"FLAT AMOUNT >> {_flatAmount}\n");

            int flatMax = _flatAmount.ToString().Length;
            int lastNameMax = "Owner".Length;
            int monthMax = "Removal month".Length;
            int dateMax = "Removal date".Length;
            int counterMax = "Counter".Length;
            for (int i = 0; i < _flatAmount; ++i)
            {
                if (_users[i].LastName.Length > lastNameMax)
                {
                    lastNameMax = _users[i].LastName.Length;
                }
            }
            StringBuilder sbFormat = new StringBuilder($"| {{0,-{flatMax}}} | {{1,-{lastNameMax}}} | ");
            List<string> titleTable = new List<string>();
            titleTable.Add("№");
            titleTable.Add("Owner");
            for (int i = 0; i < PersonalEnergy.QUARTER_AMOUNT * 3; i += 3)
            {
                sbFormat.Append($"{{{i + 2},-{monthMax}}} | {{{i + 3},-{dateMax}}} | {{{i + 4},-{counterMax}}} | ");
                titleTable.Add("Removal month");
                titleTable.Add("Removal date");
                titleTable.Add("Counter");
            }
            string format = sbFormat.ToString();
            sb.AppendFormat(format, titleTable.ToArray());
            sb.Append("\n\n");
            foreach(PersonalEnergy item in _users)
            {
                List<string> mm = new List<string>();
                mm.Add(item.Flat.ToString());
                mm.Add(item.LastName);
                for (int i = 0; i < PersonalEnergy.QUARTER_AMOUNT; ++i)
                {
                    mm.Add(item.Measurements[i].RemovalDate.ToString("MMMM"));
                    mm.Add(item.Measurements[i].RemovalDate.ToString("dd.MM.yy", new CultureInfo(ElectricityMeter.CULTURE_FORMAT)));
                    mm.Add(item.Measurements[i].Counter.ToString(ElectricityMeter.COUNTER_FORMAT));
                }

                sb.AppendFormat(format, mm.ToArray());
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}

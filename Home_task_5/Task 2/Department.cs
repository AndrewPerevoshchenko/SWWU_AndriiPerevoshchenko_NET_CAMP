using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal class Department : Component
    {
        private Dictionary<string, int> _location = new Dictionary<string, int>(); //Обгортка ліста (індексатор певний: за назвою знаємо одразу індекс компонента в лісті)
        private List<Component> _components = new List<Component>();
        public Dictionary<string, int> Location
        {
            get { return _location; }
        }
        public List<Component> Components
        {
            get { return _components; }
        }
        public Department(string name) : base(name)
        {

        }
        public override void Add(in Component component)
        {
            _components.Add(component);
            _dimensions = new Dimensions(_components.Max(x => x.Dimensions._length),
                _components.Max(x => x.Dimensions._width), _components.Sum(x => x.Dimensions._height));
            _location.Add(component.Name, _components.Count - 1);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbSub = new StringBuilder("It has: ");
            int subLengthBefore = sbSub.Length;
            foreach (Component component in _components)
            {
                sb.Append(component.ToString());
                sbSub.Append(component.Name + " | ");
            }
            if (sbSub.Length != subLengthBefore)
            {
                sbSub.Remove(sbSub.Length - 2, 2);
            }
            else
            {
                sbSub.Append("Nothing");
            }
            return "DEPARTMENT: " + base.ToString() + "\t" + sbSub.ToString() + "\n\n" + sb.ToString();
        }
    }
}

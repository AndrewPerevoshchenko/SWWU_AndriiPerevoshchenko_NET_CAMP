using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_9
{
    internal abstract class Cook : IDishOrderHandler, IComparable<Cook>
    {
        public const string COOK_ANONYMOUS_NAME = "Cook";
        private Guid _ID = Guid.NewGuid();
        private IDishOrderHandler _nextHandler;
        private readonly string _surname;
        public Guid ID 
        { 
            get { return _ID; } 
        }
        public string Surname
        {
            get { return _surname; }
        }
        public Cook(string surname)
        {      
            _surname = surname != String.Empty && surname != null ? surname : COOK_ANONYMOUS_NAME;
        }
        public IDishOrderHandler SetNext(IDishOrderHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }
        public virtual (Order, Guid?) Handle(Order request)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Handle(request);
            }
            else
            {
                return (null, null);
            }
        }
        public int CompareTo(Cook? other)
        {
            if (other == null) return 1;
            return this.GetType().Name.CompareTo(other.GetType().Name);
        }
    }
}

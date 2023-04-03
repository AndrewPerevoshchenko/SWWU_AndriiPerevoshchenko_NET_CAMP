using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ObjectiveC;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace HT_23._03._23
{
    internal class Tensor<T>
    {
        private uint _dimension;
        private uint[] _sizes;
        private Type _type; //Тип тензора
        private T? _element; //Яким елементом і якого типу заповнюємо
        private dynamic _tensor; //Структура тензора
        
        public Tensor(T? element, uint dimension = 3, params uint[] lengths)
        {
            _dimension = dimension;
            _sizes = new uint[dimension - 1];
            for (int i = 0; i < _sizes.Length; ++i)
            {
                _sizes[i] = lengths[i];
            }
            _type = typeof(uint);
            _element = element;
            _tensor = 0;
        }
        public void createUnitTensor()
        {
            if (_dimension == 1)
            {
                _tensor = _element;
                return;
            }
            int i = 1;
            dynamic internalStructure = _element; //Внутрішня структура кожного з вимірів
            Type generic = typeof(List<>); //Для запам'ятовування типу
            Type constructed = typeof(T);
            while (i < _dimension)
            {
                dynamic temp = new List<dynamic>();
                for (int j = 0; j < _sizes[i - 1]; ++j)
                {
                    temp.Add(internalStructure);
                }
                internalStructure = temp;
                Type[] arg = { constructed };
                constructed = generic.MakeGenericType(arg);
                ++i;
            }
            _type = constructed;
            _tensor = internalStructure;
        }
        private string ListPacking(string strInternal, uint amount) //Запаковування для ToString() внутрішньої структури
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\n");
            strInternal = strInternal.Replace("\n", "\n\t");
            for (int i = 0; i < amount; ++i)
            {        
                sb.Append($"\t{strInternal}\n");                    
            }
            sb.Append("}");
            return sb.ToString();
        }
        public override string ToString()
        {           
            if(_dimension == 1)
            {
                return _tensor.ToString();
            }
            StringBuilder sb = new StringBuilder();
            dynamic temp = _tensor;
            while (temp.GetType() == typeof(List<dynamic>) && temp[0].GetType() != typeof(T))
            {
                temp = temp[0];
            }
            sb.Append('{');
            for (int i = 0; i < _sizes[0]; ++i)
            {
                sb.Append($"{temp[i]}, ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append('}');
            temp = sb.ToString();
            for(int i = 2; i < _dimension; ++i)
            {
                temp = ListPacking(temp, _sizes[i-1]);
            }
            return temp;
        }      
        public new Type GetType()
        {
            return _type;
        }
    }
}

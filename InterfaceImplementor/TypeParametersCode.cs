using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterfaceImplementor
{
    /// <summary>
    /// List of type parameters of generic type or method
    /// </summary>
    public class TypeParametersCode: ICode
    {
        private List<TypeCode> _list;
        private string _code;

        public TypeParametersCode(Type[] args)
        {
            _list = new List<TypeCode>();
            if (args != null)
                foreach (var type in args.Where(t=>t!=null))
                    _list.Add(type);
        }

        public string Code
        {
            get
            {
                if (_code == null)                
                    _code = Generate(false);
                
                return _code;
            }            
        }

        public int TypeParametersCount { get { return _list.Count; } }

        /// <summary>
        /// Get type parameters at specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TypeCode this[int index]
        {
            get { return _list[index]; }
        }

        /// <summary>
        /// Enumerates type parameters
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TypeCode> AsEnumerable()
        {
            return _list.AsEnumerable();
        } 

        public string Generate(bool stdForm)
        {
            if (_list == null || _list.Count == 0)
                return string.Empty;
            return String.Format("<{0}>", String.Join(", ", _list.Select(tc => tc.Generate(stdForm))));
        }
    }
}

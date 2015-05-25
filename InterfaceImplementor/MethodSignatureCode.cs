using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InterfaceImplementor
{
    public class MethodSignatureCode: ICode
    {
        private readonly List<ParameterCode> _args;
        public MethodSignatureCode(MethodInfo mi)
        {
            Name = mi.Name;
            if (mi.IsGenericMethod)
            {
                TypeParameters = new TypeParametersCode(mi.GetGenericArguments());
            }
            _args = mi.GetParameters().Select(pi => new ParameterCode(pi)).ToList();                
        }

        public string Name { get; private set; }

        public bool IsGeneric { get { return TypeParameters != null; } }

        /// <summary>
        /// Type parameters of a generic method
        /// </summary>
        public TypeParametersCode TypeParameters { get; private set; }

        public int ParametersCount
        {
            get { return _args.Count; }
        }

        public ParameterCode this[int index]
        {
            get { return _args[index]; }
        }

        public IEnumerable<ParameterCode> Parameters { get { return _args.AsEnumerable(); } }
         
        public string Generate(bool stdForm)
        {
            var sb = new StringBuilder();
            sb.Append(Name);
            if (TypeParameters != null)
                sb.Append(TypeParameters.Generate(stdForm));
            sb.Append("(");
            sb.Append(String.Join(", ", _args.Select(a => a.Generate(stdForm))));        
            sb.Append(")");
            return sb.ToString();
        }
    }
}

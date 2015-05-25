using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InterfaceImplementor
{
    /// <summary>
    /// MethodInfo extended wrapper
    /// </summary>
    public class MethodCode
    {
        private ConstraintCode[] _constraints = new ConstraintCode[0];

        public MethodCode(MethodInfo mi)
        {
            Signature = new MethodSignatureCode(mi);
            DeclaringType = mi.DeclaringType;
            ReturnType = mi.ReturnType;
            if (Signature.TypeParameters != null)
                _constraints = Signature.TypeParameters.AsEnumerable()
                    .Select(tp => ConstraintCode.Create(tp.Type))
                    .Where(c => c != null).ToArray();
        }

        public TypeCode DeclaringType { get; private set; }

        public TypeCode ReturnType { get; private set; }

        public string Name { get { return Signature.Name; } }

        public bool IsGeneric { get { return Signature.TypeParameters != null; } }

        /// <summary>
        /// Type parameters of a generic method
        /// </summary>
        public TypeParametersCode TypeParameters { get; private set; }

        public int ParametersCount
        {
            get { return Signature.ParametersCount; }
        }

        public IEnumerable<ParameterCode> Parameters { get { return Signature.Parameters; } }

        public MethodSignatureCode Signature { get; private set; }

        public int ConstraintCount
        {
            get { return _constraints.Length; }
        }

        public ConstraintCode this[int index]
        {
            get { return _constraints[index]; }
        }

        public IEnumerable<ConstraintCode> Constraints { get { return _constraints.AsEnumerable(); } }
    }
}

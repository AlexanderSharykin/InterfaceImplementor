using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InterfaceImplementor
{
    public class ConstraintCode: ICode
    {
        private ConstraintCode()
        {
        }

        /// <summary>
        /// Constrained type
        /// </summary>
        public TypeCode Type { get; private set; }

        public TypeCode BaseType { get; private set; }

        public TypeCode[] Interfaces { get; private set; }

        public bool IsStruct { get; private set; }

        public bool IsClass { get; private set; }

        public bool IsNew { get; private set; }

        public string Generate(bool stdForm)
        {
            var sb = new StringBuilder();
            sb.Append(Type.Generate(stdForm)).Append(": ");
            if (BaseType != null)
                sb.Append(BaseType.Generate(stdForm)).Append(", ");
            
            if (IsClass)
                sb.Append("class, ");
            if (IsStruct)
                sb.Append("struct, ");

            foreach (var i in Interfaces)
                sb.Append(i.Generate(stdForm)).Append(", ");

            if (IsNew)
                sb.Append("new(), ");
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }

        public static ConstraintCode Create(Type t)
        {
            Type[] genericConstraints = t.GetGenericParameterConstraints().Where(x => x != typeof(ValueType)).ToArray();
            var a = t.GenericParameterAttributes;

            // no generic constraints
            if (genericConstraints.Length == 0 && a == GenericParameterAttributes.None)
                return null;

            var constraint = new ConstraintCode();
            constraint.Type = t;            

            var interfaces = new List<TypeCode>();
            if (genericConstraints.Length > 0)
            {
                foreach (var tc in genericConstraints)
                {
                    if (tc.IsInterface)
                        // constraint of interface
                        interfaces.Add(tc);
                    else
                        // constraint of base type
                        constraint.BaseType = tc;
                }
            }
            constraint.Interfaces = interfaces.ToArray();

            if (a != GenericParameterAttributes.None)
            {
                if (a.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
                    constraint.IsClass = true;
                if (a.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
                    constraint.IsStruct = true;
                else if (a.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
                    constraint.IsNew = true;
            }
            return constraint;
        }
    }
}

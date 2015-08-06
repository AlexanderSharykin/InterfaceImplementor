using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterfaceImplementor
{
    /// <summary>
    /// Class generates speakable c# names for .Net types (most important, for generics)
    /// </summary>
    public class TypeCode : ICode
    {
        private string _fullName;       

        public TypeCode(Type type)
        {
            Type = type.IsByRef ? type.GetElementType() : type;
            Type[] gp = Type.GetGenericArguments();
            if (gp.Length > 0)
                TypeParameters = new TypeParametersCode(gp);
        }

        /// <summary>
        /// System type
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Type name
        /// </summary>
        public string FullName
        {
            get
            {
                if (_fullName == null)
                    _fullName = Generate(false);
                return _fullName;
            }
        }

        /// <summary>
        /// Type parameters of a generic type
        /// </summary>
        public TypeParametersCode TypeParameters { get; private set; }

        public string Generate(bool stdForm)
        {
            Type t = Type;

            switch (t.FullName)
            {
                case "System.Void": return "void";
                case "System.Object": return "object";
                case "System.Int64": return "long";
                case "System.UInt64": return "ulong";
                case "System.Int32": return "int";
                case "System.UInt32": return "uint";
                case "System.Int16": return "short";
                case "System.UInt16": return "ushort";
                case "System.Byte": return "byte";
                case "System.SByte": return "sbyte";
                case "System.Double": return "double";
                case "System.Single": return "float";
                case "System.Decimal": return "decimal";
                case "System.Boolean": return "bool";
                case "System.String": return "string";
                case "System.Char": return "char";
            }

            if (t.IsGenericParameter)
            {
                if (stdForm)
                    // used for signature comparison
                    return "_T" + t.GenericParameterPosition;
                return t.Name;
            }

            if (t.IsArray)
                return new TypeCode(t.GetElementType()).Generate(stdForm) +
                       "[" + new string(',', t.GetArrayRank() - 1) + "]";


            var nullable = Nullable.GetUnderlyingType(t);
            if (nullable != null && nullable.IsPrimitive)
                return new TypeCode(nullable).Generate(stdForm) + "?";

            string name;
            if (t.IsGenericType)
            {
                string fullName = t.FullName ?? t.ToString();
                name = fullName.Substring(0, fullName.IndexOf('`'));
            }
            else
                name = t.FullName;
            name = name.Replace('+', '.'); // NOTE: '+' occures in the names of nested types

            if (TypeParameters != null)
                name += TypeParameters.Generate(stdForm);

            return name;
        }

        public static implicit operator TypeCode(Type t)
        {
            return new TypeCode(t);
        }
    }
}

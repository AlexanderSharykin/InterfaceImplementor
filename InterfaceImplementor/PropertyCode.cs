using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InterfaceImplementor
{
    /// <summary>
    /// PropertyInfo wrapper
    /// </summary>
    public class PropertyCode
    {
        public PropertyCode(PropertyInfo pr)
        {
            Name = pr.Name;
            DeclaringType = pr.DeclaringType;
            PropertyType = pr.PropertyType;
            CanRead = pr.CanRead;
            CanWrite = pr.CanWrite;
        }

        public string Name { get; private set; }

        public TypeCode DeclaringType { get; private set; }

        public TypeCode PropertyType { get; private set; }

        public bool CanRead { get; private set; }

        public bool CanWrite { get; private set; }
    }
}

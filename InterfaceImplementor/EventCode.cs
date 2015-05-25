using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InterfaceImplementor
{
    /// <summary>
    /// EventInfo wrapper
    /// </summary>
    public class EventCode
    {
        public EventCode(EventInfo ev)
        {
            Name = ev.Name;
            DelegateType = ev.EventHandlerType;
            DeclaringType = ev.DeclaringType;
        }

        public string Name { get; set; }        
        
        public TypeCode DelegateType { get; private set; }

        public TypeCode DeclaringType { get; private set; }
    }
}

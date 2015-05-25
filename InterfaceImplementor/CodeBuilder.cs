using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeCode = InterfaceImplementor.TypeCode;

namespace InterfaceImplementor
{
    public class CodeBuilder
    {
        private static readonly string DefaultTarget = "_target";
        private static readonly string DefaultClass = "Sample";

        public CodeBuilder()
        {
            ClassName = DefaultClass;
            TargetName = DefaultTarget;
        }

        public string ClassName { get; set; }

        public string TargetName { get; set; }

        public string ImplementInterface(Type i)
        {
            if (i.IsInterface == false)
                throw new ArgumentException("Interface expected");
            var tc = new TypeCode(i);

            // class declaration
            var sb = new StringBuilder(String.Format("public class {0}: ", ClassName));            
            sb.AppendLine(tc.FullName);
            sb.AppendLine("{");

            // local variable to store target implementation
            sb.AppendLine(String.Format("private {0} {1};", tc.FullName, TargetName));
            sb.AppendLine();

            // .ctor
            sb.AppendLine(String.Format("public {0}({1} target)", ClassName, tc.FullName));
            sb.AppendLine(String.Format("{{ {0} = target; }}", TargetName));
            sb.AppendLine();

            var types = new List<Type>() { i };
            // interface can inherit from many others interfaces
            types.AddRange(i.GetInterfaces());

            var methods = new List<MethodCode>();
            var properties = new List<PropertyCode>();
            var indexers = new List<IndexerCode>();
            var events = new List<EventCode>();

            var implementor = new Implementor() { Target = TargetName };

            // implement each interface
            foreach (var t in types)
            {
                methods.AddRange(t.GetMethods().Where(m => m.IsSpecialName == false).Select(method => new MethodCode(method)));

                foreach (var prop in t.GetProperties())                
                    if (prop.GetIndexParameters().Length == 0)
                        properties.Add(new PropertyCode(prop));
                    else 
                        indexers.Add(new IndexerCode(prop));                

                events.AddRange(t.GetEvents().Select(ev => new EventCode(ev)));
            }
            
            var ambiguousMethods = methods.GroupBy(m => m.Signature.Generate(true));

            foreach (var group in ambiguousMethods)
            {
                bool isExplicit = group.Skip(1).Any();
                foreach (var methodCode in group)
                {
                    sb.AppendLine(implementor.GenerateMethod(methodCode, isExplicit, methodCode.DeclaringType));
                    sb.AppendLine();
                }                
            }

            var ambiguousProperties = properties.GroupBy(p => p.Name);

            foreach (var group in ambiguousProperties)
            {
                bool isExplicit = group.Skip(1).Any();
                foreach (var propertyCode in group)
                {
                    sb.AppendLine(implementor.GenerateProperty(propertyCode, isExplicit, propertyCode.DeclaringType));
                    sb.AppendLine();
                }
            }

            var ambiguousIndexers = indexers.GroupBy(p => p.Generate(true));

            foreach (var group in ambiguousIndexers)
            {
                bool isExplicit = group.Skip(1).Any();
                foreach (var indexerCode in group)
                {
                    sb.AppendLine(implementor.GenerateIndexer(indexerCode, isExplicit, indexerCode.DeclaringType));
                    sb.AppendLine();
                }
            }

            var ambiguousEvents = events.GroupBy(e => e.Name);

            foreach (var group in ambiguousEvents)
            {
                bool isExplicit = group.Skip(1).Any();
                foreach (var eventCode in group)
                {
                    sb.AppendLine(implementor.GenerateEvent(eventCode, isExplicit, eventCode.DeclaringType));
                    sb.AppendLine();
                }
            }

            sb.Append("}");
            return sb.ToString();
        }
    }
}

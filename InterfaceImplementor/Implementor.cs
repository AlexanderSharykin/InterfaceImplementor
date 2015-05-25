using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterfaceImplementor
{
    /// <summary>
    /// Class generates implementation of interface members by delegaing their job to underlying object of the same interface type
    /// </summary>
    public class Implementor: IInterfaceImplementor
    {
        private static readonly string DefaultTarget = "_target";

        private string _target;

        /// <summary>
        /// Gets or sets name of object, which will perform real work
        /// </summary>
        public string Target
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_target))
                    return DefaultTarget;
                return _target;
            }
            set { _target = value; }
        }

        private string TargetCall(TypeCode declaringType, bool isExplicit)
        {
            if (isExplicit)
                return string.Format("(({0}){1})", declaringType.FullName, Target);
            return Target;
        }

        public string GenerateMethod(MethodCode method, bool isExplicit, TypeCode declaringType)
        {
            var sb = new StringBuilder();
            if (isExplicit == false)
                sb.Append("public ");

            sb.Append(method.ReturnType.FullName).Append(' ');

            if (isExplicit)
                sb.Append(declaringType.FullName).Append('.');

            sb.AppendLine(method.Signature.Generate(false));

            if (isExplicit == false && method.ConstraintCount > 0)
            {
                // list all constraint on generic parameters in case of implicit implementation
                for (int c = 0; c < method.ConstraintCount; c++)
                {
                    sb.Append("where ");
                    sb.Append(method[c].Generate(false));
                    if (c == method.ConstraintCount - 1)
                        sb.AppendLine();
                    else
                        sb.AppendLine(",");
                }
            }

            sb.AppendLine("{");
            // non-void methods must return value
            if (method.ReturnType.Type != typeof(void))
                sb.Append("return ");

            // invoke method of underlying object
            sb.Append(TargetCall(declaringType, isExplicit)).Append('.');
            sb.Append(method.Signature.Name);
            if (method.IsGeneric)
                sb.Append(method.Signature.TypeParameters.Generate(false));

            // list argument
            sb.AppendLine(String.Format("({0});", string.Join(", ", method.Parameters.Select(p=>p.ArgumentCode))));
            sb.Append("}");
            return sb.ToString();
        }

        public string GenerateProperty(PropertyCode property, bool isExplicit, TypeCode declaringType)
        {
            var sb = new StringBuilder();
            if (isExplicit == false)
                sb.Append("public ");

            sb.Append(property.PropertyType.FullName).Append(' ');

            if (isExplicit)
                sb.Append(declaringType.FullName).Append('.');

            sb.AppendLine(property.Name);

            string accessor = TargetCall(declaringType, isExplicit) + "." + property.Name;

            sb.AppendLine("{");
            if (property.CanRead)
                sb.AppendLine(String.Format("  get {{ return {0}; }}", accessor));
            if (property.CanWrite)
                sb.AppendLine(String.Format("  set {{ {0} = value; }}", accessor));
            sb.Append("}");

            return sb.ToString();
        }

        public string GenerateIndexer(IndexerCode indexer, bool isExplicit, TypeCode declaringType)
        {
            var sb = new StringBuilder();
            if (isExplicit == false)
                sb.Append("public ");

            sb.Append(indexer.IndexerType.FullName).Append(' ');

            if (isExplicit)
                sb.Append(declaringType.FullName).Append('.');
            
            // indexer signature
            sb.AppendLine(indexer.Generate(false));

            sb.AppendLine("{");

            // access to indexer of underlying object
            string accessor = TargetCall(declaringType, isExplicit) + "[" + string.Join(", ", indexer.Parameters.Select(p => p.ArgumentCode)) + "]";

            if (indexer.CanRead)
                sb.AppendLine(String.Format("  get {{ return {0}; }}", accessor));
            if (indexer.CanWrite)
                sb.AppendLine(String.Format("  set {{ {0} = value; }}", accessor));

            sb.Append("}");
            return sb.ToString();
        }

        public string GenerateEvent(EventCode ev, bool isExplicit, TypeCode declaringType)
        {
            if (isExplicit == false)
                return String.Format("public event {0} {1} = delegate {{}};", ev.DelegateType.FullName, ev.Name);

            return String.Format(
@"event {0} {1}.{2}
{{
  add    {{ {3}.{2} += value; }}
  remove {{ {3}.{2} -= value; }}
}}", ev.DelegateType.FullName, declaringType.FullName, ev.Name, TargetCall(declaringType, true));
        }
    }
}

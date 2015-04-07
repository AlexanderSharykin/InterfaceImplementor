using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WrapperGenerator
{
    /// <summary>
    /// Class generates implemention of interface
    /// </summary>
    public class Generator
    {
        private static readonly string Impl = "_impl";
        private static readonly string Tab = "\t";
        private static readonly string Out = "out ";
        private static readonly string Ref = "ref ";

        /// <summary>
        /// Generates implemention of interface
        /// </summary>
        /// <param name="i">Interface type</param>
        /// <returns></returns>
        public string ImplementInterface(Type i)
        {
            if (i.IsInterface == false)
                throw new ArgumentException("Interface expected");
            var sb = new StringBuilder();
            sb.Append(GetTypeName(i)).Append(' ').Append(Impl).AppendLine(";");

            var types = new List<Type>() {i};
            // interface can be inherited from many others interfaces
            types.AddRange(i.GetInterfaces());
            
            var methods = new List<string>();
            var returnTypes = new List<Type>();

            var properties = new List<string>();
            var propertyTypes = new List<Type>();

            var events = new List<string>();
            var eventTypes = new List<Type>();

            // implement each interface
            foreach (var t in types)
            {
                foreach (var method in t.GetMethods().Where(m => m.IsSpecialName == false)) // NOTE: get_PropertyName, set_PropertyName, add_EventName, remove_EventName should be ignored here
                {
                    MethodCode(method, sb, methods, returnTypes);
                    sb.AppendLine();
                }                

                foreach (var prop in t.GetProperties())
                {
                    PropertyCode(prop, sb, properties, propertyTypes);
                    sb.AppendLine();
                }                

                foreach (var ev in t.GetEvents())
                {
                    EventCode(ev, sb, events, eventTypes);
                    sb.AppendLine();
                }                
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// Returns c# type name from compiler type name
        /// </summary>
        /// <param name="t">Type info</param>
        public static string GetTypeName(Type t)
        {
            return GetTypeName(t, false);
        }

        /// <summary>
        /// Returns c# type name from compiler type name
        /// </summary>
        /// <param name="t">Type info</param>
        /// <param name="shouldNormalize">Use position index in names of type parameters</param>
        /// <returns></returns>
        private static string GetTypeName(Type t, bool shouldNormalize)
        {
            if (t.IsByRef)
                t = t.GetElementType();

            if (t.FullName == "System.Void")
                return "void";
            if (t.FullName == "System.Object")
                return "object";
            if (t.FullName == "System.Int64")
                return "long";
            if (t.FullName == "System.Int32")
                return "int";
            if (t.FullName == "System.Int16")
                return "short";
            if (t.FullName == "System.Boolean")
                return "bool";
            if (t.FullName == "System.String")
                return "string";

            if (t.IsGenericParameter)
            {                
                if (shouldNormalize)
                    // used for signature comparison
                    return "_T" + t.GenericParameterPosition;                
                return t.Name;
            }

            var args = t.GetGenericArguments();
            // NOTE: t.IsGeneric == false when generic parameter is used with ref keyword
            if (args.Length == 0)
                return t.FullName ?? t.Name; // NOTE: t.FullName is null for array types

            // generic type name
            var nameBuilder = new StringBuilder(t.Name.Substring(0, t.Name.IndexOf('`')));
            nameBuilder.Insert(0, t.Namespace + Type.Delimiter);

            ListTypeParameters(args, nameBuilder, shouldNormalize);
            // the type is an array of generics
            if (t.IsArray)
                nameBuilder.Append("[]");
            return nameBuilder.ToString();
        }

        /// <summary>
        /// List type parameters in andle brackets
        /// </summary>
        /// <param name="gArgs">List of type arguments</param>
        /// <param name="builder"></param>
        /// <param name="shouldNormalize"></param>
        private static void ListTypeParameters(Type[] gArgs, StringBuilder builder, bool shouldNormalize = false)
        {
            if (gArgs.Length == 0)
                return;
            builder.Append('<');
            for (int i = 0; i < gArgs.Length; i++)
                builder.Append(GetTypeName(gArgs[i], shouldNormalize)).Append(", ");
            builder.Remove(builder.Length - 2, 2);
            builder.Append('>');
        }

        /// <summary>
        /// Generates code for method
        /// </summary>
        /// <param name="method">Method reflected data</param>
        /// <param name="sb"></param>
        /// <param name="signatures">List of signatures for implemented methods</param>
        /// <param name="returnTypes">List of return types for implemented methods</param>
        private void MethodCode(MethodInfo method, StringBuilder sb, IList<string> signatures, IList<Type> returnTypes)
        {           
            var parameters = method.GetParameters();
            var gArgs = method.GetGenericArguments();

            var sBuilder = new StringBuilder();
            MethodSignatureCode(method, parameters, gArgs, sBuilder, true);

            var s = sBuilder.ToString();
            int idx = signatures.IndexOf(s);            
            bool shoudBeExplicit = idx >= 0;

            // add attributes
            AttributeCode(method, sb);

            // NOTE: if there are 2 methods with the same signature in the interface, at least one of them must be implemented as explicit
            if (shoudBeExplicit)
            {
                // method is already implemented
                if (method.ReturnType == returnTypes[idx])
                    return;
                sb.Append(GetTypeName(method.ReturnType)).Append(' ');
                sb.Append(GetTypeName(method.DeclaringType)).Append('.');
            }                
            else
            {
                signatures.Add(s);
                returnTypes.Add(method.ReturnType);
                sb.Append("public ");
                sb.Append(GetTypeName(method.ReturnType)).Append(' ');
            }
            
            // method signature with real (not normalized) parameters names
            MethodSignatureCode(method, parameters, gArgs, sb);

            // NOTE: explicite method implementation inherits constraints from the base method 
            if (shoudBeExplicit == false && gArgs.Length > 0)
                sb.Append(CreateGenericConstraint(gArgs));

            // method implementation
            sb.AppendLine("{");

            // method is implemented by delegating real work to the wrapped object
            sb.Append(Tab);
            if (method.ReturnType != typeof (void))
                sb.Append("return ");
            if (shoudBeExplicit)            
                sb.AppendFormat("(({0}){1}).", GetTypeName(method.DeclaringType), Impl);                            
            else
                sb.Append(Impl).Append('.');
            sb.Append(method.Name);

            if (method.IsGenericMethod)                            
                ListTypeParameters(gArgs, sb);            

            sb.Append('(');
            ListArguments(parameters, sb);
            sb.AppendLine(");");

            sb.AppendLine("}");
        }

        /// <summary>
        /// Generates part of method definition: method name + [optional: list of type parameters] + list of parameters
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters">Parameters</param>
        /// <param name="gArgs">Type parameters</param>
        /// <param name="sb"></param>
        /// <param name="shouldNormalize">Use position index in names of parameters</param>
        /// <returns></returns>
        private void MethodSignatureCode(MethodInfo method, ParameterInfo[] parameters, Type[] gArgs, StringBuilder sb, bool shouldNormalize = false)
        {            
            sb.Append(method.Name);                        
            if (method.IsGenericMethod)            
                ListTypeParameters(gArgs, sb, shouldNormalize);            
            
            // add parameters list            
            sb.Append('(');
            DeclareParameters(parameters, sb, shouldNormalize);
            sb.AppendLine(")");     
        }

        /// <summary>
        /// Generates code for generic constraint
        /// </summary>
        /// <param name="args">Parameters</param>
        /// <returns></returns>
        private string CreateGenericConstraint(Type[] args)
        {
            if (args.Length == 0)
                return string.Empty;
            var constraintBuilder = new StringBuilder();
            foreach (var t in args.Where(x => x.IsGenericParameter))
            {
                Type[] gArgsConstraint = t.GetGenericParameterConstraints().Where(x => x != typeof (ValueType)).ToArray();
                var a = t.GenericParameterAttributes;

                // no generic constraints
                if (gArgsConstraint.Length == 0 && a == GenericParameterAttributes.None)
                    continue;

                constraintBuilder.Append("where ").Append(GetTypeName(t)).Append(" : ");

                if (gArgsConstraint.Length > 0)
                {
                    // constraint of base type or/and list of interfaces
                    // class type should always be the 1st in the sequence according to the syntax of generic constraint
                    foreach (var tc in gArgsConstraint)
                        constraintBuilder.Append(GetTypeName(tc)).Append(", ");
                }

                if (a != GenericParameterAttributes.None)
                {
                    if (a.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
                        constraintBuilder.Append("class, ");
                    if (a.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
                        constraintBuilder.Append("struct, ");
                    else if (a.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
                        constraintBuilder.Append("new(), ");
                }

                constraintBuilder.Remove(constraintBuilder.Length - 2, 2);
                constraintBuilder.AppendLine();
            }

            return constraintBuilder.ToString();
        }

        /// <summary>
        /// Generates code for method parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sb"></param>
        /// <param name="shouldNormalize"></param>
        private void DeclareParameters(ParameterInfo[] parameters, StringBuilder sb, bool shouldNormalize = false)
        {
            if (parameters.Length == 0)
                return;            
            for (int i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                sb.Append(i == parameters.Length - 1 && p.GetCustomAttributes(typeof (ParamArrayAttribute), false).Length > 0 ? "params ": string.Empty)
                    .Append(p.ParameterType.IsByRef ? (p.IsOut ? Out : Ref) : string.Empty)
                    .Append(GetTypeName(p.ParameterType, shouldNormalize))
                    .Append(' ')
                    .Append(shouldNormalize ? "_p" + p.Position : p.Name)
                    .Append(p.IsOptional ? "=" + GetOptionParamValue(p) : "")
                    .Append(", ");
            }
            sb.Remove(sb.Length - 2, 2);
        }

        /// <summary>
        /// Generates code for arguments list in method invokation
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sb"></param>
        private void ListArguments(ParameterInfo[] parameters, StringBuilder sb)
        {
            if (parameters.Length == 0)
                return;
            for (int i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                sb.Append(p.ParameterType.IsByRef ? (p.IsOut ? Out : Ref) : string.Empty).Append(p.Name).Append(", ");
            }
            
            sb.Remove(sb.Length - 2, 2);
        }

        /// <summary>
        /// Returns string with default value of parameter
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetOptionParamValue(ParameterInfo p)
        {
            var value = p.RawDefaultValue;
            if (value == null)
                return "null";
            if (value is string)
                return "\"" + value + "\"";
            return value.ToString();
        }

        /// <summary>
        /// Generates code for property
        /// </summary>
        /// <param name="property"></param>
        /// <param name="sb"></param>
        /// <param name="signatures"></param>
        /// <param name="propertyTypes"></param>
        private void PropertyCode(PropertyInfo property, StringBuilder sb, IList<string> signatures, IList<Type> propertyTypes)
        {
            var idxParams = property.GetIndexParameters();            
            bool isIndexer = idxParams.Length > 0;
            var signatureBuilder = new StringBuilder();
            
            PropertySignatureCode(property, signatureBuilder, idxParams, true);
            var signature = signatureBuilder.ToString();

            int idx = signatures.IndexOf(signature);
            bool shouldBeExplicit = idx >= 0;

            AttributeCode(property, sb);

            if (shouldBeExplicit)
            {
                if (propertyTypes[idx] == property.PropertyType)
                    return;
                sb.Append(GetTypeName(property.PropertyType)).Append(' ');
                sb.Append(GetTypeName(property.DeclaringType)).Append('.');
            }
            else
            {
                sb.Append("public ");
                sb.Append(GetTypeName(property.PropertyType)).Append(' ');
                signatures.Add(signature);
                propertyTypes.Add(property.PropertyType);
            }
            
            PropertySignatureCode(property, sb, idxParams);

            var propAccessor = new StringBuilder();
            propAccessor.Append(shouldBeExplicit
                                    ? string.Format("(({0}){1})", GetTypeName(property.DeclaringType), Impl)
                                    : Impl);
            if (isIndexer)
            {
                propAccessor.Append('[');
                ListArguments(idxParams, propAccessor);
                propAccessor.Append(']');
            }
            else
                propAccessor.Append('.').Append(property.Name);

            sb.AppendLine("{");

            if (property.CanRead)
                // create property getter
                sb.Append(Tab).Append("get { return ").Append(propAccessor).AppendLine("; }");            

            if (property.CanWrite)            
                // create property getter
                sb.Append(Tab).Append("set { ").Append(propAccessor).AppendLine(" = value; }");
            
            sb.AppendLine("}");
        }

        /// <summary>
        /// Generates code for property name (with parameters for index properties)
        /// </summary>
        /// <param name="property"></param>
        /// <param name="signatureBuilder"></param>
        /// <param name="idx"></param>
        /// <param name="shouldNormalize"></param>
        private void PropertySignatureCode(PropertyInfo property, StringBuilder signatureBuilder, ParameterInfo[] idx, bool shouldNormalize = false)
        {
            bool isIndexer = idx.Length > 0;
            if (isIndexer)
            {
                signatureBuilder.Append("this[");
                DeclareParameters(idx, signatureBuilder, shouldNormalize);
                signatureBuilder.AppendLine("]");
            }
            else
                signatureBuilder.AppendLine(property.Name);
        }

        private void AttributeCode(MemberInfo member, StringBuilder sb)
        {            
            foreach (var a in member.GetCustomAttributesData())            
                sb.AppendLine(a.ToString());            
        }

        /// <summary>
        /// Generates code for event
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="sb"></param>
        /// <param name="events"></param>
        /// <param name="eventTypes"></param>
        private void EventCode(EventInfo ev, StringBuilder sb, IList<string> events, IList<Type> eventTypes)
        {
            AttributeCode(ev, sb);
            int idx = events.IndexOf(ev.Name);
            bool shoudBeExplicit = idx >= 0;
            if (shoudBeExplicit)
            {
                if (ev.EventHandlerType == eventTypes[idx])
                    return;
                // explicit implementation of event
                sb.Append("event ")
                    .Append(GetTypeName(ev.EventHandlerType))
                    .Append(' ')
                    .Append(GetTypeName(ev.DeclaringType))
                    .Append('.')
                    .AppendLine(ev.Name);
                sb.AppendLine("{");
                sb.AppendLine("add    { throw new NotImplementedException(); }");
                sb.AppendLine("remove { throw new NotImplementedException(); }");
                sb.AppendLine("}");
            }
            else
            {
                sb.Append("public event ");
                sb.Append(GetTypeName(ev.EventHandlerType));
                sb.Append(' ');
                sb.Append(ev.Name);
                sb.AppendLine(";");
                events.Add(ev.Name);
                eventTypes.Add(ev.EventHandlerType);
            }
        }
    }
}

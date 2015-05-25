using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InterfaceImplementor
{
    public class ParameterCode:ICode
    {
        private static readonly string Ref = "ref ";
        private static readonly string Out = "out ";

        public ParameterCode(ParameterInfo paramInfo)
        {
            Type pt = paramInfo.ParameterType;
            ParameterType = pt;
            Name = paramInfo.Name;
            if (pt.IsByRef)
            {
                if (paramInfo.IsOut)
                    IsOut = true;
                else
                    IsRef = true;
            }
            else 
                IsParams = paramInfo.GetCustomAttributes(typeof (ParamArrayAttribute), false).Length > 0;
            Position = paramInfo.Position;
            IsParams = paramInfo.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0;
            if (paramInfo.IsOptional)
            {
                IsOptional = true;
                DefaultValue = paramInfo.RawDefaultValue;
            }
        }

        public bool IsRef { get; private set; }

        public bool IsOut { get; private set; }

        public bool IsParams { get; private set; }

        public TypeCode ParameterType { get; private set; }

        public string Name { get; private set; }

        public int Position { get; private set; }

        public bool IsOptional { get; private set; }

        public object DefaultValue { get; private set; }

        private string _stdName;
        public string StdName
        {
            get
            {
                if (_stdName == null)
                    _stdName = "_p" + Position;
                return _stdName;
            }
        }

        public string ArgumentCode
        {
            get { return string.Format("{0}{1}", IsRef ? Ref : IsOut ? Out : "", Name); }
        }

        public string Generate(bool stdForm)
        {
            var sb = new StringBuilder();
            if (IsRef)
                sb.Append(Ref);
            else if (IsOut)            
                sb.Append(stdForm ? Ref : Out);                      

            if (stdForm)
            {
                sb.Append(ParameterType.Generate(true)).Append(' ');
                sb.Append(StdName);
            }
            else
            {
                if (IsParams)                
                    sb.Append("params ");
                sb.Append(ParameterType.Generate(false)).Append(' ');
                sb.Append(Name);
                if (IsOptional)
                {
                    sb.Append(" = ");
                    sb.Append(GetOptionParamValue());
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns string with default value of parameter
        /// </summary>        
        /// <returns></returns>
        private string GetOptionParamValue()
        {
            var value = DefaultValue;
            if (value == null)
                return "null";
            if (value is string)
                return "\"" + value + "\"";
            return value.ToString();
        }
    }
}

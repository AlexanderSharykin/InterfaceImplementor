using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InterfaceImplementor
{
    /// <summary>
    /// Indexer property wrapper
    /// </summary>
    public class IndexerCode: ICode
    {
        private readonly List<ParameterCode> _args;

        public IndexerCode(PropertyInfo pr)
        {
            ParameterInfo[] pars = pr.GetIndexParameters();
            if (pars.Length == 0)
                throw new ArgumentException("Indexer expected");
            DeclaringType = pr.DeclaringType;
            IndexerType = pr.PropertyType;
            CanRead = pr.CanRead;
            CanWrite = pr.CanWrite;
            _args = pars.Select(pi => new ParameterCode(pi)).ToList();
        }

        public TypeCode DeclaringType { get; private set; }

        public TypeCode IndexerType { get; private set; }

        public bool CanRead { get; private set; }

        public bool CanWrite { get; private set; }

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
            return string.Format("this[{0}]", String.Join(", ", Parameters.Select(p => p.Generate(stdForm))));
        }
    }
}

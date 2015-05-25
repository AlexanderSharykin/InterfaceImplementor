using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfaceImplementor;
using NUnit.Framework;
using Tests.Types;

namespace Tests
{
    [TestFixture]
    public class GenericTests
    {
        private CodeBuilder _builder;

        [SetUp]
        public void Initialize()
        {
            _builder = new CodeBuilder();
        }


        [TestCase]
        public void CompileEnumerable()
        {
            var i = typeof(IEnumerable<string>);
            string code = _builder.ImplementInterface(i);
            Assert.AreEqual(true, Utils.CompileImplementation(code, i, _builder.ClassName));
        }

        [TestCase]
        public void CompileList()
        {
            var i = typeof(IList<string>);
            string code = _builder.ImplementInterface(i);
            Assert.AreEqual(true, Utils.CompileImplementation(code, i, _builder.ClassName));
        }

        [TestCase]
        public void CompileDictionary()
        {
            var i = typeof(IDictionary<string, Abc.NestedAbc>);
            string code = _builder.ImplementInterface(i);
            Assert.AreEqual(true, Utils.CompileImplementation(code, i, _builder.ClassName));
        }

        [TestCase]
        public void CompileGeneric()
        {
            var i = typeof(IGenericNoConstraint<object, object>);
            string code = _builder.ImplementInterface(i);
            Assert.AreEqual(true, Utils.CompileImplementation(code, i, _builder.ClassName));
        }

        [TestCase]
        public void CompileGenericMethodWithConstraint()
        {
            var i = typeof(IGenericMethodWithConstraint);
            string code = _builder.ImplementInterface(i);
            Assert.AreEqual(true, Utils.CompileImplementation(code, i, _builder.ClassName));
        }
    }
}

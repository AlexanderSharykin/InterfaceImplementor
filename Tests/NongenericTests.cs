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
    public class NongenericTests
    {
        private CodeBuilder _builder;

        [SetUp]
        public void Initialize()
        {
            _builder = new CodeBuilder();
        }

        [TestCase]
        public void CompileFoo()
        {
            var i = typeof(IFoo);
            string code = _builder.ImplementInterface(i);
            Assert.AreEqual(true, Utils.CompileImplementation(code, i, _builder.ClassName));
        }

        [TestCase]
        public void CompileFooOld()
        {
            var i = typeof(IFooOld);
            string code = _builder.ImplementInterface(i);
            Assert.AreEqual(true, Utils.CompileImplementation(code, i, _builder.ClassName));
        }

        [TestCase]
        public void CompileFooAmbiguous()
        {
            var i = typeof(IFooAmbiguous);
            string code = _builder.ImplementInterface(i);
            Assert.AreEqual(true, Utils.CompileImplementation(code, i, _builder.ClassName));
        }
    }
}

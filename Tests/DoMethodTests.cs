using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using InterfaceImplementor;
using NUnit.Framework;
using Tests.Types;

namespace Tests
{
    [TestFixture]
    public class DoMethodTests
    {
        private CodeBuilder _builder;

        [SetUp]
        public void Initialize()
        {
            _builder = new CodeBuilder();
        }

        [TestCase]
        public void CompileDo()
        {
            var i = typeof (IWork);
            string code = _builder.ImplementInterface(i);
            Assert.AreEqual(true, Utils.CompileImplementation(code, i, _builder.ClassName));            
        }

        [TestCase]
        public void CompileDoRef()
        {
            var i = typeof(IWorkRef);
            string code = _builder.ImplementInterface(i);
            Assert.AreEqual(true, Utils.CompileImplementation(code, i, _builder.ClassName));
        }

        [TestCase]
        public void CompileDoOut()
        {
            var i = typeof(IWorkOut);
            string code = _builder.ImplementInterface(i);
            Assert.AreEqual(true, Utils.CompileImplementation(code, i, _builder.ClassName));
        }

        [TestCase]
        public void CompileDoParams()
        {
            var i = typeof(IWorkParams);
            string code = _builder.ImplementInterface(i);
            Assert.AreEqual(true, Utils.CompileImplementation(code, i, _builder.ClassName));
        }

        [TestCase]
        public void CompileDoAmbiguous()
        {
            var i = typeof(IWorkAmbiguous);
            string code = _builder.ImplementInterface(i);
            Assert.AreEqual(true, Utils.CompileImplementation(code, i, _builder.ClassName));
        }

        [TestCase]
        public void CompileDoRefOutAmbiguous()
        {
            var i = typeof(IWorkRefOutAmbiguous);
            string code = _builder.ImplementInterface(i);
            Assert.AreEqual(true, Utils.CompileImplementation(code, i, _builder.ClassName));
        }
    }
}

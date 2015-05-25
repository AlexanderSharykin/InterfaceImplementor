using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.CodeDom;
using Microsoft.CSharp;
using Tests.Types;
using TypeCode = InterfaceImplementor.TypeCode;

namespace Tests
{
    [TestFixture]
    public class TypeNameTests
    {
        /// <summary>
        /// Built-in method to get readable type name
        /// </summary>
        /// <param name="type"></param>
        /// <see cref="http://stackoverflow.com/a/6592598/1506454"/>
        /// <returns></returns>
        private string GetFriendlyTypeName(Type type)
        {
            using (var p = new CSharpCodeProvider())
            {
                var r = new CodeTypeReference(type);
                return p.GetTypeOutput(r);
            }
        }

        [TestCase]
        public void VoidType()
        {
            var tVoid = typeof (void);
            var tc = new TypeCode(tVoid);
            Assert.AreEqual(GetFriendlyTypeName(tVoid), tc.FullName);
        }

        [TestCase]
        public void PrimitiveType()
        {
            var tBool = typeof(bool);
            var tc = new TypeCode(tBool);
            Assert.AreEqual(GetFriendlyTypeName(tBool), tc.FullName);
        }

        [TestCase]
        public void Array()
        {
            var tArr = typeof(int[]);
            var tc = new TypeCode(tArr);
            Assert.AreEqual(GetFriendlyTypeName(tArr), tc.FullName);
        }

        [TestCase]
        public void MultidimensionalArray()
        {
            var tArr = typeof(double[,,]);
            var tc = new TypeCode(tArr);
            Assert.AreEqual(GetFriendlyTypeName(tArr), tc.FullName);
        }

        [TestCase]
        public void GenericList()
        {
            var tGen = typeof(List<int>);
            var tc = new TypeCode(tGen);
            Assert.AreEqual(GetFriendlyTypeName(tGen), tc.FullName);
        }

        [TestCase]
        public void GenericDictionary()
        {
            var tGen = typeof(Dictionary<string, object>);
            var tc = new TypeCode(tGen);
            Assert.AreEqual(GetFriendlyTypeName(tGen), tc.FullName);
        }

        [TestCase]
        public void ArrayOfGeneric()
        {
            var tGen = typeof(IEnumerable<decimal>[]);
            var tc = new TypeCode(tGen);
            Assert.AreEqual(GetFriendlyTypeName(tGen), tc.FullName);
        }

        [TestCase]
        public void NestedType()
        {
            var tVoid = typeof(Abc.NestedAbc);
            var tc = new TypeCode(tVoid);
            Assert.AreEqual(GetFriendlyTypeName(tVoid), tc.FullName);
        }
    }
}

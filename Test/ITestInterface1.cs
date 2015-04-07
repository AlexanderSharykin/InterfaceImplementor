using System;
using System.Collections.Generic;
using System.ComponentModel;
using Nested = WrapperGenerator.Test.TestType.NestedTestType.AnotherNestedTestType;

namespace WrapperGenerator.Test
{
    public interface ITestInterface1
    {
        void Action();
        void Action(ref int source, out int target);

        void Q(ref int[] args);
        void X(bool[] args);
        void Y<TWww>(ref List<TWww>[] args);
        IEnumerable<string>[] Z<TWww>(out List<TWww>[] args);

        void Q(ref Nested[] args);
        void X(Nested[] args);
        void Y(ref List<Nested>[] args);
        IEnumerable<string>[] Z(out List<Nested>[] args);

        [Description("method")]
        [Demo(1, Message = "demo"), Demo(2, Message = null)]        
        void Do();
                
        void Process(params int[] args);

        Dictionary<string, IList<TValue>> GetDictionary<TValue>() 
            where TValue : class, new();

        [Description("indexed property")]
        object this[int index] { get; set; }
        

        object Code { get; }
        string Name { get; set; }

        [Description("event")]
        event EventHandler Load;
    }
}
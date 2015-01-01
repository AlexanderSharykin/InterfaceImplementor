using System;
using System.Collections.Generic;

namespace WrapperGenerator.Test
{
    public interface ITestInterface1
    {
        void Action();
        void Action(ref int source, out int target);

        void Do();
        
        void Process(params int[] args);

        Dictionary<string, IList<TValue>> GetDictionary<TValue>() 
            where TValue : class, new();

        object this[int index] { get; set; }

        object Code { get; }
        string Name { get; set; }

        event EventHandler Load;
    }
}
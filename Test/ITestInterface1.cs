using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WrapperGenerator.Test
{
    public interface ITestInterface1
    {
        void Action();
        void Action(ref int source, out int target);

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
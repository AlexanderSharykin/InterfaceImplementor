using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrapperGenerator.Test
{    
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class DemoAttribute : Attribute
    {
        private int _id;
        public DemoAttribute(int id)
        {
            _id = id;
        }

        public int Id { get { return _id; } }

        public string Name { get; set; }
        
        public string Message { get; set; }
    }
}

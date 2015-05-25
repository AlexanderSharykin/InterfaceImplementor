using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.Types
{
    public interface IFoo
    {
        int FooDo(string a, ref string b, out string c);        

        bool Getter { get; }
        bool Setter { set; }
        bool Normal { get; set; }

        object this[int idx] { get; set; }
        object this[int x, int y, int z] { get; set; }
        object this[string key] { get; }
        object this[bool key] { set; }

        event Action<object> Notification;
    }

    public interface IFooOld
    {
        string FooDo(string x, ref string y, out string z);

        string Normal { get; }

        double this[int i, int j, int k] { get; set; }

        event Action<string> Notification;       
    }

    public interface IFooAmbiguous: IFoo, IFooOld
    {        
    }
}

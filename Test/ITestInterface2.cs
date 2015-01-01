using System.Windows.Forms;

namespace WrapperGenerator.Test
{
    public interface ITestInterface2<V>: ITestInterface1 where V: Form
    {
        // duplicate method
        void Action();

        // new method. base method is hidden
        bool Do();

        string Code { get; set; }

        V Get(string code);        
    }
}
namespace Tests.Types
{
    public interface IWork
    {
        void Do(object[] args);
    }

    public interface IWorkRef
    {
        void Do(ref object[] args);
    }

    public interface IWorkOut
    {
        void Do(out object[] args);
    }

    public interface IWorkParams
    {
        void Do(params object[] args);
    }

    public interface IWorkAmbiguous: IWork, IWorkParams
    {        
    }

    public interface IWorkRefOutAmbiguous : IWorkRef, IWorkOut
    {
    }
}

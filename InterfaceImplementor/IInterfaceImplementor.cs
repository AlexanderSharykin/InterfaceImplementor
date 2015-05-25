namespace InterfaceImplementor
{
    public interface IInterfaceImplementor
    {
        string GenerateMethod(MethodCode method, bool isExplicit, TypeCode declaringType);

        string GenerateProperty(PropertyCode property, bool isExplicit, TypeCode declaringType);

        string GenerateIndexer(IndexerCode indexer, bool isExplicit, TypeCode declaringType);

        string GenerateEvent(EventCode ev, bool isExplicit, TypeCode declaringType);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.Types
{
    public interface IGenericNoConstraint<A, B>
    {
        Dictionary<A, B> GetPairs(int count);

        IEnumerable<A> Keys { get; }

        IEnumerable<B> Values { get; }

        bool ContainsKey(A key);

        Dictionary<A, T> Convert<T>(Func<B, T> converter);
    }

    public interface IGenericMethodWithConstraint
    {
        U Run<T, U>(T value) where U : T, new();
    }
}

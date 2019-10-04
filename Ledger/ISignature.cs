using System;

namespace Ledger
{
    public interface ISignature<T>
    {
        DateTime Timestamp { get; }
        string Value { get; }

        //ISignature<T> Create(T data, string hash, DateTime timestamp);
    }
}
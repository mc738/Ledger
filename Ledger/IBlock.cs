using System;

namespace Ledger
{
    public interface IBlock<T>
    {
        T Data { get; }
        IEntry<T> Entry { get; }
        string Hash { get; }
        int Index { get; }
        int Nonce { get; }
        string PreviousHash { get; }
        DateTime Timestamp { get; }

        //string CalculateHash();
        void GenerateHash(int difficulty, int nonce);
        void Mine();
        bool Validate(IBlock<T> block);
        bool Validate(ISignedData<T> data);
    }
}
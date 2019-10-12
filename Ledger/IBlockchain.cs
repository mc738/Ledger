using System;
using System.Collections.Generic;

namespace Ledger
{
    public interface IBlockchain<T>
    {
        ISignedData<T> this[int index] { get; }

        IEnumerable<IBlock<T>> Chain { get; }
        int Count { get; }
        int Difficulty { get; }
        IBlock<T> Last { get; }
        IEnumerable<ITag> Tags { get; }

        ISignedData<T> Add(T data, DateTime time, int nonce, params string[] tags);
        ISignedData<T> Add(T data, params string[] tags);
        void AddBlock(IBlock<T> block);
        void AddTag(ITag tag);
        bool Compare(IBlockchain<T> newChain);
        IBlock<T> GetBlock(int index, bool allRedirect = false);
        IEnumerable<ISignedData<T>> GetByTag(string tag);
        ISignedData<T> GetSingedData(int index);
        bool IsValid();
        string ToJson();
        string ToString();
        bool ValidateBlock(IBlock<T> block);
        bool ValidateData(ISignedData<T> data);
        void LoadFromJson(string json);
    }
}
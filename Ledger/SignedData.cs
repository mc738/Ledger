using System;
using System.Collections.Generic;
using System.Text;

namespace Ledger
{
    public class SignedData<T> : ISignedData<T>
    {
        private readonly int index;
        private readonly ISignature<T> signature;
        private readonly T data;

        public int Index => index;
        public ISignature<T> Signature => signature;
        public T Data => data;

        private SignedData(int index, string hash, T data)
        {
            this.index = index;
            this.signature = Signature<T>.Create(data, hash, DateTime.Now);
            this.data = data;
        }

        //public static SignedData<Transaction> CreateFromTransaction(Transaction transaction)
        //{
        //    return new SignedData<Transaction>(transaction.Index, transaction.Signature, transaction);
        //}

        public static ISignedData<T> CreateFromBlock(IBlock<T> block)
        {
            return new SignedData<T>(block.Index, block.Hash, block.Data);
        }
    }
}

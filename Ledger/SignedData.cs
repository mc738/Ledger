using System;
using System.Collections.Generic;
using System.Text;

namespace Ledger
{
    public class SignedData<T>
    {
        private readonly int index;
        private readonly string signature;
        private readonly T data;

        public int Index => index;
        public string Signature => signature;
        public T Data => data;

        private SignedData(int index, string signature, T data)
        {
            this.index = index;
            this.signature = signature;
            this.data = data;
        }

        public static SignedData<T> CreateFromBlock(Block<T> block)
        {
            return new SignedData<T>(block.Index, block.Hash, block.Data);
        }
    }
}

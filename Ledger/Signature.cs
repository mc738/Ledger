using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ledger
{
    public class Signature<T> : ISignature<T>
    {
        private readonly string hash;
        private readonly DateTime timestamp;


        public DateTime Timestamp => timestamp;

        public string Value { get; private set; }

        private Signature(string hash, DateTime timestamp)
        {
            this.hash = hash;
            this.timestamp = timestamp;
        }

        public static ISignature<T> Create(T data, string hash, DateTime timestamp)
        {
            var sig = new Signature<T>(hash, timestamp);

            sig.Sign(data);

            return sig;
        }


        private void Sign(T data)
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{Timestamp}-{hash}-{data}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            Value = Convert.ToBase64String(outputBytes);
        }

    }
}

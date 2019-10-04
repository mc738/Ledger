using System;
using System.Security.Cryptography;
using System.Text;

namespace Ledger
{
    public class Block<T> : IBlock<T>
    {
        private readonly int difficulty;

        private int index { get; set; }
        private DateTime timestamp { get; set; }
        private string previousHash { get; set; }
        private string hash { get; set; }
        private IEntry<T> data { get; set; }
        private int nonce { get; set; } = 0;


        public int Index => index;
        public DateTime Timestamp => timestamp;
        public string PreviousHash => previousHash;
        public string Hash => hash;
        public T Data => data.Data;
        public IEntry<T> Entry => data;
        public int Nonce => nonce;

        public Block(int index, DateTime timeStamp, string previousHash, T data, int difficulty)
        {
            this.index = index;
            this.timestamp = timeStamp;
            this.previousHash = previousHash;
            this.data = Entry<T>.Create(data);
            this.difficulty = difficulty;
            hash = CalculateHash();
        }

        public static IBlock<T> Create(IBlock<T> previousBlock, T data, int difficulty)
        {
            var block = new Block<T>(previousBlock.Index + 1, DateTime.Now, previousBlock.Hash, data, difficulty);

            return block;
        }

        public static IBlock<T> Create(IBlock<T> previousBlock, T data, int difficulty, DateTime timestamp, int nonce)
        {
            var block = new Block<T>(previousBlock.Index + 1, timestamp, previousBlock.Hash, data, difficulty);


            block.GenerateHash(difficulty, nonce);

            return block;
        }


        private string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{Timestamp}-{PreviousHash ?? ""}-{Data}-{Nonce}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }

        public void Mine()
        {
            var leadingZeros = new string('0', difficulty);
            while (this.Hash == null || this.Hash.Substring(0, difficulty) != leadingZeros)
            {
                this.nonce++;
                this.hash = this.CalculateHash();
            }
        }

        public void GenerateHash(int difficulty, int nonce)
        {
            this.nonce = nonce;
            var leadingZeros = new string('0', difficulty);

            this.hash = this.CalculateHash();


            //while (this.Hash == null || this.Hash.Substring(0, difficulty) != leadingZeros)
            //{               
            //}
        }


        public bool Validate(IBlock<T> block)
        {
            var temp = new Block<T>(Index, Timestamp, PreviousHash, block.Data, difficulty);

            temp.GenerateHash(2, nonce);

            return temp.Hash == Hash;
        }

        public bool Validate(ISignedData<T> data)
        {
            var temp = new Block<T>(Index, Timestamp, PreviousHash, data.Data, difficulty);

            //temp.CalculateHash();
            temp.GenerateHash(2, nonce);
            //temp.Mine(2);

            return temp.Hash == Hash;
        }


    }
}

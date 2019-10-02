using System;
using System.Security.Cryptography;
using System.Text;

namespace Ledger
{
    public class Block<T>
    {
        private readonly int difficulty;

        private int index { get; set; }
        private DateTime timestamp { get; set; }
        private string previousHash { get; set; }
        private string hash{ get; set; }
        private T data { get; set; }
        private int nonce { get; set; } = 0;


        public int Index => index;
        public DateTime Timestamp => timestamp;
        public string PreviousHash => previousHash;
        public string Hash => hash;
        public T Data => data;
        public int Nonce => nonce;


        public Block(int index, DateTime timeStamp, string previousHash, T data, int difficulty)
        {
            this.index = index;
            this.timestamp = timeStamp;
            this.previousHash = previousHash;
            this.data = data;
            this.difficulty = difficulty;
            hash = CalculateHash();
        }

        public static Block<T> Create(Block<T> previousBlock, T data, int difficulty)
        {
            var block = new Block<T>(previousBlock.index + 1, DateTime.Now, previousBlock.Hash, data, difficulty);

            return block;
        }

   
        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{Timestamp}-{PreviousHash ?? ""}-{Data}-{Nonce}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }

        public void Mine()
        {
            var leadingZeros = new string('0',  difficulty);
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


        public bool Validate(Block<T> block)
        {
            var temp = new Block<T>(Index, Timestamp, PreviousHash, block.Data, difficulty);

            temp.GenerateHash(2, nonce);

            return temp.Hash == Hash;
        }

        public bool Validate(SignedData<T> data)
        {
            var temp = new Block<T>(Index, Timestamp, PreviousHash, data.Data, difficulty);

            //temp.CalculateHash();
            temp.GenerateHash(2, nonce);
            //temp.Mine(2);

            return temp.Hash == Hash;
        }


    }
}

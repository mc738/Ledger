using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ledger
{
    public class Blockchain<T>
    {
        private readonly int difficulty;

        private IList<Block<T>> chain { set; get; }
     


        public IEnumerable<Block<T>> Chain => chain;
        public int Difficulty => difficulty;
        public Block<T> Last => GetLatestBlock();

        public Block<T> this[int index]
        {
            get { return index < chain.Count ? chain[index] : null; }
        }

        public Blockchain(int difficulty)
        {
            InitializeChain();
            AddGenesisBlock();
            this.difficulty = difficulty;
        }


        public void InitializeChain()
        {
            chain = new List<Block<T>>();
        }

        private Block<T> CreateGenesisBlock()
        {
            return new Block<T>(0, DateTime.Now, null, default(T), difficulty);
        }

        private void AddGenesisBlock()
        {
            chain.Add(CreateGenesisBlock());
        }

        private Block<T> GetLatestBlock()
        {
            return chain[chain.Count - 1];
        }

        public SignedData<T> Add(T data)
        {
            var latestBlock = GetLatestBlock();

            var block = Block<T>.Create(latestBlock, data, difficulty);

            block.Mine();
            chain.Add(block);

            return SignedData<T>.CreateFromBlock(block);

        }

        public SignedData<T> GetSingedData(int index)
        {           
            if (index >= chain.Count)
                return null;

            return SignedData<T>.CreateFromBlock(chain[index]);


        }

        public bool ValidateData(SignedData<T> data)
        {
            if (data.Index >= chain.Count)
                return false;

            return chain[data.Index].Validate(data);
        }


        public bool IsValid()
        {
            for (int i = 1; i < chain.Count; i++)
            {
                var currentBlock = chain[i];
                var previousBlock = chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }
            return true;
        }


        public bool ValidateBlock(Block<T> block)
        {
            if (block.Index >= chain.Count)
                return false;

            var blockInChain = chain[block.Index];

            return blockInChain.Validate(block);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}

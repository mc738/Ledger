using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ledger
{
    public class Blockchain<T> : IBlockchain<T>
    {
        private readonly int difficulty;

        private Dictionary<string, ITag> tags { get; set; }
        private IList<IBlock<T>> chain { set; get; }
        private bool allowRedirects { get; set; }

        public int Count => chain.Count;
        public IEnumerable<IBlock<T>> Chain => chain;
        public int Difficulty => difficulty;
        public IBlock<T> Last => GetLatestBlock();
        public IEnumerable<ITag> Tags => tags.Values;

        public ISignedData<T> this[int index]
        {
            get { return index < chain.Count ? SignedData<T>.CreateFromBlock(GetBlock(index, allowRedirects)) : null; }
        }

        public Blockchain(int difficulty, bool addGenesisBlock = true)
        {
            InitializeChain();

            if (addGenesisBlock)
                AddGenesisBlock();

            this.difficulty = difficulty;
            tags = new Dictionary<string, ITag>();
        }

        public static IBlockchain<T> CreateFromJson(string json)
        {
            var jObject = JObject.Parse(json);

            var chain = new Blockchain<T>(jObject["difficulty"].ToObject<int>(), false);

            foreach (var item in jObject["blocks"])
            {
                if (item["index"].ToObject<int>() == 0)
                {
                    var date = item["timestamp"].ToObject<DateTime>();

                    chain.AddGenesisBlock(date);
                }
                else
                {
                    chain.AddBlock(Block<T>.Create(chain.Last, item["data"].ToObject<T>(), chain.difficulty, item["timestamp"].ToObject<DateTime>(), item["nonce"].ToObject<int>()));

                }

            }

            return chain;
        }


        public void LoadFromJson(string json)
        {
            var jObject = JObject.Parse(json);

            chain.Clear();

            //chain = new Blockchain<T>(jObject["difficulty"].ToObject<int>(), false);

            foreach (var item in jObject["blocks"])
            {
                if (item["index"].ToObject<int>() == 0)
                {
                    var date = item["timestamp"].ToObject<DateTime>();

                    AddGenesisBlock(date);
                }
                else
                {
                    AddBlock(Block<T>.Create(Last, item["data"].ToObject<T>(), difficulty, item["timestamp"].ToObject<DateTime>(), item["nonce"].ToObject<int>()));

                }

            }
        }

        public string ToJson()
        {
            var jObject = new JObject();

            var blocks = new JArray();

            jObject.Add("difficulty", difficulty);

            foreach (var block in chain)
            {
                var blockObj = new JObject();

                blockObj.Add("index", block.Index);
                blockObj.Add("timestamp", block.Timestamp);

                if (block.Data != null)
                    blockObj.Add("data", JToken.FromObject(block.Data));

                blockObj.Add("nonce", block.Nonce);
                blockObj.Add("hash", block.Hash);
                blockObj.Add("previousHash", block.PreviousHash);

                blocks.Add(blockObj);
            }

            jObject.Add("blocks", blocks);

            return jObject.ToString();
        }

        public IBlock<T> GetBlock(int index, bool allRedirect = false)
        {
            if (index >= chain.Count)
                return null;

            var block = chain[index];

            if (allRedirect && block.Entry.HasRedirect)
                return GetBlock(block.Entry.Redirect);
            else
                return block;
        }

        public void AddTag(ITag tag)
        {
            if (!tags.ContainsKey(tag.Name))
                tags.Add(tag.Name, tag);
        }

        public IEnumerable<ISignedData<T>> GetByTag(string tag)
        {
            var result = new List<ISignedData<T>>();

            if (tags.ContainsKey(tag))
            {

                foreach (var index in tags[tag].Indexes)
                {
                    result.Add(SignedData<T>.CreateFromBlock(GetBlock(index, allowRedirects)));
                }

            }

            return result;
        }

        public bool Compare(IBlockchain<T> newChain)
        {
            //Find the max shared index between the 2 chains

            var sharedMaxIndex = Count > newChain.Count ? newChain.Count : Count;

            for (int i = 0; i < sharedMaxIndex; i++)
            {
                if (chain[i].Hash != newChain.GetBlock(i).Hash)
                    return false;
            }

            return true;
        }

        private void InitializeChain()
        {
            chain = new List<IBlock<T>>();
        }

        private IBlock<T> CreateGenesisBlock(DateTime? time = null)
        {
            return new Block<T>(0, time.HasValue ? time.Value : DateTime.Now, null, default(T), difficulty);
        }

        private void AddGenesisBlock(DateTime? time = null)
        {
            chain.Add(CreateGenesisBlock(time));
        }

        private IBlock<T> GetLatestBlock()
        {
            return chain[chain.Count - 1];
        }

        public ISignedData<T> Add(T data, params string[] tags)
        {
            var latestBlock = GetLatestBlock();

            var block = Block<T>.Create(latestBlock, data, difficulty);

            block.Mine();
            chain.Add(block);

            foreach (var tag in tags)
            {
                if (this.tags.ContainsKey(tag))
                    this.tags[tag].AddIndex(block.Index);
            }

            return SignedData<T>.CreateFromBlock(block);

        }

        public ISignedData<T> Add(T data, DateTime time, int nonce, params string[] tags)
        {
            var latestBlock = GetLatestBlock();

            var block = Block<T>.Create(latestBlock, data, difficulty, time, nonce);

            block.Mine();
            chain.Add(block);

            foreach (var tag in tags)
            {
                if (this.tags.ContainsKey(tag))
                    this.tags[tag].AddIndex(block.Index);
            }

            return SignedData<T>.CreateFromBlock(block);

        }

        public void AddBlock(IBlock<T> block)
        {

            chain.Add(block);
        }

        public ISignedData<T> GetSingedData(int index)
        {
            if (index >= chain.Count)
                return null;

            return SignedData<T>.CreateFromBlock(chain[index]);


        }

        public bool ValidateData(ISignedData<T> data)
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

                if (currentBlock.Hash != currentBlock.Hash)
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


        public bool ValidateBlock(IBlock<T> block)
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

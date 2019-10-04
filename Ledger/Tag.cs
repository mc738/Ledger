using System;
using System.Collections.Generic;
using System.Text;

namespace Ledger
{
    public class Tag : ITag
    {
        private readonly string name;
        private List<int> indexes { get; set; }

        public string Name => name;
        public IEnumerable<int> Indexes => indexes;

        public Tag(string name)
        {
            this.name = name;
            indexes = new List<int>();
        }

        public void AddIndex(int index)
        {
            indexes.Add(index);
        }

        public void RemoveIndex(int index)
        {
            indexes.RemoveAll(x => x == index);
        }

    }
}

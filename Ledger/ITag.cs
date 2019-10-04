using System.Collections.Generic;

namespace Ledger
{
    public interface ITag
    {
        IEnumerable<int> Indexes { get; }
        string Name { get; }

        void AddIndex(int index);
        void RemoveIndex(int index);
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Ledger
{
    public class Entry<T> : IEntry<T>
    {
        private readonly T data;



        private int? redirect { get; set; }
        private List<int> links { get; set; }


        public T Data => data;
        public int Redirect => redirect.HasValue ? redirect.Value : 0;
        public bool HasRedirect => redirect.HasValue;

        private Entry(T data)
        {
            this.data = data;
        }

        public static Entry<T> Create(T data)
        {
            return new Entry<T>(data);
        }

        public void SetRedirect(int index)
        {
            redirect = index;
        }

        public void ClearRedirect()
        {
            redirect = null;
        }
    }
}

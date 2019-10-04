using Ledger;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();

            int difficulty = 3;

            var chain = new Blockchain<int>(difficulty);

            chain.AddTag(new Tag("even"));
            chain.AddTag(new Tag("odd"));

            var data = new List<ISignedData<int>>();

            for (int i = 0; i < 10; i++)
            {
                var tag = i % 2 == 0 ? "even" : "odd"; 

                data.Add(chain.Add(i, tag));
            }

            Console.WriteLine($"Chain is:{chain.IsValid()}");

            var d = chain[7];

           
 
            Console.WriteLine($"Fake block is valid: { chain.ValidateData(d) }");

 
            foreach (var item in data)
            {
              Console.WriteLine($"Data at index '{item.Index}' is valided: {chain.ValidateData(item)}");
            }

           var odd = chain.GetByTag("odd");

            var json = chain.ToJson();

          
            var newChain = Blockchain<int>.CreateFromJson(json);

            

            Console.WriteLine($"New chain is valid: {newChain.IsValid()}");
            Console.WriteLine($"Data is valid: {newChain.ValidateData(d)}");
            Console.WriteLine($"Chains compared and equal: {chain.Compare(newChain)}");

        }

        static void Test()
        {
            var chain = Blockchain<int>.CreateFromJson(File.ReadAllText(@"C:\Users\MaxClifford\source\repos\Ledger\TestChain.json"));
            var fakeChain = Blockchain<int>.CreateFromJson(File.ReadAllText(@"C:\Users\MaxClifford\source\repos\Ledger\FakeChain.json"));

            Console.WriteLine($"Chains are the same? {chain.Compare(fakeChain)}");
        }
    }
}

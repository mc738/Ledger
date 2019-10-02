using Ledger;
using System;
using System.Collections.Generic;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int difficulty = 3;

            var chain = new Blockchain<int>(difficulty);

            var data = new List<SignedData<int>>();

            for (int i = 0; i < 10; i++)
            {
                data.Add(chain.Add(i));
            }

            Console.WriteLine($"Chain is:{chain.IsValid()}");

            var prevBlock = chain[7];
            var block = chain[8];

            var fakeBlock = Block<int>.Create(prevBlock, 1000, difficulty);
 
            Console.WriteLine($"Fake block is valid: { block.Validate(fakeBlock) }");
            Console.WriteLine($"Real block is valid: { block.Validate(block) }");


            foreach (var item in data)
            {
              Console.WriteLine($"Data at index '{item.Index}' is valided? {chain.ValidateData(item)}");
            }

            

            //Console.WriteLine(chain.ToString());


        }
    }
}

Ledger
======
Ledger is a library for creating blockchains and integrated them into applications. 
Blockchains are used as a data structure that can hold generic data.
Data can be added, allowing it to be signed and easier validated against the chain.
Chains support serialization/deserialization in to JSON, allowing for easy storage and retrieval.

Struture
=========

## Blockchain

  The blockchain is a generic collection that holds ordered, verifiable data. It consists of a collection of blocks
  
## Block

  The block is a generic data structure that specific data. 

## Entry

  The wrapper in the block  that holds the data, as well as other information such as redirects.

## SignedData

  A generic data structure holding a peice of data and the information needed to validate it agaisnt the blockchain.
  
## Signature

  A timestamped signature generated from SignedData that can be used for validation without needing the data.

## Tag

  A name an collection of indexes in a chain, allowing for easier retrieval of data from the chain.

Examples
========

## Create a chain, add to it, and validate it
```C#
 //Create a change and set it's difficulty
 var chain = new Blockchain<string>(3);

 //Add data to the chain
 SingedData<string> data = chain.Add("Hello World!", tag);
 
 //Validate data
 bool isValid = chain.ValidateData(data);
```            

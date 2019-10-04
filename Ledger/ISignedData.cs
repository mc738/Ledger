namespace Ledger
{
    public interface ISignedData<T>
    {
        T Data { get; }
        int Index { get; }
        ISignature<T> Signature { get; }
    }
}
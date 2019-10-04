namespace Ledger
{
    public interface IEntry<T>
    {
        T Data { get; }
        bool HasRedirect { get; }
        int Redirect { get; }

        void ClearRedirect();
        void SetRedirect(int index);
    }
}
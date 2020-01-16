namespace FinanceSharp.Data {
    public unsafe delegate void ManipulateStructHandler<TStruct>(TStruct* input) where TStruct : unmanaged, DataStruct;
}
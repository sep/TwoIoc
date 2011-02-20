namespace Net35
{
    public delegate TResult Func<TResult>();
    public delegate TResult Func<T1, TResult>(T1 param1);
    public delegate TResult Func<T1, T2, TResult>(T1 param1, T2 param2);
    public delegate TResult Func<T1, T2, T3, TResult>(T1 param1, T2 param2, T3 param3);
    public delegate TResult Func<T1, T2, T3, T4, TResult>(T1 param1, T2 param2, T3 param3, T4 param4);

    public delegate void Action();
    public delegate void Action<T1, T2>(T1 param1, T2 param2);
    public delegate void Action<T1, T2, T3>(T1 param1, T2 param2, T3 param3);
    public delegate void Action<T1, T2, T3, T4>(T1 param1, T2 param2, T3 param3, T4 param4);
}
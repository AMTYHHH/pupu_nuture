//定义了具有不同参数数量的委托类型，提供给EventCenter使用
public delegate void CallBack();
public delegate void CallBack<T>(T arg);
public delegate void CallBack<T, X>(T arg1, X arg2);
public delegate void CallBack<T, X, Y>(T arg1, X arg2, Y arg3);
public delegate void CallBack<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);
public delegate void CallBack<T, X, Y, Z, W>(T arg1, X arg2, Y arg3, Z arg4, W arg5);
﻿namespace Senparc.Core.Cache
{
    public interface ICommonDataCache<T> : IBaseCache<T> where T : class, new()
    {
    }
}
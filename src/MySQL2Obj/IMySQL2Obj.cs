﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MySQL2ObjWrapper
{
    public interface IMySQL2Obj
    {
        
        Task<List<T>> QueryAsync<T>(string SQLQuery, Dictionary<string, object> Params) where T : new();
        Task<List<T>> QueryAsync<T>(string SQLQuery) where T : new();
        List<T> Query<T>(string SQLQuery, Dictionary<string, object> Params) where T : new();
        List<T> Query<T>(string SQLQuery) where T : new();
        Task<int> QueryAsync(string SQLQuery, Dictionary<string, object> Params);
        Task<int> QueryAsync(string SQLQuery);
        int Query(string SQLQuery, Dictionary<string, object> Params);
        int Query(string SQLQuery);
    }
}

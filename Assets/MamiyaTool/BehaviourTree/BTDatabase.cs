using System;
using System.Collections.Generic;

namespace MamiyaTool
{
    public class BTDatabase
    {
        private Dictionary<string, IBTData> datas;
        /******************************************************************
         * 
         * 
         * 
         ******************************************************************/
        public BTDatabase() {
            datas = new Dictionary<string, IBTData>();
        }
        public void AddData(string key, IBTData data)
        {
            datas[key] = data;
        }
        public IBTData GetData(string key)
        {
            if(datas.TryGetValue(key, out IBTData data))
                return data;
            return null;
        }
        public T GetData<T>(string key)
        {
            IBTData data = GetData(key);
            var typeData = data as IBTData<T>;
            if(typeData != null)
                return typeData.Data;
            return default;
        }
        public void UpdateData<T>(string key, T data)
        {
            IBTData btData = GetData(key);
            if(btData == null)
                return;

            IBTData<T> typeData = btData as IBTData<T>;
            typeData.Data = data;
        }
        public bool RemoveData(string key)
        {
            return datas.Remove(key);
        }
        public void Clear()
        {
            datas.Clear();
        }
    }
}
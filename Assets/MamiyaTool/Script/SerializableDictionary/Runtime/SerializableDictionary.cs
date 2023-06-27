using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MamiyaTool
{
    public class SerializableDictionary { }
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : SerializableDictionary, IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        // 真正的数据结构
        [SerializeField] private List<SerializableKeyValuePair> list = new List<SerializableKeyValuePair>();
        // 访问用字典
        private Lazy<Dictionary<TKey, int>> m_keyPositions;
        // 延迟初始化：每次访问时构造新的访问器
        private Dictionary<TKey, int> keyPositions => m_keyPositions.Value;
        // 构造方法
        public SerializableDictionary()
        {
            m_keyPositions = new Lazy<Dictionary<TKey, int>>(MakeKeyPositions);
        }
        private Dictionary<TKey, int> MakeKeyPositions()
        {
            Dictionary<TKey, int> dictionary = new Dictionary<TKey, int>(list.Count);
            for(int i = 0; i < list.Count; i++)
                dictionary[list[i].Key] = i;
            return dictionary;
        }
        #region ISerializationCallbackReceiver
        // 反序列化方法
        public void OnAfterDeserialize()
        {
            m_keyPositions = new Lazy<Dictionary<TKey, int>>(MakeKeyPositions);
        }
        public void OnBeforeSerialize() { }
        #endregion
        #region IDictionary<TKey, TValue>
        public TValue this[TKey key] {
            get => list[keyPositions[key]].Value;
            set {
                SerializableKeyValuePair pair = new SerializableKeyValuePair(key, value);
                if(keyPositions.ContainsKey(key))
                    list[keyPositions[key]] = pair;
                else {
                    keyPositions.Add(key, list.Count);
                    list.Add(pair);
                }
            }
        }
        public ICollection<TKey> Keys => list.Select(v => v.Key).ToArray();
        public ICollection<TValue> Values => list.Select(v => v.Value).ToArray();
        public void Add(TKey key, TValue value)
        {
            SerializableKeyValuePair pair = new SerializableKeyValuePair(key, value);
            if(keyPositions.ContainsKey(key))
                throw new ArgumentException("An element with the same key already exists in the dictionary.");
            else {
                keyPositions.Add(key, list.Count);
                list.Add(pair);
            }
        }
        public bool ContainsKey(TKey key) => keyPositions.ContainsKey(key);
        public bool Remove(TKey key)
        {
            if(keyPositions.TryGetValue(key, out int index)) {
                keyPositions.Remove(key);
                list.RemoveAt(index);
                for(int i = index; i < list.Count; i++)
                    keyPositions[list[i].Key] = i;
                return true;
            } else
                return false;
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            if(keyPositions.TryGetValue(key, out int index)) {
                value = list[index].Value;
                return true;
            } else {
                value = default;
                return false;
            }
        }
        #endregion
        #region ICollection <KeyValuePair<TKey, TValue>>
        public int Count => list.Count;
        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);
        public void Clear() => list.Clear();
        public bool Contains(KeyValuePair<TKey, TValue> item) => ContainsKey(item.Key);
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            int numbers = list.Count;
            if(array.Length - arrayIndex < numbers)
                throw new ArgumentException("arrayIndex");
            for(int i = 0; i < numbers; i++) {
                SerializableKeyValuePair entry = list[i];
                array[arrayIndex] = ToKeyValuePair(entry);
            }
        }
        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);
        #endregion
        #region IEnumerable<KeyValuePair<TKey, TValue>>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return list.Select(ToKeyValuePair).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public bool IsReadOnly => false;
        private KeyValuePair<TKey, TValue> ToKeyValuePair(SerializableKeyValuePair skvp)
        {
            return new KeyValuePair<TKey, TValue>(skvp.Key, skvp.Value);
        }
        #endregion

        [Serializable]
        public struct SerializableKeyValuePair
        {
            public TKey Key;
            public TValue Value;

            public SerializableKeyValuePair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}
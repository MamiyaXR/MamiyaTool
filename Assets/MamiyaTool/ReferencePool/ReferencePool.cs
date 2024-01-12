using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MamiyaTool
{
    #region 引用接口
    public interface IReference
	{
		/// <summary>
		/// 清理引用
		/// </summary>
		void Clear();
	}
	#endregion

	#region 引用池
	public static class ReferencePool
	{
		private static readonly Dictionary<Type, ReferenceCollection> m_ReferenceCollections = new Dictionary<Type, ReferenceCollection>();
		/// <summary>
		/// 强制检查
		/// </summary>
		public static bool EnableStrictCheck { get; set; } = false;
		/// <summary>
		/// 引用池的数量
		/// </summary>
		public static int Count => m_ReferenceCollections.Count;
		/// <summary>
		/// 获取所有引用池信息
		/// </summary>
		/// <returns>所有引用池信息</returns>
		public static ReferencePoolInfo[] GetAllReferencePoolInfos()
        {
			int index = 0;
			ReferencePoolInfo[] result = null;

			lock(m_ReferenceCollections) {
				result = new ReferencePoolInfo[m_ReferenceCollections.Count];
				foreach(var collection in m_ReferenceCollections) {
					result[index++] = new ReferencePoolInfo(collection.Key,
						collection.Value.UnusedReferenceCount,
						collection.Value.UsingReferenceCount,
						collection.Value.AcquireReferenceCount,
						collection.Value.ReleaseReferenceCount,
						collection.Value.AddReferenceCount,
						collection.Value.RemoveReferenceCount);
                }
            }

			return result;
        }
		/// <summary>
		/// 清除所有引用池
		/// </summary>
		public static void ClearAll()
        {
			lock(m_ReferenceCollections) {
				foreach(var collection in m_ReferenceCollections) {
					collection.Value.RemoveAll();
                }

				m_ReferenceCollections.Clear();
            }
        }
		/// <summary>
		/// 从引用池获取引用
		/// </summary>
		/// <typeparam name="T">引用类型</typeparam>
		/// <returns>引用</returns>
		public static T Acquire<T>() where T : class, IReference, new()
        {
			return GetReferenceCollection(typeof(T)).Acquire<T>();
        }
		/// <summary>
		/// 从引用池获取引用
		/// </summary>
		/// <param name="type">引用类型</param>
		/// <returns>引用</returns>
		public static IReference Acquire(Type type)
        {
			InternalCheckReferenceType(type);
			return GetReferenceCollection(type).Acquire();
        }
		/// <summary>
		/// 回收引用
		/// </summary>
		/// <param name="reference">引用</param>
		public static void Release(IReference reference)
        {
			if(reference == null)
				throw new Exception("Reference is invalid.");

			Type type = reference.GetType();
			InternalCheckReferenceType(type);
			GetReferenceCollection(type).Release(reference);
        }
		/// <summary>
		/// 向引用池中追加指定数量的引用
		/// </summary>
		/// <typeparam name="T">引用类型</typeparam>
		/// <param name="count">追加数量</param>
		public static void Add<T>(int count) where T : class, IReference, new()
        {
			GetReferenceCollection(typeof(T)).Add(count);
        }
		/// <summary>
		/// 向引用池中追加指定数量的引用
		/// </summary>
		/// <param name="type">引用类型</param>
		/// <param name="count">追加数量</param>
		public static void Add(Type type, int count)
        {
			InternalCheckReferenceType(type);
			GetReferenceCollection(type).Add(count);
        }
		/// <summary>
		/// 从引用池中移除指定数量的引用
		/// </summary>
		/// <typeparam name="T">引用类型</typeparam>
		/// <param name="count">移除数量</param>
		public static void Remove<T>(int count) where T : class, IReference
        {
			GetReferenceCollection(typeof(T)).Remove(count);
        }
		/// <summary>
		/// 从引用池中移除指定数量的引用
		/// </summary>
		/// <param name="type">引用类型</param>
		/// <param name="count">移除数量</param>
		public static void Remove(Type type, int count)
        {
			InternalCheckReferenceType(type);
			GetReferenceCollection(type).Remove(count);
        }
		/// <summary>
		/// 从引用池中移除所有引用
		/// </summary>
		/// <typeparam name="T">引用类型</typeparam>
		public static void RemoveAll<T>() where T : class, IReference
        {
			GetReferenceCollection(typeof(T)).RemoveAll();
        }
		/// <summary>
		/// 从引用池中移除所有引用
		/// </summary>
		/// <param name="type">引用类型</param>
		public static void RemoveAll(Type type)
        {
			InternalCheckReferenceType(type);
			GetReferenceCollection(type).RemoveAll();
        }

		private static void InternalCheckReferenceType(Type type)
        {
			if(!EnableStrictCheck)
				return;

			if(type == null)
				throw new Exception("Reference type is invalid.");

			if(!type.IsClass || type.IsAbstract)
				throw new Exception("Reference type is not a non-abstract class type.");

			if(!typeof(IReference).IsAssignableFrom(type))
				throw new Exception($"Reference type '{type.FullName}' is invalid");
        }
		private static ReferenceCollection GetReferenceCollection(Type type)
        {
			if(type == null) {
				throw new Exception("ReferenceType is invalid.");
            }

			ReferenceCollection result = null;
			lock(m_ReferenceCollections) {
				if(!m_ReferenceCollections.TryGetValue(type, out result)) {
					result = new ReferenceCollection(type);
					m_ReferenceCollections.Add(type, result);
                }
            }
			return result;
        }

        private sealed class ReferenceCollection
		{
			private readonly Queue<IReference> m_References;
			public Type ReferenceType => m_ReferenceType;
			private readonly Type m_ReferenceType;
			/// <summary>
			/// 池子中尚未使用的对象个数
			/// </summary>
			public int UnusedReferenceCount => m_References.Count;
			/// <summary>
			/// 正在使用尚未回收的对象个数
			/// </summary>
			public int UsingReferenceCount => m_UsingReferenceCount;
			private int m_UsingReferenceCount;
			/// <summary>
			/// 从池子中获取对象的次数
			/// </summary>
			public int AcquireReferenceCount => m_AcquireReferenceCount;
			private int m_AcquireReferenceCount;
			/// <summary>
			/// 回收对象的次数
			/// </summary>
			public int ReleaseReferenceCount => m_ReleaseReferenceCount;
			private int m_ReleaseReferenceCount;
			/// <summary>
			/// 往池子中添加对象的次数
			/// </summary>
			public int AddReferenceCount => m_AddReferenceCount;
			private int m_AddReferenceCount;
			/// <summary>
			/// 从池子移除对象的次数
			/// </summary>
			public int RemoveReferenceCount => m_RemoveReferenceCount;
			private int m_RemoveReferenceCount;
			
			public ReferenceCollection(Type referenceType)
			{
				m_References = new Queue<IReference>();
				m_ReferenceType = referenceType;

				m_UsingReferenceCount = 0;
				m_AcquireReferenceCount = 0;
				m_ReleaseReferenceCount = 0;
				m_AddReferenceCount = 0;
				m_RemoveReferenceCount = 0;
			}
			public T Acquire<T>() where T : class, IReference, new()
			{
				if(typeof(T) != m_ReferenceType)
					throw new Exception("Type is invalid.");

				m_UsingReferenceCount++;
				m_AcquireReferenceCount++;
				lock(m_References) {
					if(m_References.Count > 0) {
						return (T)m_References.Dequeue();
					}
				}

				m_AddReferenceCount++;
				return new T();
			}
			public IReference Acquire()
			{
				m_UsingReferenceCount++;
				m_AcquireReferenceCount++;
				lock(m_References) {
					if(m_References.Count > 0) {
						return m_References.Dequeue();
					}
				}

				m_AddReferenceCount++;
				return (IReference)Activator.CreateInstance(m_ReferenceType);
			}
			public void Release(IReference reference)
			{
				reference.Clear();
				lock(m_References) {
					if(EnableStrictCheck && m_References.Contains(reference)) {
						throw new Exception("The reference has been released.");
                    }

					m_References.Enqueue(reference);
				}

				m_ReleaseReferenceCount++;
				m_UsingReferenceCount--;
			}
			public void Add(int count)
            {
				lock(m_References) {
					m_AddReferenceCount += count;
					while(count-- > 0) {
						m_References.Enqueue((IReference)Activator.CreateInstance(m_ReferenceType));
                    }
                }
            }
			public void Remove(int count)
            {
				lock(m_References) {
					if(count > m_References.Count) {
						count = m_References.Count;
					}

					m_RemoveReferenceCount += count;
					while(count-- > 0) {
						m_References.Dequeue();
                    }
                }
            }
			public void RemoveAll()
            {
				lock(m_References) {
					m_RemoveReferenceCount += m_References.Count;
					m_References.Clear();
                }
            }
		}
	}
    #endregion

    #region 引用信息
    [StructLayout(LayoutKind.Auto)]
	public struct ReferencePoolInfo
	{
		public readonly Type Type;
		public readonly int UnusedReferenceCount;
		public readonly int UsingReferenceCount;
		public readonly int AcquireReferenceCount;
		public readonly int ReleaseReferenceCount;
		public readonly int AddReferenceCount;
		public readonly int RemoveReferenceCount;

		public ReferencePoolInfo(Type type, int unusedReferenceCount, int usingReferenceCount, int acquireReferenceCount, int releaseReferenceCount, int addReferenceCount, int removeReferenceCount)
		{
			Type = type;
			UnusedReferenceCount = unusedReferenceCount;
			UsingReferenceCount = usingReferenceCount;
			AcquireReferenceCount = acquireReferenceCount;
			ReleaseReferenceCount = releaseReferenceCount;
			AddReferenceCount = addReferenceCount;
			RemoveReferenceCount = removeReferenceCount;
		}
	}
	#endregion
}
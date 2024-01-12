using System;
using System.Reflection;

namespace MamiyaTool
{
    #region 单例接口
    public interface ISingleton
    {
        void OnSingletonInit();
    }
    #endregion

    #region 单例基类
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        protected static T m_Instance;
        private static object m_Lock = new object();
        public static T Instance {
            get {
                lock(m_Lock) {
                    if(m_Instance == null)
                        m_Instance = SingletonCreator.CreateSingleton<T>();
                }
                return m_Instance;
            }
        }
        protected Singleton() { }
        public virtual void Dispose() { m_Instance = null; }
        public virtual void OnSingletonInit() { }

        #region 单例生成器
        private static class SingletonCreator
        {
            public static T CreateSingleton<T>() where T : class, ISingleton
            {
                Type type = typeof(T);
                ConstructorInfo[] ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                if(ctor == null)
                    throw new Exception($"Non-Public Constructor() not found! in {typeof(T)}");

                T result = ctor.Invoke(null) as T;
                result.OnSingletonInit();
                return result;
            }
        }
        #endregion
    }
    #endregion
}
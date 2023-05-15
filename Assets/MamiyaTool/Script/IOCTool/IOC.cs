using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MamiyaTool {
    #region InjectAttribute
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InjectAttribute : Attribute {
        public string Name { get; set; }
        public InjectAttribute(string name) {
            Name = name;
        }
        public InjectAttribute() {

        }
    }
    #endregion

    #region IMContainer
    public interface IMContainer {
        void Clear();
        void Inject(object obj);
        void InjectAll();
        void Register<TSource, TTarget>(string name = null);
        void RegisterRelation<TFor, TBase, TConcrete>();
        void RegisterInstance<TBase>(TBase @default, bool injectNow) where TBase : class;
        void RegisterInstance(Type type, object @default, bool injectNow);
        void RegisterInstance(Type baseType, object instance = null, string name = null, bool injectNow = true);
        void RegisterInstance<TBase>(TBase instance, string name, bool injectNow = true) where TBase : class;
        void RegisterInstance<TBase>(TBase instance) where TBase : class;
        T Resolve<T>(string name = null, bool requireInstance = false, params object[] args) where T : class;
        TBase ResolveRelation<TBase>(Type tfor, params object[] arg);
        TBase ResolveRelation<TFor, TBase>(params object[] arg);
        IEnumerable<TType> ResolveAll<TType>();
        void Register(Type source, Type target, string name = null);
        IEnumerable<object> ResolveAll(Type type);
        TypeMappingCollection Mappings { get; set; }
        TypeInstanceCollection Instances { get; set; }
        TypeRelationCollection RelationshipMappings { get; set; }
        object Resolve(Type baseType, string name = null, bool requireInstance = false,
            params object[] constructorArgs);
        object ResolveRelation(Type tfor, Type tbase, params object[] arg);
        void RegisterRelation(Type tfor, Type tbase, Type tconcrete);
        object CreateInstance(Type type, params object[] args);
    }
    #endregion

    #region collection
    public class Tuple<T1, T2>
    {
        public readonly T1 Item1;
        public readonly T2 Item2;

        public Tuple(T1 item1, T2 item2) {
            Item1 = item1;
            Item2 = item2;
        }

        public override bool Equals(Object obj) {
            Tuple<T1, T2> p = obj as Tuple<T1, T2>;
            if(obj == null)
                return false;

            if(Item1 == null) {
                if(p.Item1 != null)
                    return false;
            } else {
                if(p.Item1 == null || !Item1.Equals(p.Item1))
                    return false;
            }

            if(Item2 == null) {
                if(p.Item2 != null)
                    return false;
            } else {
                if(p.Item2 == null || !Item2.Equals(p.Item2))
                    return false;
            }

            return true;
        }

        public override int GetHashCode() {
            int hash = 0;
            if(Item1 != null)
                hash ^= Item1.GetHashCode();
            if(Item2 != null)
                hash ^= Item2.GetHashCode();
            return hash;
        }
    }
    public class TypeMappingCollection : Dictionary<Tuple<Type, string>, Type> {
        public Type this[Type from, string name = null] {
            get {
                Tuple<Type, string> key = new Tuple<Type, string>(from, name);
                Type mapping = null;
                if(this.TryGetValue(key, out mapping))
                    return mapping;
                return null;
            }
            set {
                Tuple<Type, string> key = new Tuple<Type, string>(from, name);
                this[key] = value;
            }
        }
    }
    public class TypeInstanceCollection : Dictionary<Tuple<Type, string>, object> {
        public object this[Type from, string name = null] {
            get {
                Tuple<Type, string> key = new Tuple<Type, string>(from, name);
                object mapping = null;
                if(this.TryGetValue(key, out mapping))
                    return mapping;
                return null;
            }
            set {
                Tuple<Type, string> key = new Tuple<Type, string>(from, name);
                this[key] = value;
            }
        }
    }
    public class TypeRelationCollection : Dictionary<Tuple<Type, Type>, Type> {
        public Type this[Type from, Type to] {
            get {
                Tuple<Type, Type> key = new Tuple<Type, Type>(from, to);
                Type mapping = null;
                if(this.TryGetValue(key, out mapping))
                    return mapping;
                return null;
            }
            set {
                Tuple<Type, Type> key = new Tuple<Type, Type>(from, to);
                this[key] = value;
            }
        }
    }
    #endregion

    #region MContainer
    public class MContainer : IMContainer {
        private TypeInstanceCollection _instances;
        private TypeMappingCollection _mappings;
        private TypeRelationCollection _relationshipMappings;

        public TypeMappingCollection Mappings {
            get { return _mappings ?? (_mappings = new TypeMappingCollection()); }
            set { _mappings = value; }
        }
        public TypeInstanceCollection Instances {
            get { return _instances ?? (_instances = new TypeInstanceCollection()); }
            set { _instances = value; }
        }
        public TypeRelationCollection RelationshipMappings {
            get { return _relationshipMappings ?? (_relationshipMappings = new TypeRelationCollection()); }
            set { _relationshipMappings = value; }
        }
        public IEnumerable<TType> ResolveAll<TType>() {
            foreach(var obj in ResolveAll(typeof(TType))) {
                yield return (TType)obj;
            }
        }

        public IEnumerable<object> ResolveAll(Type type) {
            foreach(KeyValuePair<Tuple<Type, string>, object> kv in Instances) {
                if(kv.Key.Item1 == type && !string.IsNullOrEmpty(kv.Key.Item2))
                    yield return kv.Value;
            }

            foreach(KeyValuePair<Tuple<Type, string>, Type> kv in Mappings) {
                if(!string.IsNullOrEmpty(kv.Key.Item2)) {
#if NETFX_CORE
                    var condition = type.GetTypeInfo().IsSubclassOf(mapping.From);
#else
                    var condition = type.IsAssignableFrom(kv.Key.Item1);
#endif
                    if(condition) {
                        var item = Activator.CreateInstance(kv.Value);
                        Inject(item);
                        yield return item;
                    }
                }
            }
        }
        public void Clear() {
            Instances.Clear();
            Mappings.Clear();
            RelationshipMappings.Clear();
        }
        public void Inject(object obj) {
            if(obj == null)
                return;
#if !NETFX_CORE
            var members = obj.GetType().GetMembers();
#else
            var members = obj.GetType().GetTypeInfo().DeclaredMembers;
#endif
            foreach(var memberInfo in members) {
                var injectAttribute = memberInfo.GetCustomAttributes(typeof(InjectAttribute), true).FirstOrDefault() as InjectAttribute;
                if(injectAttribute != null) {
                    if(memberInfo is PropertyInfo) {
                        var propertyInfo = memberInfo as PropertyInfo;
                        propertyInfo.SetValue(obj, Resolve(propertyInfo.PropertyType, injectAttribute.Name), null);
                    } else if(memberInfo is FieldInfo) {
                        var fieldInfo = memberInfo as FieldInfo;
                        fieldInfo.SetValue(obj, Resolve(fieldInfo.FieldType, injectAttribute.Name));
                    }
                }
            }
        }
        public void Register<TSource>(string name = null) {
            Mappings[typeof(TSource), name] = typeof(TSource);
        }
        public void Register<TSource, TTarget>(string name = null) {
            Mappings[typeof(TSource), name] = typeof(TTarget);
        }
        public void Register(Type source, Type target, string name = null) {
            Mappings[source, name] = target;
        }
        public void RegisterInstance(Type baseType, object instance = null, bool injectNow = true) {
            RegisterInstance(baseType, instance, null, injectNow);
        }
        public virtual void RegisterInstance(Type baseType, object instance = null, string name = null,
            bool injectNow = true) {
            Instances[baseType, name] = instance;
            if(injectNow) {
                Inject(instance);
            }
        }
        public void RegisterInstance<TBase>(TBase instance) where TBase : class {
            RegisterInstance<TBase>(instance, true);
        }
        public void RegisterInstance<TBase>(TBase instance, bool injectNow) where TBase : class {
            RegisterInstance<TBase>(instance, null, injectNow);
        }
        public void RegisterInstance<TBase>(TBase instance, string name, bool injectNow = true) where TBase : class {
            RegisterInstance(typeof(TBase), instance, name, injectNow);
        }
        public T Resolve<T>(string name = null, bool requireInstance = false, params object[] args) where T : class {
            return (T)Resolve(typeof(T), name, requireInstance, args);
        }
        public object Resolve(Type baseType, string name = null, bool requireInstance = false,
                params object[] constructorArgs) {
            // Look for an instance first
            var item = Instances[baseType, name];
            if(item != null)
                return item;

            if(requireInstance)
                return null;

            // Check if there is a mapping of the type
            var namedMapping = Mappings[baseType, name];
            if(namedMapping != null) {
                var obj = CreateInstance(namedMapping, constructorArgs);
                //Inject(obj);
                return obj;
            }

            return null;
        }
        public object CreateInstance(Type type, params object[] constructorArgs) {
            if(constructorArgs != null && constructorArgs.Length > 0) {
                //return Activator.CreateInstance(type,BindingFlags.Public | BindingFlags.Instance,Type.DefaultBinder, constructorArgs,CultureInfo.CurrentCulture);
                var obj2 = Activator.CreateInstance(type, constructorArgs);
                Inject(obj2);
                return obj2;
            }
#if !NETFX_CORE
            ConstructorInfo[] constructor = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
#else
        ConstructorInfo[] constructor = type.GetTypeInfo().DeclaredConstructors.ToArray();
#endif
            if(constructor.Length < 1) {
                var obj2 = Activator.CreateInstance(type);
                Inject(obj2);
                return obj2;
            }

            var maxParameters = constructor.First().GetParameters();
            foreach(var c in constructor) {
                var parameters = c.GetParameters();
                if(parameters.Length > maxParameters.Length) {
                    maxParameters = parameters;
                }
            }
            var args = maxParameters.Select(p => {
                if(p.ParameterType.IsArray) {
                    return ResolveAll(p.ParameterType);
                }
                return Resolve(p.ParameterType) ?? Resolve(p.ParameterType, p.Name);
            }).ToArray();
            var obj = Activator.CreateInstance(type, args);
            Inject(obj);
            return obj;
        }
        public TBase ResolveRelation<TBase>(Type tfor, params object[] args) {
            try {
                return (TBase)ResolveRelation(tfor, typeof(TBase), args);
            } catch(InvalidCastException castIssue) {
                throw new Exception(
                    string.Format("Resolve Relation couldn't cast  to {0} from {1}", typeof(TBase).Name, tfor.Name),
                    castIssue);
            }
        }
        public void InjectAll() {
            foreach(object instance in Instances.Values) {
                Inject(instance);
            }
        }
        public void RegisterRelation<TFor, TBase, TConcrete>() {
            RelationshipMappings[typeof(TFor), typeof(TBase)] = typeof(TConcrete);
        }
        public void RegisterRelation(Type tfor, Type tbase, Type tconcrete) {
            RelationshipMappings[tfor, tbase] = tconcrete;
        }
        public object ResolveRelation(Type tfor, Type tbase, params object[] args) {
            var concreteType = RelationshipMappings[tfor, tbase];
            if(concreteType == null)
                return null;

            var result = CreateInstance(concreteType, args);
            //Inject(result);
            return result;
        }
        public TBase ResolveRelation<TFor, TBase>(params object[] arg) {
            return (TBase)ResolveRelation(typeof(TFor), typeof(TBase), arg);
        }
    }
    #endregion
}
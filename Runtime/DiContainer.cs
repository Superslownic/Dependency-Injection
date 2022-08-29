using System;
using System.Collections.Generic;
using ReflectionBaking;

namespace DependencyInjection
{
  public sealed class DiContainer
  {
    private readonly DiContainer _parent;

    private readonly Dictionary<uint, IDependency> _container = new Dictionary<uint, IDependency>();

    public DiContainer()
    {
    }

    public DiContainer(DiContainer parent)
    {
      _parent = parent;
    }

    public T Resolve<T>()
    {
      if (_container.TryGetValue(BakedType<T>.UID, out IDependency dependency))
        return (T) (dependency as Single).Reference;

      if (_parent != null)
        return _parent.Resolve<T>();

      throw new KeyNotFoundException($"Cannot resolve type: {typeof(T)}");
    }

    public T Resolve<T>(string tag)
    {
      if (_container.TryGetValue(BakedType<T>.UID, out IDependency dependency))
      {
        if (!(dependency is Multiple multiple))
          throw new InvalidOperationException($"Dependency: [{typeof(T)}] with tag: [{tag}] is not Multiple");
        
        if (multiple.TryGetValue(tag, out object instance))
          return (T) instance;
      }

      if (_parent != null)
        return _parent.Resolve<T>(tag);

      throw new KeyNotFoundException($"Cannot resolve type: [{typeof(T)}] with tag: [{tag}]");
    }

    public void Register(object instance, Type type)
    {
      _container.Add(BakedTypePool.GetUID(type), new Single(instance));
    }

    public void Register<T>(T instance)
    {
      Register(instance, typeof(T));
    }

    public void Register(object instance, Type type, string tag)
    {
      uint uid = BakedTypePool.GetUID(type);
      
      if (_container.ContainsKey(uid))
      {
        if (!(_container[uid] is Multiple multiple))
          throw new InvalidOperationException($"Cannot register type: [{type}] with tag: [{tag}], because container already has it as singleton");

        multiple.Add(tag, instance);
        return;
      }

      _container.Add(uid, new Multiple().Add(tag, instance));
    }

    public void Register<T>(T instance, string tag)
    {
      Register(instance, typeof(T), tag);
    }
  }
}
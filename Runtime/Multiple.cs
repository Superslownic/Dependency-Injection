using System.Collections.Generic;

namespace DependencyInjection
{
  public class Multiple : IDependency
  {
    private readonly Dictionary<string, object> _references = new Dictionary<string, object>();

    public Multiple Add(string tag, object instance)
    {
      _references.Add(tag, instance);
      return this;
    }

    public bool TryGetValue(string tag, out object instance)
    {
      return _references.TryGetValue(tag, out instance);
    }
  }
}
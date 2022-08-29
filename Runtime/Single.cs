namespace DependencyInjection
{
  public class Single : IDependency
  {
    public readonly object Reference;

    public Single(object reference)
    {
      Reference = reference;
    }
  }
}
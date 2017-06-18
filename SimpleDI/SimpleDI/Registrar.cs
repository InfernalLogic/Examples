using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace SimpleDI
{
  public interface IRegistrar
  {
    void RegisterAsImplementedInterfaces(Type type);
    IReadOnlyDictionary<Type, Type> RegisteredTypes { get; }
  }

  public class Registrar : IRegistrar
  {
    public IReadOnlyDictionary<Type, Type> RegisteredTypes =>
      new ReadOnlyDictionary<Type, Type>(_registeredTypes);

    private IDictionary<Type, Type> _registeredTypes = new Dictionary<Type, Type>();

    public void RegisterAsImplementedInterfaces(Type type)
    {
      Type[] implementedInterfaces = type.GetInterfaces();

      if (!implementedInterfaces.Any())
        throw new ArgumentException($"{type} cannot be registered because it does not implement an interface.");

      if (!IsRegisterableType(type))
        throw new ArgumentException($"{type} cannot be registered because it is an interface or abstract class.");

      RegisterType(type, implementedInterfaces);
    }

    private void RegisterType(Type type, Type[] implementedInterfaces)
    {
      foreach (var @interface in implementedInterfaces)
        _registeredTypes.Add(@interface, type);
    }

    private bool IsRegisterableType(Type type)
    {
      TypeInfo typeInfo = type.GetTypeInfo();

      return typeInfo.IsClass
        && !typeInfo.IsAbstract
        && !typeInfo.IsInterface;
    }
  }
}

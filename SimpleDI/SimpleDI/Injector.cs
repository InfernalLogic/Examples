using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleDI
{
  public interface IInjector
  {
    T Resolve<T>() where T : class;
  }

  public class Injector : IInjector
  {
    private IReadOnlyDictionary<Type, Type> _registeredTypes;
    private MethodInfo _resolveMethodInfo;

    public Injector(IRegistrar registrar)
    {
      _registeredTypes = registrar.RegisteredTypes;
      _resolveMethodInfo = typeof(Injector).GetMethod(nameof(Resolve));
    }

    public T Resolve<T>() where T : class
    {
      Type typeToResolve = _registeredTypes.ContainsKey(typeof(T))
        ? _registeredTypes[typeof(T)]
        : typeof(T);

      ConstructorInfo constructor = typeToResolve
        .GetConstructors()
        .First();

      return Instantiate<T>(typeToResolve, constructor);
    }

    private T Instantiate<T>(Type typeToResolve, ConstructorInfo constructor) where T : class
    {
      List<object> injectedParameters = new List<object>();

      foreach (var parameter in constructor.GetParameters())
      {
        var parameterType = parameter.ParameterType;
        var resolve = _resolveMethodInfo.MakeGenericMethod(parameterType);
        injectedParameters.Add(resolve.Invoke(obj: this, parameters: null));
      }

      return (T)Activator.CreateInstance(typeToResolve, injectedParameters.ToArray());
    }
  }
}
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace SimpleDI.Tests
{
  public class InjectorTests
  {
    private IInjector _injector;

    private void BuildInjector(params (Type, Type)[] registeredTypes)
    {
      var typeDictionary = new Dictionary<Type, Type>();
      foreach (var registration in registeredTypes)
        typeDictionary.Add(registration.Item1, registration.Item2);

      var _mockRegistrar = new Mock<IRegistrar>();
      _mockRegistrar.SetupGet(registrar => registrar.RegisteredTypes)
        .Returns(typeDictionary);

      _injector = new Injector(_mockRegistrar.Object);
    }

    [Fact]
    public void ResolvesUnregisteredType_WithDefaultContructor()
    {
      BuildInjector();

      var resolvedInstance = _injector.Resolve<ClassWithNoInterface>();

      Assert.NotNull(resolvedInstance);
    }

    [Fact]
    public void ResolvesRegisteredType_WithDefaultContructor()
    {
      BuildInjector((typeof(IInterface), typeof(ClassWithOneInterface)));

      var resolvedInstance = _injector.Resolve<IInterface>();

      Assert.NotNull(resolvedInstance);
    }

    [Fact]
    public void ResolvesType_WithConstructorParameters()
    {
      BuildInjector(
        (typeof(IInterface), typeof(ClassWithOneInterface)),
        (typeof(IOtherInterface), typeof(ClassWithOtherInterface)));

      ClassWithTwoDependencies resolvedInstance = _injector.Resolve<ClassWithTwoDependencies>();

      Assert.True(resolvedInstance.InjectedIInterface != null
        && resolvedInstance.InjectedIOtherInterface != null);
    }
  }
}

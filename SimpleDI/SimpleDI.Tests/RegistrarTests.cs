using System;
using System.Collections.Generic;
using Xunit;

namespace SimpleDI.Tests
{
  public class RegistrarTests
  {
    private IRegistrar _registrar = new Registrar();

    [Fact]
    public void ThrowsException_WhenTypeImplementsNoInterfaces()
    {
      Assert.Throws<ArgumentException>(() => {
        _registrar.RegisterAsImplementedInterfaces(typeof(ClassWithNoInterface));
      });
    }

    [Theory]
    [InlineData(typeof(IInterface))]
    [InlineData(typeof(AbstractClass))]
    public void ThrowsArgumentException_WhenTypeCannotBeInstantiated(Type type)
    {
      Assert.Throws<ArgumentException>(() => {
        _registrar.RegisterAsImplementedInterfaces(type);
      });
    }

    [Theory]
    [InlineData(typeof(ClassWithOneInterface), typeof(IInterface))]
    [InlineData(typeof(ClassWithTwoInterfaces), typeof(IInterface))]
    [InlineData(typeof(ClassWithTwoInterfaces), typeof(IOtherInterface))]
    public void RegistersAllInterfaces_WhenValidClass(Type concreteClass, Type implementedInterface)
    {
      _registrar.RegisterAsImplementedInterfaces(concreteClass);
      Assert.Equal(concreteClass, _registrar.RegisteredTypes[implementedInterface]);
    }
  }


}

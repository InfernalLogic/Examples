namespace SimpleDI.Tests
{
  public class ClassWithNoInterface { }
  public abstract class AbstractClass { }
  public interface IInterface { }
  public interface IOtherInterface { }
  public class ClassWithOneInterface : IInterface { }
  public class ClassWithOtherInterface : IOtherInterface { }
  public class ClassWithTwoInterfaces : IInterface, IOtherInterface { }

  public class ClassWithTwoDependencies
  {
    public IInterface InjectedIInterface { get; }
    public IOtherInterface InjectedIOtherInterface { get; }

    public ClassWithTwoDependencies(IInterface @interface, IOtherInterface otherInterface)
    {
      InjectedIInterface = @interface;
      InjectedIOtherInterface = otherInterface;
    }
  }
}

using System;

namespace PlayingWithHangfire
{
  public class SingletonDependency
  {
    public Guid Id { get; private set; } = Guid.NewGuid();
  }
}

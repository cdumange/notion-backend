using System.Security.Cryptography;

namespace utils;
public class JustifiedValue<T>
{
  protected JustifiedValue(bool isError) => this.isError = isError;
  public T? Value { get; private set; } = default;
  public Exception? Exception { get; private set; } = null;
  private readonly bool isError = false;
  // ForValue allows to return a value. isError allows to return a value while being considered an error.
  public static JustifiedValue<T> FromValue(T? value, bool isError = false)
  {
    return new JustifiedValue<T>(isError) { Value = value };
  }

  public static JustifiedValue<T> FromException(Exception ex)
  {
    return new JustifiedValue<T>(true) { Exception = ex };
  }

  public static implicit operator bool(JustifiedValue<T> value)
  {
    return !value.isError;
  }

  public static implicit operator JustifiedValue<T>(T? value)
  {
    return JustifiedValue<T>.FromValue(value);
  }

  public static implicit operator JustifiedValue<T>(Exception ex)
  {
    return JustifiedValue<T>.FromException(ex);
  }

  public static implicit operator T(JustifiedValue<T> value)
  {
    return value.Value;
  }
}

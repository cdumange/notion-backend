namespace notion.models.dto;
public record User
{
  public Guid ID { get; set; }
  public string Email { get; set; } = string.Empty;
  public DateTime CreationDate { get; set; }
  public DateTime? ModificationDate { get; set; }

  public class Exceptions
  {
    public static Exception UserNotFound = new Exception("no user for this email");
    public static Exception UserAlreadyExists = new Exception("a user with this email already exists");
  }
}

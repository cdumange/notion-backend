namespace notion.models.dto;
public record User
{
  public Guid ID { get; set; }
  public string Email { get; set; } = string.Empty;
  public DateTime CreationDate { get; set; }
  public DateTime? ModificationDate { get; set; }
}

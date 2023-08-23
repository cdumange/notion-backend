
namespace notion.dal.models
{
    public class User
    {
        public Guid ID { get; set; }
        public string Email { get; set; } = string.Empty;

        public DateTime creation_date { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}
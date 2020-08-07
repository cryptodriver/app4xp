namespace Domain.Models
{
    public class Setting : BaseModel
    {
        public int AccountId { get; set; } = 0;
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

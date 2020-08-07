namespace Domain.Models
{
    public class Account : BaseModel
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public int IsCurrent { get; set; } = 0;
    }
}

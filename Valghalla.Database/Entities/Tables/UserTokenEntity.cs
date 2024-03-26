namespace Valghalla.Database.Entities.Tables
{
    public partial class UserTokenEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Value { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}

namespace Valghalla.Application.User
{
    public class SystemRole
    {
        public static readonly SystemRole Administrator = new(new Guid("92663087-2642-4e4c-acf8-d344ef6c8118"), "shared.user.role.administrator");
        public static readonly SystemRole Participant = new(new Guid("f2b44691-beB8-4aaa-bc12-e8845e010474"), "shared.user.role.participant");

        public Guid Id { get; }

        public string Name { get; }

        private SystemRole(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

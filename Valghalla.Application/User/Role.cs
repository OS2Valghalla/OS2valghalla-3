namespace Valghalla.Application.User
{
    public class Role
    {
        public static readonly Role Administrator = new(SystemRole.Administrator.Id, SystemRole.Administrator.Name);
        public static readonly Role Participant = new(SystemRole.Participant.Id, SystemRole.Participant.Name);
        public static readonly Role TeamResponsible = new(new Guid("8172f9cc-7790-4404-9955-ea48e7de6512"), "shared.user.role.team_responsible");
        public static readonly Role WorkLocationResponsible = new(new Guid("9a92dc91-d8f1-4ecf-ab97-043d827663a7"), "shared.user.role.work_location_responsible");

        public Guid Id { get; }

        public string Name { get; }

        private Role(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public static IEnumerable<Role> GetAllRoles() => new[] { Administrator, Participant, TeamResponsible, WorkLocationResponsible };
        public static Role Parse(Guid id) => GetAllRoles().First(i => i.Id == id);
        public static bool TryParse(Guid id, out Role role)
        {
            role = GetAllRoles().FirstOrDefault(i => i.Id == id);
            return role != null;
        }

        public bool IsAuthorized(Role role)
        {
            if (Id == Administrator.Id)
            {
                return
                    role.Id == Administrator.Id;
            }
            else if (Id == TeamResponsible.Id)
            {
                return
                    role.Id == TeamResponsible.Id;
            }
            else if (Id == WorkLocationResponsible.Id)
            {
                return
                    role.Id == WorkLocationResponsible.Id;
            }
            else if (Id == Participant.Id)
            {
                return
                    role.Id == Participant.Id ||
                    role.Id == TeamResponsible.Id ||
                    role.Id == WorkLocationResponsible.Id;
            }

            return false;
        }
    }
}

namespace Valghalla.Application.TaskValidation
{
    public sealed class TaskValidationRule
    {
        public static readonly TaskValidationRule Alive = new(new Guid("0cfc8f2f-d09f-4776-9e27-6ecf7fdd668d"));
        public static readonly TaskValidationRule Age18 = new(new Guid("78dca41a-0a59-445d-b639-de22ea7e5569"));
        public static readonly TaskValidationRule ResidencyMunicipality = new (new Guid("df6094f4-26b5-48f3-9d49-7508537230a2"));
        public static readonly TaskValidationRule Citizenship = new(new Guid("4ac40dc3-2843-402c-88d9-a09f73e5ee8c"));
        public static readonly TaskValidationRule Disenfranchised = new(new Guid("6a4d315e-e459-4e87-81eb-2339dbe5df5b"));

        public Guid Id { get; init; }

        public TaskValidationRule(Guid id) => Id = id;
    }
}

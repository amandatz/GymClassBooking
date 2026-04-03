public record PlanType
{
    public static readonly PlanType Monthly = new(1, "Monthly", 12);
    public static readonly PlanType Quarterly = new(2, "Quarterly", 20);
    public static readonly PlanType Annual = new(3, "Annual", 30);

    public int Id { get; }
    public string Name { get; }
    public int MonthlyLimit { get; }

    private PlanType(int id, string name, int monthlyLimit) =>
        (Id, Name, MonthlyLimit) = (id, name, monthlyLimit);

    public static IEnumerable<PlanType> List() =>
        [Monthly, Quarterly, Annual];

    public static PlanType FromId(int id) =>
        List().FirstOrDefault(x => x.Id == id)
        ?? throw new InvalidOperationException($"Invalid plan id: {id}");

    public override string ToString() => Name;
}
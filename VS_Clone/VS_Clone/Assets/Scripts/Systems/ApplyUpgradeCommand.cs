public class ApplyStatUpgradeCommand : ICommand
{
    private readonly PlayerStats stats;
    private readonly string statId;
    private readonly float delta;
    public string Description => $"Upgrade {statId} by {delta}";

    public ApplyStatUpgradeCommand(PlayerStats stats, string statId, float delta)
    {
        this.stats = stats; this.statId = statId; this.delta = delta;
    }

    public void Execute() => stats.Modify(statId, delta);
    public void Undo() => stats.Modify(statId, -delta);
}

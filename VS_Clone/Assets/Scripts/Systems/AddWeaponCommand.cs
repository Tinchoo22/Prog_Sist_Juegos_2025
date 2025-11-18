using UnityEngine;

public class AddWeaponCommand : ICommand
{
    private readonly Transform owner;
    private readonly WeaponFactory factory;
    private readonly WeaponConfig config;
    private Weapon created;
    public string Description => $"Add weapon {config?.id}";

    public AddWeaponCommand(Transform owner, WeaponFactory factory, WeaponConfig config)
    {
        this.owner = owner; this.factory = factory; this.config = config;
    }

    public void Execute()
    {
        if (created == null) created = factory.CreateOn(owner, config);
        else created.gameObject.SetActive(true);
    }

    public void Undo()
    {
        if (created != null) created.gameObject.SetActive(false);
    }
}

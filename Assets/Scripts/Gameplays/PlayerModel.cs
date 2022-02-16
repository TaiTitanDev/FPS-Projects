
[System.Serializable]
public class PlayerModel
{
    private int maxHealth;
    private int health;
    private int numberAmmoRifle;
    private int numberAmmoRevolver;

    public int MaxHealth { get => maxHealth; }
    public int Health { get => health; set => health = value > MaxHealth ? MaxHealth: value; }
    public int NumberAmmoRifle { get => numberAmmoRifle; set => numberAmmoRifle = value; }
    public int NumberAmmoRevolver { get => numberAmmoRevolver; set => numberAmmoRevolver = value; }

    public PlayerModel()
    {
        this.maxHealth = 250;
        this.health = 100;
        this.numberAmmoRifle = 90;
        this.numberAmmoRevolver = 32;
    }
}

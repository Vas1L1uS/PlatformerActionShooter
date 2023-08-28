public interface IHealth
{
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    void GetDamage(float damage);
}
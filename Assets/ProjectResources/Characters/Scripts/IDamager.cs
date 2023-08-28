public interface IDamager
{
    float Damage { get; set; }
    void TakeDamage(IHealth target);
}
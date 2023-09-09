public interface IDamager
{
    int Damage { get; set; }
    void TakeDamage(IHealth target);
}
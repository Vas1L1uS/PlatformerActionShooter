using UnityEngine;

public class ItemHealth : PickableItem
{
    [SerializeField] private int _recoverHealth = 1;

    protected override void CheckAndPick(GameObject player)
    {
        if (player.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            if (playerHealth.CurrentHealth != playerHealth.MaxHealth)
            {
                playerHealth.GetHealth(_recoverHealth);
                Collected();
            }
        }
    }
}
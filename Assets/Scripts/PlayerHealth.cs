using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;

    void OnTriggerEnter(Collider other)
    {
        ItemController itemController = other.GetComponent<ItemController>();
        if (itemController != null && itemController.item is Garbage)
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        health--;
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            health = 0;
            EventManager.Instance.OnPlayerDeath.Invoke();
        }
    }
}
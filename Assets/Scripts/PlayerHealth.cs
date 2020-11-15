using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    private static readonly int EatGarbage = Animator.StringToHash("EatGarbage");

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!GetComponent<PlayerMovement>().PlayerHasControl)
        {
            return;
        }
        ItemController itemController = other.GetComponent<ItemController>();
        if (itemController != null && itemController.item is Garbage)
        {
            TakeDamage();
            Destroy(other.gameObject, .1f);
        }
    }

    private void TakeDamage()
    {
        GetComponent<Animator>().SetTrigger(EatGarbage);
        
        health--;
        
        EventManager.Instance.OnAteGarbage.Invoke();
        GetComponent<Animator>().SetInteger("damageLevel", 3 - health);

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
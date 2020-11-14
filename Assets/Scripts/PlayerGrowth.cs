using UnityEngine;

public class PlayerGrowth : MonoBehaviour
{
    public int numberOfFoodUntilAdult = 10;
    [SerializeField] private int foodEaten = 0;
    [SerializeField] private bool adult = false;

    private void OnTriggerEnter(Collider other)
    {
        ItemController itemController = other.GetComponent<ItemController>();
        if (itemController != null && itemController.item is Food)
        {
            EatFood();
            Destroy(other.gameObject);
        }
    }

    private void EatFood()
    {
        foodEaten++;
        CheckGrowth();
    }

    private void CheckGrowth()
    {
        if (adult) return;
        
        if (foodEaten >= numberOfFoodUntilAdult)
        {
            adult = true;
            EventManager.Instance.OnPlayerAdult.Invoke();
        }
    }
}
using UnityEngine;

public class PlayerGrowth : MonoBehaviour
{
    public int numberOfFoodUntilAdult = 10;
    [SerializeField] private int foodEaten = 0;
    [SerializeField] private bool adult = false;
    private static readonly int IsAdult = Animator.StringToHash("isAdult");

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        ItemController itemController = other.GetComponent<ItemController>();
            if (itemController != null && itemController.item is Food)
            {
                EatFood();
                Destroy(other.gameObject, .1f);
            }
    }

    private void EatFood()
    {
        Debug.Log("eat!");
        GetComponent<Animator>().SetTrigger("EatItem");
        EventManager.Instance.OnAteItem.Invoke();
        Debug.Log(GetComponent<Animator>().parameters);
        foodEaten++;
        CheckGrowth();
    }

    private void CheckGrowth()
    {
        if (adult) return;
        
        if (foodEaten >= numberOfFoodUntilAdult)
        {
            GetComponent<Animator>().SetBool(IsAdult, true);
            GetComponent<Animator>().SetTrigger("GrowUp");
            adult = true;
            EventManager.Instance.OnPlayerAdult.Invoke();
        }
    }
}
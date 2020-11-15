using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<ItemController>()) return;
        
        Destroy(other.gameObject);
    }
}

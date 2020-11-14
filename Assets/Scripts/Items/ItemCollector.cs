using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<ItemController>()) return;
        
        Destroy(other.gameObject);
    }
}

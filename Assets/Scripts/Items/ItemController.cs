using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Item item;
    [HideInInspector] public float speed = 3f;

    void Update()
    {
        MoveItem();
    }

    private void MoveItem()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }
}
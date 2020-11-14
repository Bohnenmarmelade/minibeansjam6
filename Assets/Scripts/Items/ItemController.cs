using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Item item;
    public float speed = 0.5f;

    void Update()
    {
        MoveItem();
    }

    private void MoveItem()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }
}
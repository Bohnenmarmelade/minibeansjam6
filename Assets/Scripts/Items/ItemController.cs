using System;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Item item;
    public float speed = 0.5f;

    private void Awake()
    {
        EventManager.Instance.OnLevelFinished.AddListener(HandleLevelFinished);
    }

    void Update()
    {
        MoveItem();
    }

    private void MoveItem()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }

    private void HandleLevelFinished()
    {
        speed = 10f;
    }
}
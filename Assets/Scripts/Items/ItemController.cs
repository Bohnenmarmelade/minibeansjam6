using System;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Item item;
    [HideInInspector] public float speed = 3f;

    private void OnEnable()
    {
        EventManager.Instance.OnLevelFinished.AddListener(OnLevelEnd);
    }

    private void OnDisable()
    {
        EventManager.Instance.OnLevelFinished.RemoveListener(OnLevelEnd);
    }

    void OnLevelEnd()
    {
        speed *= 3;
    }

    void Update()
    {
        MoveItem();
    }

    private void MoveItem()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }
}
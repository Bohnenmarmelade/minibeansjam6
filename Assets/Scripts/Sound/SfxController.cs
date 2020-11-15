using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class SfxController : MonoBehaviour
{
    public AudioClip eatItem;
    public AudioClip eatGarbage;
    public AudioClip winLevel;
    public AudioClip death;
    public AudioClip buttonPress;
    public AudioClip growUp;
    
    private AudioSource _source;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        EventManager eventManager = EventManager.Instance;
        eventManager.OnAteItem.AddListener(HandleEatItem);
        eventManager.OnAteGarbage.AddListener(HandleEatGarbage);
        eventManager.OnPlayerDeath.AddListener(HandleDeath);
        eventManager.OnStartGame.AddListener(HandleButtonPress);
        eventManager.OnPlayerAdult.AddListener(HandleGrowUp);
    }

    private void OnDisable()
    {
        EventManager eventManager = EventManager.Instance;
        eventManager.OnAteItem.RemoveListener(HandleEatItem);
        eventManager.OnAteGarbage.RemoveListener(HandleEatGarbage);
        eventManager.OnPlayerDeath.RemoveListener(HandleDeath);
        eventManager.OnStartGame.RemoveListener(HandleButtonPress);
        eventManager.OnPlayerAdult.RemoveListener(HandleGrowUp);
    }

    private void HandleGrowUp()
    {
        _source.PlayOneShot(growUp);
    }
    private void HandleButtonPress()
    {
        _source.PlayOneShot(buttonPress);
    }
    
    private void HandleEatItem()
    {
        _source.PlayOneShot(eatItem);
    }

    private void HandleEatGarbage()
    {
        _source.PlayOneShot(eatGarbage);
    }

    private void HandleDeath()
    {
        _source.PlayOneShot(death);
    }
}

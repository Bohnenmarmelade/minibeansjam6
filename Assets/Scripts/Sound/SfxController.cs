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

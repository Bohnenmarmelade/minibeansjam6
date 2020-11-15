using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip levelMusic;
    
    private AudioClip _clipToPlay;
    
    private AudioSource _source;
    
    private bool _fadeIn = false;
    private bool _fadeOut = false;

    private float _fadeTime = .2f;
    private float _startVolume;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _source = GetComponent<AudioSource>();

        _clipToPlay = menuMusic;
        _startVolume = _source.volume;
        _source.volume = 0.0f;
        _source.clip = _clipToPlay;
        _source.Play();
        _fadeIn = true;
    }
    private void Update () {
        if (_fadeOut) {
            _source.volume -= _startVolume * Time.deltaTime / _fadeTime;
            if (_source.volume < 0.1) {
                _fadeOut = false;
                _fadeIn = true;
                _source.clip = _clipToPlay;
                _source.Play();
            }
        } else if (_fadeIn) {
            _source.volume += _startVolume * Time.deltaTime / _fadeTime;
            if (_source.volume > _startVolume) {
                _fadeIn = false;
            }
        }
    }

    private void OnEnable()
    {
        EventManager eventManager = EventManager.Instance;
        eventManager.OnPlayerDeath.AddListener(HandleMenuMusic);
        eventManager.OnStartGame.AddListener(HandleLevelMusic);
    }

    private void HandleMenuMusic()
    {
        _clipToPlay = menuMusic;
        _fadeOut = true;
    }

    private void HandleLevelMusic()
    {
        _clipToPlay = levelMusic;
        _fadeOut = true;
    }
        
}

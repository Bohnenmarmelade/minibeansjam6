using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : Singleton<Score>
{
    public int levels = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        levels = 0;
        EventManager.Instance.OnLevelFinished.AddListener(UpdateLevelScore);
        EventManager.Instance.OnPlayerDeath.AddListener(OnPlayerDeath);
    }

    private void UpdateLevelScore()
    {
        levels++;
    }

    private void OnPlayerDeath()
    {
        levels--;
        SceneManager.LoadScene(2);
    }
    
    
}

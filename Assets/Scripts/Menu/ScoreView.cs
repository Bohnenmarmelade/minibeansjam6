using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    public Text text;
    private int level = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        level = Score.Instance.levels;
        text.text = "You created " + level + " new whale worlds!";
    }

    public void StartNewWorld()
    {
        level = 0;
        Score.Instance.levels = 0;
        SceneManager.LoadScene(1);
    }
}

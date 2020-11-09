using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;
    public GameObject[] levelDone;

    void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 10);

        for(int i = 0; i < levelButtons.Length; i++)
        {
            if (i  > levelReached)
            {
                levelButtons[i].interactable = false;                  
            }
            if (i > levelReached - 1)
            {
                levelDone[i].SetActive(false);
            }
        }
    }
    public void Select(int level)
    {
        PlayerPrefs.SetInt("selectedLevel", level);
        SceneManager.LoadScene("GameScene");
    }
}

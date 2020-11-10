using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    // array with level buttons
    public Button[] levelButtons;
    // array with green hook images to give player feedback on completed levels
    public GameObject[] levelDone;

    void Start()
    {
        // change this variable to activate more puzzles for testing
        int levelReached = PlayerPrefs.GetInt("levelReached", 0);

        for(int i = 0; i < levelButtons.Length; i++)
        {
            if (i  > levelReached)
            {
                // set level buttons interactability to false if player didnt reach that level yet
                levelButtons[i].interactable = false;                  
            }
            if (i > levelReached - 1)
            {
                // deactive green hook symbol for levels player didnt complete yet
                levelDone[i].SetActive(false);
            }
        }
    }
    // method called when player selects a level
    public void Select(int level)
    {
        // save selected level in player prefs and then load GameScene
        PlayerPrefs.SetInt("selectedLevel", level);
        SceneManager.LoadScene("GameScene");
    }
}

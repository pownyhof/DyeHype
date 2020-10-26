using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelControl : MonoBehaviour
{

    [SerializeField]
    private GameObject buttonTemplate;

    void Start()
    {
        GenerateMenu();
    }

    void GenerateMenu()
    {

        for (int i = 1; i < 100; i++)
        {
            int puzzleNumber = i;
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
            newButton.SetActive(true);

            newButton.GetComponent<LevelButton>().SetText(puzzleNumber.ToString());
            newButton.transform.SetParent(buttonTemplate.transform.parent, false);
        }
    }
}

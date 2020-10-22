using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class LevelData : MonoBehaviour
{
    public static List<GameData.GameBoardData> getData()
    {
        List<GameData.GameBoardData> data = new List<GameData.GameBoardData>();

        //blue 1, green 2, yellow 3, red 4

        data.Add(new GameData.GameBoardData(
            new int[121] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 2, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
            new int[121] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 2, 4, 2, 3, 2, 4, 1, 3, 4, 0, 3, 1, 3, 1, 4, 1, 2, 3, 1, 2, 0, 1, 4, 2, 4, 3, 2, 1, 2, 4, 3, 0, 3, 2, 1, 3, 1, 4, 3, 4, 2, 1, 0, 4, 3, 4, 1, 4, 1, 2, 3, 4, 3, 0, 1, 2, 3, 2, 3, 2, 1, 4, 2, 1, 0, 2, 4, 1, 3, 2, 4, 3, 1, 3, 2, 0, 1, 3, 2, 4, 3, 1, 4, 2, 4, 1, 0, 3, 1, 4, 3, 1, 2, 1, 4, 1, 3, 0, 4, 2, 1, 2, 3, 4, 2, 3, 2, 4 }));

        data.Add(new GameData.GameBoardData(    
            new int[121] { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,3,0,2,0,0,0,0,0,0,4,3,0,2,4,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,4,3,0,2,4,0,0,0,0,0,0,0,0,3,0,2,0,0,0,0,0,0,0,0,0,0,0,2,0},
            new int[121] { 0,0,0,0,0,0,0,0,0,0,0,0,1,3,1,3,1,2,3,1,2,1,0,4,2,4,2,4,3,4,2,4,3,0,3,4,3,4,3,1,2,4,3,1,0,2,1,2,1,2,4,1,3,4,2,0,1,3,1,2,3,1,4,1,2,1,0,4,2,4,3,4,2,3,2,3,4,0,3,1,2,1,2,4,1,3,4,1,0,4,2,3,4,3,1,2,4,3,2,0,2,4,2,1,2,3,4,2,4,1,0,1,3,4,3,1,4,3,1,2,3}));

        return data;
    }
}*/

public class GameData : MonoBehaviour
{

    public static GameData Instance;
    
    public struct GameBoardData
    {
        public int[] unsolved_data;
        public int[] solved_data;

        public GameBoardData(int[] unsolved, int[] solved) : this()
        {
            this.unsolved_data = unsolved;
            this.solved_data = solved;
        }
    };

    // public Dictionary<string, List<GameBoardData>> dyehype_game = new Dictionary<string, List<GameBoardData>>();

    public List<GameData.GameBoardData> dyehype_game = new List<GameData.GameBoardData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);
    }
    
    void Start()
    {
        // dyehype_game.Add("levelOne", LevelData.getData());
        populateList();
    }

    public void populateList()
    {

        //blue 1, green 2, yellow 3, red 4

        dyehype_game.Add(new GameData.GameBoardData(
            new int[121] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 2, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
            new int[121] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 2, 4, 2, 3, 2, 4, 1, 3, 4, 0, 3, 1, 3, 1, 4, 1, 2, 3, 1, 2, 0, 1, 4, 2, 4, 3, 2, 1, 2, 4, 3, 0, 3, 2, 1, 3, 1, 4, 3, 4, 2, 1, 0, 4, 3, 4, 1, 4, 1, 2, 3, 4, 3, 0, 1, 2, 3, 2, 3, 2, 1, 4, 2, 1, 0, 2, 4, 1, 3, 2, 4, 3, 1, 3, 2, 0, 1, 3, 2, 4, 3, 1, 4, 2, 4, 1, 0, 3, 1, 4, 3, 1, 2, 1, 4, 1, 3, 0, 4, 2, 1, 2, 3, 4, 2, 3, 2, 4 }));

        dyehype_game.Add(new GameData.GameBoardData(
            new int[121] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 3, 0, 2, 0, 0, 0, 0, 0, 0, 4, 3, 0, 2, 4, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 3, 0, 2, 4, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0 },
            new int[121] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 1, 3, 1, 2, 3, 1, 2, 1, 0, 4, 2, 4, 2, 4, 3, 4, 2, 4, 3, 0, 3, 4, 3, 4, 3, 1, 2, 4, 3, 1, 0, 2, 1, 2, 1, 2, 4, 1, 3, 4, 2, 0, 1, 3, 1, 2, 3, 1, 4, 1, 2, 1, 0, 4, 2, 4, 3, 4, 2, 3, 2, 3, 4, 0, 3, 1, 2, 1, 2, 4, 1, 3, 4, 1, 0, 4, 2, 3, 4, 3, 1, 2, 4, 3, 2, 0, 2, 4, 2, 1, 2, 3, 4, 2, 4, 1, 0, 1, 3, 4, 3, 1, 4, 3, 1, 2, 3 }));

        
    }

}

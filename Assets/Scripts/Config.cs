using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class Config : MonoBehaviour
{

#if UNITY_ANDROID && !UNITY_EDITOR
    private static string dir = Application.persistentDataPath;
#else
    private static string dir = Directory.GetCurrentDirectory();
#endif

    private static string file = @"\gameData.ini";
    private static string path = dir + file;


    public static void DeleteFile()
    {
        File.Delete(path);
    }

    public static void SaveBoardData(GameData.GameBoardData boardData, int boardIndex, int errorNumber)
    {
        File.WriteAllText(path, string.Empty);
        StreamWriter writer = new StreamWriter(path, false);
        string currentTime = "#time:" + Timer.GetCurrentTime();
        string errorNumberString = "#errors:" + errorNumber;
        string boardIndexString = "#boardIndex:" + boardIndex.ToString();
        string unsolvedString = "#unsolved:";
        string solvedString = "#solved:";

        foreach(var unsolvedData in boardData.unsolved_data)
        {
            unsolvedString += unsolvedData.ToString() + ",";
        }
        foreach (var solvedData in boardData.solved_data)
        {
            solvedString += solvedData.ToString() + ",";
        }

        writer.WriteLine(currentTime);
        writer.WriteLine(errorNumberString);
        writer.WriteLine(boardIndexString);
        writer.WriteLine(unsolvedString);
        writer.WriteLine(solvedString);

        writer.Close();
    }


    public static GameData.GameBoardData ReadGridData()
    {
        string line;
        StreamReader file = new StreamReader(path);

        int[] unsolved_data = new int[121];
        int[] solved_data = new int[121];

        int unsolvedIndex = 0;
        int solvedIndex = 0;

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if(word[0] == "#unsolved")
            {
                string[] substrings = Regex.Split(word[1], ",");

                foreach(var value in substrings)
                {
                    int squareNumber = -1;
                    if(int.TryParse(value, out squareNumber))
                    {
                        unsolved_data[unsolvedIndex] = squareNumber;
                        unsolvedIndex++;
                    }
                }
            }

            if (word[0] == "#solved")
            {
                string[] substrings = Regex.Split(word[1], ",");

                foreach (var value in substrings)
                {
                    int squareNumber = -1;
                    if (int.TryParse(value, out squareNumber))
                    {
                        solved_data[solvedIndex] = squareNumber;
                        solvedIndex++;
                    }
                }
            }
        }

        file.Close();
        return new GameData.GameBoardData(unsolved_data, solved_data);
    }

    public static int ReadGameLevel()
    {
        int level = -1;
        string line;
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#boardIndex")
            {
                int.TryParse(word[1], out level);
            }
        }

        file.Close();
        return level;
    }

    public static float ReadTime()
    {
        float time = -1.0f;
        string line;
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#time")
            {
                float.TryParse(word[1], out time);
            }
        }

        file.Close();
        return time;
    }

    public static int ErrorNumber()
    {
        int errors = 0;
        string line;
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if(word[0] == "#errors")
            {
                int.TryParse(word[1], out errors);
            }
        }

        file.Close();
        return errors;
    }

    public static bool GameFileExist()
    {
        return File.Exists(path);
    }
}

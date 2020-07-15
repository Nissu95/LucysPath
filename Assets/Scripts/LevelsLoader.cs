using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System;

public class LevelsLoader
{

    static List<Level> levels;
    static List<LevelWon> levelsWon;

    static string path = "Assets/Resources/levels";
    static string levelsWonPath = Application.persistentDataPath + "/levelsWon";

    public static void SaveLevel(Level level, ref int index)
    {

        if (levels == null)
            levels = new List<Level>();

        if (levels.Count <= index)
        {
            index = levels.Count;
            levels.Add(level);
        }
        else
            levels[index] = level;

        FileStream file;

        if (File.Exists(path)) file = File.OpenWrite(path);
        else file = File.Create(path);


        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, levels);
        file.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
    }
    public static void ReadLevels()
    {
        FileStream file;

        if (File.Exists(path))
        {
            file = File.OpenRead(path);
            Debug.Log("File found");
        }
        else
        {
            Debug.LogError("File not found");
            levels = new List<Level>();
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        levels = (List<Level>)bf.Deserialize(file);
        file.Close();
    }

    public static Level GetLevel(int index)
    {
        if (index >= 0 && index < levels.Count)
            return levels[index];

        return null;
    }

    public static int GetLevelsCount()
    {
        return levels.Count;
    }

    public static List<Level> GetLevels()
    {
        if (levels == null)
        {
            Debug.Log("Levels null buscando archivo");
            ReadLevels();
        }

        return levels;
    }

    public static void SaveLevelWon(int stars, int index)
    {
        string destination = levelsWonPath;
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        if (levelsWon == null)
            levelsWon = new List<LevelWon>();

        LevelWon level = new LevelWon(stars);

        if (levelsWon.Count <= index)
            levelsWon.Add(level);
        else
            levelsWon[index] = level;

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, levelsWon);
        file.Close();
    }

    public static void ReadLevelsWon()
    {
            string destination = levelsWonPath;
            FileStream file;

        if (File.Exists(destination))
        {
            file = File.OpenRead(destination);
            Debug.Log("FileFound");
        }
        else
        {
            Debug.LogError("File not found");
            return;
        }

            BinaryFormatter bf = new BinaryFormatter();
            levelsWon = (List<LevelWon>)bf.Deserialize(file);
            file.Close();
    }

    public static LevelWon GetLevelWon(int index)
    {
        if (levelsWon != null && index < levelsWon.Count)
            return levelsWon[index];

        return null;
    }
    public static List<LevelWon> GetLevelsWon()
    {
        if (levelsWon == null)
        {
            Debug.Log("Levels won null buscando archivo");
            ReadLevelsWon();
        }

        return levelsWon;
    }
    
}






[Serializable]
public class Level
{
    int[,] items;
    bool won = false;
    int stars = 0;
    public Level(LevelButton[,] levelButtons, int colums, int rows)
    {
        items = new int[colums, rows];

        for (int i = 0; i < colums; i++)
            for (int j = 0; j < rows; j++)
            {
                items[i, j] = levelButtons[i, j].GetIndex();
            }
    }

    public int[,] GetItems()
    {
        return items;
    }

    public bool GetWon()
    {
        return won;
    }

    public void SetWon(bool _won)
    {
        won = _won;
    }
    public int GetColumns()
    {
        return items.GetLength(0);
    }

    public int GetRows()
    {
        return items.GetLength(1);
    }
}




[Serializable]
public class LevelWon
{
    int m_Stars;
    bool m_Won;
    public LevelWon(int stars)
    {
        m_Stars = stars;
    }
    public int GetStars()
    {
        return m_Stars;
    }
}
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

    static string editorPath = "Assets/StreamingAssets/resources/levels";
    static string inGamePath = "resources/levels";
    static string levelsWonPath = Application.persistentDataPath + "/levelsWon";

    public static void SaveLevel(Level level, ref int index)
    {
#if UNITY_EDITOR
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

        if (File.Exists(editorPath)) file = File.OpenWrite(editorPath);
        else file = File.Create(editorPath);


        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, levels);
        file.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(editorPath);
#endif
    }
    public static void ReadLevels()
    {
        Stream file;

        BetterStreamingAssets.Initialize();

        if (BetterStreamingAssets.FileExists(inGamePath))
        {
            file = BetterStreamingAssets.OpenRead(inGamePath);
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
            levelsWon = new List<LevelWon>();
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
    GridObject[,] grid;
    //bool won = false;
    public Level(LevelButton[,] levelButtons, int colums, int rows)
    {
        grid = new GridObject[colums, rows];

        for (int i = 0; i < colums; i++)
            for (int j = 0; j < rows; j++)
            {
                grid[i, j] = new GridObject();
                grid[i, j].Index = levelButtons[i, j].GetIndex();
                grid[i, j].Locked = levelButtons[i, j].IsLocked();
                grid[i, j].Star = levelButtons[i, j].HasStar();
                grid[i, j].Rotation = levelButtons[i, j].GetRotation();
            }
    }

    public GridObject[,] GetGrid()
    {
        return grid;
    }

   /* public void SetWon(bool _won)
    {
        won = _won;
    }*/

    public int GetColumns()
    {
        return grid.GetLength(0);
    }

    public int GetRows()
    {
        return grid.GetLength(1);
    }
}

[Serializable]
public class GridObject
{
    int index;
    int rotation;
    bool locked;
    bool star;

    public int Index
    {
        get { return index; }
        set { index = value; }
    }

    public bool Locked
    {
        get { return locked; }
        set { locked = value; }
    }

    public bool Star
    {
        get { return star; }
        set { star = value; }
    }

    public int Rotation
    {
        get { return rotation; }
        set { rotation = value; }
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
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class LevelsLoader
{

    static List<Level> levels;

    static string path = "Assets/Resources/levels";

    public static void SaveLevels(Level level, ref int index)
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


        List<Level> data = levels;
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
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
            Debug.LogError("Levels null buscando archivo");
            ReadLevels();
        }

        return levels;
    }
}
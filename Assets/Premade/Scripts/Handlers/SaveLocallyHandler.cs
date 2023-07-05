using Newtonsoft.Json;
using UnityEngine;

public static class SaveLocallyHandler
{
    #region --- Public Methods ---

    public static void SaveBool(string key, bool boolean)
    {
        PlayerPrefs.SetInt(key, boolean ? 1 : 0);
    }
    
    public static bool LoadBool(string key)
    {
        return PlayerPrefs.GetInt(key) != 0;
    }

    public static void SaveObject<T>(string key, T obj)
    {
        if (obj == null)
        {
            return;
        }

        var json = JsonConvert.SerializeObject(obj);

        if (json == null)
        {
            return;
        }

        SaveString(key, json);
    }
    
    public static T LoadObject<T>(string key)
    {
        var json = LoadString(key);

        try
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch
        {
            return default(T);
        }
    }

    public static void SaveString(string key, string name)
    {
        PlayerPrefs.SetString(key, name);
    }
    
    public static string LoadString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static void SaveInt(string key, int number)
    {
        PlayerPrefs.SetInt(key, number);
    }
    
    public static int LoadInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }
    
    #endregion
}
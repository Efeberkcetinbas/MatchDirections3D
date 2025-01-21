using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StickerSaveSystem 
{
    private const string StickerDataKey = "StickerData";

    public static void SaveStickerData(List<StickerData> stickerDataList)
    {
        string json = JsonUtility.ToJson(new StickerDataWrapper { stickers = stickerDataList });
        PlayerPrefs.SetString(StickerDataKey, json);
        PlayerPrefs.Save();
    }

    public static List<StickerData> LoadStickerData()
    {
        if (!PlayerPrefs.HasKey(StickerDataKey)) return null;

        string json = PlayerPrefs.GetString(StickerDataKey);
        StickerDataWrapper wrapper = JsonUtility.FromJson<StickerDataWrapper>(json);
        return wrapper?.stickers ?? new List<StickerData>();
    }
}

[System.Serializable]
public class StickerDataWrapper
{
    public List<StickerData> stickers;
}

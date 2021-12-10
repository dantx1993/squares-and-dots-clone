using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ThePattern;
using ThePattern.Json;

[Serializable]
public class PlayerData
{
    public DataBind<int> Level = new DataBind<int>(1);
    public DataBind<int> MapIndex = new DataBind<int>(0);
    public DataBind<bool> IsMusicOn = new DataBind<bool>(true);
    public DataBind<bool> IsSoundOn = new DataBind<bool>(true);

    public PlayerData()
    {
        Level = new DataBind<int>(1);
        MapIndex = new DataBind<int>(0);
        IsMusicOn = new DataBind<bool>(true);
        IsSoundOn = new DataBind<bool>(true);
    }
}

public class StorageUserInfo : BaseSingleton<StorageUserInfo>
{
    public PlayerData PlayerData;

    protected override void Init()
    {
        base.Init();
        LoadData();
        PlayerData.Level.RegisterNotifyOnChanged(value => SaveData());
        PlayerData.MapIndex.RegisterNotifyOnChanged(value => SaveData());
        PlayerData.IsMusicOn.RegisterNotifyOnChanged(value => SaveData());
        PlayerData.IsSoundOn.RegisterNotifyOnChanged(value => SaveData());
    }

    private void LoadData()
    {
        string json = PlayerPrefs.GetString(GameConfig.PLAYER_DATA, "");
        if(string.IsNullOrEmpty(json))
        {
            PlayerData = new PlayerData();
            return;
        }
        PlayerData = json.ToObject<PlayerData>();
    }
    private void SaveData()
    {
        string json = PlayerData.ToJsonFormat();
        PlayerPrefs.SetString(GameConfig.PLAYER_DATA, json);
    }

#if UNITY_EDITOR
    [MenuItem("SaveAndLoad/Reset PlayerPrefs")]
    private static void ResetSaveFile()
    {
        PlayerPrefs.DeleteAll();
    }
#endif
}

using System;
using System.Collections.Generic;
using UnityEngine;

using ThePattern.Json;

using LevelEditor;

public class MapConfig
{
    private List<MapData> _mapDatas;

    private int _gridMapWidth = GameConfig.GRID_WIDTH;
    private int _gridMapHeight = GameConfig.GRID_HEIGHT;
    private float _cellSize = GameConfig.GRID_CELLSIZE;
    private Vector3 _gridOriginal = GameConfig.GridOriginal;
    private LevelEditor.Grid _gridMap;

    public LevelEditor.Grid GridMap => _gridMap;

    public int CurrentLevel
    {
        get
        {
            int level = StorageUserInfo.Instance.PlayerData.Level.Value;
            StorageUserInfo.Instance.PlayerData.MapIndex.Value = (level - 1) % _mapDatas.Count;
            return StorageUserInfo.Instance.PlayerData.MapIndex.Value;
        }
    }

    public MapData CurrentMap => _mapDatas[CurrentLevel];

    public MapConfig()
    {
        LoadData();
        _gridOriginal -= new Vector3(_cellSize, _cellSize, 0) * 0.5f;
        _gridMap = new LevelEditor.Grid(_gridMapWidth, _gridMapHeight, _cellSize, _gridOriginal);
    }

    #region LoadData
    public void LoadData()
    {
        string path = GameConfig.MAP_DATA_JSON.Replace(".json", "");
        TextAsset jsonData = Resources.Load(path) as TextAsset;
        if (jsonData == null || String.IsNullOrEmpty(jsonData.ToString()))
        {
            Debug.LogError($"Can't load text asset from {path}");
            _mapDatas = new List<MapData>();
            return;
        }
        Debug.LogWarning("TextAsset is Loaded: " + jsonData.ToString());
        _mapDatas = jsonData.ToString().ToObject<List<MapData>>();
    }
    #endregion
}

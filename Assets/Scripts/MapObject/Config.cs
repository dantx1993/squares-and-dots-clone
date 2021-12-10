using System;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig
{
    // Save and Load
    public const string PLAYER_DATA = "PlayerData";
    public const string MAP_DATA_JSON = "MapData/MapData.json";

    // Grid Map:
    public const float CREATED_CELL_SIZE = 0.9f;
    public const float CREATED_SQUARE_SIZE_XY = 0.9f;
    public const float CREATED_DOT_SIZE_XY = 0.4f;
    public const float CREATED_DIRECTION_SIZE_X = 0.304f;
    public const float CREATED_DIRECTION_SIZE_Y = 0.152f;
    public const int GRID_WIDTH = 7;
    public const int GRID_HEIGHT = 11;
    public const float GRID_CELLSIZE = 0.9f;
    private static Vector3 _gridOriginal = new Vector3(-2.7f, -4.5f, 0f);
    public static Vector3 GridOriginal => _gridOriginal;


    // UI:
    public const string UPDATE_CURRENT_MOVE = "UpdateCurrentMove";
}

[Serializable]
public enum EGameState
{
    PREPARE,
    CHOSING,
    MOVING,
    FINISHING,
}

[Serializable]
public enum EDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

[Serializable]
public enum EColor
{
    RED,
    GREEN,
    BLUE,
    YELLOW
}
[Serializable]
public enum EMapObject
{
    SQUARE,
    DOT,
    DIRECTION
}

[Serializable]
public class SquareConfig
{
    public EDirection direction;
    public EColor color;
}

[Serializable]
public class DotConfig
{
    public EColor color;
}

[Serializable]
public class DirectionConfig
{
    public EDirection direction;
}

[Serializable]
public class ColorDefine
{
    public EColor colorType;
    public Color color;
}

[Serializable]
public class MapObject
{
    public EMapObject type;
    public Vector2Int gridPos;
    public EColor color;
    public EDirection direction;

    public MapObject() {}
    public MapObject(MapObject input)
    {
        type = input.type;
        gridPos = input.gridPos;
        color = input.color;
        direction = input.direction;
    }
}

[Serializable]
public class MapData
{
    public List<MapObject> squareObjects = new List<MapObject>();
    public List<MapObject> dotObjects = new List<MapObject>();
    public List<MapObject> directionObjects = new List<MapObject>();
}

[Serializable]
public class CurrentMove
{
    public Dictionary<SquareController, ValueTuple<Vector3, EDirection, bool>> current;
    public CurrentMove()
    {
        current = new Dictionary<SquareController, ValueTuple<Vector3, EDirection, bool>>();
    }
}

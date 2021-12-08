using System;
using System.Collections.Generic;
using UnityEngine;

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
    public Vector3 position;
    public EColor color;
    public EDirection direction;
}

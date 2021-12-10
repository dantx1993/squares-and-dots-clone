using System;
using System.Collections.Generic;
using UnityEngine;

using ThePattern.Unity;

using LevelEditor;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private List<ColorDefine> _colorDefines;
    [SerializeField] private SquareController _squarePrefab;
    [SerializeField] private DotController _dotPrefab;
    [SerializeField] private DirectionController _directionPrefab;
    [SerializeField] private Transform _levelParent;

    private MapConfig _mapConfig;
    private List<SquareController> _listQuareController;
    private List<CurrentMove> _currentMoves;

    private void Awake()
    {
        Debug.Log("Init MapManager");
        _squarePrefab.CreatePool(5);
        _dotPrefab.CreatePool(5);
        _directionPrefab.CreatePool(5);
        _mapConfig = new MapConfig();
        _listQuareController = new List<SquareController>();
        _currentMoves = new List<CurrentMove>();
        EventHub.Instance.UpdateEvent(GameConfig.UPDATE_CURRENT_MOVE, _currentMoves.Count);
    }

    #region Map
    public void LoadMap()
    {
        MapData currentMapData = _mapConfig.CurrentMap;
        _squarePrefab.RecycleAll();
        _dotPrefab.RecycleAll();
        _directionPrefab.RecycleAll();
        _listQuareController.Clear();
        _currentMoves.Clear();
        EventHub.Instance.UpdateEvent(GameConfig.UPDATE_CURRENT_MOVE, _currentMoves.Count);
        currentMapData.squareObjects.ForEach(square =>
        {
            SquareController temp = _squarePrefab.Spawn<SquareController>(_levelParent, _mapConfig.GridMap.GetWorldPosition(square.gridPos) + new Vector3(GameConfig.CREATED_CELL_SIZE, GameConfig.CREATED_CELL_SIZE, 0) * 0.5f);
            temp.Initialize(square, GameConfig.GRID_CELLSIZE);
            _listQuareController.Add(temp);
        });
        currentMapData.dotObjects.ForEach(dot =>
        {
            DotController temp = _dotPrefab.Spawn<DotController>(_levelParent, _mapConfig.GridMap.GetWorldPosition(dot.gridPos) + new Vector3(GameConfig.CREATED_CELL_SIZE, GameConfig.CREATED_CELL_SIZE, 0) * 0.5f);
            temp.Initialize(dot, GameConfig.GRID_CELLSIZE);
        });
        currentMapData.directionObjects.ForEach(direction =>
        {
            DirectionController temp = _directionPrefab.Spawn<DirectionController>(_levelParent, _mapConfig.GridMap.GetWorldPosition(direction.gridPos) + new Vector3(GameConfig.CREATED_CELL_SIZE, GameConfig.CREATED_CELL_SIZE, 0) * 0.5f);
            temp.Initialize(direction, GameConfig.GRID_CELLSIZE);
        });
    }

    public void CheckForNextLevel()
    {
        bool isDone = true;
        foreach (SquareController square in _listQuareController)
        {
            if (!square.IsRightColor)
            {
                isDone = false;
                break;
            }
        }
        if(isDone)
        {
            GameManager.Instance.CurrentState.Value = EGameState.FINISHING;
            StorageUserInfo.Instance.PlayerData.Level.Value++;
        }
    }
    public void AddCurrentMove()
    {
        CurrentMove currentMove = new CurrentMove();
        _listQuareController.ForEach(square =>
        {
            currentMove.current.Add(square, new ValueTuple<Vector3, EDirection, bool>(square.transform.position, square.Config.direction, square.IsRightColor));
        });
        _currentMoves.Add(currentMove);
        EventHub.Instance.UpdateEvent(GameConfig.UPDATE_CURRENT_MOVE, _currentMoves.Count);
    }
    public void UndoMove()
    {
        CurrentMove currentMove = _currentMoves[_currentMoves.Count - 1];
        _currentMoves.RemoveAt(_currentMoves.Count - 1);
        foreach (KeyValuePair<SquareController, ValueTuple<Vector3, EDirection, bool>> item in currentMove.current)
        {
            item.Key.transform.position = item.Value.Item1;
            item.Key.Direction = item.Value.Item2;
            item.Key.IsRightColor = item.Value.Item3;
        }
        EventHub.Instance.UpdateEvent(GameConfig.UPDATE_CURRENT_MOVE, _currentMoves.Count);
    }
    #endregion

    #region Defination
    public Color GetColorByType(EColor colorType)
    {
        return _colorDefines.Find(colorDefine => colorDefine.colorType == colorType).color;
    }

    public Vector3 GetEulerAnglesByType(EDirection direction)
    {
        Vector3 eulerAngles = Vector3.zero;
        switch (direction)
        {
            case EDirection.UP:
                eulerAngles = new Vector3(0, 0, 180);
                break;
            case EDirection.DOWN:
                eulerAngles = Vector3.zero;
                break;
            case EDirection.LEFT:
                eulerAngles = new Vector3(0, 0, 270);
                break;
            case EDirection.RIGHT:
                eulerAngles = new Vector3(0, 0, 90);
                break;
        }
        return eulerAngles;
    }
    public Vector3 GetMoveDirection(EDirection direction)
    {
        Vector3 result = new Vector3(1, 0, 0);
        switch (direction)
        {
            case EDirection.UP:
                result = new Vector3(0, 1, 0);
                break;
            case EDirection.DOWN:
                result = new Vector3(0, -1, 0);
                break;
            case EDirection.LEFT:
                result = new Vector3(-1, 0, 0);
                break;
            case EDirection.RIGHT:
                result = new Vector3(1, 0, 0);
                break;
        }
        return result;
    }
    #endregion
}

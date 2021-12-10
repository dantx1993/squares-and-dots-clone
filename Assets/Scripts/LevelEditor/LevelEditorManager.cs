using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

using ThePattern.Unity;
using ThePattern.Json;

namespace LevelEditor
{
    public class LevelEditorManager : Singleton<LevelEditorManager>
    {
        // SerializeField:
        [Header("Defination")]
        [SerializeField] private List<ColorDefine> _colorDefines;

        [Header("Grid Map")]
        [SerializeField] private Transform _gridMapOriginalPoint;
        [SerializeField] private int _gridMapWidth;
        [SerializeField] private int _gridMapHeight;
        [SerializeField] private float _cellSize;
        [SerializeField] private SpriteRenderer _gridPrefab;
        [SerializeField] private Transform _gridParent;

        [Header("Map Object")]
        [SerializeField] private SquareObject _squareObjectPrefab;
        [SerializeField] private DotObject _dotObjectPrefab;
        [SerializeField] private DirectionObject _directionObjectPrefab;

        // Private Variables
        private Mouse2DEventUtils _mouseUtils;
        private Grid _gridMap;   
        private MapObject _currentData;
        private List<MapData> _mapDatas;
        private MapData _currentMapData = new MapData();
        private int _currentIndex;
        private List<MapObject> _clickedDatas;
        private List<IMapObject> _currentListView = new List<IMapObject>();

        // Properties:
        public MapObject CurrentData
        {
            get => _currentData;
            set
            {
                _currentData = value;
                EventHub.Instance.UpdateEvent(Config.UPDATE_CHOSING_OBJECT_EVENT, _currentData);
            }
        }
        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                _currentIndex = value;
                _currentMapData = _mapDatas[_currentIndex];
                GenerateViewForMapData(_currentMapData);
                EventHub.Instance.UpdateEvent(Config.UPDATE_CURRENT_MAP_DATA, new ValueTuple<int, int>(_mapDatas.Count, _currentIndex));
            }
        }

        #region Mono Method
        private void Awake() 
        {
            LoadData();
            
            _mouseUtils = new Mouse2DEventUtils();
            _currentData = null;
            _gridPrefab.CreatePool(_gridMapWidth * _gridMapHeight);
            _squareObjectPrefab.CreatePool<SquareObject>(5);
            _dotObjectPrefab.CreatePool<DotObject>(5);
            _directionObjectPrefab.CreatePool<DirectionObject>(5);
            _gridMapOriginalPoint.transform.position -= new Vector3(_cellSize, _cellSize, 0) * 0.5f;
            _gridMap = new Grid(_gridMapWidth, _gridMapHeight, _cellSize, _gridMapOriginalPoint.transform.position);
            _gridMap.OnGridValuedChanged += OnGridChangedValue;
            GenerateGrid();
            CurrentIndex = 0;
        }

        private void Update() 
        {
            if(Input.GetMouseButtonDown(0))    
            {
                if (_mouseUtils.IsPointerOverUI()) return;
                if(_currentData == null) return;
                Debug.Log("Touching Pos: " + Input.mousePosition);
                _clickedDatas = _gridMap.GetValue(_mouseUtils.GetMouseWorldPosition());
                if(CheckForCreateObject(_clickedDatas))
                {
                    _gridMap.SetValue(_mouseUtils.GetMouseWorldPosition(), new MapObject(_currentData));
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (_mouseUtils.IsPointerOverUI()) return;
                _gridMap.SetValue(_mouseUtils.GetMouseWorldPosition(), null);
            }
        }
        #endregion

        #region Grid
        private void GenerateGrid()
        {
            for (int i = 0; i < _gridMap.Width; i++)
            {
                for (int j = 0; j < _gridMap.Height; j++)
                {
                    SpriteRenderer item = _gridPrefab.Spawn<SpriteRenderer>(_gridParent, _gridMap.GetWorldPosition(new Vector2Int(i, j)) + new Vector3(_cellSize, _cellSize, 0) * 0.5f);
                    item.size = new Vector2(_cellSize, _cellSize);
                }
            }
        }
        private void OnGridChangedValue(Vector2Int gridPos)
        {
            Vector3 worldPos = _gridMap.GetWorldPosition(gridPos);
            List<MapObject> datas = _gridMap.GetValue(gridPos);
            if(datas.Count > 0)
            {
                datas.ForEach(data =>
                {
                    if(data != null)
                    {
                        data.gridPos = gridPos;
                        bool isAdded = AddDataInCurrentMapData(data);
                        Debug.Log($"isAdded: {isAdded}");
                        if(isAdded)
                            GenerateObject(new MapObject(data), worldPos);
                    }
                });
            }
            else
            {
                RemoveDataOutCurrentMapData(gridPos);
            }
        }
        #endregion

        #region Generate Map Object
        private void GenerateViewForMapData(MapData data)
        {
            _squareObjectPrefab.RecycleAll();
            _dotObjectPrefab.RecycleAll();
            _directionObjectPrefab.RecycleAll();
            _gridMap.ClearData();
            _currentListView.Clear();
            data.squareObjects.ForEach(square =>
            {
                Debug.Log("square.gridPos: " + square.gridPos);
                GenerateObject(square, _gridMap.GetWorldPosition(square.gridPos));
                _gridMap.SetValue(square.gridPos, square);
            });
            data.dotObjects.ForEach(dot =>
            {
                Debug.Log("dot.gridPos: " + dot.gridPos);
                GenerateObject(dot, _gridMap.GetWorldPosition(dot.gridPos));
                _gridMap.SetValue(dot.gridPos, dot);
            });
            data.directionObjects.ForEach(direction =>
            {
                Debug.Log("direction.gridPos: " + direction.gridPos);
                GenerateObject(direction, _gridMap.GetWorldPosition(direction.gridPos));
                _gridMap.SetValue(direction.gridPos, direction);
            });
        }
        public void DeleteMapByIndex(int index)
        {
            _mapDatas.RemoveAt(index);
            if(_mapDatas.Count <= 0)
            {
                _mapDatas.Add(new MapData());
            }
            CurrentIndex = index;
        }
        public void AddNewMap()
        {
            _mapDatas.Add(new MapData());
            CurrentIndex = _mapDatas.Count - 1;
        }
        private void GenerateObject(MapObject data, Vector3 worldPos)
        {
            IMapObject mapObject = null;
            switch (data.type)
            {
                case EMapObject.SQUARE:
                    mapObject = (IMapObject)_squareObjectPrefab.Spawn<SquareObject>(_gridParent, worldPos + new Vector3(_cellSize, _cellSize, 0) * 0.5f);
                    break;
                case EMapObject.DOT:
                    mapObject = (IMapObject)_dotObjectPrefab.Spawn<DotObject>(_gridParent, worldPos + new Vector3(_cellSize, _cellSize, 0) * 0.5f);
                    break;
                case EMapObject.DIRECTION:
                    mapObject = (IMapObject)_directionObjectPrefab.Spawn<DirectionObject>(_gridParent, worldPos + new Vector3(_cellSize, _cellSize, 0) * 0.5f);
                    break;
            }
            mapObject?.Initialize(data);
            mapObject?.ChangeSize(_cellSize);
            _currentListView.Add(mapObject);
        }
        private bool CheckForCreateObject(List<MapObject> datas)
        {
            if(datas.Count > 0)
            {
                return true;
            }
            bool result = true;
            datas.ForEach(data =>
            {
                if(!(((data.type == EMapObject.SQUARE) && _currentData.type > (int)EMapObject.SQUARE)
                    || ((data.type > (int)EMapObject.SQUARE && _currentData.type == EMapObject.SQUARE))))
                {
                    result = false;
                }
            });
            return result;
        }
        private bool AddDataInCurrentMapData(MapObject data)
        {
            MapObject findObject = null;
            
            switch (data.type)
            {
                case EMapObject.SQUARE:
                    findObject = _currentMapData.squareObjects.Find(obj => obj.gridPos.x == data.gridPos.x && obj.gridPos.y == data.gridPos.y);
                    if(findObject == null)
                    {
                        _currentMapData.squareObjects.Add(new MapObject(data));
                        return true;
                    }
                    return false;
                case EMapObject.DOT:
                    findObject = _currentMapData.dotObjects.Find(obj => obj.gridPos.x == data.gridPos.x && obj.gridPos.y == data.gridPos.y);
                    if (findObject == null)
                    {
                        _currentMapData.dotObjects.Add(new MapObject(data));
                        return true;
                    }
                    return false;
                case EMapObject.DIRECTION:
                    findObject = _currentMapData.directionObjects.Find(obj => obj.gridPos.x == data.gridPos.x && obj.gridPos.y == data.gridPos.y);
                    if (findObject == null)
                    {
                        _currentMapData.directionObjects.Add(new MapObject(data));
                        return true;
                    }
                    return false;
            }

            return false;
        }
        private void RemoveDataOutCurrentMapData(Vector2Int gridPos)
        {
            MapObject findObject = null;
            findObject = _currentMapData.squareObjects.Find(obj => obj.gridPos.x == gridPos.x && obj.gridPos.y == gridPos.y);
            if (findObject != null)
            {
                _currentMapData.squareObjects.Remove(findObject);
                goto End;
            }
            findObject = _currentMapData.dotObjects.Find(obj => obj.gridPos.x == gridPos.x && obj.gridPos.y == gridPos.y);
            if (findObject != null)
            {
                _currentMapData.dotObjects.Remove(findObject);
                goto End;
            }
            findObject = _currentMapData.directionObjects.Find(obj => obj.gridPos.x == gridPos.x && obj.gridPos.y == gridPos.y);
            if (findObject != null)
            {
                _currentMapData.directionObjects.Remove(findObject);
                goto End;
            }
            End:
            IMapObject mapObject = _currentListView.Find(obj => 
                                                obj.GetData().gridPos.x == gridPos.x 
                                                && obj.GetData().gridPos.y == gridPos.y 
                                                && obj.GetData().type == findObject.type);
            if(mapObject != null)
            {
                _currentListView.Remove(mapObject);
                mapObject.GetGameObject().Recycle();
            }
        }
        #endregion

        #region Utils Renderer
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
        #endregion

        #region Save And Load
        public void LoadData()
        {
#if UNITY_EDITOR
            string path = GameConfig.MAP_DATA_JSON.Replace(".json", "");
            TextAsset jsonData = Resources.Load(path) as TextAsset; 
            if(jsonData == null || String.IsNullOrEmpty(jsonData.ToString()))
            {
                Debug.LogError($"Can't load text asset from {path}");
                _mapDatas = new List<MapData>();
                _mapDatas.Add(new MapData());
                return;
            }
            Debug.LogWarning("TextAsset is Loaded: " + jsonData.ToString());
            _mapDatas = jsonData.ToString().ToObject<List<MapData>>();
#endif
        }
        public void SaveData()
        {
#if UNITY_EDITOR
            try
            {
                string path = Path.Combine(Application.dataPath, "Resources", GameConfig.MAP_DATA_JSON);
                string json = _mapDatas.ToJsonFormat();
                Debug.Log(json);
                if(File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            writer.Write(json);
                        }
                    }
                }
                else
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            writer.Write(json);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
#endif
        }

        #endregion
    }
}
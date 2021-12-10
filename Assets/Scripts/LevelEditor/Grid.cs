using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class Grid
    {
        private Action<Vector2Int> _onGridValueChanged;
        private int _width;
        private int _height;
        private float _cellSize;
        private Vector3 _originPosition;
        private List<MapObject>[,] _gridArray;

        public int Width => _width;
        public int Height => _height;
        public float CellSize => _cellSize;
        public Action<Vector2Int> OnGridValuedChanged
        {
            get => _onGridValueChanged;
            set => _onGridValueChanged = value;
        }

        public Grid(int width, int height, float cellSize, Vector3 originPosition)
        {
            this._width = width;
            this._height = height;
            this._cellSize = cellSize;
            this._originPosition = originPosition;

            _gridArray = new List<MapObject>[width, height];
            
            for (int i = 0; i < _gridArray.GetLength(0); i++)
            {
                for (int j = 0; j < _gridArray.GetLength(1); j++)
                {
                    _gridArray[i, j] = new List<MapObject>();
                }
            }
        }

        public Grid(int width, int height, float cellSize, Vector3 originPosition, Action<List<MapObject>[,]> showDebug)
        {
            this._width = width;
            this._height = height;
            this._cellSize = cellSize;
            this._originPosition = originPosition;

            _gridArray = new List<MapObject>[width, height];

            for (int i = 0; i < _gridArray.GetLength(0); i++)
            {
                for (int j = 0; j < _gridArray.GetLength(1); j++)
                {
                    _gridArray[i, j] = new List<MapObject>();
                }
            }

            showDebug?.Invoke(_gridArray);
        }

        public Vector3 GetWorldPosition(Vector2Int gridPos)
        {
            return new Vector3(gridPos.x, gridPos.y) * _cellSize + _originPosition;
        }

        private Vector2Int GetXY(Vector3 worldPosition)
        {
            Vector2Int result = new Vector2Int
            (
                x: Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize),
                y: Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize)
            );
            Debug.Log("Grid Pos: " + result);
            return result;
        }

        public void SetValue(Vector2Int gridPos, MapObject value)
        {
            if (gridPos.x >= 0 && gridPos.y >= 0 && gridPos.x < _width && gridPos.y < _height)
            {
                if(value != null)
                {
                    _gridArray[gridPos.x, gridPos.y].Add(value);
                }
                else 
                {
                    _gridArray[gridPos.x, gridPos.y].Clear();
                }
                OnGridValuedChanged?.Invoke(gridPos);
            }
        }

        public void SetValue(Vector3 worldPosition, MapObject value)
        {
            Vector2Int gridPos = GetXY(worldPosition);
            SetValue(gridPos, value);
        }

        public List<MapObject> GetValue(Vector2Int gridPos)
        {
            if (gridPos.x >= 0 && gridPos.y >= 0 && gridPos.x < _width && gridPos.y < _height)
            {
                return _gridArray[gridPos.x, gridPos.y];
            }
            else
            {
                return null;
            }
        }

        public List<MapObject> GetValue(Vector3 worldPosition)
        {
            Vector2Int gridPos = GetXY(worldPosition);
            return GetValue(gridPos);
        }

        public void ClearData()
        {
            foreach (List<MapObject> item in _gridArray)
            {
                item.Clear();
            }
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public interface IMapObject
    {
        void Initialize(MapObject data);
        void ChangeSize(float cellSize);
        MapObject GetData();
        GameObject GetGameObject();
    }
}


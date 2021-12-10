using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public interface ICreateTab
    {
        MapObject GetData();
    }
}

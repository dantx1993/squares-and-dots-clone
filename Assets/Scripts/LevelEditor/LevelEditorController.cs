using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ThePattern.Unity;

public class LevelEditorController : Singleton<LevelEditorController>
{
    [SerializeField] private List<ColorDefine> _colorDefines;

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
}

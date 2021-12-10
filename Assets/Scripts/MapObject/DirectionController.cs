using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour
{
    [SerializeField] private DirectionConfig _config;
    [SerializeField] private GameObject _arrow;
    public DirectionConfig Config => _config;

    public EDirection Direction
    {
        set
        {
            _config.direction = value;
            ChangeDirectionRenderer();
        }
    }

    public void Initialize(MapObject data, float cellSize)
    {
        Direction = data.direction;
        float changedValue = cellSize / GameConfig.CREATED_CELL_SIZE;
        _arrow.GetComponent<SpriteRenderer>().size = new Vector2(GameConfig.CREATED_DIRECTION_SIZE_X * changedValue, GameConfig.CREATED_DIRECTION_SIZE_Y * changedValue);
    }

    #region Renderer
    private void ChangeDirectionRenderer()
    {
        _arrow.transform.localEulerAngles = MapManager.Instance.GetEulerAnglesByType(_config.direction);
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotController : MonoBehaviour
{
    [SerializeField] private DotConfig _config;
    [SerializeField] private SpriteRenderer _dotRenderer;
    public DotConfig Config => _config;
    public EColor Color
    {
        set
        {
            _config.color = value;
            ChangeColorRenderer();
        }
    }

    public void Initialize(MapObject data, float cellSize)
    {
        Color = data.color;
        float changedValue = cellSize / GameConfig.CREATED_CELL_SIZE;
        _dotRenderer.size = new Vector2(GameConfig.CREATED_DOT_SIZE_XY * changedValue, GameConfig.CREATED_DOT_SIZE_XY * changedValue);
    }

    #region Renderer
    private void ChangeColorRenderer()
    {
        _dotRenderer.color = MapManager.Instance.GetColorByType(_config.color);
    }
    #endregion
}

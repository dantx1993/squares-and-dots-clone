using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{   
    public class SquareObject : MonoBehaviour, IMapObject
    {
        [Header("Renderer")]
        [SerializeField] private SpriteRenderer _squareRenderer;
        [SerializeField] private SpriteRenderer _arrow;

        private MapObject _data;

        public MapObject GetData() => _data;

        public void Initialize(MapObject data)
        {
            _data = data;
            ChangeDirectionRenderer(data.direction);
            ChangeColorRenderer(data.color);
        }

        public void ChangeSize(float cellSize)
        {
            float changedValue = cellSize / GameConfig.CREATED_CELL_SIZE;
            _squareRenderer.size = new Vector2(GameConfig.CREATED_SQUARE_SIZE_XY * changedValue, GameConfig.CREATED_SQUARE_SIZE_XY * changedValue);
            _arrow.size = new Vector2(GameConfig.CREATED_DIRECTION_SIZE_X * changedValue, GameConfig.CREATED_DIRECTION_SIZE_Y * changedValue);
        }

        public GameObject GetGameObject() => this.gameObject;

        #region Renderer
        private void ChangeDirectionRenderer(EDirection direction)
        {
            _arrow.transform.localEulerAngles = LevelEditorManager.Instance.GetEulerAnglesByType(direction);
        }
        private void ChangeColorRenderer(EColor color)
        {
            _squareRenderer.color = LevelEditorManager.Instance.GetColorByType(color);
        }
        #endregion
    }
}
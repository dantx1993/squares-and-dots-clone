using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class DotObject : MonoBehaviour, IMapObject
    {
        [Header("Renderer")]
        [SerializeField] private SpriteRenderer _dotRenderer;

        private MapObject _data;

        public MapObject GetData() => _data;

        public void Initialize(MapObject data)
        {
            _data = data;
            ChangeColorRenderer(data.color);
        }

        public void ChangeSize(float cellSize)
        {
            float changedValue = cellSize / GameConfig.CREATED_CELL_SIZE;
            _dotRenderer.size = new Vector2(GameConfig.CREATED_DOT_SIZE_XY * changedValue, GameConfig.CREATED_DOT_SIZE_XY * changedValue);
        }

        public GameObject GetGameObject() => this.gameObject;

        #region Renderer
        private void ChangeColorRenderer(EColor color)
        {
            _dotRenderer.color = LevelEditorManager.Instance.GetColorByType(color);
        }
        #endregion
    }
}

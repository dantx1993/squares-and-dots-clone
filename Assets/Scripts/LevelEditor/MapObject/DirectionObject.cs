using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{  
    public class DirectionObject : MonoBehaviour, IMapObject
    {
        [Header("Renderer")]
        [SerializeField] private SpriteRenderer _arrow;

        private MapObject _data;

        public MapObject GetData() => _data;

        public void Initialize(MapObject data)
        {
            _data = data;
            ChangeDirectionRenderer(data.direction);
        }
        public void ChangeSize(float cellSize)
        {
            float changedValue = cellSize / GameConfig.CREATED_CELL_SIZE;
            _arrow.size = new Vector2(GameConfig.CREATED_DIRECTION_SIZE_X * changedValue, GameConfig.CREATED_DIRECTION_SIZE_Y * changedValue);
        }

        public GameObject GetGameObject() => this.gameObject;

        #region Renderer
        private void ChangeDirectionRenderer(EDirection direction)
        {
            _arrow.transform.localEulerAngles = LevelEditorManager.Instance.GetEulerAnglesByType(direction);
        }
        #endregion
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class CurrentSquare : MonoBehaviour, ICurrentChoose
    {
        [SerializeField] private Image _squareRenderer;
        [SerializeField] private Transform _arrow;

        public void ChangeData(MapObject data)
        {
            ChangeColorRenderer(data);
            ChangeDirectionRenderder(data);
        }

        #region Renderer
        private void ChangeColorRenderer(MapObject data)
        {
            _squareRenderer.color = LevelEditorManager.Instance.GetColorByType(data.color);
        }
        private void ChangeDirectionRenderder(MapObject data)
        {
            _arrow.transform.localEulerAngles = LevelEditorManager.Instance.GetEulerAnglesByType(data.direction);
        }
        #endregion
    }
}
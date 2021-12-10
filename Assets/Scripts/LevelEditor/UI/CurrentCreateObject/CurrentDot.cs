using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class CurrentDot : MonoBehaviour, ICurrentChoose
    {
        [SerializeField] private Image _dotRenderer;

        public void ChangeData(MapObject data)
        {
            ChangeColorRenderer(data);
        }

        #region Renderer
        private void ChangeColorRenderer(MapObject data)
        {
            _dotRenderer.color = LevelEditorManager.Instance.GetColorByType(data.color);
        }
        #endregion
    }
}

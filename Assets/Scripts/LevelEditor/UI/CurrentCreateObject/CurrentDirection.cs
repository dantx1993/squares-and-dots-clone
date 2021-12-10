using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class CurrentDirection : MonoBehaviour, ICurrentChoose
    {
        [SerializeField] private Transform _arrow;

        public void ChangeData(MapObject data)
        {
            ChangeDirectionRenderder(data);
        }

        #region Renderer
        private void ChangeDirectionRenderder(MapObject data)
        {
            _arrow.transform.localEulerAngles = LevelEditorManager.Instance.GetEulerAnglesByType(data.direction);
        }
        #endregion
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class CreateDirectionTab : MonoBehaviour, ICreateTab
    {
        [SerializeField] private Transform _arrow;
        [SerializeField] private Dropdown _directionDropdown;

        private MapObject _data;

        private EDirection Direction
        {
            set
            {
                _data.direction = value;
                ChangeDirectionRenderder();
            }
        }

        private void Awake()
        {
            _data = new MapObject();
            _data.type = EMapObject.DIRECTION;
            SetUpDirectionDropdown();
            Direction = (EDirection)_directionDropdown.value;
        }

        private void OnEnable()
        {
            _directionDropdown.onValueChanged.AddListener(OnDirectionDropdownChanged);
        }

        private void OnDisable()
        {
            _directionDropdown.onValueChanged.RemoveListener(OnDirectionDropdownChanged);
        }

        #region Dropdown
        private void SetUpDirectionDropdown()
        {
            _directionDropdown.options.Clear();
            List<EDirection> directions = ((EDirection[])Enum.GetValues(typeof(EDirection))).ToList();
            directions.ForEach(value =>
            {
                _directionDropdown.options.Add(new Dropdown.OptionData(value.ToString()));
            });
        }
        private void OnDirectionDropdownChanged(int value)
        {
            Direction = (EDirection)value;
        }
        #endregion

        #region Renderer
        private void ChangeDirectionRenderder()
        {
            _arrow.transform.localEulerAngles = LevelEditorManager.Instance.GetEulerAnglesByType(_data.direction);
        }
        #endregion

        public MapObject GetData()
        {
            return _data;
        }
    }
}

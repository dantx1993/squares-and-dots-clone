using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class CreateSquareTab : MonoBehaviour
    {
        [SerializeField] private Image _squareRenderer;
        [SerializeField] private Transform _arrow;
        [SerializeField] private Dropdown _directionDropdown;
        [SerializeField] private Dropdown _colorDropdown;

        private MapObject _data;

        private EColor Color
        {
            set
            {
                _data.color = value;
                ChangeColorRenderer();
            }
        }

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
            _data.type = EMapObject.SQUARE;
            SetUpDirectionDropdown();
            SetUpColorDropdown();
            Color = (EColor)_colorDropdown.value;
            Direction = (EDirection)_directionDropdown.value;
        }

        private void OnEnable() 
        {
            _directionDropdown.onValueChanged.AddListener(OnDirectionDropdownChanged);
            _colorDropdown.onValueChanged.AddListener(OnColorDropdownChanged);    
        }

        private void OnDisable()
        {
            _directionDropdown.onValueChanged.RemoveListener(OnDirectionDropdownChanged);
            _colorDropdown.onValueChanged.RemoveListener(OnColorDropdownChanged);
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
        private void SetUpColorDropdown()
        {
            _colorDropdown.options.Clear();
            List<EColor> directions = ((EColor[])Enum.GetValues(typeof(EColor))).ToList();
            directions.ForEach(value =>
            {
                _colorDropdown.options.Add(new Dropdown.OptionData(value.ToString()));
            });
        }
        private void OnDirectionDropdownChanged(int value)
        {
            Direction = (EDirection)value;
        }
        private void OnColorDropdownChanged(int value)
        {
            Color = (EColor)value;
        }
        #endregion

        #region Renderer
        private void ChangeColorRenderer()
        {
            _squareRenderer.color = LevelEditorController.Instance.GetColorByType(_data.color);
        }
        private void ChangeDirectionRenderder()
        {
            _arrow.transform.localEulerAngles = LevelEditorController.Instance.GetEulerAnglesByType(_data.direction);
        }
        #endregion
    }
}


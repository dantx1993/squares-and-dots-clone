using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class CreateDotTab : MonoBehaviour, ICreateTab
    {
        [SerializeField] private Image _dotRenderer;
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

        private void Awake()
        {
            _data = new MapObject();
            _data.type = EMapObject.DOT;
            SetUpColorDropdown();
            Color = (EColor)_colorDropdown.value;
        }

        private void OnEnable()
        {
            _colorDropdown.onValueChanged.AddListener(OnColorDropdownChanged);
        }

        private void OnDisable()
        {
            _colorDropdown.onValueChanged.RemoveListener(OnColorDropdownChanged);
        }

        #region Dropdown
        private void SetUpColorDropdown()
        {
            _colorDropdown.options.Clear();
            List<EColor> directions = ((EColor[])Enum.GetValues(typeof(EColor))).ToList();
            directions.ForEach(value =>
            {
                _colorDropdown.options.Add(new Dropdown.OptionData(value.ToString()));
            });
        }
        private void OnColorDropdownChanged(int value)
        {
            Color = (EColor)value;
        }
        #endregion

        #region Renderer
        private void ChangeColorRenderer()
        {
            _dotRenderer.color = LevelEditorManager.Instance.GetColorByType(_data.color);
        }
        #endregion

        public MapObject GetData()
        {
            return _data;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class CreatePopupController : MonoBehaviour
    {
        [SerializeField] private CreateSquareTab _createSquareTab;
        [SerializeField] private CreateDotTab _createDotTab;
        [SerializeField] private CreateDirectionTab _createDirectionTab;

        [SerializeField] private Button _squareTabButton;
        [SerializeField] private Button _dotTabButton;
        [SerializeField] private Button _directionTabButton;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _closeButton;

        [SerializeField] private Color _activeTabColor;
        [SerializeField] private Color _deactiveTabColor;
        [SerializeField] private Image _squareTabButonImage;
        [SerializeField] private Image _dotTabButtonImage;
        [SerializeField] private Image _directionTabButtonImage;

        private ICreateTab _currentTab;
        private EMapObject _currentType;

        public EMapObject CurrentType
        {
            set
            {
                _currentType = value;
                ChangeTabButton(_currentType);
            }
        }

        #region Mono Method
        private void OnEnable() 
        {
            CurrentType = EMapObject.SQUARE;
            ChangeTabButton(_currentType);
            AddAllButtonEvent();
        }
        private void OnDisable() 
        {
            RemoveAllButtonEvent();
        }
        #endregion

        #region UI
        public void Show()
        {
            this.gameObject.SetActive(true);
        }
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
        private void ChangeTabButton(EMapObject type)
        {
            _createSquareTab.gameObject.SetActive(type == EMapObject.SQUARE);
            _createDotTab.gameObject.SetActive(type == EMapObject.DOT);
            _createDirectionTab.gameObject.SetActive(type == EMapObject.DIRECTION);
            _squareTabButonImage.color = type == EMapObject.SQUARE ? _activeTabColor : _deactiveTabColor;
            _dotTabButtonImage.color = type == EMapObject.DOT ? _activeTabColor : _deactiveTabColor;
            _directionTabButtonImage.color = type == EMapObject.DIRECTION ? _activeTabColor : _deactiveTabColor;
            switch (type)
            {
                case EMapObject.SQUARE:
                    _currentTab = _createSquareTab;
                    break;
                case EMapObject.DOT:
                    _currentTab = _createDotTab;
                    break;
                case EMapObject.DIRECTION:
                    _currentTab = _createDirectionTab;
                    break;
            }
        }
        #endregion

        #region Button Event
        private void AddAllButtonEvent()
        {
            _squareTabButton.onClick.AddListener(OnSquareTabButtonClicked);
            _dotTabButton.onClick.AddListener(OnDotTabButtonClicked);
            _directionTabButton.onClick.AddListener(OnDirectionTabButtonClicked);
            _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }
        private void RemoveAllButtonEvent()
        {
            _squareTabButton.onClick.RemoveListener(OnSquareTabButtonClicked);
            _dotTabButton.onClick.RemoveListener(OnDotTabButtonClicked);
            _directionTabButton.onClick.RemoveListener(OnDirectionTabButtonClicked);
            _confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }
        private void OnSquareTabButtonClicked()
        {
            if(_currentType == EMapObject.SQUARE) return;
            CurrentType = EMapObject.SQUARE;
        }
        private void OnDotTabButtonClicked()
        {
            if (_currentType == EMapObject.DOT) return;
            CurrentType = EMapObject.DOT;
        }
        private void OnDirectionTabButtonClicked()
        {
            if (_currentType == EMapObject.DIRECTION) return;
            CurrentType = EMapObject.DIRECTION;
        }
        private void OnConfirmButtonClicked()
        {
            LevelEditorManager.Instance.CurrentData = _currentTab.GetData();
            Hide();
        }
        private void OnCloseButtonClicked()
        {
            Hide();
        }
        #endregion
    }
}


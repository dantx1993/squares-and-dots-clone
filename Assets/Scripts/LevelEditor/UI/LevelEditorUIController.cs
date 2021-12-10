using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ThePattern.Unity;

namespace LevelEditor
{
    public class LevelEditorUIController : MonoBehaviour
    {
        [SerializeField] private Dropdown _levelChooseDropdown;
        [SerializeField] private Text _levelText;

        [SerializeField] private Button _newLvlButton;
        [SerializeField] private Button _createNewObjectButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _deleteButton;

        [SerializeField] private CreatePopupController _createPopupController;

        [SerializeField] private CurrentChoose _currentChoose;

        private void Awake() 
        {
            _createPopupController.Hide();
            _currentChoose.HideAllCurrentChoose();
            EventHub.Instance.RegisterEvent(Config.UPDATE_CHOSING_OBJECT_EVENT, OnUpdateChosingObject);
            EventHub.Instance.RegisterEvent(Config.UPDATE_CURRENT_MAP_DATA, OnUpdateDropdownData);
        }
        private void OnDestroy()
        {
            EventHub.Instance.RemoveEvent(Config.UPDATE_CHOSING_OBJECT_EVENT, OnUpdateChosingObject);
            EventHub.Instance.RemoveEvent(Config.UPDATE_CURRENT_MAP_DATA, OnUpdateDropdownData);
        }
        private void OnEnable() 
        {
            AddAllButtonEvent();
            OnAddAllDataEvent();
        }
        private void OnDisable()
        {
            RemoveAllButtonEvent();
            OnRemoveAllDataEvent();
        }

        #region Data Event
        private void OnAddAllDataEvent()
        {
            _levelChooseDropdown.onValueChanged.AddListener(OnLevelChooseDropdownChanged);
        }
        private void OnRemoveAllDataEvent()
        {
            _levelChooseDropdown.onValueChanged.RemoveListener(OnLevelChooseDropdownChanged);
        }
        public void OnUpdateChosingObject(object data)
        {
            MapObject currentMapObject = (MapObject)data;
            _currentChoose.ChangeData(currentMapObject);
        }
        private void OnUpdateDropdownData(object data)
        {
            ValueTuple<int, int> receiveData = (ValueTuple<int, int>)data;
            ChangeDropdownData(receiveData.Item1, receiveData.Item2);
        }
        private void OnLevelChooseDropdownChanged(int value)
        {
            if(LevelEditorManager.Instance.CurrentIndex != value)
                LevelEditorManager.Instance.CurrentIndex = value;
            _levelText.text = "Level " + (value + 1);
        }
        #endregion

        #region UI
        private void ChangeDropdownData(int levelTotal, int currentLevel)
        {
            if (_levelChooseDropdown.options.Count != levelTotal)
            {
                _levelChooseDropdown.options.Clear();
                for (int i = 1; i <= levelTotal; i++)
                {
                    _levelChooseDropdown.options.Add(new Dropdown.OptionData("" + i));
                }
            }
            if (_levelChooseDropdown.value != currentLevel)
                _levelChooseDropdown.value = currentLevel;
            _levelText.text = "Level " + (_levelChooseDropdown.value + 1);
        }
        #endregion

        #region Button Event
        private void AddAllButtonEvent()
        {
            _saveButton.onClick.AddListener(OnSaveButtonClicked);
            _deleteButton.onClick.AddListener(OnDeleteButtonClicked);
            _newLvlButton.onClick.AddListener(OnNewLevelButtonClicked);
            _createNewObjectButton.onClick.AddListener(OnCreateNewObjectButtonClicked);
        }
        private void RemoveAllButtonEvent()
        {
            _saveButton.onClick.RemoveListener(OnSaveButtonClicked);
            _deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
            _newLvlButton.onClick.RemoveListener(OnNewLevelButtonClicked);
            _createNewObjectButton.onClick.RemoveListener(OnCreateNewObjectButtonClicked);
        }
        private void OnSaveButtonClicked()
        {
            LevelEditorManager.Instance.SaveData();
        }
        private void OnDeleteButtonClicked()
        {
            LevelEditorManager.Instance.DeleteMapByIndex(_levelChooseDropdown.value);
        }
        private void OnNewLevelButtonClicked()
        {
            LevelEditorManager.Instance.AddNewMap();
        }
        private void OnCreateNewObjectButtonClicked()
        {
            _createPopupController.Show();
        }
        #endregion

    }
}

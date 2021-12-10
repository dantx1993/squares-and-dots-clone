using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ThePattern.Unity;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] private Text _levelText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _undoButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _soundButton;

    [SerializeField] private Image _musicButtonImage;
    [SerializeField] private Image _soundButtonImage;
    [SerializeField] private Sprite _soundOnSprite;
    [SerializeField] private Sprite _soundOffSprite;
    [SerializeField] private Sprite _musicOnSprite;
    [SerializeField] private Sprite _musicOffSpite;

    private void Awake() 
    {
        _levelText.text = StorageUserInfo.Instance.PlayerData.Level.Value + "\nLevel";
        _musicButtonImage.sprite = StorageUserInfo.Instance.PlayerData.IsMusicOn.Value ? _musicOnSprite : _musicOffSpite;
        _soundButtonImage.sprite = StorageUserInfo.Instance.PlayerData.IsSoundOn.Value ? _soundOnSprite : _soundOffSprite;
        StorageUserInfo.Instance.PlayerData.Level.RegisterNotifyOnChanged(value =>
        {
            _levelText.text = value + "\nLevel";
        });
        StorageUserInfo.Instance.PlayerData.IsMusicOn.RegisterNotifyOnChanged(value =>
        {
            _musicButtonImage.sprite = value ? _musicOnSprite : _musicOffSpite;
        });
        StorageUserInfo.Instance.PlayerData.IsSoundOn.RegisterNotifyOnChanged(value =>
        {
            _soundButtonImage.sprite = value ? _soundOnSprite : _soundOffSprite;
        });
        EventHub.Instance.RegisterEvent(GameConfig.UPDATE_CURRENT_MOVE, OnCurrentMoveUpdated);
    }
    private void OnEnable() 
    {
        AddAllButtonEvent();
    }
    private void OnDisable() 
    {
        RemoveAllButtonEvent();
    }
    private void OnDestroy() 
    {
        EventHub.Instance.RemoveEvent(GameConfig.UPDATE_CURRENT_MOVE, OnCurrentMoveUpdated);
    }

    #region Button Event
    public void AddAllButtonEvent()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _undoButton.onClick.AddListener(OnUndoButtonClicked);
        _soundButton.onClick.AddListener(OnSoundButtonClicked);
        _musicButton.onClick.AddListener(OnMusicButtonClicked);
    }
    public void RemoveAllButtonEvent()
    {
        _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        _undoButton.onClick.RemoveListener(OnUndoButtonClicked);
        _soundButton.onClick.RemoveListener(OnSoundButtonClicked);
        _musicButton.onClick.RemoveListener(OnMusicButtonClicked);
    }
    private void OnRestartButtonClicked()
    {
        SoundManager.Instance.PlaySFX("Undo");
        if (GameManager.Instance.CurrentState.Value != EGameState.CHOSING) return;
        GameManager.Instance.NextLevel();
    }
    private void OnUndoButtonClicked()
    {
        SoundManager.Instance.PlaySFX("Undo");
        if (GameManager.Instance.CurrentState.Value != EGameState.CHOSING) return;
        MapManager.Instance.UndoMove();
    }
    private void OnMusicButtonClicked()
    {
        SoundManager.Instance.PlaySFX("MenuClicked");
        StorageUserInfo.Instance.PlayerData.IsMusicOn.Value = !StorageUserInfo.Instance.PlayerData.IsMusicOn.Value;
    }
    private void OnSoundButtonClicked()
    {
        SoundManager.Instance.PlaySFX("MenuClicked");
        StorageUserInfo.Instance.PlayerData.IsSoundOn.Value = !StorageUserInfo.Instance.PlayerData.IsSoundOn.Value;
    }
    #endregion

    private void OnCurrentMoveUpdated(object obj)
    {
        int currentMoveCount = (int)obj;
        _undoButton.interactable = currentMoveCount > 0;
        _restartButton.interactable = currentMoveCount > 0;
    }
}

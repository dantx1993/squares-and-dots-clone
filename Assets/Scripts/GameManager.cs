using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ThePattern;
using ThePattern.Unity;

public class GameManager : Singleton<GameManager>
{
    private DataBind<EGameState> _currentState = new DataBind<EGameState>(EGameState.PREPARE);

    public DataBind<EGameState> CurrentState => _currentState;

    private void Awake() 
    {
        StorageUserInfo.Instance.PlayerData.Level.RegisterNotifyOnChanged(value =>
        {
            NextLevel();
        });
        // _currentState.RegisterNotifyOnChanged(value => Debug.Log(value));
    }

    private void Start() 
    {
        _currentState.Value = EGameState.PREPARE;
        NextLevel();
        SoundManager.Instance.PlayBGM("BGM01");
    }
    public void NextLevel()
    {
        _currentState.Value = EGameState.PREPARE;
        MapManager.Instance.LoadMap();
        this.ActionWaitTime(0.5f, () =>
        {
            _currentState.Value = EGameState.CHOSING;
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using ThePattern.Unity;

public class SquareController : MonoBehaviour
{
    // SerializeField:
    [Header("Config")]
    [SerializeField] private SquareConfig _config;
    [Header("Renderer")]
    [SerializeField] private SpriteRenderer _squareRenderer;
    [SerializeField] private SpriteRenderer _arrow;
    [Header("Controller")]
    [SerializeField] private ClickableObject _clickable;
    [SerializeField] private SquareChecking _checking;

    private bool _isRightColor;

    // Properties:
    public SquareConfig Config => _config;
    public bool IsRightColor
    {
        get => _isRightColor;
        set => _isRightColor = value;
    }
    public float Size => _squareRenderer.size.x;
    public EDirection Direction
    {
        set
        {
            _config.direction = value;
            ChangeDirectionRenderer();
        }
    }
    public EColor Color
    {
        set
        {
            _config.color = value;
            ChangeColorRenderer();
        }
    }

    #region MonoBehaviour Method
    private void OnEnable() 
    {
        _clickable.OnCompleted += OnClick;
    }

    private void OnDisable() 
    {
        _clickable.OnCompleted -= OnClick;
        DOTween.KillAll();
        StopAllCoroutines();
    }
    #endregion

    public void Initialize(MapObject data, float cellSize)
    {
        Color = data.color;
        Direction = data.direction;
        float changedValue = cellSize / GameConfig.CREATED_CELL_SIZE;
        _squareRenderer.size = new Vector2(GameConfig.CREATED_SQUARE_SIZE_XY * changedValue, GameConfig.CREATED_SQUARE_SIZE_XY * changedValue);
        _arrow.size = new Vector2(GameConfig.CREATED_DIRECTION_SIZE_X * changedValue, GameConfig.CREATED_DIRECTION_SIZE_Y * changedValue);
        _clickable.FitCollider(_squareRenderer);
        _isRightColor = false;
    }

    #region Player Input
    private void OnClick()
    {
        if(GameManager.Instance.CurrentState.Value != EGameState.CHOSING) 
        {
            return;
        }
        Moving(_config.direction, true);
    }
    #endregion

    public void Moving(EDirection direction, bool isSaveData = false)
    {
        SoundManager.Instance.PlaySFX("Move");
        GoOutObject();
        if(isSaveData)
        {
            GameManager.Instance.CurrentState.Value = EGameState.MOVING;
            MapManager.Instance.AddCurrentMove();
        }
        _checking.CheckingPushSquare(direction);
        this.transform.DOMove(this.transform.position + MapManager.Instance.GetMoveDirection(direction) * Size, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                if (isSaveData)
                {
                    this.ActionWaitForEndOfFrame(() =>
                    {
                        GameManager.Instance.CurrentState.Value = EGameState.CHOSING;
                    });
                }
                _checking.Checking(OnCollisionWithDot, OnCollisionWithDirection);
                Debug.Log("Move completed");
            });
    }

    #region Renderer
    private void ChangeDirectionRenderer()
    {
        _arrow.transform.localEulerAngles = MapManager.Instance.GetEulerAnglesByType(_config.direction);
    }
    private void ChangeColorRenderer()
    {
        _squareRenderer.color = MapManager.Instance.GetColorByType(_config.color);
    }
    #endregion

    #region Collision
    private void OnCollisionWithDot(EColor color)
    {
        
        _isRightColor = color == _config.color;
        if(_isRightColor)
        {
            SoundManager.Instance.PlaySFX("OnPosition");
        }
        if(GameManager.Instance.CurrentState.Value != EGameState.FINISHING)
            MapManager.Instance.CheckForNextLevel();
    }
    private void OnCollisionWithDirection(EDirection direction)
    {
        Debug.Log(direction);
        Direction = direction;
    }
    private void GoOutObject()
    {
        _isRightColor = false;
    }
    #endregion
}

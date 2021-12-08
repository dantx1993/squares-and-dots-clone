using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

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
    public float Size => _squareRenderer.size.x;
    private EDirection Direction
    {
        set
        {
            _config.direction = value;
            ChangeDirectionRenderer();
        }
    }

    #region MonoBehaviour Method
    private void Awake() 
    {
        _clickable.FitCollider(_squareRenderer);
        _isRightColor = false;
        ChangeDirectionRenderer();
        ChangeColorRenderer();
    }

    private void OnEnable() 
    {
        _clickable.OnCompleted += OnClick;
    }

    private void OnDisable() 
    {
        _clickable.OnCompleted -= OnClick;
    }
    #endregion

    #region Player Input
    private void OnClick()
    {
        GoOutObject();
        this.transform.DOMove(this.transform.position + GetMoveDirection(_config.direction) * Size, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Debug.Log($"{this.gameObject.name} completed move");
                _checking.Checking(OnCollisionWithDot, OnCollisionWithDirection);
            });
    }
    #endregion

    #region Renderer
    private void ChangeDirectionRenderer()
    {
        Vector3 eulerAngles = Vector3.zero;
        switch (_config.direction)
        {
            case EDirection.UP:
                eulerAngles = new Vector3(0, 0, 180);
                break;
            case EDirection.DOWN:
                eulerAngles = Vector3.zero;
                break;
            case EDirection.LEFT:
                eulerAngles = new Vector3(0, 0, 270);
                break;
            case EDirection.RIGHT:
                eulerAngles = new Vector3(0, 0, 90);
                break;
        }
        _arrow.transform.localEulerAngles = eulerAngles;
    }
    private void ChangeColorRenderer()
    {
        _squareRenderer.color = MapManager.Instance.GetColorByType(_config.color);
    }
    #endregion

    #region Move
    private Vector3 GetMoveDirection(EDirection direction)
    {
        Vector3 result = new Vector3(1, 0, 0);
        switch (direction)
        {
            case EDirection.UP:
                result = new Vector3(0, 1, 0);
                break;
            case EDirection.DOWN:
                result = new Vector3(0, -1, 0);
                break;
            case EDirection.LEFT:
                result = new Vector3(-1, 0, 0);
                break;
            case EDirection.RIGHT:
                result = new Vector3(1, 0, 0);
                break;
        }
        return result;
    }
    #endregion

    #region Collision
    private void OnCollisionWithDot(EColor color)
    {
        _isRightColor = color == _config.color;
        Debug.Log($"Right color: {_isRightColor}");
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

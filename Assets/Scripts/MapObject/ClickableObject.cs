using System;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider2D;
    private Camera _mainCamera;
    private RaycastHit2D[] _raycastResults = new RaycastHit2D[2];
    private int _raycastResultCount = 0;
    private Vector2 _inputWorldPosition;
    private Action _onCompleted;

    public Camera MainCamera
    {
        get
        {
            if (!_mainCamera) _mainCamera = Camera.main;
            return _mainCamera;
        }
    }

    public Action OnCompleted { get => _onCompleted; set => _onCompleted = value; }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            RaycastChoosable(Input.mousePosition);
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                RaycastChoosable(touch.position);
            }
        }
#endif
    }

    private void RaycastChoosable(Vector2 touchPosition)
    {
        _inputWorldPosition = MainCamera.ScreenToWorldPoint(touchPosition);
        _raycastResultCount = Physics2D.RaycastNonAlloc(_inputWorldPosition, Vector2.up, _raycastResults, 0.1f);
        if (_raycastResultCount > 0)
        {
            foreach (RaycastHit2D result in _raycastResults)
            {
                if (result.transform.gameObject == this.gameObject)
                {
                    OnClickOnCharacter(_inputWorldPosition);
                    return;
                }
            }
        }
    }

    private void OnClickOnCharacter(Vector2 touchingPosition)
    {
        OnCompleted?.Invoke();
    }

    public void FitCollider(SpriteRenderer spriteRenderer)
    {
        Vector2 size = spriteRenderer.size;
        _boxCollider2D.size = size;
        _boxCollider2D.offset = Vector2.zero;
    }
}

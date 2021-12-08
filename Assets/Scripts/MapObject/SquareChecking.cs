using System;
using System.Collections.Generic;
using UnityEngine;

public class SquareChecking : MonoBehaviour
{
    [SerializeField] private LayerMask _collectibleLayer;
    private RaycastHit2D[] _raycastResults = new RaycastHit2D[2];
    private int _raycastResultCount = 0;

    public void Checking(Action<EColor> onCollisionWithDot, Action<EDirection> onCollisionWithDirection)
    {
        _raycastResultCount = Physics2D.RaycastNonAlloc(this.transform.position, Vector2.up, _raycastResults, 0.001f, _collectibleLayer);
        if (_raycastResultCount > 0)
        {
            foreach (RaycastHit2D result in _raycastResults)
            {
                if(result.transform != null && result.transform.gameObject != this.gameObject)
                {
                    DotController dotController = result.transform.GetComponent<DotController>();
                    if (dotController != null)
                    {
                        onCollisionWithDot?.Invoke(dotController.Config.color);
                        return;
                    }
                    DirectionController directionController = result.transform.GetComponent<DirectionController>();
                    if (directionController != null)
                    {
                        onCollisionWithDirection?.Invoke(directionController.Config.direction);
                        return;
                    }
                }
            }
        }
    }
}

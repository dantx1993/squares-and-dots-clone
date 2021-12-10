using System;
using System.Collections.Generic;
using UnityEngine;

public class SquareChecking : MonoBehaviour
{
    [SerializeField] private LayerMask _collectibleLayer;
    [SerializeField] private LayerMask _squareLayer;
    private RaycastHit2D[] _raycastResults = new RaycastHit2D[5];
    private int _raycastResultCount = 0;
    private RaycastHit2D[] _raycastSquareResults = new RaycastHit2D[5];
    private int _raycastSquareResultCount = 0;

    public void Checking(Action<EColor> onCollisionWithDot, Action<EDirection> onCollisionWithDirection)
    {
        _raycastResults = Physics2D.RaycastAll(this.transform.position, Vector2.up, 0.001f, _collectibleLayer);
        if (_raycastResults.Length > 0)
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
    public void CheckingPushSquare(EDirection direction)
    {
        _raycastSquareResults = Physics2D.RaycastAll(this.transform.position, MapManager.Instance.GetMoveDirection(direction), GameConfig.CREATED_CELL_SIZE * 0.6f, _squareLayer);
        if (_raycastSquareResults.Length > 0)
        {
            foreach (RaycastHit2D result in _raycastSquareResults)
            {
                if(result.transform != null)
                if (result.transform != null && result.transform.gameObject != this.gameObject)
                {
                    SquareController squareController = result.transform.GetComponent<SquareController>();
                    if (squareController != null)
                    {
                        squareController.Moving(direction);
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class CurrentChoose : MonoBehaviour
    {
        [SerializeField] private CurrentSquare _currentSquare;
        [SerializeField] private CurrentDot _currentDot;
        [SerializeField] private CurrentDirection _currentDirection;

        private ICurrentChoose _currentChoosing;

        public void ChangeData(MapObject data)
        {
            _currentSquare.gameObject.SetActive(data.type == EMapObject.SQUARE);
            _currentDot.gameObject.SetActive(data.type == EMapObject.DOT);
            _currentDirection.gameObject.SetActive(data.type == EMapObject.DIRECTION);
            switch (data.type)
            {
                case EMapObject.SQUARE:
                    _currentChoosing = _currentSquare;
                    break;
                case EMapObject.DOT:
                    _currentChoosing = _currentDot;
                    break;
                case EMapObject.DIRECTION:
                    _currentChoosing = _currentDirection;
                    break;
            }
            _currentChoosing.ChangeData(data);
        }
        public void HideAllCurrentChoose()        
        {
            _currentSquare.gameObject.SetActive(false);
            _currentDot.gameObject.SetActive(false);
            _currentDirection.gameObject.SetActive(false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour
{
    [SerializeField] private DirectionConfig _config;
    [SerializeField] private GameObject _arrow;
    public DirectionConfig Config => _config;


    private void Awake() 
    {
        ChangeDirectionRenderer(_arrow.transform, _config.direction);
    }

    private void ChangeDirectionRenderer(Transform arrow, EDirection direction)
    {
        Vector3 eulerAngles = Vector3.zero;
        switch (direction)
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
        arrow.localEulerAngles = eulerAngles;
    }
}

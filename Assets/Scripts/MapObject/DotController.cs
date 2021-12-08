using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotController : MonoBehaviour
{
    [SerializeField] private DotConfig _config;
    [SerializeField] private SpriteRenderer _dotRenderer;
    public DotConfig Config => _config;
}

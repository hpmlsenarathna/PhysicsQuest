using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingshotArea : MonoBehaviour
{
    [SerializeField] private LayerMask SlingAreaMask;
    public bool isWithinSlingshotArea()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(InputManager.mousePos);
        if (Physics2D.OverlapPoint(worldPosition, SlingAreaMask)){
            return true;
        }
        else { return false; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconHandler : MonoBehaviour
{
    [SerializeField] private Image[] Icons;
    [SerializeField] private Color usedColor;

    public void UseShot(int shotNumber)
    {
        if (shotNumber < Icons.Length)
        {
            Icons[shotNumber].color = usedColor;
        }
    }
}

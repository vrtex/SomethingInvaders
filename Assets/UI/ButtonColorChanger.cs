using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    public List<Button> buttons;
    public Color activeColor;
    public Color inactiveColor;

    private void Start()
    {
        SetButton(0);
    }

    public void ClearButtons()
    {
        foreach(var b in buttons)
        {
            ColorBlock colors = b.colors;
            colors.normalColor = inactiveColor;
            b.colors = colors;
        }
    }

    public void SetButton(int index)
    {
        ClearButtons();

        ColorBlock colors = buttons[index].colors;
        colors.normalColor = activeColor;
        buttons[index].colors = colors;
    }
}

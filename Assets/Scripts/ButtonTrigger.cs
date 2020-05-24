using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class ButtonTrigger : MonoBehaviour
{
    public event Action<Color> OnColorChoose;

    public void TriggerColorChooseEvent()
    {
        OnColorChoose.Invoke(GetComponent<Image>().color);
    }
}

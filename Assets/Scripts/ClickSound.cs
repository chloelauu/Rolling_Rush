using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickSound : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData e)
    {
        AudioManager.Play(AudioManager.Sfx.Click);
    }
}
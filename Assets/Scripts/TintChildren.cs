using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TintChildren : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Graphic[] graphics;   // the extra parts to tint (bars, text, etc.)
    public Color normal  = new Color32(0xEA, 0xF2, 0xFF, 255);
    public Color hover   = new Color32(0xFF, 0xC9, 0x4D, 255);
    public Color pressed = new Color32(0xD9, 0xA9, 0x3A, 255);

    bool over;

    void Set(Color c)
    {
        foreach (Graphic g in graphics) g.color = c;
    }

    public void OnPointerEnter(PointerEventData e) { over = true;  Set(hover); }
    public void OnPointerExit(PointerEventData e)  { over = false; Set(normal); }
    public void OnPointerDown(PointerEventData e)  { Set(pressed); }
    public void OnPointerUp(PointerEventData e)    { Set(over ? hover : normal); }

    // reset when the button gets hidden (e.g. on pause), so it doesn't stay yellow
    void OnDisable() { over = false; Set(normal); }
}

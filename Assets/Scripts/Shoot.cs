using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shoot : MonoBehaviour, IPointerDownHandler
{
    public event Action OnShoot;
    public void OnPointerDown(PointerEventData eventData)
    {
        OnShoot?.Invoke();
    }
}

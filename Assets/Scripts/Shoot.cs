using System;
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

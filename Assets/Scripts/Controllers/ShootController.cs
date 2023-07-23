using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootController : MonoBehaviour, IPointerDownHandler
{
    public event Action OnShoot;
    public void OnPointerDown(PointerEventData eventData)
    {
        OnShoot?.Invoke();
    }
}

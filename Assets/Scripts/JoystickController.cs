using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector2 startingPosition;
    private int joystickLimit = 100;
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.position.x < startingPosition.x + joystickLimit && eventData.position.x > startingPosition.x - joystickLimit
            && eventData.position.y < startingPosition.y + joystickLimit && eventData.position.y > startingPosition.y - joystickLimit)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startingPosition;
    }

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickController2 : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image bgImage;
    [SerializeField] Image joystickImage;
    [SerializeField] float offset;

    private Vector2 inputDirection = Vector2.right;
    public Vector2 InputDirection => inputDirection;
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = Vector2.zero;
        float bgImageSizeX = bgImage.rectTransform.sizeDelta.x;
        float bgImageSizeY = bgImage.rectTransform.sizeDelta.y;

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x /= bgImageSizeX; 
            pos.y /= bgImageSizeY;
            inputDirection = new Vector2(pos.x, pos.y);
            inputDirection = inputDirection.magnitude > 1 ? inputDirection.normalized : inputDirection;

            joystickImage.rectTransform.anchoredPosition = new Vector2(inputDirection.x*(bgImageSizeX/offset), inputDirection.y*(bgImageSizeY/offset));
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
         inputDirection = Vector2.zero;
        joystickImage.rectTransform.anchoredPosition = Vector2.zero;
    }

    void Start()
    {
        inputDirection = Vector2.zero;
    }

    void Update()
    {
        
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Controllers
{
    public class JoystickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image bgImage;
        [SerializeField] private Image joystickImage;
        [SerializeField] private float offset;

        private Vector2 _inputDirection = Vector2.right;
        public Vector2 InputDirection => _inputDirection;

        private void Start()
        {
            _inputDirection = Vector2.zero;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pos = Vector2.zero;
            var sizeDelta = bgImage.rectTransform.sizeDelta;
            float bgImageSizeX = sizeDelta.x;
            float bgImageSizeY = sizeDelta.y;

            if(RectTransformUtility.ScreenPointToLocalPointInRectangle
                   (bgImage.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
            {
                pos.x /= bgImageSizeX; 
                pos.y /= bgImageSizeY;
                _inputDirection = new Vector2(pos.x, pos.y);
                _inputDirection = _inputDirection.magnitude > 1 ? _inputDirection.normalized : _inputDirection;
                joystickImage.rectTransform.anchoredPosition 
                    = new Vector2(_inputDirection.x*(bgImageSizeX/offset), _inputDirection.y*(bgImageSizeY/offset));
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _inputDirection = Vector2.zero;
            joystickImage.rectTransform.anchoredPosition = Vector2.zero;
        }    
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Vector3 healthBarOffset;
    //[SerializeField] private PlayerController playerController;
    [SerializeField] private RectTransform rectTransform;

    private int healthAmount = 500;
    private Transform healthTarget;
    private Transform healthTransform;

    public void Initialize(Transform playerTransform)
    {
        healthTarget = playerTransform;
        healthTransform = transform;
    }

    public void SetHealth()
    {
        healthBar.fillAmount -= 0.02f;
        healthAmount -= 10;
        healthText.text = $"{healthAmount}";
    }

    private void Update()
    {
        if(healthTarget != null)
        {

            healthTransform.position = healthTarget.position;
        }
    }

    /*private void LateUpdate()
    {


        float angle = Vector2.Angle(Vector2.right, playerController.PlayerDirection);
        rectTransform.localRotation = new Quaternion(0, 0, -angle, 0);


        if (playerController.PlayerDirection.y < 0)
        {
            Vector3 tempVector3 = rectTransform.localEulerAngles;
            tempVector3.z = angle;
            rectTransform.localEulerAngles = tempVector3;
        }

        else
        {
            Vector3 tempVector3 = rectTransform.localEulerAngles;
            tempVector3.z = -angle;
            rectTransform.localEulerAngles = tempVector3;
        }
    }*/


}

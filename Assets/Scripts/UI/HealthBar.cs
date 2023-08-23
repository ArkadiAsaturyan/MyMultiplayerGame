using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour, IPunObservable
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    private float startingHealth = 500f;
    private float currentHealth = 500f;
    private float previousHealth = 500f;

    private Transform healthTarget;
    private Transform healthTransform;

    private void Awake()
    {
        PhotonNetwork.SendRate = 10;
        PhotonNetwork.SerializationRate = 10;
    }

    public void Initialize(Transform playerTransform)
    {
        healthTarget = playerTransform;
        healthTransform = transform;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
            SetHealthForOpponent();
        }
    }

    private void SetHealthForOpponent()
    {
        float differenceInPercent = (previousHealth - currentHealth) / startingHealth;
        healthBar.fillAmount -= differenceInPercent;
        healthText.text = $"{currentHealth}";
        previousHealth = currentHealth;
        if (currentHealth <= 50)
        {
            healthBar.color = Color.red;
        }
    }

    public void SetHealth()
    {
        healthBar.fillAmount -= 0.02f;
        currentHealth -= 10;
        healthText.text = $"{currentHealth}";
        if (currentHealth <= 50)
        {
            healthBar.color = Color.red;
        }
    }

    private void Update()
    {
        if(healthTarget != null)
        {
            healthTransform.position = healthTarget.position;
        }
    }
}

using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour, IPunObservable
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private TextMeshProUGUI healthText;

        private const float StartingHealth = 500f;
        private float _currentHealth = 500f;
        private float _previousHealth = 500f;

        private Transform _healthTarget;
        private Transform _healthTransform;

        private void Awake()
        {
            PhotonNetwork.SendRate = 10;
            PhotonNetwork.SerializationRate = 10;
        }

        public void Initialize(Transform playerTransform)
        {
            _healthTarget = playerTransform;
            _healthTransform = transform;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_currentHealth);
            }
            else
            {
                _currentHealth = (float)stream.ReceiveNext();
                SetHealthForOpponent();
            }
        }

        private void SetHealthForOpponent()
        {
            float differenceInPercent = (_previousHealth - _currentHealth) / StartingHealth;
            healthBar.fillAmount -= differenceInPercent;
            healthText.text = $"{_currentHealth}";
            _previousHealth = _currentHealth;
            if (_currentHealth <= 50)
            {
                healthBar.color = Color.red;
            }
        }

        public void SetHealth()
        {
            healthBar.fillAmount -= 0.02f;
            _currentHealth -= 10;
            healthText.text = $"{_currentHealth}";
            if (_currentHealth <= 50)
            {
                healthBar.color = Color.red;
            }
        }

        private void Update()
        {
            if(_healthTarget != null)
            {
                _healthTransform.position = _healthTarget.position;
            }
        }
    }
}

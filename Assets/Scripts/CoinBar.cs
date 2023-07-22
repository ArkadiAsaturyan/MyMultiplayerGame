using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinBar : MonoBehaviour
{
    [SerializeField] private Image progress;
    [SerializeField] private TextMeshProUGUI collectedCoinsTexr;
    [SerializeField] private int coinsAmount;

    private float oneCoinProgress;

    private void Start()
    {
        oneCoinProgress = 1 / coinsAmount;
    }

    private int collectedCoins;
    public void CoinCollected(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            collectedCoins++;
            collectedCoinsTexr.text = $"{collectedCoins}";
            progress.fillAmount += 0.1f;
        }
    }
}

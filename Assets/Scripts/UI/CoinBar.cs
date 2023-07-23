using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinBar : MonoBehaviour
{
    [SerializeField] private Image progress;
    [SerializeField] private TextMeshProUGUI collectedCoinsTexr;
    [SerializeField] private float coinsAmount;

    private float oneCoinProgress;

    private void Start()
    {
        oneCoinProgress = 1f / coinsAmount;
    }

    private int collectedCoins;
    public void CoinCollected(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            collectedCoins++;
            collectedCoinsTexr.text = $"{collectedCoins}";
            progress.fillAmount += oneCoinProgress;
        }
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private WinnerPopup winnerPopup;

    private int playersCount;
    private List<PlayerController> players;

    private void Awake()
    {
        Debug.Log("awake in popupManager");

        StartCoroutine(CheckPhotonViews());
    }

    private void PlayerDestroyed(PlayerController player)
    {
        players.Remove(player);
        playersCount--;
        if(players.Count == 1)
        {
            winnerPopup.gameObject.SetActive(true);
            winnerPopup.Setup(players[0].Name, players[0].CollectedCoins);
            Debug.Log("Winner points: " + players[0].CollectedCoins);
            Debug.Log("player.Name: " + players[0].Name);
            
        }
    }
    
    private IEnumerator CheckPhotonViews()
    {
        Debug.Log("CheckPhotonViews");

        while (true)
        {
            yield return new WaitForSeconds(1);

            players = PhotonNetwork.PhotonViews
            .Where(x => x.gameObject.tag == "Player")
            .Select(p => p.GetComponent<PlayerController>()).ToList();

            if (players.Count >= playersCount + 1)
            {
                Debug.Log("players.Count: " + players.Count);

                players[playersCount].OnPlayerDestroyed += PlayerDestroyed;
                playersCount++;
            }
        }
    }
}

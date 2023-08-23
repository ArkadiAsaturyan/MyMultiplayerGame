using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UI;
using UnityEngine;

namespace Controllers
{
    public class PopupController : MonoBehaviour
    {
        [SerializeField] private WinnerPopup winnerPopup;

        private int _playersCount;
        private List<PlayerController> _players;

        private void Awake()
        {
            StartCoroutine(CheckPhotonViews());
        }

        private void PlayerDestroyed(PlayerController player)
        {
            _players.Remove(player);
            _playersCount--;
            if(_players.Count == 1)
            {
                StartCoroutine(ShowWinnerPopup());
            }
        }

        private IEnumerator ShowWinnerPopup()
        {
            yield return new WaitForSeconds(1);
            winnerPopup.gameObject.SetActive(true);
            winnerPopup.Setup(_players[0].Name, _players[0].CollectedCoins);
        }
    
        private IEnumerator CheckPhotonViews()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                _players = PhotonNetwork.PhotonViews
                    .Where(x => x.gameObject.CompareTag("Player"))
                    .Select(p => p.GetComponent<PlayerController>()).ToList();

                if (_players.Count >= _playersCount + 1)
                {
                    _players[_playersCount].OnPlayerDestroyed += PlayerDestroyed;
                    _playersCount++;
                }
            }
        }
    }
}

using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig", order = 100)]

    public class PlayerConfig : ScriptableObject 
    {
        [SerializeField] private int playerIndex;

        public int PlayerIndex => playerIndex;

        public void DecrementPlayerIndex()
        {
            playerIndex--;
        }

        public void IncrementPlayerIndex()
        {
            playerIndex++;
        }
    }
}

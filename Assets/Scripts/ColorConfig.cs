using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(fileName = "Color", menuName = "Configs/Color", order = 100)]
    public class ColorConfig : ScriptableObject 
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

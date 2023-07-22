using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

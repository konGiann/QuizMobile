using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager _instance;

        public PlayerProfile profile;


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = GetComponent<PlayerManager>();
            }

            profile = new PlayerProfile();
        }


    } 
}

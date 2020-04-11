using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager _instance;

        #region inspector fields

        [Header("Background Music")]
        public AudioClip BackgroundMusic;

        [Header("Sound Effects")]
        public AudioClip[] CorrectAnswers;
        public AudioClip[] WrongAnswers;

        #endregion

        #region fields

        [HideInInspector]
        public AudioSource audioController;

        #endregion

        // Start is called before the first frame update
        void Awake()
        {
            if (_instance == null)
            {
                _instance = GetComponent<SoundManager>();
            }

            audioController = gameObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            audioController.clip = BackgroundMusic;
            audioController.Play();
            audioController.loop = true;
        }

    } 
}

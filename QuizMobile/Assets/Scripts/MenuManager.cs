using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager _instance;

        #region inspector fields

        public GameObject MenuPanel;


        #endregion


        #region fields

        [HideInInspector]
        public string selectedCategory;

        public Text religionLevel;
        public Text cultureLevel;
        public Text natureLevel;
        public Text historyLevel;
        public Text covidLevel;
        public Text geographyLevel;
        
        #endregion
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = GetComponent<MenuManager>();
            }
        }

        private void Start()
        {
            religionLevel.text = PlayerPrefmanager.GetCategoryLevel("Religion").ToString();
            cultureLevel.text = PlayerPrefmanager.GetCategoryLevel("Culture").ToString();
            natureLevel.text = PlayerPrefmanager.GetCategoryLevel("Nature").ToString();
            historyLevel.text = PlayerPrefmanager.GetCategoryLevel("History").ToString();
            covidLevel.text = PlayerPrefmanager.GetCategoryLevel("COVID-19").ToString();
            geographyLevel.text = PlayerPrefmanager.GetCategoryLevel("Geography").ToString();
        }

        // check which category was clicked and assign it 
        // then load main game scene
        public void OnButtonClick()
        {
            selectedCategory = EventSystem.current.currentSelectedGameObject.name;
            SceneManager.LoadScene(2);
        }
    }
}

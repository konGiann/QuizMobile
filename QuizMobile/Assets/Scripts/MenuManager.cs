using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
        
        #endregion
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = GetComponent<MenuManager>();
            }
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

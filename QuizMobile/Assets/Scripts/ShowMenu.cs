using UnityEngine;

public class ShowMenu : MonoBehaviour
{
    public Animator menuAnimator;

    private bool isMenuActive;

    void Start()
    {        
        isMenuActive = menuAnimator.GetBool("isActive");
    }
   
    public void OnMenuClick()
    {
        isMenuActive = !isMenuActive;
        menuAnimator.SetBool("isActive", isMenuActive);        
    }
}

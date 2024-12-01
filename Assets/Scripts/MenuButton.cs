using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuButton : MonoBehaviour
{
    [SerializeField] GameObject background;
    [SerializeField] GameObject buttonClose;
    public void Begin()
    {
        SceneManager.LoadScene("Thomas");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Settings()
    {
        background.SetActive(true);
        buttonClose.SetActive(true);
    }

    public void CloseSettings()
    {
    background.SetActive(false); 
    buttonClose.SetActive(false);
    }

    public void CloseSettingsOnEscape(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            CloseSettings();
        }
    }
    
}

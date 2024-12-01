using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuButton : MonoBehaviour
{
    [SerializeField] GameObject main;
    [SerializeField] GameObject settings;
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
        main.SetActive(false);
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
    main.SetActive(true); 
    settings.SetActive(false);
    }

    public void CloseSettingsOnEscape(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            CloseSettings();
        }
    }
    
}

using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject returnPanel;

    [Header("Panels")]
    [SerializeField] private List<GameObject> panels = new();

    private readonly Stack<GameObject> currentPanels = new();

    // Start is called before the first frame update
    void Start()
    {
        TogglePanel(startPanel);
    }

    public void TogglePanel(GameObject panelToToggle)
    {
        currentPanels.Push(panelToToggle);

        foreach (GameObject panel in panels)
        {
            panel.SetActive(panel == panelToToggle);
        }

        if (panelToToggle == returnPanel)
        {
            Time.timeScale = 0f;
        }
    }

    public void ReturnToPreviousPanel()
    {
        if (currentPanels.Count <= 1) { return; }

        currentPanels.Pop();

        foreach (GameObject panel in panels)
        {
            panel.SetActive(panel == currentPanels.Peek());
        }
        Time.timeScale = 1f;
    }

    public void ReturnToPreviousPanel(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (currentPanels.Count > 1)
            {
                ReturnToPreviousPanel();
            }
            else
            {
                Time.timeScale = 0f;
                TogglePanel(returnPanel);
            }
        }
    }

    public void LoadTitleScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Thibault");
    }

    public void Quit()
    {
        Application.Quit();
    }
}

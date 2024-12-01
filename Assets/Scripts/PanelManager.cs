using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

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
    }

    public void ReturnToPreviousPanel()
    {
        if (currentPanels.Count <= 1) { return; }

        currentPanels.Pop();

        foreach (GameObject panel in panels)
        {
            panel.SetActive(panel == currentPanels.Peek());
        }
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
                TogglePanel(returnPanel);
            }
        }
    }
}

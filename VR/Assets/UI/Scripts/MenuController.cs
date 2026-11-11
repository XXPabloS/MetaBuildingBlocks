using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Welcome UI")]
    [SerializeField] private GameObject welcomePanel;

    [Header("Inventory UI")]
    [SerializeField] private GameObject inventoryPanel;

    [Header("Settings UI")]
    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        welcomePanel.SetActive(true);
        CloseAllMenus();
    }

    public void StartExperience()
    {
        welcomePanel.SetActive(false);
    }

    public void DisplayMenu()
    {
        bool isOpen = inventoryPanel.activeSelf || settingsPanel.activeSelf;

        if (isOpen)
        {
            CloseAllMenus();
        }
        else
        {
            SetMenu(inventoryPanel);
        }
    }

    public void InventoryButton()
    {
        if (inventoryPanel.activeSelf) return;

        SetMenu(inventoryPanel);
    }

    public void SettingsButton()
    {
        if (settingsPanel.activeSelf) return;

        SetMenu(settingsPanel);
    }

    public void EyeButton()
    {
        CloseAllMenus();
    }

    private void SetMenu(GameObject panelToOpen)
    {
        inventoryPanel.SetActive(false);
        settingsPanel.SetActive(false);

        panelToOpen.SetActive(true);
    }

    private void CloseAllMenus()
    {
        inventoryPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
}
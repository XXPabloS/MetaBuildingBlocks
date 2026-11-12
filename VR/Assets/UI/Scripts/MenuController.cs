
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Main Menu")]
    public GameObject displayMenuButton;

    [Header("Panels")]
    public GameObject inventoryPanel;
    public GameObject settingsPanel;
    public GameObject welcomePanel;

    [Header("Menu Buttons")]
    public GameObject inventoryButton;
    public GameObject eyeButton;

    [Header("Button Opacity")]
    public Image inventoryButtonImage;

    [Range(0f, 1f)]
    public float activeOpacity = 1f;

    [Range(0f, 1f)]
    public float inactiveOpacity = 0.35f;

    private bool menuIsOpen = false;

    void Start()
    {
        CloseMenu();
    }

    public void ToggleMenu()
    {
        if (menuIsOpen)
            CloseMenu();
        else
            OpenMenu();
    }

    public void OpenMenu()
    {
        menuIsOpen = true;

        inventoryPanel.SetActive(true);
        settingsPanel.SetActive(false);

        SetMenuButtonsVisible(true);
        UpdateButtonOpacity();
    }

    public void CloseMenu()
    {
        menuIsOpen = false;

        inventoryPanel.SetActive(false);
        settingsPanel.SetActive(false);

        SetMenuButtonsVisible(false);
    }

    private void SetMenuButtonsVisible(bool visible)
    {
        inventoryButton.SetActive(visible);
        eyeButton.SetActive(visible);
    }

    public void ShowInventory()
    {
        if (!menuIsOpen || inventoryPanel.activeSelf)
            return;

        inventoryPanel.SetActive(true);
        settingsPanel.SetActive(false);

        UpdateButtonOpacity();
    }

    public void ShowSettings()
    {
        if (!menuIsOpen || settingsPanel.activeSelf)
            return;

        settingsPanel.SetActive(true);
        inventoryPanel.SetActive(false);

        UpdateButtonOpacity();
    }

    public void HidePanels()
    {
        CloseMenu();
    }

    private void UpdateButtonOpacity()
    {
        Color inventoryColor = inventoryButtonImage.color;

        inventoryColor.a = inventoryPanel.activeSelf
            ? activeOpacity
            : inactiveOpacity;

        inventoryButtonImage.color = inventoryColor;
    }

    public void ShowWelcome()
    {
        welcomePanel.SetActive(true);
    }

    public void HideWelcome()
    {
        welcomePanel.SetActive(false);
    }
}
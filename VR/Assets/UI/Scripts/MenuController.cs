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

    [Header("Additional Inventory Elements")]
    public GameObject[] additionalInventoryElements;

    [Header("Menu Buttons")]
    public GameObject inventoryButton;
    public GameObject eyeButton;
    public GameObject settingsButton;

    [Header("Button Opacity")]
    public Image inventoryButtonImage;
    public Image settingsButtonImage;

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

        if (inventoryPanel != null)
            inventoryPanel.SetActive(true);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        SetAdditionalInventoryElements(true);
        SetMenuButtonsVisible(true);
        UpdateButtonOpacity();
    }

    public void CloseMenu()
    {
        menuIsOpen = false;

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        SetAdditionalInventoryElements(false);
        SetMenuButtonsVisible(false);
        UpdateButtonOpacity();
    }

    public void OnPrefabSelected()
    {
        CloseMenu();
    }

    private void SetMenuButtonsVisible(bool visible)
    {
        if (inventoryButton != null)
            inventoryButton.SetActive(visible);

        if (eyeButton != null)
            eyeButton.SetActive(visible);

        if (settingsButton != null)
            settingsButton.SetActive(visible);
    }

    private void SetAdditionalInventoryElements(bool visible)
    {
        if (additionalInventoryElements == null)
            return;

        foreach (GameObject element in additionalInventoryElements)
        {
            if (element != null)
                element.SetActive(visible);
        }
    }

    public void ShowInventory()
    {
        if (!menuIsOpen)
            return;

        if (inventoryPanel != null)
            inventoryPanel.SetActive(true);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        SetAdditionalInventoryElements(true);
        UpdateButtonOpacity();
    }

    public void ShowSettings()
    {
        if (!menuIsOpen)
            return;

        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        SetAdditionalInventoryElements(false);
        UpdateButtonOpacity();
    }

    public void HidePanels()
    {
        CloseMenu();
    }

    private void UpdateButtonOpacity()
    {
        if (inventoryButtonImage != null)
        {
            Color color = inventoryButtonImage.color;
            color.a = inventoryPanel != null && inventoryPanel.activeSelf ? activeOpacity : inactiveOpacity;
            inventoryButtonImage.color = color;
        }

        if (settingsButtonImage != null)
        {
            Color color = settingsButtonImage.color;
            color.a = settingsPanel != null && settingsPanel.activeSelf ? activeOpacity : inactiveOpacity;
            settingsButtonImage.color = color;
        }
    }

    public void ShowWelcome()
    {
        if (welcomePanel != null)
            welcomePanel.SetActive(true);
    }

    public void HideWelcome()
    {
        if (welcomePanel != null)
            welcomePanel.SetActive(false);
    }
}
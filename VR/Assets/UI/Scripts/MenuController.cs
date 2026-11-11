
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Welcome UI")]
    [SerializeField] private GameObject welcomePanel;

    [Header("Inventory UI")]
    [SerializeField] private GameObject inventoryPanel;

    private void Start()
    {
        welcomePanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }
    public void StartExperience()
    {
        welcomePanel.SetActive(false);
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }
}
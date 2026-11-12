using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCatalog : MonoBehaviour
{
    public FurniturePlacer placer;
    public Button buttonPrefab;

    [Header("Furniture Previews")]
    public Sprite[] furniturePreviews;

    [Header("Pagination")]
    public Transform[] containers;
    public GameObject[] inventoryPages;
    public Button previousPageButton;
    public Button nextPageButton;
    public Image previousPageButtonImage;
    public Image nextPageButtonImage;
    public TMP_Text pageNumberText;

    [Range(0f, 1f)]
    public float activeOpacity = 1f;

    [Range(0f, 1f)]
    public float inactiveOpacity = 0.35f;

    public int buttonsPerPage = 12;

    private int currentPage = 0;
    private int totalPages = 0;

    void Start()
    {
        GenerateInventory();
        ShowPage(0);
    }

    void GenerateInventory()
    {
        if (placer == null || buttonPrefab == null || containers == null || containers.Length == 0 || buttonsPerPage <= 0)
        {
            Debug.LogError("InventoryCatalog: Missing references or invalid settings.");
            return;
        }

        if (placer.furniturePrefabs == null)
        {
            Debug.LogError("InventoryCatalog: Furniture prefabs are missing.");
            return;
        }

        int totalFurniture = placer.furniturePrefabs.Length;
        totalPages = Mathf.CeilToInt((float)totalFurniture / buttonsPerPage);

        foreach (Transform container in containers)
        {
            if (container == null) continue;
            foreach (Transform child in container)
                Destroy(child.gameObject);
        }

        for (int i = 0; i < totalFurniture; i++)
        {
            int index = i;
            int pageIndex = i / buttonsPerPage;

            if (pageIndex >= containers.Length)
            {
                Debug.LogWarning("Not enough containers for all furniture.");
                break;
            }

            Transform container = containers[pageIndex];
            if (container == null) continue;

            Button b = Instantiate(buttonPrefab, container, false);
            b.name = "Btn_" + placer.furniturePrefabs[i].name;

            TMP_Text label = b.GetComponentInChildren<TMP_Text>();
            if (label != null)
                label.text = placer.furniturePrefabs[i].name;

            Transform previewTransform = b.transform.Find("PreviewImage");

            if (previewTransform != null)
            {
                Image previewImage = previewTransform.GetComponent<Image>();
                if (previewImage != null && furniturePreviews != null && index < furniturePreviews.Length)
                {
                    previewImage.sprite = furniturePreviews[index];
                    previewImage.preserveAspect = true;
                }
            }

            b.onClick.AddListener(() => placer.SelectPrefabFromUI(index));
        }
    }

    public void NextPage()
    {
        if (currentPage < Mathf.Min(totalPages, inventoryPages.Length) - 1)
            ShowPage(currentPage + 1);
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
            ShowPage(currentPage - 1);
    }

    public void ShowPage(int page)
    {
        if (inventoryPages == null || inventoryPages.Length == 0)
            return;

        int visiblePages = Mathf.Min(totalPages, inventoryPages.Length);

        if (visiblePages == 0)
        {
            currentPage = 0;
            foreach (GameObject inventoryPage in inventoryPages)
                if (inventoryPage != null)
                    inventoryPage.SetActive(false);

            UpdateNavigation();
            return;
        }

        currentPage = Mathf.Clamp(page, 0, visiblePages - 1);

        for (int i = 0; i < inventoryPages.Length; i++)
            if (inventoryPages[i] != null)
                inventoryPages[i].SetActive(i == currentPage);

        UpdateNavigation();
    }

    void UpdateNavigation()
    {
        int visiblePages = Mathf.Min(totalPages, inventoryPages.Length);
        bool canGoPrevious = currentPage > 0;
        bool canGoNext = currentPage < visiblePages - 1;

        if (previousPageButton != null)
            previousPageButton.interactable = canGoPrevious;

        if (nextPageButton != null)
            nextPageButton.interactable = canGoNext;

        if (previousPageButtonImage != null)
        {
            Color color = previousPageButtonImage.color;
            color.a = canGoPrevious ? activeOpacity : inactiveOpacity;
            previousPageButtonImage.color = color;
        }

        if (nextPageButtonImage != null)
        {
            Color color = nextPageButtonImage.color;
            color.a = canGoNext ? activeOpacity : inactiveOpacity;
            nextPageButtonImage.color = color;
        }

        if (pageNumberText != null)
            pageNumberText.text = visiblePages > 0 ? (currentPage + 1) + " / " + visiblePages : "0 / 0";
    }
}
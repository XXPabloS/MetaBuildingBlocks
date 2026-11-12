using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCatalog : MonoBehaviour
{
    public FurniturePlacer placer;
    public Button buttonPrefab;
    public Transform container;
    public int pageSize = 12;

    [Header("Furniture Previews")]
    public Sprite[] furniturePreviews;

    private int currentPage = 0;

    int PageCount => Mathf.Max(1, Mathf.CeilToInt(placer.furniturePrefabs.Length / (float)pageSize));

    void Start()
    {
        ShowPage(0);
    }

    void ShowPage(int page)
    {
        currentPage = page;
        ClearButtons();

        int start = page * pageSize;
        int end = Mathf.Min(start + pageSize, placer.furniturePrefabs.Length);

        for (int i = start; i < end; i++)
            SpawnButton(i);
    }

    void ClearButtons()
    {
        foreach (Transform child in container)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }
    }

    void SpawnButton(int index)
    {
        Button b = Instantiate(buttonPrefab, container, false);
        b.name = "Btn_" + placer.furniturePrefabs[index].name;

        var label = b.GetComponentInChildren<TMP_Text>();
        if (label != null)
            label.text = placer.furniturePrefabs[index].name;

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

    public void NextPage()
    {
        ShowPage((currentPage + 1) % PageCount);
    }

    public void PreviousPage()
    {
        ShowPage((currentPage - 1 + PageCount) % PageCount);
    }
}
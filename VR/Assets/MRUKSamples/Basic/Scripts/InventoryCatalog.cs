
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCatalog : MonoBehaviour
{
    public FurniturePlacer placer;
    public Button buttonPrefab;
    public Transform container;

    [Header("Furniture Previews")]
    public Sprite[] furniturePreviews;

    void Start()
    {
        for (int i = 0; i < placer.furniturePrefabs.Length; i++)
        {
            int index = i;

            Button b = Instantiate(buttonPrefab, container, false);
            var trig = b.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
            var entry = new UnityEngine.EventSystems.EventTrigger.Entry
            {
                eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown
            };

            entry.callback.AddListener(_ =>
            {
                placer.SelectPrefabFromUI(index);
            });

            trig.triggers.Add(entry);
            b.name = "Btn_" + placer.furniturePrefabs[i].name;

            var label = b.GetComponentInChildren<TMP_Text>();
            if (label != null)
                label.text = placer.furniturePrefabs[i].name;

            // Assign the corresponding furniture preview image.
            Transform previewTransform = b.transform.Find("PreviewImage");

            if (previewTransform != null)
            {
                Image previewImage = previewTransform.GetComponent<Image>();

                if (previewImage != null &&
                    furniturePreviews != null &&
                    index < furniturePreviews.Length)
                {
                    previewImage.sprite = furniturePreviews[index];
                    previewImage.preserveAspect = true;
                }
            }

            b.onClick.AddListener(() =>
            {
                placer.SelectPrefabFromUI(index);
            });
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCatalog : MonoBehaviour
{
    public FurniturePlacer placer;   // el objeto que tiene FurniturePlacer
    public Button buttonPrefab;      // el boton
    public Transform container;      // el contenido de InventoryPanel 

    void Start()
    {
        for (int i = 0; i < placer.furniturePrefabs.Length; i++)
        {
            int index = i;

            Button b = Instantiate(buttonPrefab, container, false);
            var trig = b.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
            var entry = new UnityEngine.EventSystems.EventTrigger.Entry { eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown };
            entry.callback.AddListener(_ =>
            {
                placer.SelectPrefabFromUI(index);
            });
            trig.triggers.Add(entry);
            b.name = "Btn_" + placer.furniturePrefabs[i].name;

            var label = b.GetComponentInChildren<TMP_Text>();
            if (label != null) label.text = placer.furniturePrefabs[i].name;

            b.onClick.AddListener(() =>
            {
                placer.SelectPrefabFromUI(index);
            });
        }
    }
}
using UnityEngine;
using Meta.XR.MRUtilityKit;
using Meta.XR.MRUtilityKitSamples.HandInput;

public class FurniturePlacer : MonoBehaviour
{
    public GameObject[] furniturePrefabs;
    public Transform rayOrigin;
    public LayerMask furnitureMask;   // capa "Furniture"

    private int currentIndex = 0;
    private GameObject selected;

    void Update()
    {
        var room = MRUK.Instance ? MRUK.Instance.GetCurrentRoom() : null;
        if (room == null) return;

        HandlePrefabSwitch();

        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        bool click = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)|| HandInputManager.Instance.ThumbTapDown;

        if (selected != null)
        {
            if (room.Raycast(ray, 10f, out RaycastHit moveHit, out MRUKAnchor a))
                selected.transform.position = moveHit.point;

            if (click) selected = null; 
            return;                      
        }

        if (!click) return;

        if (Physics.Raycast(ray, out RaycastHit furnHit, 10f, furnitureMask))
        {
            selected = furnHit.collider.transform.root.gameObject;
            return;
        }

        if (room.Raycast(ray, 10f, out RaycastHit hit, out MRUKAnchor anchor))
        {
            Instantiate(furniturePrefabs[currentIndex], hit.point, Quaternion.identity);
        }
    }
    void HandlePrefabSwitch()
    {
        if (selected != null) return;
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Touch))
            SetPrefab((currentIndex + 1) % furniturePrefabs.Length);
    }

    void SetPrefab(int index)
    {
        if (index < 0 || index >= furniturePrefabs.Length) return;
        currentIndex = index;
        Debug.Log($"Mueble seleccionado: {furniturePrefabs[index].name}");
    }
}

using UnityEngine;
using Meta.XR.MRUtilityKit;
using Meta.XR.MRUtilityKitSamples.HandInput;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class FurniturePlacer : MonoBehaviour
{
    public GameObject[] furniturePrefabs;
    public Transform rayOrigin;
    public LayerMask furnitureMask;
    public OVRRayHelper rayHelper;

    public float rotationSpeed = 90f;
    public float scaleSpeed = 1.0f;
    public float stickDeadzone = 0.2f;

    private Vector3 pickupPosition;
    private int currentIndex = 0;
    private GameObject selected;

    public GameObject inventoryPanel;
    public GameObject settingsPanel;
    public GameObject welcomePanel;

    public GameObject inventoryButton;
    public GameObject settingsButton;
    public GameObject eyeButton;

    private float blockClickUntil;
    private readonly List<RaycastResult> uiHits = new List<RaycastResult>();

    void Update()
    {
        var room = MRUK.Instance ? MRUK.Instance.GetCurrentRoom() : null;
        if (room == null) return;

        HandlePrefabSwitch();

        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        bool hit = TryGetSurface(room, ray, selected, out Vector3 dotPos, out Vector3 dotNormal);
        float dotDist = Vector3.Distance(ray.origin, dotPos);

        bool overUI = TryGetUI(ray, out Vector3 uiPos, out float uiDist);
        if (overUI && (!hit || uiDist < dotDist))
        {
            hit = true;
            dotPos = uiPos;
            dotNormal = -ray.direction;
            dotDist = uiDist;
        }

        if (rayHelper != null)
        {
            rayHelper.UpdatePointerRay(new OVRInputRayData
            {
                IsOverCanvas = hit,
                DistanceToCanvas = dotDist,
                WorldPosition = dotPos,
                WorldNormal = dotNormal,
                IsActive = OVRInput.Get(OVRInput.RawButton.RIndexTrigger),
                ActivationStrength = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger)
            });
        }

        bool click = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) || HandInputManager.Instance.ThumbTapDown;
        if (overUI || IsMenuOpen() || Time.time < blockClickUntil) click = false;

        if (selected != null)
        {
            HandleRotationAndScale();

            if (TryGetSurface(room, ray, selected, out Vector3 point, out Vector3 normal))
                selected.transform.position = ComputePosition(selected, point, normal);

            if (click)
            {
                Physics.SyncTransforms();
                if (Overlaps(selected)) 
                { 
                    selected.transform.position = pickupPosition; 
                    Sounds.Instance?.Play(Sounds.Instance.error); 
                }
                else Sounds.Instance?.Play(Sounds.Instance.place);

                selected = null;
            }

            if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.Touch))
            {
                GameObject temp = selected;
                selected = null;
                Destroy(temp);
                Sounds.Instance?.Play(Sounds.Instance.remove);
            }
            return;
        }

        if (!click) return;

        if (Physics.Raycast(ray, out RaycastHit furnHit, 10f, furnitureMask))
        {
            Select(furnHit.collider.transform.root.gameObject);
            pickupPosition = selected.transform.position;
            Sounds.Instance?.Play(Sounds.Instance.select);
            return;
        }

        if (TryGetSurface(room, ray, null, out Vector3 p, out Vector3 n))
        {
            var go = Instantiate(furniturePrefabs[currentIndex], p, Quaternion.identity);
            SetLayerRecursively(go, LayerMaskToLayer(furnitureMask));
            go.transform.position = ComputePosition(go, p, n);
            Physics.SyncTransforms();

            if (Overlaps(go)) 
            { 
                Destroy(go); 
                Sounds.Instance?.Play(Sounds.Instance.error); 
            }
            else 
            { 
                Select(go); 
                Sounds.Instance?.Play(Sounds.Instance.place); 
            }
        }
    }

    bool TryGetSurface(MRUKRoom room, Ray ray, GameObject ignore, out Vector3 point, out Vector3 normal)
    {
        point = Vector3.zero;
        normal = Vector3.up;
        float best = float.MaxValue;
        bool found = false;

        if (room.Raycast(ray, 10f, out RaycastHit roomHit, out MRUKAnchor a))
        {
            best = roomHit.distance;
            point = roomHit.point;
            normal = roomHit.normal;
            found = true;
        }

        var hits = Physics.RaycastAll(ray, 10f, furnitureMask);
        foreach (var h in hits)
        {
            if (ignore != null && h.transform.IsChildOf(ignore.transform)) continue;
            if (h.distance < best)
            {
                best = h.distance;
                point = h.point;
                normal = h.normal;
                found = true;
            }
        }

        if (found && Vector3.Dot(normal, ray.direction) > 0f) normal = -normal;
        return found;
    }

    bool TryGetUI(Ray ray, out Vector3 point, out float distance)
    {
        point = Vector3.zero;
        distance = 0f;
        var eventSystem = EventSystem.current;
        if (eventSystem == null) return false;

        uiHits.Clear();
        eventSystem.RaycastAll(new OVRPointerEventData(eventSystem) { worldSpaceRay = ray }, uiHits);
        if (uiHits.Count == 0) return false;

        point = uiHits[0].worldPosition;
        distance = uiHits[0].distance;
        return true;
    }

    Vector3 ComputePosition(GameObject obj, Vector3 point, Vector3 normal)
    {
        var box = obj.GetComponentInChildren<BoxCollider>();
        if (box == null) return point;

        Transform t = box.transform;
        Vector3 s = t.lossyScale;
        float hx = Mathf.Abs(box.size.x * s.x) * 0.5f;
        float hy = Mathf.Abs(box.size.y * s.y) * 0.5f;
        float hz = Mathf.Abs(box.size.z * s.z) * 0.5f;

        float d = Mathf.Abs(Vector3.Dot(normal, t.right)) * hx
                + Mathf.Abs(Vector3.Dot(normal, t.up)) * hy
                + Mathf.Abs(Vector3.Dot(normal, t.forward)) * hz;

        Vector3 center = t.TransformPoint(box.center);
        Vector3 pivotToCenter = center - obj.transform.position;
        return point + normal * (d + 0.001f) - pivotToCenter;
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform t in obj.transform)
            SetLayerRecursively(t.gameObject, layer);
    }

    int LayerMaskToLayer(LayerMask mask)
    {
        return Mathf.RoundToInt(Mathf.Log(mask.value, 2));
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

    // Selects the furniture prefab and hides the inventory interface.
    public void SelectPrefabFromUI(int index)
    {
        SetPrefab(index);

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
        if (inventoryButton != null)
            inventoryButton.SetActive(false);
        if (settingsButton != null)
            settingsButton.SetActive(false);
        if (eyeButton != null)
            eyeButton.SetActive(false);

        blockClickUntil = Time.time + 0.25f;
    }

    void Select(GameObject obj)
    {
        selected = obj;
    }

    void HandleRotationAndScale()
    {
        Vector2 stick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        bool gripRight = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        bool gripLeft = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        bool grip = gripRight || gripLeft;

        bool horizontal = Mathf.Abs(stick.x) >= Mathf.Abs(stick.y);
        float value = horizontal ? stick.x : -stick.y;
        if (Mathf.Abs(value) <= stickDeadzone) return;

        if (grip)
        {
            float angle = -value * rotationSpeed * Time.deltaTime;
            Vector3 axis = horizontal ? Vector3.forward : Vector3.right;
            selected.transform.Rotate(axis, angle, Space.World);
        }
        else if (horizontal)
        {
            float angle = -value * rotationSpeed * Time.deltaTime;
            selected.transform.Rotate(Vector3.up, angle, Space.World);
        }
        else
        {
            float factor = 1f + value * scaleSpeed * Time.deltaTime;
            selected.transform.localScale *= factor;
        }
    }

    bool Overlaps(GameObject obj)
    {
        var box = obj.GetComponentInChildren<BoxCollider>();
        if (box == null) return false;

        Vector3 center = box.transform.TransformPoint(box.center);
        Vector3 half = Vector3.Scale(box.size, box.transform.lossyScale) * 0.5f;
        var hits = Physics.OverlapBox(center, half * 0.95f, box.transform.rotation, furnitureMask);

        foreach (var h in hits)
            if (!h.transform.IsChildOf(obj.transform)) return true;

        return false;
    }

    bool IsMenuOpen()
    {
        return (inventoryPanel != null && inventoryPanel.activeInHierarchy)
            || (settingsPanel != null && settingsPanel.activeInHierarchy)
            || (welcomePanel != null && welcomePanel.activeInHierarchy);
    }
}
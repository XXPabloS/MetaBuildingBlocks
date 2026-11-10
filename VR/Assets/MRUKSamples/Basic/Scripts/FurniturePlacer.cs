using UnityEngine;
using Meta.XR.MRUtilityKit;
using Meta.XR.MRUtilityKitSamples.HandInput;

public class FurniturePlacer : MonoBehaviour
{
    public GameObject[] furniturePrefabs;
    public Transform rayOrigin;
    public LayerMask furnitureMask;   // capa "Furniture"

    public float rotationSpeed = 90f; // los grados q rota por segundo
    public float scaleSpeed = 1.0f;
    public float stickDeadzone = 0.2f;

    private Vector3 pickupPosition;
    private int currentIndex = 0;
    private GameObject selected;

    void Update()
    {
        var room = MRUK.Instance ? MRUK.Instance.GetCurrentRoom() : null;
        if (room == null) return;

        HandlePrefabSwitch();

        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        bool click = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) || HandInputManager.Instance.ThumbTapDown;

        if (selected != null)
        {
            // Rotacion y escalado de los objetos
            HandleRotationAndScale();

            if (TryGetSurface(room, ray, selected, out Vector3 point, out Vector3 normal))
                selected.transform.position = ComputePosition(selected, point, normal);

            if (click)
            {
                Physics.SyncTransforms();
                if (Overlaps(selected)) selected.transform.position = pickupPosition;
                selected = null;
            }
            return;
        }

        if (!click) return;

        if (Physics.Raycast(ray, out RaycastHit furnHit, 10f, furnitureMask))
        {
            Select(furnHit.collider.transform.root.gameObject);
            pickupPosition = selected.transform.position;
            return;
        }

        if (TryGetSurface(room, ray, null, out Vector3 p, out Vector3 n))
        {
            var go = Instantiate(furniturePrefabs[currentIndex], p, Quaternion.identity);
            SetLayerRecursively(go, LayerMaskToLayer(furnitureMask));

            go.transform.position = ComputePosition(go, p, n);
            Physics.SyncTransforms();

            if (Overlaps(go)) Destroy(go);
            else  Select(go); // para que luego de instanciarlo se quede seleccionado por si quiere modificar escala/rotacion
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

    Vector3 ComputePosition(GameObject obj, Vector3 point, Vector3 normal)
    {
        var box = obj.GetComponentInChildren<BoxCollider>();
        if (box == null) return point;

        Transform t = box.transform;
        Vector3 s = t.lossyScale;

        // Ejes reales del collider
        float hx = Mathf.Abs(box.size.x * s.x) * 0.5f;
        float hy = Mathf.Abs(box.size.y * s.y) * 0.5f;
        float hz = Mathf.Abs(box.size.z * s.z) * 0.5f;

        // Distancia del centro a su borde en la direccion de la normal
        float d = Mathf.Abs(Vector3.Dot(normal, t.right)) * hx + Mathf.Abs(Vector3.Dot(normal, t.up)) * hy + Mathf.Abs(Vector3.Dot(normal, t.forward)) * hz;

        // Centro real del collider
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

    void Select(GameObject obj)
    {
        selected = obj;
    }

    // sin grip: izquierda/derecha lo rota en Y y arriba/abajo lo escala
    // con grip: izquierda/derecha lo rota en Z y arriba/abajo lo rota en X
    void HandleRotationAndScale()
    {
        // el joystcik derecho del mando
        Vector2 stick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

        // el boton de agarre del mando derecho, hace de modificador
        bool gripRight = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        bool gripLeft = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);

        bool grip = gripRight || gripLeft;

        // solo cuenta el eje q mas se esta empujando para q no rote en diagonal
        bool horizontal = Mathf.Abs(stick.x) >= Mathf.Abs(stick.y);
        float value = horizontal ? stick.x : stick.y;

        if (Mathf.Abs(value) <= stickDeadzone) return;

        if (grip)
        {
            // con grip: rotacion en Z horizontal o en X vertical sobre los ejes del mundo
            float angle = -value * rotationSpeed * Time.deltaTime;
            Vector3 axis = horizontal ? Vector3.forward : Vector3.right;
            selected.transform.Rotate(axis, angle, Space.World);
        }
        else if (horizontal)
        {
            // sin grip: rotacion en Y sobre solo el eje vertical
            float angle = -value * rotationSpeed * Time.deltaTime;
            selected.transform.Rotate(Vector3.up, angle, Space.World);
        }
        else
        {
            // sin grip: escala uniforme
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
}
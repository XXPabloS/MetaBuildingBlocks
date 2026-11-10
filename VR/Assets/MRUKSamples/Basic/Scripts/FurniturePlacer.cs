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

            // Rotacion y escalado de los objetos
            HandleRotationAndScale();

            if (click) selected = null; 

            return;                      
        }

        if (!click) return;

        if (Physics.Raycast(ray, out RaycastHit furnHit, 10f, furnitureMask))
        {
            Select(furnHit.collider.transform.root.gameObject);
            return;
        }

        if (room.Raycast(ray, 10f, out RaycastHit hit, out MRUKAnchor anchor))
        {
            GameObject placed = Instantiate(furniturePrefabs[currentIndex], hit.point, Quaternion.identity);
            Select(placed); // para que luego de instanciarlo se quede seleccionado por si quiere modificar escala/rotacion
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

    void Select(GameObject obj)
    {
        selected = obj;
    }

    // izquierda/derecha lo rota y arriba / abajo lo escala
    void HandleRotationAndScale() 
    {
        // el joystcik derecho del mando
        Vector2 stick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

        // Rotacion eje X sobre solo el eje vertical
        if (Mathf.Abs(stick.x) > stickDeadzone)
        {
            float angle = -stick.x * rotationSpeed * Time.deltaTime;
            selected.transform.Rotate(Vector3.up, angle, Space.World);
        }

        // escala uniforme en eje y con limites
        if (Mathf.Abs(stick.y) > stickDeadzone)
        {
            float factor = 1f + stick.y * scaleSpeed * Time.deltaTime;
            selected.transform.localScale *= factor;
        }
    }
    
}
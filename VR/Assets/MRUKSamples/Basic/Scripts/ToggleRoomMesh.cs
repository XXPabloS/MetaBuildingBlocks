using UnityEngine;
using Meta.XR.MRUtilityKit;

public class ToggleRoomMesh : MonoBehaviour
{
    public EffectMesh effectMesh;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
            effectMesh.HideMesh = !effectMesh.HideMesh;
    }
}
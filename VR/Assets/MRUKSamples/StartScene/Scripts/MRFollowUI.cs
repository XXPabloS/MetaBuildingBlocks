
using Meta.XR.MRUtilityKitSamples.StartScene;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyProject.UI
{
    public class MRFollowUI : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [Header("Follow Settings")]
        [SerializeField]
        private float spawnDistanceFromCamera = 0.8f;

        [SerializeField]
        private float followSpeed = 2.5f;

        [Header("Meta XR References")]
        [SerializeField]
        private EventSystem menuEventSystem;

        [SerializeField]
        private UICursor cursor;

        private OVRCameraRig cameraRig;
        private Transform parentTransform;

        private void Awake()
        {
            cameraRig = FindAnyObjectByType<OVRCameraRig>();

            parentTransform = transform.parent;

            // Preserve Meta's EventSystem handling
            if (EventSystem.current == null)
            {
                EventSystem.current = menuEventSystem;
            }
            else if (menuEventSystem != null &&
                     EventSystem.current != menuEventSystem)
            {
                EventSystem.current.enabled = false;
                EventSystem.current = menuEventSystem;
            }
        }

        private void LateUpdate()
        {
            Billboard();
        }

        private void Billboard()
        {
            if (!cameraRig || !parentTransform)
                return;

            Transform eye = cameraRig.centerEyeAnchor;

            Vector3 direction =
                parentTransform.position - eye.position;

            if (direction.sqrMagnitude > 0.01f)
            {
                parentTransform.rotation =
                    Quaternion.LookRotation(direction);

                Vector3 targetPosition =
                    eye.position +
                    eye.forward * spawnDistanceFromCamera;

                parentTransform.position =
                    Vector3.Lerp(
                        parentTransform.position,
                        targetPosition,
                        Time.deltaTime * followSpeed
                    );
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (cursor != null)
                cursor.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (cursor != null)
                cursor.gameObject.SetActive(false);
        }
    }
}
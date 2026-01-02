using UnityEngine;
using Drip.Managers;

namespace Drip.Core
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Positions")]
        [SerializeField] private Transform dropPhasePosition;
        [SerializeField] private Transform dividePhasePosition;
        [SerializeField] private Transform dousePhasePosition;

        [Header("Transition Settings")]
        [SerializeField] private float transitionSpeed = 2f;
        [SerializeField] private bool smoothTransition = true;

        private Transform targetPosition;
        private bool isTransitioning = false;

        private void Start()
        {
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnPhaseChanged += HandlePhaseChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnPhaseChanged -= HandlePhaseChanged;
            }
        }

        private void Update()
        {
            if (isTransitioning && targetPosition != null)
            {
                UpdateCameraTransition();
            }
        }

        private void HandlePhaseChanged(GamePhase newPhase)
        {
            switch (newPhase)
            {
                case GamePhase.Drop:
                    MoveTo(dropPhasePosition);
                    break;
                case GamePhase.Divide:
                    MoveTo(dividePhasePosition);
                    break;
                case GamePhase.Douse:
                    MoveTo(dousePhasePosition);
                    break;
            }
        }

        public void MoveTo(Transform position)
        {
            if (position == null)
            {
                Debug.LogWarning("Target camera position is null");
                return;
            }

            targetPosition = position;

            if (smoothTransition)
            {
                isTransitioning = true;
            }
            else
            {
                transform.position = targetPosition.position;
                transform.rotation = targetPosition.rotation;
            }
        }

        private void UpdateCameraTransition()
        {
            // Smoothly move to target position
            transform.position = Vector3.Lerp(
                transform.position, 
                targetPosition.position, 
                Time.deltaTime * transitionSpeed
            );

            // Smoothly rotate to target rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetPosition.rotation, 
                Time.deltaTime * transitionSpeed
            );

            // Check if we've reached the target
            float distance = Vector3.Distance(transform.position, targetPosition.position);
            float angle = Quaternion.Angle(transform.rotation, targetPosition.rotation);

            if (distance < 0.01f && angle < 0.1f)
            {
                transform.position = targetPosition.position;
                transform.rotation = targetPosition.rotation;
                isTransitioning = false;
            }
        }
    }
}

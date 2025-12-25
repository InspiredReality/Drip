using UnityEngine;

namespace Drip.Phases.Drop
{
    public class Bucket : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float moveRange = 3f; // How far left/right the bucket can move

        [Header("Input")]
        [SerializeField] private bool useMouseControl = true;
        [SerializeField] private bool useTouchControl = true;

        private Vector3 startPosition;
        private Camera mainCamera;

        private void Start()
        {
            startPosition = transform.position;
            mainCamera = Camera.main;
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            Vector3 targetPosition = transform.position;

            // Mouse control for desktop/WebGL
            if (useMouseControl && Input.GetMouseButton(0))
            {
                Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.x = mousePos.x;
            }
            // Touch control for mobile
            else if (useTouchControl && Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchPos = mainCamera.ScreenToWorldPoint(touch.position);
                targetPosition.x = touchPos.x;
            }
            // Keyboard control (fallback)
            else
            {
                float horizontal = Input.GetAxis("Horizontal");
                targetPosition.x += horizontal * moveSpeed * Time.deltaTime;
            }

            // Clamp position within range
            targetPosition.x = Mathf.Clamp(targetPosition.x, 
                startPosition.x - moveRange, 
                startPosition.x + moveRange);

            transform.position = targetPosition;
        }
    }
}

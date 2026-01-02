using UnityEngine;
using Drip.Managers;

namespace Drip.Core
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        [Header("Input Settings")]
        [SerializeField] private bool enableMouseInput = true;
        [SerializeField] private bool enableTouchInput = true;
        [SerializeField] private bool enableKeyboardInput = true;

        // Input events
        public System.Action<Vector2> OnPointerDown;
        public System.Action<Vector2> OnPointerUp;
        public System.Action<Vector2> OnPointerDrag;
        public System.Action<Vector2> OnPointerClick;

        private Camera mainCamera;
        private Vector2 lastPointerPosition;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            // Mouse Input (Desktop/WebGL)
            if (enableMouseInput)
            {
                HandleMouseInput();
            }

            // Touch Input (Mobile)
            if (enableTouchInput && Input.touchCount > 0)
            {
                HandleTouchInput();
            }

            // Keyboard Input
            if (enableKeyboardInput)
            {
                HandleKeyboardInput();
            }
        }

        private void HandleMouseInput()
        {
            Vector2 mousePosition = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                lastPointerPosition = mousePosition;
                OnPointerDown?.Invoke(GetWorldPosition(mousePosition));
            }

            if (Input.GetMouseButton(0))
            {
                if (Vector2.Distance(mousePosition, lastPointerPosition) > 1f)
                {
                    OnPointerDrag?.Invoke(GetWorldPosition(mousePosition));
                    lastPointerPosition = mousePosition;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnPointerUp?.Invoke(GetWorldPosition(mousePosition));
                
                // Check if it was a click (not a drag)
                if (Vector2.Distance(mousePosition, lastPointerPosition) < 5f)
                {
                    OnPointerClick?.Invoke(GetWorldPosition(mousePosition));
                }
            }
        }

        private void HandleTouchInput()
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    lastPointerPosition = touchPosition;
                    OnPointerDown?.Invoke(GetWorldPosition(touchPosition));
                    break;

                case TouchPhase.Moved:
                    OnPointerDrag?.Invoke(GetWorldPosition(touchPosition));
                    lastPointerPosition = touchPosition;
                    break;

                case TouchPhase.Ended:
                    OnPointerUp?.Invoke(GetWorldPosition(touchPosition));
                    
                    // Check if it was a tap (not a swipe)
                    if (Vector2.Distance(touchPosition, lastPointerPosition) < 50f)
                    {
                        OnPointerClick?.Invoke(GetWorldPosition(touchPosition));
                    }
                    break;
            }
        }

        private void HandleKeyboardInput()
        {
            // Keyboard shortcuts
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Pause game or go back
                if (GameManager.Instance != null)
                {
                    if (GameManager.Instance.CurrentState == GameState.Playing)
                    {
                        GameManager.Instance.ChangeGameState(GameState.Paused);
                    }
                    else if (GameManager.Instance.CurrentState == GameState.Paused)
                    {
                        GameManager.Instance.ChangeGameState(GameState.Playing);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Advance phase (for testing)
                if (GameManager.Instance != null && 
                    GameManager.Instance.CurrentState == GameState.Playing)
                {
                    GameManager.Instance.AdvancePhase();
                }
            }
        }

        private Vector2 GetWorldPosition(Vector2 screenPosition)
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPosition);
            return new Vector2(worldPos.x, worldPos.y);
        }

        public Vector3 GetWorldPosition3D(Vector2 screenPosition)
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            return mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, mainCamera.nearClipPlane));
        }

        public Ray GetScreenRay(Vector2 screenPosition)
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            return mainCamera.ScreenPointToRay(screenPosition);
        }
    }
}

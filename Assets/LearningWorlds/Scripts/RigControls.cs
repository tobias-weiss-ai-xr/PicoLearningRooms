using UnityEngine;

namespace LearningWorlds
{
    /// <summary>
    /// Simple WASD + Mouse Look controls for desktop testing.
    /// Used as a fallback when VR controllers are not available.
    /// </summary>
    public class RigControls : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 5f;
        public float rotationSpeed = 100f;
        public float jumpForce = 5f;

        [Header("Mouse Look")]
        public float mouseSensitivity = 2f;
        public bool invertY = false;

        private CharacterController characterController;
        private Camera playerCamera;
        private float verticalRotation = 0f;

        private void Awake()
        {
            // Try to find or create CharacterController
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                characterController = gameObject.AddComponent<CharacterController>();
                characterController.height = 1.8f;
                characterController.radius = 0.5f;
            }

            // Find camera in children
            if (playerCamera == null)
            {
                playerCamera = GetComponentInChildren<Camera>();
                if (playerCamera == null)
                {
                    GameObject camObj = new GameObject("PlayerCamera");
                    camObj.transform.SetParent(transform);
                    playerCamera = camObj.AddComponent<Camera>();
                    playerCamera.transform.localPosition = new Vector3(0f, 1.6f, 0f);
                }
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            if (characterController == null || playerCamera == null)
                return;

            HandleMovement();
            HandleMouseLook();
            HandleJump();
        }

        private void HandleMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        private void HandleMouseLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            if (invertY)
                mouseY *= -1f;

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        private void HandleJump()
        {
            if (Input.GetButtonDown("Jump") && characterController.isGrounded)
            {
                characterController.Move(Vector3.up * jumpForce);
            }
        }

        /// <summary>
        /// Teleports the rig to a specific position.
        /// </summary>
        public void Teleport(Vector3 position)
        {
            if (characterController != null)
            {
                characterController.enabled = false;
                transform.position = position;
                characterController.enabled = true;
            }
            else
            {
                transform.position = position;
            }
        }

        /// <summary>
        /// Resets the camera rotation.
        /// </summary>
        public void ResetRotation()
        {
            verticalRotation = 0f;
            if (playerCamera != null)
            {
                playerCamera.transform.localRotation = Quaternion.identity;
            }
            transform.rotation = Quaternion.identity;
        }
    }
}

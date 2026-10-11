#region Usings
using UnityEngine;
#endregion

namespace LearningWorlds.Pse
{
    /// <summary>
    /// Rotates electron shells around an atom nucleus.
    /// </summary>
    public class ElectronRotator : MonoBehaviour
    {
        [Tooltip("Rotation speed of the electrons around the nucleus (degrees per second)")]
        public float rotationSpeed = 20.0f;

        private Transform[] electronChildren;

        private void Start()
        {
            // Get all child transforms (electron shells)
            electronChildren = GetComponentsInChildren<Transform>();
        }

        private void Update()
        {
            foreach (Transform electronShell in electronChildren)
            {
                // Skip self (the atom itself)
                if (electronShell == transform)
                    continue;

                // Rotate around the Y-axis to simulate electron orbit
                float angleOffset = Time.deltaTime * rotationSpeed;
                electronShell.RotateAround(transform.position, Vector3.up, angleOffset);
            }
        }
    }
}

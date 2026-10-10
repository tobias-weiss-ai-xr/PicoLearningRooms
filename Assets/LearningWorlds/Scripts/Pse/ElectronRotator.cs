#region Usings
using UnityEngine;

namespace LearningWorlds.PSE
{
    public class ElectronRotator : MonoBehaviour  {
        [Tooltip("Rotation speed of the electrons around the nucleus")]
        public float rotationSpeed = 20.0f;

        private static Transform[] _electronChildren;

        void Start() {
            _electronChildren = GetComponentsInChildren<Transform>(includeInactive: true);
        }

        void Update()
        {
            foreach (Transform electronShell in _electronChildren)
            {
                float angleOffset = Time.deltaTime * rotationSpeed;

                // Rotate around the Y-axis to simulate electron movement
                electronShell.RotateAround(transform.position, Vector3.up, angleOffset);
            }
        }
    }}


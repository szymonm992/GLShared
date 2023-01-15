using GLShared.General.Interfaces;
using GLShared.General.Models;
using Zenject;
using UnityEngine;
using GLShared.General.Enums;
using UnityEditor;
using GLShared.General.ScriptableObjects;

namespace GLShared.General.Components
{
    public class UTIdlerWheel : UTPhysicWheelBase
    {
        [SerializeField] private LayerMask terrainMask;
        [SerializeField] private Vector3 idlerWheelOffset;
        private Vector3 idlerForcePoint;

        public override HitInfo HitInfo => base.HitInfo;
        public override bool IsIdler => true;

        [SerializeField]
        protected UTWheelDebug debugSettings = new UTWheelDebug()
        {
            DrawGizmos = true,
            DrawOnDisable = false,
            DrawMode = UTDebugMode.All,
            DrawWheelDirection = false,
            DrawShapeGizmo = true,
            DrawSprings = true,
        };

        public void OnValidate()
        {
            if (rig == null)
            {
                rig = transform.GetComponentInParent<Rigidbody>();
            }
            tirePosition = GetTirePosition();
        }

        protected override void FixedUpdate()
        {
            tirePosition = GetTirePosition();
        }

        protected override Vector3 GetTirePosition()
        {
            Vector3 offsetStartPosition = OffsetPosition(transform, idlerWheelOffset);
            isGrounded = Physics.CheckSphere(offsetStartPosition, wheelRadius, terrainMask);
            idlerForcePoint = isGrounded ? transform.position + (transform.forward * wheelRadius) : transform.position;
            hitInfo = new HitInfo()
            {
                rayHit = new RaycastHit()
                {
                    point = idlerForcePoint,
                    distance = wheelRadius,
                    normal = -Transform.forward
                },
            };
            return offsetStartPosition;
        }

        private Vector3 OffsetPosition(Transform origin, Vector3 offset)
        {
            if(offset != Vector3.zero)
            {
                return origin.position + (origin.right * offset.x)
                + (origin.up * offset.y) + (origin.forward * offset.z);
                
            }
            return origin.position;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            bool drawCurrently = (debugSettings.DrawGizmos) && (debugSettings.DrawMode == UTDebugMode.All)
                || (debugSettings.DrawMode == UTDebugMode.EditorOnly && !Application.isPlaying)
                || (debugSettings.DrawMode == UTDebugMode.PlaymodeOnly && Application.isPlaying);

            if (drawCurrently && (this.enabled) || (debugSettings.DrawOnDisable && !this.enabled))
            {
                if (rig != null)
                {
                    if (!Application.isPlaying)
                    {
                        tirePosition = GetTirePosition();
                    }

                    if (debugSettings.DrawShapeGizmo)
                    {
                        Handles.color = Color.white;
                        Handles.DrawLine(tirePosition + transform.forward * 0.05f, tirePosition - transform.forward * 0.05f, 4f);
                        Gizmos.color = isGrounded ? Color.green : Color.red;
                        Gizmos.DrawWireSphere(tirePosition, wheelRadius);
                    }

                    if (isGrounded)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(idlerForcePoint, .08f);;
                    }

                    if (debugSettings.DrawWheelDirection)
                    {
                        Handles.color = isGrounded ? Color.green : Color.red;
                        Handles.DrawLine(tirePosition, tirePosition + transform.forward, 2f);
                    }
                }
            }
        }
        #endif
    }
}

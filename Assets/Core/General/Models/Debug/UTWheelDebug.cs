using UnityEngine;
using System;
using GLShared.General.Enums;

namespace GLShared.General.Models
{
 
    [Serializable]
    public class UTWheelDebug
    {
        [Header("Gizmos")]
        [SerializeField] private bool drawGizmos;
        [SerializeField] private bool drawOnDisable;
        [SerializeField] private bool drawWheelDirection;
        [SerializeField] private bool drawShapeGizmo;
        [SerializeField] private bool drawSprings;
        [SerializeField] private UTDebugMode drawMode;

        public UTDebugMode DrawMode
        {
            get => drawMode;
            set
            {
                this.drawMode = value;
            }
        }
        public bool DrawGizmos
        {
            get => drawGizmos;
            set
            {
                this.drawGizmos = value;
            }
        }
        
        public bool DrawOnDisable
        {
            get => drawOnDisable;
            set
            {
                this.drawOnDisable = value;
            }
        }

        public bool DrawWheelDirection
        {
            get => drawWheelDirection;
            set
            {
                this.drawWheelDirection = value;
            }
        }
        public bool DrawShapeGizmo
        {
            get => drawShapeGizmo;
            set
            {
                this.drawShapeGizmo = value;
            }
        }
        public bool DrawSprings
        {
            get => drawSprings;
            set
            {
                this.drawSprings = value;
            }
        }
    }
}

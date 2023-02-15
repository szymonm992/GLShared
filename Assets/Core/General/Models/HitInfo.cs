using UnityEngine;

namespace GLShared.General.Models
{
	[System.Serializable]
	public class HitInfo
	{
		public RaycastHit rayHit;
		private float normalAndUpAngle;

        public Vector3 Point => rayHit.point;
        public Vector3 Normal => rayHit.normal;
        public float Distance => rayHit.distance;
        public Collider Collider => rayHit.collider;
        public float NormalAndUpAngle => normalAndUpAngle;

        public bool CanCollide(float maxAngle, LayerMask wheelsMask)
		{
            return CalculateNormalAngle(rayHit.normal) <= maxAngle && rayHit.collider.gameObject.layer.IsInLayerMask(wheelsMask);
		}

		private float CalculateNormalAngle(Vector3 normal)
		{
			normalAndUpAngle = Vector3.Angle(Vector3.up, normal);
            return normalAndUpAngle;
        }

		
	}
}

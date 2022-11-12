using UnityEngine;

namespace GLShared.General.Models
{
	[System.Serializable]
	public class HitInfo
	{
		public RaycastHit rayHit;
		private float normalAndUpAngle;

		public void CalculateNormalAndUpDifferenceAngle()
        {
			this.normalAndUpAngle = Vector3.Angle(Vector3.up, rayHit.normal);
		}

		public Vector3 Point
		{
			get => rayHit.point;
		}

		public Vector3 Normal
		{
			get => rayHit.normal;
		}

		public float Distance
        {
			get => rayHit.distance;
        }

		public Collider Collider
        {
			get => rayHit.collider;
        }

		public float NormalAndUpAngle
        {
			get => normalAndUpAngle;

		}
	}
}

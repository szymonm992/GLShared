using UnityEngine;

using Sfs2X;


namespace GLShared.Networking
{
	public class SmartFoxConnection : MonoBehaviour
	{
        public bool somethiing = true;
        public bool somethiing33 = true;

        private SmartFox sfs;
		public SmartFox Connection 
		{
			get => sfs;	
			set
			{
				sfs = value;
			}
		}

		
		public bool IsInitialized
		{
			get
			{
				return (this.sfs != null);
			}
		}

		public void Disconnect()
		{
			OnApplicationQuit();
		}

		private void OnApplicationQuit()
		{
			if (IsInitialized)
			{
				if (sfs.IsConnected)
				{
					sfs.Disconnect();
				}
			}
		}
	}
}

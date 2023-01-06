using UnityEngine;
using Sfs2X;


namespace GLShared.Networking.Components
{
	public class SmartFoxConnection : MonoBehaviour
	{
        public int PORT = 9933;

        private SmartFox sfs;

		public string DisconnectError { get; set; }
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

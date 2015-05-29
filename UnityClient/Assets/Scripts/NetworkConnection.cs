using UnityEngine;
using System.Collections;

public class NetworkConnection : MonoBehaviour {

	public string ipaddress;

	private bool connected = false;

	private void OnConnectedToServer(){
		connected = true;
	}

	private void OnServerInitialized(){
		connected = true;
	}
	private void OnDisconnectedFromServer(){
		connected = false;
	}
	void Start(){
		ipaddress = Network.player.ipAddress;
	}
	private void OnGUI(){
		if (!connected) {

			if (GUILayout.Button ("Connect")){
				Network.Connect ("127.0.0.1", 25001);
			}
				

			if (GUILayout.Button ("Host")){
				Network.InitializeServer (4, 25001, true);
			}
		} 
		else
			GUILayout.Label ("Connections: " + Network.connections.Length.ToString ());
			GUILayout.Label ("My IP: " + ipaddress);
	}
}

using UnityEngine;
using System.Collections;

public class NetworkConnection : MonoBehaviour {

	public string ipaddress;
	public ScreenFeedback sf;

	private bool connected = false;


	void Start(){
		ipaddress = Network.player.ipAddress;
	}

	private void OnConnectedToServer(){
		connected = true;
	}

	private void OnServerInitialized(){
		connected = true;
	}
	private void OnDisconnectedFromServer(){
		connected = false;
	}

	private void OnGUI(){
		if (!connected) {

			if (GUILayout.Button ("Connect")){
				Network.Connect ("192.168.0.3", 25001);
			}
				

			if (GUILayout.Button ("Host")){
				Network.InitializeServer (4, 25001, true);
			}
		} 
		else
			GUILayout.Label ("Connections: " + Network.connections.Length.ToString ());
			GUILayout.Label ("My IP: " + ipaddress);
	}

	[RPC]
	void getData(string data){
		print (data);
		string[] spilt = data.Split(':');
		string to = spilt [0];
		string command = spilt [1];
		print (sf.ID);
		if (to.Contains (sf.ID)) {
			sf.printCommand(command);
			sf.slotCount++;
		}
		
	}
}

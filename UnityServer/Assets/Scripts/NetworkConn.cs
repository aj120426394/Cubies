using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkConn : MonoBehaviour {

	private int Port = 25001;
	private Dictionary<string, string> client;
	
	
	// Use this for initialization
	void Start () {
		this.client = new Dictionary<string, string> ();
		this.client.Add("C1", "192.168.0.1");
		this.client.Add("C2", "192.168.0.2");
		this.client.Add("C3", "192.168.0.3");
		this.client.Add("C4", "192.168.0.4");
		DontDestroyOnLoad (this);

		OpenConnection ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void OpenConnection(){
		if (Network.peerType == NetworkPeerType.Disconnected) {
			Network.InitializeServer(5,Port);
		}
	}
	
	private void CloseConnection(){
		if (!(Network.peerType == NetworkPeerType.Disconnected)) {
			Network.Disconnect(250);
		}
	}


	public void sendData(string str){
		this.GetComponent<NetworkView>().RPC("getData", RPCMode.All, str);
	}

	[RPC]
	void getData(string str){
		//Debug.Log ("Do you get anything? " + str);
	}
}

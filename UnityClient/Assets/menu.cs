using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour {

	private string IP = "192.168.0.106";
	private int Port = 25001;
	public GameObject target;
	private string ID = "";

	// Use this for initialization
	void Start () {
		string clientIP = Network.player.ipAddress;
		string spilt = clientIP.Split('.');
		this.ID = "C" + spilt [spilt - 1];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if (Network.peerType == NetworkPeerType.Disconnected) {
			if (GUI.Button (new Rect (100, 100, 100, 25), "Start client")) {
				Network.Connect (IP, Port);
			}
		} else {
			GUI.Label(new Rect(100,100,100,25), "Cleint");

			if(GUI.Button(new Rect(100,125,100,25), "Logout")){
				Network.Disconnect(250);
			}
		}
	}

	public void input(string data){
	}

	[RPC]
	void getData(string data){
		//target.GetComponent<Renderer>().material.color = Color.green;
		//print ("Do you get anything? " + str + " from: " + sender.sender.ipAddress);
		string[] spilt = data.Split(':');
		string to = spilt [0];
		string command = spilt [1];
		if (to.Equals (this.ID)) {
			input (command);
		}
	}	
}

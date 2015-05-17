using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour {

	private string IP = "192.168.0.106";
	private int Port = 25001;
		public GameObject target;

	// Use this for initialization
	void Start () {
	
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

	[RPC]
	void ChangeColor(string str, NetworkMessageInfo sender){
		target.GetComponent<Renderer>().material.color = Color.green;
		print ("Do you get anything? " + str + " from: " + sender.sender.ipAddress);
	}

}

using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {


	private SerialConn serialConn;
	private NetworkConn networkConn;

	private int updateInterval = 4; //the delay between updates
	private int updateSlower = 0; //container
	private int clientNum = 0;

	// Use this for initialization
	void Start () {
		this.serialConn = new SerialConn ();
		this.networkConn = (NetworkConn)GameObject.Find ("NetworkConnection").GetComponent<NetworkConn> ();
		serialConn.sendData ("shipColor");
		this.clientNum = networkConn.getCleintNum ();
	}
	
	// Update is called once per frame
	void Update () {
		updateSlower ++; //Recieve data every 4 frames
		if(updateSlower >= updateInterval){ //
			string data = serialConn.recieveData ();
			if(data.Length > 0){
				setShipColor (data);
			}
			updateSlower = 0;
		}
	}

	private void setShipColor(string data){
		this.networkConn.sendData (data);
		string[] spilt = data.Split(':');
		string from = spilt[0];
		string color = spilt[1].Remove(1);
		PlayerPrefs.SetString (from, color);
		this.serialConn.broadcastData (this.clientNum, color);
	}
}

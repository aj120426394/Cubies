using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class game : MonoBehaviour {
	public float speed = 0.5f;

	public character[] characters;
	private bool gamestart = false;

	
	private List<string> newFunction;
	private List<List<string>> functionList;
	
	private SerialConn sc;
	private character c1;
	private NetworkConnection nc;

	private Camera camera;

	// Use this for initialization
	void Start () {
		print ("GAME RULES: I = if T = then  B = blue  R = Red G = green  Y = yellow");
		sc = new SerialConn ("/dev/cu.usbmodem330471", 9600);
		nc = new NetworkConnection (GetComponent<NetworkView> ());
		nc.OpenConnection ();
		foreach (character c in characters) {
			c.setSerialConnection (this.sc);
		}
		sc.sendData ("StartInput");
		this.camera = GetComponent<Camera> ();
	}

	void OnDestroy(){
		nc.CloseConnection ();
	}

	public int updateInterval = 4; //the delay between updates
	public int updateSlower = 0; //container
	
	// Update is called once per frame
	void Update () {
		//inputFunctions2 ();

		if (gamestart) {
			calCamera();
		}
		//calCamera();

		updateSlower ++; //Recieve data every 4 frames
		if(updateSlower >= updateInterval){ //
			string data = sc.recieveData ();
			//string data = "";
			if(data.Length > 0){
				//print ("In coming data: " + data);
				inputFunctions (data);
				nc.sendData(data);
			}
			updateSlower = 0;
		}
		inputFunctions2 ();
	}

	/* Handles input keys */
	void inputFunctions(string data){
		//writing a function
		string[] spilt = data.Split(':');
		string from = spilt[0];
		string command = spilt[1].Remove(1);

		int i = int.Parse(from.Substring(1))-1;
		character client = characters[i];
		client.input(command);
	}
	
	void inputFunctions2(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			print ("space");
			foreach(character c in characters){
				c.setGameStart(true);
				string tmp = "GameStart";
				this.sc.sendData(tmp);
			}
			this.gamestart = true;
		}
	}

	private void calCamera(){
		float right = 0f;
		float left = 0f;
		float targetX = 0f;
		float targetY = 0f;
		float targetZ = 0f;
		Vector3 velocity = Vector3.zero;

		bool objectMove = true;
		// Calculate the size of cam
		for (int i = 0; i < characters.Length; i++) {
			if(i == 0){
				right = characters[i].transform.position.x;
				left = characters[i].transform.position.x;
			}else{
				if(characters[i].transform.position.x > right){
					right = characters[i].transform.position.x;
				}
				if(characters[i].transform.position.x < left){
					left = characters[i].transform.position.x;
				}
			}
			targetX += characters[i].transform.position.x;
			targetY += characters[i].transform.position.y;
			targetZ += characters[i].transform.position.z;

			if(!characters[i].getMoveObject()){
				objectMove = false;
			}
		}

		float camSize = 0f;
		if (!objectMove) {
			camSize = 5f;
		} else {
			camSize = (right - left) / 3f;
			if (camSize <= 1f) {
				camSize = 1f;
			}
		}

		this.camera.orthographicSize = Mathf.MoveTowards (this.camera.orthographicSize, camSize, 2.0f * Time.deltaTime);

		targetX = targetX / characters.Length;
		targetY = targetY / characters.Length;
		targetZ = targetZ / characters.Length;

		Vector3 target = new Vector3 (targetX, targetY, targetZ);

		Vector3 point = camera.WorldToViewportPoint(target);
		Vector3 delta = target - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
		Vector3 destination = transform.position + delta;
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, 0.1f);
	}

	/*
	 * Using for RPC sending to client
	 */

	[RPC]
	void ChangeColor(string str){
		//Debug.Log ("Do you get anything? " + str);
	}
}

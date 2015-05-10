using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class game : MonoBehaviour {
	public float speed = 0.5f;

	public character[] characters;

	
	private List<string> newFunction;
	private List<List<string>> functionList;
	
	private SerialConn sc;
	private character c1;

	private Camera camera;

	// Use this for initialization
	void Start () {
		print ("GAME RULES: I = if T = then  B = blue  R = Red G = green  Y = yellow");
		sc = new SerialConn ("/dev/cu.usbmodem330471", 9600);
		foreach (character c in characters) {
			c.setSerialConnection (this.sc);
		}
		sc.sendData ("StartInput");
		this.camera = GetComponent<Camera> ();
	}
	public int updateInterval = 4; //the delay between updates
	public int updateSlower = 0; //container
	
	// Update is called once per frame
	void Update () {
		//inputFunctions2 ();
		calCamera();
		updateSlower ++; //Recieve data every 4 frames
		
		//string[] data = {"C1:I","C1:N","C1:R","C1:O","C1:N","C1:G","C1:T","C1:Y","C1:O","C1:B","C1:E"};
		//string data = sc.recieveData ();
		/*
		foreach(string d in data){
			inputFunctions(d);
		}
		*/
		//this.sc.sendData("C1:R");

		if(updateSlower >= updateInterval){ //
			string data = sc.recieveData ();
			if(data.Length > 0){
				//print ("In coming data: " + data);
				inputFunctions (data);
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
		}
	}

	private void calCamera(){
		float right = 0f;
		float left = 0f;
		float targetX = 0f;
		float targetY = 0f;
		float targetZ = 0f;
		//character rc = characters[0];
		//character lc = characters[0];
		Vector3 velocity = Vector3.zero;

		// Calculate the size of cam
		foreach (character c in characters) {
			if(c.transform.position.x > right){
				right = c.transform.position.x;
				//rc = c;
			}
			if(c.transform.position.x < left){
				left = c.transform.position.x;
				//lc = c;
			}
			targetX += c.transform.position.x;
			targetY += c.transform.position.y;
			targetZ += c.transform.position.z;
		}
		int camSize = (int)((right - left) / 3);
		if (camSize <= 0) {
			camSize = 1;
		}
		//this.camera.orthographicSize = camSize;
		this.camera.orthographicSize = Mathf.MoveTowards (this.camera.orthographicSize, camSize, 2.0f * Time.deltaTime);

		targetX = targetX / characters.Length;
		targetY = targetY / characters.Length;
		targetZ = targetZ / characters.Length;

		//print ("(" + targetX + ", " + targetY + ", " + targetZ + ")");

		Vector3 target = new Vector3 (targetX, targetY, targetZ);



		Vector3 point = camera.WorldToViewportPoint(target);
		Vector3 delta = target - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
		Vector3 destination = transform.position + delta;
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, 0.1f);


	}
}

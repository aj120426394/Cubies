using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class game : MonoBehaviour {
	public float speed = 0.5f;
	public obstacle startPlanet;
	public obstacle finishPlanet;

	public character[] characters;
	private bool gamestart = false;
	
	private SerialConn sc;
	private character c1;
	private NetworkConn networkConn;

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


		this.networkConn = (NetworkConn)GameObject.Find ("NetworkConnection").GetComponent<NetworkConn> ();

		/*
		 * the code of adding new character.

		Object prefab = AssetDatabase.LoadAssetAtPath ("Assets/Prefab/Ship.prefab", typeof(GameObject));
		GameObject testPrefab = (GameObject)Instantiate (prefab, new Vector3 (-17.4f, 5.6f, -1.5f), Quaternion.identity);

		GameObject test = GameObject.Find("START");
		obstacle testObs = (obstacle)test.GetComponent<obstacle> ();

		testPrefab.GetComponent<character> ().startObstacle = startPlanet;
		testPrefab.GetComponent<character> ().finishObstacle = finishPlanet;
		testPrefab.name = "C2";

		*/

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
				networkConn.sendData(data);
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
			}
			string tmp = "GameStart";
			this.sc.sendData(tmp);
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

			//print (targetX + ", " + targetY + ", " + targetZ);

			if(!characters[i].getMoveObject()){
				objectMove = false;
			}
		}

		float camSize = 0f;
		if (!objectMove) {
			camSize = 10f;
		} else {
			camSize = (right - left) / 3f;
			if (camSize <= 2f) {
				camSize = 2f;
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
	 * Used for network connection
	 */



}

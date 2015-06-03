using UnityEngine;

//using UnityEditor;

using System.Collections;
using System.Collections.Generic;

public class game : MonoBehaviour {
	public float speed = 0.5f;
	public obstacle start;
	public obstacle firstPlanet;
	public obstacle finishPlanet;
	public obstacle goal;
	public GameObject Rship;
	public GameObject Gship;
	public GameObject Bship;
	public GameObject Yship;

	
	private Dictionary<string, character> characters;
	private bool gamestart = false;
	
	private SerialConn serialConn;
	private NetworkConn networkConn;

	private Camera camera;

	private int characterNum;

	// Use this for initialization
	void Start () {
		/*
		 * Testing playerpref
		 */

		PlayerPrefs.SetInt ("clientNum", 3);
		PlayerPrefs.SetString ("C1", "G");
		PlayerPrefs.SetString ("C2", "B");
		PlayerPrefs.SetString ("C3", "R");


		print ("GAME RULES: I = if T = then  B = blue  R = Red G = green  Y = yellow");

		this.serialConn = new SerialConn ();
		this.networkConn = (NetworkConn)GameObject.Find ("NetworkConnection").GetComponent<NetworkConn> ();

		this.characters = new Dictionary<string, character> ();



		characterNum = PlayerPrefs.GetInt("clientNum");
		for (int i = 0; i < characterNum; i++) {
			string charName = "C" + (i+1);
			string shipColor = PlayerPrefs.GetString(charName);
			print(shipColor);
			GameObject shipPrefab = null;
			//Object shipPrefab = AssetDatabase.LoadAssetAtPath (prefabAddr, typeof(GameObject));

			if(shipColor == "R"){
				shipPrefab = Rship;
			}else if(shipColor == "G"){
				shipPrefab = Gship;
			}else if(shipColor == "B"){
				shipPrefab = Bship;
			}else if(shipColor == "Y"){
				shipPrefab = Yship;
			}
			GameObject shipObject = (GameObject)Instantiate (shipPrefab, new Vector3 (start.transform.position.x, start.transform.position.y, -1.7f), Quaternion.identity);

			character ship = shipObject.GetComponent<character>();

			ship.start = start;
			ship.firstObstacle = firstPlanet;
			ship.finishObstacle = finishPlanet;
			ship.goal = goal;
			ship.name = charName;
			this.characters.Add(charName, ship);
		}

		foreach (character c in characters.Values) {
			c.setSerialConnection (this.serialConn);
		}
	
		serialConn.broadcastData (characterNum, "StartInput");
		this.camera = GetComponent<Camera> ();



	}

	public int updateInterval = 4; //the delay between updates
	public int updateSlower = 0; //container
	
	// Update is called once per frame
	void Update () {

		if (gamestart) {
			calCamera();
		}

		updateSlower ++; //Recieve data every 4 frames
		if(updateSlower >= updateInterval){ //
			string data = serialConn.recieveData ();
			//string data = "";
			if(data.Length > 0){

				inputFunctions (data);
				networkConn.sendData(data);
			}
			updateSlower = 0;
		}
		inputFunctions2 ();



		if(Input.GetKeyDown("i"))
		{
			string temp = "C1:I\n";
			inputFunctions(temp);
		}else if(Input.GetKeyDown("t"))
		{
			string temp = "C1:T\n";
			inputFunctions(temp);
		}
		else if(Input.GetKeyDown("l"))
		{
			string temp = "C1:L\n";
			inputFunctions(temp);
		}
		else if(Input.GetKeyDown("o"))
		{
			string temp = "C1:O\n";
			inputFunctions(temp);
		}
		else if(Input.GetKeyDown("n"))
		{
			string temp = "C1:N\n";
			inputFunctions(temp);
		}
		else if(Input.GetKeyDown("e"))
		{
			string temp = "C1:E\n";
			inputFunctions(temp);
		}
		else if(Input.GetKeyDown("r"))
		{
			string temp = "C1:R\n";
			inputFunctions(temp);
		}
		else if(Input.GetKeyDown("g"))
		{
			string temp = "C1:G\n";
			inputFunctions(temp);
		}
		else if(Input.GetKeyDown("b"))
		{
			string temp = "C1:B\n";
			inputFunctions(temp);
		}
		else if(Input.GetKeyDown("y"))
		{
			Debug.Log("yellow");
			string temp = "C1:Y\n";
			inputFunctions(temp);
		}


	}

	/* Handles input keys */
	void inputFunctions(string data){
		//writing a function
		print ("In coming data: " + data);
		string[] spilt = data.Split(':');
		string from = spilt[0];
		string command = spilt[1].Remove(1);

		int i = int.Parse(from.Substring(1))-1;
		character client = this.characters[from];
		client.input(command);
	}
	
	void inputFunctions2(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			print ("space");
			foreach(character c in characters.Values){
				c.setGameStart(true);
			}
			this.serialConn.broadcastData(this.characterNum,"GameStart");
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
		int i = 0;
		foreach (character c in characters.Values) {
			if(i == 0){
				right = c.transform.position.x;
				left = c.transform.position.x;
			}else{
				if(c.transform.position.x > right){
					right = c.transform.position.x;
				}
				if(c.transform.position.x < left){
					left = c.transform.position.x;
				}
			}
			targetX += c.transform.position.x;
			targetY += c.transform.position.y;
			targetZ += c.transform.position.z;
			
			//print (targetX + ", " + targetY + ", " + targetZ);
			
			if(!c.getMoveObject()){
				objectMove = false;
			}
			i++;
		}

		float camSize = 0f;
		if (!objectMove) {
			camSize = 12f;
			this.camera.orthographicSize = Mathf.MoveTowards (this.camera.orthographicSize, camSize, 6.0f * Time.deltaTime);

			Vector3 target = new Vector3 (1f, 1f, -15f);
			
			Vector3 point = camera.WorldToViewportPoint(target);
			Vector3 delta = target - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, 0.1f);

		} else {
			camSize = (right - left) / 2f;
			if (camSize <= 2f) {
				camSize = 2f;
			}
			targetX = targetX / characters.Count;
			targetY = targetY / characters.Count;
			targetZ = targetZ / characters.Count;
			
			Vector3 target = new Vector3 (targetX, targetY, targetZ);
			
			Vector3 point = camera.WorldToViewportPoint(target);
			Vector3 delta = target - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, 0.1f);
		}


	}

	/*
	 * Used for network connection
	 */



}

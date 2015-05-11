﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class character : MonoBehaviour {

	public obstacle startObstacle;
	private obstacle moveToObstacle;



	//private List<string> newStatement;
	private string newStatement = "";
	//private List<obstacle> moveableObs;
	private Dictionary<string, obstacle> moveableObs;
	private List<Statement> statementList;

	private bool gameStart = false;
	private float posX;
	private float posY;

	private bool inputAble = true;
	private bool moveObject = false;

	private SerialConn sc;
	private LineRenderer lr;

	void Start(){
		this.statementList = new List<Statement> ();
		//this.moveableObs = new List<obstacle> ();
		this.moveableObs = new Dictionary<string, obstacle> ();
		this.moveToObstacle = startObstacle;

		lr = gameObject.AddComponent<LineRenderer>();
		lr.SetWidth ((float)0.05, (float)0.05);
		lr.material = new Material(Shader.Find("Sprites/Default"));
		lr.SetColors (Color.black, Color.white);
		lr.SetVertexCount (2);

		drawline (startObstacle.transform.position);
	}
	/*
	 * Connecting SerialConnection from global class
	 */
	public void setSerialConnection(SerialConn sc){
		this.sc = sc;
	}
	public bool getMoveObject(){
		return this.moveObject;
	}
	/*
	 * Set game start
	 */
	public void setGameStart(bool gameStart){
		this.gameStart = gameStart;
		this.moveObject = true;
		this.inputAble = false;
	}

	/* Players every frame */
	void FixedUpdate() {
		movePlayer (this.moveToObstacle);
	}

	/* Moves player based on provided next position */
	void movePlayer(obstacle obs){
		//gets next objects possition
		float xPos = obs.transform.position.x;
		float yPos = obs.transform.position.y;
		float speed = 2f;
		Vector3 newPos = new Vector3 (xPos, yPos, 0.0f);
		
		float step = speed * Time.deltaTime;

		if (moveObject == true) {
			transform.position = Vector3.MoveTowards(transform.position, newPos, step);
		}

		if (transform.position == newPos) {
			this.moveObject = false;
		}
	}

	/* Finds out when the next object is reached and gives movement options */
	void OnTriggerEnter2D(Collider2D coll){
		if (!coll.name.StartsWith ("C")) {
			//print ("bumped");

			if (coll.gameObject.name == "FINISH") {
				this.inputAble = false;
				send("GameFin");
			} else {
				this.inputAble = true;
				calMovement(moveToObstacle);
			}
		}
	}
	/*
	 * Calculate the movement base on where the character is.
	 */
	private void calMovement(obstacle obs){
		IEnumerable<char> c = "";
		string result = "";
		obstacle[] connObs = obs.getConnection ();

		foreach (Statement s in statementList) {
			if(s.getType().Equals("I")){
				string temp = s.getCondtion();
				if(temp.Contains(obs.getTag())){
					c = c.Union(s.getResult());
				}
			}else if(s.getType().Equals("L")){
				string str = s.getCondtion() + s.getResult();
				obstacle lresult = checkLoop (0,0,str,obs, null);
				if(lresult != null){
					moveableObs.Add("L",lresult);
					obs.highlight();
					send("L");
				}
			}
		}

		foreach (char ch in c) {
			result += ch;
		}
		foreach (obstacle o in connObs) {
			if(result.Contains(o.getTag())){
				moveableObs.Add(o.getTag(),o);
				o.highlight();
				send(o.getTag());
			}
		}
	}
	/*
	 * Check if the obstacle be able to use LOOP
	 */
	private obstacle checkLoop(int x, int y, string str, obstacle obs, obstacle final){
		obstacle finalObstacle = final;
		int current = x;
		int next = 0;
		if (current == str.Length - 1) {
			next = 0;
		} else {
			next = current + 1;
		}
		if(y > 0 && current == str.Length-1){
			finalObstacle = obs;
		}
		//print (str + ": " + current);
		if (obs.getTag ().Contains (str.ElementAt (current))) {

			foreach(obstacle o in obs.getConnection()){

				if(o.getTag().Contains(str.ElementAt(next))){
				
					if(current == str.Length -1) {

						return checkLoop (next, y+1, str, o, finalObstacle);
			
					} else {
					
						return checkLoop (next, y, str, o, finalObstacle);
					
					} 

				}

			}

			if(y > 0){
				return finalObstacle;
			}
		}
		return null;
	}

	/*
	 * Send data to user
	 */
	private void send(string command){
		string data = this.name+":"+command;
		this.sc.sendData (data);
	}
	/*
	 * Get data from user
	 */
	public void input(string command){
		if (inputAble) {
			print ("C1 get: " + command);
			if (!gameStart) {
				newStatement += command;
				//print (newStatement);
				if (newStatement.Contains("E")) {
					print (newStatement);
					addStatement (newStatement);
					newStatement = "";
				}
			} else {
				if(moveableObs.ContainsKey(command)){
					this.moveToObstacle = moveableObs[command];
					this.moveObject = true;
					this.inputAble = false;
					foreach(obstacle o in moveableObs.Values){
						o.disHighlight();
					}
					moveableObs.Clear();
				}
			}
		}
	}
	/*
	 * Determine LOOP or IF statement and trun the possibble movement into List
	 */
	private void addStatement(string statement){
		if (statement.StartsWith ("L")) {
			//LOOP condition
			string[] word = statement.Split('T');
			string condition = word[0].Substring(2);
			string result = "";
			for(int i = 1; i < word.Length; i++){
				if(i == word.Length-1){
					result += word[i].Substring(0,word[i].Length-1);
				}else{
					result += word[i];
				}
			}
			Statement temp = new Statement("L", analyzStatement(condition), analyzStatement(result));
			statementList.Add(temp);
		} else if (statement.StartsWith ("I")) {
			// IF condition
			string[] word = statement.Split('T');
			string condition = word[0].Substring(1);
			string result = word[1].Substring(0,word[1].Length-1);

			print("condition: "+condition);
			print ("result:" +result);

			Statement temp = new Statement("I", analyzStatement(condition), analyzStatement(result));
			statementList.Add(temp);
		}

		for(int i = 0; i < statementList.Count(); i++){
			if(statementList[i].getType() == "I"){
				print ("IF: " + statementList[i].getCondtion() + " then: " + statementList[i].getResult());
			}else if(statementList[i].getType() == "L"){
				print ("LOOP: " + statementList[i].getCondtion() + " then: " + statementList[i].getResult());
			}
		}
	}

	/*
	 * Anaylizing the possible movement of IF statment and output the color in a String format.
	 */
	private string analyzStatement(string statement){
		if (statement.Contains ("O")) {
			string[] word = statement.Split(new char[]{'O'},2);
			IEnumerable<char> c;
			if(statement.Contains("N")){
				c = analyzStatement(word[0]).Intersect(analyzStatement(word[1]));
			}else{
				c = analyzStatement(word[0]).Union(analyzStatement(word[1]));
			}

			string temp = "";

			foreach (char ch in c){
				temp += ch;
			}
			return temp;
		} else {
			string color = "";
			if(statement.StartsWith("N")){
				color = "RGBYP";
				color = color.Replace(statement.Substring(1,1),string.Empty);
			}else{
				color = statement;
			}
			return color;
		}
	}

	private void drawline(Vector3 position){
		lr.SetPosition(0,transform.position);
		lr.SetPosition(1,position);
	}

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class character : MonoBehaviour {

	public obstacle start;
	public obstacle firstObstacle;
	public obstacle finishObstacle;
	public obstacle goal;
	private obstacle moveToObstacle;
	private obstacle targetObatacle;

	private string newStatement = "";
	private Dictionary<string, obstacle> moveableObs;
	private List<Statement> statementList;
	private List<obstacle> sameColorList;
	private List<obstacle> loopPath;

	private bool gameStart = false;		//Flag to show if the game is started.
	private bool inputAble = true;		//Controller to albe input the data.
	private bool moveObject = false;	//Flag to show if the character is moving.
	private bool sameColor = false;		//Controller to trigger the same color condition.
	private bool planetTrigger = false;

	private SerialConn sc;
	private LineRenderer lr;

	private const int fixUpdateInterval = 50;
	private int fixedUpdate = 0;
	private float speed = 2.5f;
	private float axisZ = 1.7f; //Controller of the hieght of the ship.

	void Start(){
		this.statementList = new List<Statement> ();
		this.sameColorList = new List<obstacle> ();
		this.loopPath = new List<obstacle> ();
		this.moveableObs = new Dictionary<string, obstacle> ();
		this.moveToObstacle = firstObstacle;
		this.targetObatacle = firstObstacle;

		lr = gameObject.AddComponent<LineRenderer>();
		lr.SetWidth ((float)0.05, (float)0.05);
		lr.material = new Material(Shader.Find("Sprites/Default"));
		lr.SetColors (Color.black, Color.white);
		lr.SetVertexCount (2);

		drawline (firstObstacle.transform.position);

		this.transform.LookAt (firstObstacle.transform.position - new Vector3 (0f, 0f, axisZ));
		if(transform.position.x > moveToObstacle.transform.position.x){
			this.transform.Rotate (transform.rotation.x, transform.rotation.y, 180);
		}
		//this.transform.Rotate (transform.rotation.x, transform.rotation.y, 150);
		//this.transform.Rotate (transform.rotation.x, transform.rotation.y, 20);


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


		// shininegh color
		if (this.sameColor) {
			this.fixedUpdate++;
		}
		if (this.sameColor && fixedUpdate == fixUpdateInterval) {
			fixedUpdate = 0;
			for(int i = 0; i < sameColorList.Count; i++){
				if(moveableObs.ContainsKey(sameColorList[i].getTag())){
					moveableObs[sameColorList[i].getTag()].disHighlight();
					sameColorList[i].highlight();
					obstacle temp = moveableObs[sameColorList[i].getTag()];
					moveableObs[sameColorList[i].getTag()] = sameColorList[i];
					sameColorList[i] = temp;
				}
			}
		}
	}

	/* Moves player based on provided next position */
	void movePlayer(obstacle obs){
		//gets next objects possition
		float xPos = obs.transform.position.x;
		float yPos = obs.transform.position.y;
		Vector3 newPos = new Vector3 (xPos, yPos, -1.7f);
		
		float step = this.speed * Time.deltaTime;

		if (moveObject == true) {
			this.planetTrigger = false;
			transform.position = Vector3.MoveTowards (transform.position, newPos, step);
			transform.LookAt(this.moveToObstacle.transform.position - new Vector3(0f,0f,axisZ));
			if(transform.position.x > moveToObstacle.transform.position.x){
				this.transform.Rotate (transform.rotation.x, transform.rotation.y, 180);
			}
			this.speed += 0.05f;
		} 

		if (this.planetTrigger) {
			this.transform.position = this.moveToObstacle.transform.position - new Vector3(0f,0f,axisZ);
		}

		if (transform.position.x == newPos.x && transform.position.y == newPos.y) {
			this.moveObject = false;
			this.planetTrigger = true;
			this.speed = 2.5f;


			//this.transform.position.x = this.moveToObstacle.transform.position.x;
			//this.transform.position.y = this.moveToObstacle.transform.position.y;
		}
	}

	/* Finds out when the next object is reached and gives movement options */
	void OnTriggerEnter(Collider coll){

		if (!coll.name.StartsWith ("C")) {
			print ("hit the ship");
			if(coll.name == this.targetObatacle.getName()){
				if (coll.gameObject.name == this.finishObstacle.getName()){
					this.inputAble = false;
					this.moveToObstacle = goal;
					this.moveObject = true;
					print ("Ready to finish");
				}else if(coll.gameObject.name == this.goal.getName()){
					send("GameFin");
				}else{
					this.loopPath.Clear();
					//this.speed = 2.5f;
					this.inputAble = true;
					calMovement(moveToObstacle);
				}
			}else{
				int index = this.loopPath.IndexOf(this.moveToObstacle) + 1;
				if(index < this.loopPath.Count){
					this.moveToObstacle = this.loopPath.ElementAt(index);
				}
			}
			//this.planetTrigger = true;
			//this.transform.position = this.moveToObstacle.transform.position - new Vector3(0f,0f,1.5f);
			//this.transform.position.x = this.moveToObstacle.transform.position.x;
			//this.transform.position.y = this.moveToObstacle.transform.position.y;
		}
		/*
		if (!coll.name.StartsWith ("C")) {
			if (coll.gameObject.name == "FINISH") {
				this.inputAble = false;
				send("GameFin");
			} else {
				if(coll.name == this.targetObatacle.getName()){
					//this.travling = false;
					this.loopPath.Clear();
					this.speed = 2f;
					this.inputAble = true;
					calMovement(moveToObstacle);
				}else{
					int index = this.loopPath.IndexOf(this.moveToObstacle) + 1;
					if(index < this.loopPath.Count){
						this.moveToObstacle = this.loopPath.ElementAt(index);
					}
				}
			}
		}
		*/
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
					lresult.highlight();
					send("L");

					this.loopPath.IndexOf(lresult);
				}
			}
		}

		foreach (char ch in c) {
			result += ch;
		}

		foreach (obstacle o in connObs) {
			//print ("oname:" + o.getName());
			if(result.Contains(o.getTag())){
				if(moveableObs.ContainsKey(o.getTag())){
					//print ("test" + o.getTag());
					print (this.sameColor);
					this.sameColorList.Add(o);
					this.sameColor = true;
				}else{
					moveableObs.Add(o.getTag(),o);
					o.highlight();
					send(o.getTag());
				}

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

			this.loopPath.Add(obs);

			foreach(obstacle o in obs.getConnection()){

				if(o.getTag().Contains(str.ElementAt(next)) && !loopPath.Contains(o)){
			
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
			print ("charter get command: " + command);
			print(command.Length);
			if (!gameStart) {
				newStatement += command;
				if (newStatement.Contains("E")) {
					print (newStatement);
					addStatement (newStatement);
					newStatement = "";
				}
			} else {
				foreach(string s in moveableObs.Keys){
					print (s);
				}
				if(moveableObs.ContainsKey(command)){
					if(command.Contains("L")){
						this.moveToObstacle = this.loopPath.ElementAt(1);
						this.targetObatacle = moveableObs[command];
						this.speed = 8f;
					}else{
						this.moveToObstacle = moveableObs[command];
						this.targetObatacle = this.moveToObstacle;
					}
					this.moveObject = true;
					this.inputAble = false;
					foreach(obstacle o in moveableObs.Values){
						o.disHighlight();
					}
					this.sameColorList.Clear();
					this.sameColor = false;
					this.moveableObs.Clear();

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

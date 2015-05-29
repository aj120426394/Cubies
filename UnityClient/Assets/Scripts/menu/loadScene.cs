using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class loadScene : MonoBehaviour {
	
	//objects to load/change in scene
	public GameObject b, r, y, g, cube;
	public Text textField;
	public NetworkView nView;

	//which ships have been taken
	private bool blueShip, redShip, yellowShip, greenShip;

	void Start(){
		nView = GetComponent<NetworkView>();
		this.blueShip = true;
		this.redShip = true;
		this.yellowShip = true;
		this.greenShip = true;
	}
	
	void FixedUpdate(){

		if (Input.GetKeyDown(KeyCode.B ) && blueShip == true) {
			nView.RPC ("changeCubeColor", RPCMode.Others, "b");
			//changeCubeColor("b");
			//print ("print");
		}
		if (Input.GetKeyDown(KeyCode.R )&& redShip == true) {
			nView.RPC ("changeCubeColor", RPCMode.Others, "r");
		}
		if (Input.GetKeyDown(KeyCode.Y) && yellowShip == true) {
			nView.RPC ("changeCubeColor", RPCMode.Others, "y");
		}
		if (Input.GetKeyDown(KeyCode.G) && greenShip == true) {
			nView.RPC ("changeCubeColor", RPCMode.Others, "g");
		}
		if (Input.GetKeyDown(KeyCode.P)&&greenShip == true) {
			nView.RPC ("changeCubeColor", RPCMode.Others, "p");
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			nView.RPC ("switchScene", RPCMode.Others, "Screen");
		}
	}

	[RPC]
	private void changeCubeColor(string data){
		GameObject temp = null;
		
		if (data.Contains ("b") && blueShip == true) {
			print ("blue");
			temp = b;
			blueShip = false;
		}
		if (data.Contains ("r") && blueShip == true) {
			print ("red");
			temp = r;
			redShip = false;
		}
		if (data.Contains ("y") && blueShip == true) {
			print ("yellow");
			temp = y;
			yellowShip = false;
		}
		if (data.Contains ("g") && blueShip == true) {
			print ("green");
			temp = g;
			greenShip = false;
		}
		Instantiate (temp, cube.transform.position, cube.transform.rotation);
		Destroy (cube);
		textField.text = "Loading...";
	}
	[RPC]
	private void switchScene(string name){
		Application.LoadLevel(name);
	}
}
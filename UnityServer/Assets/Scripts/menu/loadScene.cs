using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class loadScene : MonoBehaviour {
	
	//objects to load/change in scene
	public GameObject b, r, y, g, cube;
	public Text textField;
	
	//which ships have been taken
	private bool blueShip, redShip, yellowShip, greenShip;

	void Start(){
		this.blueShip = true;
		this.redShip = true;
		this.yellowShip = true;
		this.greenShip = true;
	}
	
	void FixedUpdate(){

		if (Input.GetKeyDown(KeyCode.B ) && blueShip == true) {
			changeCubeColor("b");
			print ("print");
		}
		if (Input.GetKeyDown(KeyCode.R )&& redShip == true) {
			changeCubeColor("r");
		}
		if (Input.GetKeyDown(KeyCode.Y) && yellowShip == true) {
			changeCubeColor("y");
		}
		if (Input.GetKeyDown(KeyCode.G) && greenShip == true) {
			changeCubeColor("g");
		}
		if (Input.GetKeyDown(KeyCode.P)&&greenShip == true) {
			changeCubeColor("p");
		}
	}
	
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
}
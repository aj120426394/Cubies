using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class circle : MonoBehaviour {
	
	public float xPos;
	public float yPos;
	//public string name;

	void Start(){
		name = gameObject.name;
	}
	void Update() {
		xPos = transform.position.x;
		yPos = transform.position.y;
		//print (xPos);
		transform.position = new Vector3 (xPos, yPos, 0);
	}
	public string getName(){
		return name;
	}
	

}

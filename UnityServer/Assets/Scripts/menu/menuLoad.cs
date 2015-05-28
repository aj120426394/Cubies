using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class menuLoad : MonoBehaviour {

	public Camera camera;
	public Camera newCamera;
	private GameObject planet;
	private GameObject scale;
	
	public float speed = 10.0f;

	private float step;
	private float xPos;
	private float yPos;

	private Vector3 newScale;
	private Vector3 newPos;
	public bool move = false;

	void OnMouseOver(){
		print ("mouseover");
	}

	void Start(){
		newScale = scale.transform.localScale*2;

		step = speed * Time.deltaTime;
		//xPos = newCamera.transform.position.x;
		//yPos = newCamera.transform.position.y;

		newPos = new Vector3(xPos, yPos, 0.0f);

		this.planet = GameObject.Find("Menu Planet");
		//print (this.planet.name);
	}

	public void LoadLevel(int level){
		Application.LoadLevel (level);
		if (level == 2) {
			DontDestroyOnLoad (planet);

			move = true;
			print ("go to level 1");
		} else {
			Destroy (planet);
		}
	}
	
	void Update(){
		moveCamera ();
	}

	public void moveCamera(){


		if (move == true) {
			print ("move camera");

			scale.transform.localScale = Vector3.MoveTowards(scale.transform.localScale, newScale, step/2);

			//camera.transform.position = Vector3.MoveTowards(camera.transform.position, newPos, step);	
			//camera.orthographicSize = Mathf.MoveTowards (camera.orthographicSize, newCamera.orthographicSize, step);
			
		}
		//if(camera.transform.position == newPos){
		
			//print ("stop movment of cam");
			//move = false;
		//}
		if(scale.transform.localScale == newScale){
			
			print ("stop movment of cam");
			move = false;
		}
	}
}

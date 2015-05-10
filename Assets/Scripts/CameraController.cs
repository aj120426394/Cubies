using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float dampTime = 0.4f;
	//public float speed = 0.5f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	private bool movePlayer = false;

	

	void FixedUpdate () {
		//print (main.listPlayers[0]);

		//target = new GameObject(main.listPlayers[0]);

		if (Input.GetKeyDown (KeyCode.Space)) {
			movePlayer = true;
		}
		if (movePlayer) {
			move();
		}
	}
	
	void move (){
		//assigns variable camera
		Camera camera = GetComponent<Camera>();
		//scale of camera
		camera.orthographicSize = 3;
		//position of camera to equal target (player)
		Vector3 point = camera.WorldToViewportPoint(target.position);
		Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
		Vector3 destination = transform.position + delta;
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
	}
}

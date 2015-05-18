using UnityEngine;
using System.Collections;

public class rotater_z : MonoBehaviour {
	
	public float speed = 1.0f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(Time.deltaTime, speed, speed/2);
	}
}

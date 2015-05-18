using UnityEngine;
using System.Collections;

public class CircularMovementZ : MonoBehaviour  {
	
	public Transform center;
	public float degreesPerSecond = 12.0f;

	private float secondsPerOrbit;
	private Vector3 v;
	public float time;
	
	void Start() {
		v = transform.position - center.position;
		 time = 0.0f;
	}
	
	void Update () {
		secondsPerOrbit = 180/degreesPerSecond;

		double temp = 1*Mathf.Cos ((Mathf.PI*(Time.realtimeSinceStartup+secondsPerOrbit/2))/secondsPerOrbit)+1.2;
		float scale = (float)temp;
		//print (scale + "   TIME: " + Time.deltaTime);
		Vector3 scalepos = new Vector3(scale,scale,scale);

		v = Quaternion.AngleAxis (degreesPerSecond * Time.deltaTime, Vector3.up) * v;

		transform.localScale = scalepos;
		transform.position = center.position + v ;	

	}
}
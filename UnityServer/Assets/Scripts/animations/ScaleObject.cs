using UnityEngine;
using System.Collections;

public class ScaleObject : MonoBehaviour {

	public Transform center;
	public float intialScale = 0.25f;
	public float additionalScale = 0.5f;

	void Update () {
		double temp = intialScale*Mathf.Cos ((Mathf.PI*(Time.realtimeSinceStartup+1/1))/1)+additionalScale;
		float scale = (float)temp;
		Vector3 scalepos = new Vector3(scale,scale,scale);
		transform.localScale = scalepos;
	
	}
}

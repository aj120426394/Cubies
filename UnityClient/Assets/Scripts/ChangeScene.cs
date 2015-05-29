using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private void OnGUI(){
		if (GUILayout.Button ("Play Game")){
			Application.LoadLevel("Screen");
		}

	}
}

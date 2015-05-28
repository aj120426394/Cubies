using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class load : MonoBehaviour {

	void Start(){
		print ("start");
	}

	public void loadScene(int scene){
		print ("yo");
		Application.LoadLevel (scene);
	}
}

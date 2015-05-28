using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class load : MonoBehaviour {
	

	public void loadScene(int scene){
		print ("yo");
		Application.LoadLevel (scene);
	}
}

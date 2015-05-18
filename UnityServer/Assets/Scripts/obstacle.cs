using UnityEngine;
using System.Collections;

public class obstacle : MonoBehaviour {

	private string oName;
	private string oTag;
	private bool moveable;
	public obstacle[] obs;
	private LineRenderer lr;

	public Material mat;
	// Use this for initialization

	public void Start () {
		this.oName = this.name;
		this.oTag = this.tag;
		this.moveable = false;

		lr = gameObject.AddComponent<LineRenderer>();
		lr.SetWidth ((float)0.05, (float)0.05);
		lr.material = new Material(Shader.Find("Sprites/Default"));
		lr.material.color = Color.white;
		lr.SetColors (Color.white, Color.white);
		lr.SetVertexCount (obs.Length*2);


	}
	
	// Update is called once per frame
	void Update () {
		drawLine ();
	}

	public string getName(){
		return this.oName;
	}

	public string getTag(){
		return this.oTag;
	}
	public void setMoveable(bool move){
		this.moveable = move;
	}

	public bool getMoveable(){
		return this.moveable;
	}

	public void test(){
		foreach (obstacle o in obs) {
			print ((string)this.name + " connect to " + o.getName());
		}
	}

	public obstacle[] getConnection(){
		return this.obs;
	}

	public void highlight(){
		((Behaviour)GetComponent ("Halo")).enabled = true;
	}

	public void disHighlight(){
		((Behaviour)GetComponent ("Halo")).enabled = false;
	}

	private void drawLine(){
		int i = 0;
		foreach (obstacle o in obs) {
			lr.SetPosition (i, transform.position);
			lr.SetPosition (++i, o.transform.position);
			i++;
		}

	}

}


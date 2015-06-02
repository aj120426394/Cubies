using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenFeedback : MonoBehaviour {
	public string ipaddress;
	private string IP; //"192.168.0.10"
	private string ID = "";
	private string C1 = "10.89.188.73";
	private string C2 = "192.168.0.9";
	private string C3 = "192.168.0.8";
	private RectTransform inventoryRect;
	private float inventoryWidth, inventoryHeight;
	public int slots;
	public int rows;
	public Sprite loopSprite;
	public Sprite ifSprite;
	public Sprite thenSprite;
	public Sprite enterSprite;
	public Sprite orSprite;
	public Sprite notSprite;
	public Sprite redSprite;
	public Sprite blueSprite;
	public Sprite greenSprite;
	public Sprite yellowSprite;
	public Sprite purpleSprite;
	public float slotPaddingLeft, slotPaddingTop;
	public float slotSize;
	public GameObject slotPrefab;
	private List<GameObject> allSlots;
	public int slotCount = 0;

	public NetworkView nView;

	// Use this for initialization
	void Start () {
<<<<<<< HEAD

=======
>>>>>>> 25519f7c46e457e37f9a29f859f5aabf77d83a00
		IP = Network.player.ipAddress;
		if (IP == C1) {
			this.ID = "C1";
		} else if (IP == C2) {
			this.ID = "C2";
		} else if (IP == C3){
			this.ID = "C3";
		}
		nView = GetComponent<NetworkView>();
		allSlots = new List<GameObject> ();
		inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
		inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;
		inventoryRect = GetComponent<RectTransform>();
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, inventoryHeight);
		int columns = slots / rows;
		for (int y = 0; y < rows; y++){
			for (int x = 0; x < columns; x++){
				GameObject newSlot = (GameObject)Instantiate(slotPrefab);
				RectTransform slotRect = newSlot.GetComponent<RectTransform>();
				newSlot.name = "Slot";
				newSlot.transform.SetParent(this.transform.parent);
				slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
				allSlots.Add(newSlot);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			nView.RPC ("UpdateScreen", RPCMode.Others, "C1:I");
		}
		if (Input.GetKeyDown (KeyCode.T)) {
			nView.RPC ("UpdateScreen", RPCMode.Others, "C1:T");
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			nView.RPC ("UpdateScreen", RPCMode.Others, "C1:L");
		}
		if (Input.GetKeyDown (KeyCode.O)) {
			nView.RPC ("UpdateScreen", RPCMode.Others, "C1:O");
		}
		if (Input.GetKeyDown (KeyCode.N)) {
			nView.RPC ("UpdateScreen", RPCMode.Others, "C1:N");
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			nView.RPC ("UpdateScreen", RPCMode.Others, "C1:E");
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			nView.RPC ("UpdateScreen", RPCMode.Others, "C1:R");
		}
		if (Input.GetKeyDown (KeyCode.G)) {
			nView.RPC ("UpdateScreen", RPCMode.Others, "C1:G");
		}
		if (Input.GetKeyDown (KeyCode.B)) {
			nView.RPC ("UpdateScreen", RPCMode.Others, "C1:B");
		}
		if (Input.GetKeyDown (KeyCode.Y)) {
			nView.RPC ("UpdateScreen", RPCMode.Others, "C1:Y");
		}
		if (Input.GetKeyDown (KeyCode.P)) {
			nView.RPC ("UpdateScreen", RPCMode.Others, "C1:P");
		}
		
	}

	[RPC]
	public void UpdateScreen(string data){
		string[] spilt = data.Split(':');
		string to = spilt [0];
		string command = spilt [1];
		if (to.Equals (this.ID)) {
			printCommand(command);
			slotCount++;
		}

	}
	void printCommand(string command){
		switch (command) {
		case "I":
			allSlots[slotCount].GetComponent<UnityEngine.UI.Image>().sprite = ifSprite;
			break;
		case "T":
			allSlots[slotCount].GetComponent<UnityEngine.UI.Image>().sprite = thenSprite;
			break;
		case "L":
			allSlots[slotCount].GetComponent<UnityEngine.UI.Image>().sprite = loopSprite;
			break;
		case "O":
			allSlots[slotCount].GetComponent<UnityEngine.UI.Image>().sprite = orSprite;
			break;
		case "E":
			allSlots[slotCount].GetComponent<UnityEngine.UI.Image>().sprite = enterSprite;
			break;
		case "N":
			allSlots[slotCount].GetComponent<UnityEngine.UI.Image>().sprite = notSprite;
			break;
		case "R":
			allSlots[slotCount].GetComponent<UnityEngine.UI.Image>().sprite = redSprite;
			break;
		case "B":
			allSlots[slotCount].GetComponent<UnityEngine.UI.Image>().sprite = blueSprite;
			break;
		case "G":
			allSlots[slotCount].GetComponent<UnityEngine.UI.Image>().sprite = greenSprite;
			break;
		case "Y":
			allSlots[slotCount].GetComponent<UnityEngine.UI.Image>().sprite = yellowSprite;
			break;
		case "P":
			allSlots[slotCount].GetComponent<UnityEngine.UI.Image>().sprite = purpleSprite;
			break;
		}

	}
}


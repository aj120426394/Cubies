using UnityEngine;
using System.Collections;

public class NetworkConnection{
	private int Port = 25001;
	private NetworkView networkView;

	public NetworkConnection(NetworkView networkView){
		this.networkView = networkView;
	}

	public void OpenConnection(){
		if (Network.peerType == NetworkPeerType.Disconnected) {
			Network.InitializeServer(5,Port);
		}
	}

	public void CloseConnection(){
		if (!(Network.peerType == NetworkPeerType.Disconnected)) {
			Network.Disconnect(250);
		}
	}

	public void sendData(string str){
		this.networkView.RPC("ChangeColor", RPCMode.All, str);
	}
}

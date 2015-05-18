﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkConnection{
	private int Port = 25001;
	private NetworkView networkView;

	private Dictionary<string, string> client;

	public NetworkConnection(NetworkView networkView){
		this.networkView = networkView;
		this.client = new Dictionary<string, string> ();
		this.client.Add("C1", "192.168.0.1");
		this.client.Add("C2", "192.168.0.2");
		this.client.Add("C3", "192.168.0.3");
		this.client.Add("C4", "192.168.0.4");


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
		this.networkView.RPC("getData", RPCMode.All, str);
	}
}

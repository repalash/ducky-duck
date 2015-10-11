using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;

public class AccInput : MonoBehaviour {

	private string acc = "0 233";
	private int i = 0, h, w, f = 1;
	float comCall = 0, x=0, m=0;
	private Socket m_Socket;

	// Use this for initialization

	void Start () {
		Input.compass.enabled = true;
		Input.location.Start ();
		m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse("192.168.55.43");
		try{
			m_Socket.Connect(remoteIPAddress, 9126);
		}catch(Exception e){
			acc = e.Message;
			f=0;
		}
	}

	void OnApplicationQuit ()
	{
		m_Socket.Close();
		m_Socket = null;
	}

	public void Send(string msgData)
	{
		if (m_Socket == null||f==0)
			return;
		System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
		byte[] sendData = encoding.GetBytes(msgData);
		//byte[] prefix = new byte[1];
		//prefix[0] = (byte)sendData.Length;
		m_Socket.Send(sendData);
	}

	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 1) {
			Send ("Shoot|");
		}
		if (Input.touchCount > 1) 
		{
			x = Input.acceleration.x;
			acc = "X: " + x;
			m = Input.compass.trueHeading-180;
			m-=comCall;
			if(m<-180)m+=360;
			m/=180;
			m*=2;
			acc += "\nM: " + m;
			acc += "\nC: " + comCall;
		}
		if (Input.touchCount > 2) {
			comCall = Input.compass.trueHeading-180;
			acc+="\nCallibrated";
		}
		//if (i % 3 == 0) 
		{
			x = Input.acceleration.x;
			m = Input.compass.trueHeading-180;
			m-=comCall;
			if(m<-180)m+=360;
			m/=180;
			m*=2;
			Send (x+","+m+"|");
		}
	}
	void OnGUI(){
		GUIStyle style = new GUIStyle ();
		style.fontSize = 40;
		GUI.Label(new Rect(10, 10, 400, 400), acc, style);
	}
}

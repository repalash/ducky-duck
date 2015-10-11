using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;

public class AimScript : MonoBehaviour {
	float wid = 5*Screen.width/Screen.height, hei = 5, f=1;
	int hc = 0, vc = 0, shoot = 1;
	float lh, lv;
	string acc = "235 87";
	public GameObject paint; 
	// Use this for initialization
	private Socket m_Socket;
	System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding ();
	void Start () {
		m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse("192.168.55.43");
		try{
			m_Socket.Connect(remoteIPAddress, 9127);
		}catch(Exception e){
			acc = e.Message;
			f=0;
		}
	}
	// Update is called once per frame
	void Update () {
		if (vc == 0) {
			acc = "Callibrate Vertical";
		} else if (hc == 0) {
			acc = "Callibrate Horizontal";
		}
		byte[] rec  = new byte[255];
		int bytes = m_Socket.Receive(rec);
		//Debug.Log (bytes);
		if (bytes != 0) {
			string t = encoding.GetString (rec);
			if(t[0] == 'S'){
				shoot = 1;
				Debug.Log("Shoot");
				Instantiate(paint, transform.localPosition, transform.localRotation);
			}
			else {
				string[] e = t.Split(",".ToCharArray());
				lv = ((float.Parse(e[0]))) ;
				lh = ((float.Parse(e[1]))) ;
				transform.localPosition = new Vector2 (-1 * lh * wid, lv * hei);
				//Debug.Log(lv + ":" + lh);
			}
		}
	}

	void OnGUI(){
		GUIStyle style = new GUIStyle ();
		style.fontSize = 40;
		GUI.Label(new Rect(10, 10, 400, 400), acc, style);
	}
}

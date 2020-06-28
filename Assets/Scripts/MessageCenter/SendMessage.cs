using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SendNBA()
    { 
        //SendMessageCenter.SendMessage(E_MessageType.NBA);
    }

    public void SendOW()
    {
        //SendMessageCenter.SendMessage(E_MessageType.Overwatch);
    }
}

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;

public class ROSManager{
	private static ROSManager instance = null;

    private ROSBridgeWebSocketConnection ros = null;
    private Boolean lineOn;

    public static ROSManager getInstance(){
		if (instance == null) {
			instance = new ROSManager();
		}
		return instance;
	}

	private ROSManager(){
		init ();
	}

    private void init() {
        ros = new ROSBridgeWebSocketConnection("ws://134.197.87.18", 9090);
        ros.AddSubscriber(typeof(RobotImageSensor));
        ros.AddPublisher(typeof(RobotTeleop));
        ros.Connect();
        lineOn = true;
    }

    public void RemoteControl() { 
    //public void RemoteControl(Vector3Msg linear, Vector3Msg angular) {
        TwistMsg msg = new TwistMsg (new Vector3Msg(0.1, 0.2, 0.3), new Vector3Msg(-0.1, -0.2, -0.3));
        ros.Publish (RobotTeleop.GetMessageTopic (), msg);
    }

    public void ROSDisconnect()
    {
        if (ros != null)
            ros.Disconnect();
    }


}

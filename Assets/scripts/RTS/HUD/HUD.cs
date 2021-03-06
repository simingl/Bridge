﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using RTS;
using ProgressBar;

public class HUD : MonoBehaviour {
	public GUISkin resourceSkin, ordersSkin, selectBoxSkin, selectionBarSkin,selectBtnSkin;
	public Texture drone_2d, drone_2d_h;

	public Texture drone_cam_front, drone_cam_down;
	public Button cellBtn; 

    
	private const int RESOURCE_BAR_HEIGHT = 30;
	private const int LINE_HEIGHT = 18;

	private static int SELECT_BAR_BTN_HEIGHT = 30, SELECT_BAR_BTN_WIDTH = 60;
	private static int ACTION_BTN_WIDTH = 76 + 16, ACTION_BTN_HEIGHT = 30;
	private static int MARGIN = 50;

	private static int MINIMAP_WIDTH, MINIMAP_HEIGHT;
	private static int SELECTION_BAR_HEIGHT, SELECTION_BAR_WIDTH;
	private static int ORDERS_BAR_WIDTH, ORDERS_BAR_HEIGHT;
	private static int INFO_BAR_HEIGHT = SELECTION_BAR_HEIGHT, INFO_BAR_WIDHT ;

	private static int RESOURCE_DAYNIGHT_TOGGLE_WIDTH = 100;
	private static int RESOURCE_NAME_WIDTH = 100;
	private static int RESOURCE_LOCATION_WIDTH = 100;
	private static int RESOURCE_DRONE_HEIGHT_WIDTH = 100;
	private static int RESOURCE_DRONE_SPEED_WIDTH = 100;
	private static int RESOURCE_DRONE_ORIENT_WIDTH = 100;
	private static int RESOURCE_BATTERY_WIDTH = 100;
	private static int RESOURCE_STATUS_WIDTH = 100;
	private static int RESOURCE_WATER_WIDTH = 100;

	private const int PIP_BTN_WIDTH = 30;
	private const int PIP_BTN_HEIGHT = 30;

	private const int ROW_MAX = 6;

	private Player player;

	public bool dayNightToggle = true;

	private CursorState activeCursorState;

	//private GameObject sun;
    private bool initialCams;
	//For selection rendering
	public static Texture2D selectionHighlight = null;
    public static Texture2D droneCamera = null;
    public static Rect selection = new Rect(0,0,0,0);
	private Vector3 startClick = -Vector3.one;



	private Camera camera_minimap;


	void Start () {
        selectionHighlight = new Texture2D(128, 128);
        droneCamera = new Texture2D(128,128);

        int WIDTH = Screen.width;
		int HEIGHT = Screen.height;
        initialCams = true;

        MINIMAP_WIDTH =(int)(0.4*WIDTH);
		MINIMAP_HEIGHT = (int)(0.33 * HEIGHT);
		SELECTION_BAR_HEIGHT = (int)(0.16 * HEIGHT);
		SELECTION_BAR_WIDTH = (int)(0.29*WIDTH);
		ORDERS_BAR_WIDTH = (int)(0.151*WIDTH);
        ORDERS_BAR_HEIGHT = ORDERS_BAR_WIDTH;// (int)(0.15*HEIGHT);
		INFO_BAR_HEIGHT = ORDERS_BAR_HEIGHT;
		INFO_BAR_WIDHT = (int)(0.20*WIDTH) ;

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		//sun = GameObject.FindGameObjectWithTag ("Sun");

		RESOURCE_DAYNIGHT_TOGGLE_WIDTH = (int)(0.3*WIDTH);;
		RESOURCE_NAME_WIDTH = (int)(0.1*WIDTH);;
		RESOURCE_LOCATION_WIDTH = (int)(0.1*WIDTH);
		RESOURCE_DRONE_HEIGHT_WIDTH = (int)(0.1*WIDTH);	
		RESOURCE_DRONE_SPEED_WIDTH = (int)(0.1*WIDTH);;
		RESOURCE_DRONE_ORIENT_WIDTH = (int)(0.1*WIDTH);;
		RESOURCE_BATTERY_WIDTH = (int)(0.1*WIDTH);
		RESOURCE_STATUS_WIDTH = (int)(0.1*WIDTH);

		ResourceManager.StoreSelectBoxItems(selectBoxSkin);
    }


    void OnGUI () {
		if(true || player && player.human) {
			if(ConfigManager.getInstance ().getHUDShowDroneSelectionBar()){
				//DrawSelectionBar();
			}
			if(ConfigManager.getInstance ().getHUDShowOrderBar()){
				DrawOrdersBar();
			}
			if(ConfigManager.getInstance ().getHUDShowResourceBar()){
				//DrawResourceBar();
			}
            if (ConfigManager.getInstance().getHUDShowMessageBar())
            {
                DrawInfoBar();
            }

			//DrawPIPBar();
			//DrawMouseDragSelectionBox ();
		}
	}

	void Update(){
		MouseDragSelection();
	}

	private void DrawResourceBar() {
		GUI.skin = resourceSkin;
		GUI.BeginGroup(new Rect(0,0,Screen.width,RESOURCE_BAR_HEIGHT));

		int offset = 0;

		offset += RESOURCE_DAYNIGHT_TOGGLE_WIDTH;
		if (true||player.getSelectedObjects().Count >0) {

			GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.9f);
			GUI.Box(new Rect(0,0,Screen.width, RESOURCE_BAR_HEIGHT),"");

            WorldObject obj = player.GetComponentInParent<Drone>();
            string name = obj.objectName; 
			string status = "Status: ";
			string location = "Location: (" + obj.transform.position.x.ToString("0.0") + ", " + obj.transform.position.z.ToString("0.0") + ")";
			string height = "Height: "+ obj.transform.position.y.ToString("0.0");
			string speed  = "Speed: ";

			float angle = 0.0F;
			Vector3 axis = Vector3.zero;
			obj.transform.rotation.ToAngleAxis(out angle, out axis);
			string orient = "Orientation: " + angle.ToString("0.0")+"'";
			string battery = "";
			if (obj is Drone) {
				Drone unit = (Drone)obj;
				battery += "Battery: " + (int)(unit.currentBattery)/60+" min " + ((int)unit.currentBattery)%60 + " sec.";
				status += unit.currentStatus;
				speed  += unit.speed.ToString("0.0");
			}
			GUI.DrawTexture(new Rect(offset, 5 ,40,20), this.drone_2d);
			offset +=40;
	//		GUI.Label(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), name);
			//DrawOutline(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), name, this.resourceSkin.GetStyle("large"), Color.white, Color.black);
			offset += RESOURCE_NAME_WIDTH;

			GUI.Label(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), location);
			//DrawOutline(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), location, this.resourceSkin.GetStyle("large"), Color.white, Color.black);
			offset += RESOURCE_LOCATION_WIDTH;

			GUI.Label(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), height);
			//DrawOutline(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), height, this.resourceSkin.GetStyle("large"), Color.white, Color.black);
			offset += RESOURCE_DRONE_HEIGHT_WIDTH;

			GUI.Label(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), speed);
			//DrawOutline(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), height, this.resourceSkin.GetStyle("large"), Color.white, Color.black);
			offset += RESOURCE_DRONE_SPEED_WIDTH;

			GUI.Label(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), orient);
			//DrawOutline(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), height, this.resourceSkin.GetStyle("large"), Color.white, Color.black);
			offset += RESOURCE_DRONE_ORIENT_WIDTH;

			GUI.Label(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), battery);
			//DrawOutline(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), battery, this.resourceSkin.GetStyle("large"), Color.yellow, Color.green);
			offset += RESOURCE_BATTERY_WIDTH;

			GUI.Label(new Rect(offset,5,Screen.width,ORDERS_BAR_HEIGHT), status);
			//DrawOutline(new Rect(offset,5,Screen.width,ORDERS_BAR_HEIGHT), cellinfo, this.resourceSkin.GetStyle("large"), Color.yellow, Color.green);
			//DrawOutline(new Rect(offset,5,Screen.width,ORDERS_BAR_HEIGHT), waterinfo, this.resourceSkin.GetStyle("large"), Color.yellow, Color.green);
		}
		//dayNightToggle = GUI.Toggle(new Rect(5, 5, RESOURCE_DAYNIGHT_TOGGLE_WIDTH, RESOURCE_BAR_HEIGHT), dayNightToggle, "Day/Night");
		GUI.EndGroup();
	}

	private void DrawSelectionBar() {
        GUI.skin = this.selectionBarSkin;
        GUI.BeginGroup(new Rect(0, Screen.height - SELECTION_BAR_HEIGHT, Screen.width, SELECTION_BAR_HEIGHT));
        GUI.Box(new Rect(MINIMAP_WIDTH, 0, SELECTION_BAR_WIDTH, SELECTION_BAR_HEIGHT), "");
        GUI.EndGroup();

        Drone[] allEntities = this.player.sceneManager.getAllDrones();
        if (allEntities.Length > 0)
        {
            for (int i = 0; i < allEntities.Length; i++)
            {
         //       GUI.skin = selectBtnSkin;
                Drone obj = allEntities[i];

                GUI.color = obj.color;

                int row = i / ROW_MAX;
                int col = i - row * ROW_MAX;

                if (obj.isSelected())
                {
                    if (GUI.Button(new Rect(MINIMAP_WIDTH + col * SELECT_BAR_BTN_WIDTH, Screen.height - SELECTION_BAR_HEIGHT + row * SELECT_BAR_BTN_HEIGHT, SELECT_BAR_BTN_WIDTH, SELECT_BAR_BTN_HEIGHT), drone_2d_h))
                    {
                        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                        {
                            this.player.toggleSelectObject(obj);
                        }
                        else {
                            this.player.setSelectedObject(obj);
                            obj.centerMainCamera();
                        }
                    }
                }
                else {
                    if (GUI.Button(new Rect(MINIMAP_WIDTH + col * SELECT_BAR_BTN_WIDTH, Screen.height - SELECTION_BAR_HEIGHT + row * SELECT_BAR_BTN_HEIGHT, SELECT_BAR_BTN_WIDTH, SELECT_BAR_BTN_HEIGHT), drone_2d) && obj.isSelectable())
                    {
                        if (Input.GetKey(KeyCode.LeftControl))
                        {
                            this.player.toggleSelectObject(obj);
                        }
                        else {
                            this.player.setSelectedObject(obj);
                            obj.centerMainCamera();
                        }
                    }
                }
            }
        }
    }
	private void DrawInfoBar(){
        int offset_width = MINIMAP_WIDTH;
        int offset_height = 0;

        GUI.skin = ordersSkin;
        GUI.BeginGroup(new Rect(0, Screen.height - INFO_BAR_HEIGHT, Screen.width, INFO_BAR_HEIGHT));
        GUI.Box(new Rect(MINIMAP_WIDTH, 0, INFO_BAR_WIDHT, INFO_BAR_HEIGHT), "");

        WorldObject obj = player.GetComponentInParent<Drone>();
        string name = obj.objectName;
        string status = "Status: ";
        string location = "Location: (" + obj.transform.position.x.ToString("0.0") + ", " + obj.transform.position.z.ToString("0.0") + ")";
        string height = "Height: " + obj.transform.position.y.ToString("0.0");
        string speed = "Speed: ";

        float angle = 0.0F;
        Vector3 axis = Vector3.zero;
        obj.transform.rotation.ToAngleAxis(out angle, out axis);
        string orient = "Orientation: " + angle.ToString("0.0") + "'";
        string battery = "";
        if (obj is Drone)
        {
            Drone unit = (Drone)obj;
            battery += "Battery: " + (int)(unit.currentBattery) / 60 + " min " + ((int)unit.currentBattery) % 60 + " sec.";
            status += unit.currentStatus;
            speed += unit.speed.ToString("0.0");
        }

        //		GUI.Label(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), name);
        //DrawOutline(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), name, this.resourceSkin.GetStyle("large"), Color.white, Color.black);
        offset_width += 10;
        offset_height += 5;

        GUI.Label(new Rect(offset_width, offset_height, Screen.width, ORDERS_BAR_HEIGHT), location);
        //DrawOutline(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), location, this.resourceSkin.GetStyle("large"), Color.white, Color.black);
        //offset_width += RESOURCE_LOCATION_WIDTH;
        offset_height += LINE_HEIGHT;

        GUI.Label(new Rect(offset_width, offset_height, Screen.width, ORDERS_BAR_HEIGHT), height);
        //DrawOutline(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), height, this.resourceSkin.GetStyle("large"), Color.white, Color.black);
        
        offset_height += LINE_HEIGHT;

        GUI.Label(new Rect(offset_width, offset_height, Screen.width, ORDERS_BAR_HEIGHT), speed);
        //DrawOutline(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), height, this.resourceSkin.GetStyle("large"), Color.white, Color.black);
        
        offset_height += LINE_HEIGHT;

        GUI.Label(new Rect(offset_width, offset_height, Screen.width, ORDERS_BAR_HEIGHT), orient);
        //DrawOutline(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), height, this.resourceSkin.GetStyle("large"), Color.white, Color.black);
        
        offset_height += LINE_HEIGHT;

        GUI.Label(new Rect(offset_width, offset_height, Screen.width, ORDERS_BAR_HEIGHT), battery);
        //DrawOutline(new Rect(offset,5 , Screen.width,ORDERS_BAR_HEIGHT), battery, this.resourceSkin.GetStyle("large"), Color.yellow, Color.green);
        
        offset_height += LINE_HEIGHT;

        GUI.Label(new Rect(offset_width, offset_height, Screen.width, ORDERS_BAR_HEIGHT), status);

        
        GUI.EndGroup();


    }

    
    private void DrawOrdersBar()
    {
        int offset = MINIMAP_WIDTH + INFO_BAR_WIDHT;

        GUI.BeginGroup(new Rect(0, Screen.height - ORDERS_BAR_HEIGHT, Screen.width * 0.8f, ORDERS_BAR_HEIGHT*2));
        GUI.Box(new Rect(offset, 0, ORDERS_BAR_WIDTH, ORDERS_BAR_HEIGHT*2), "");
        GUI.EndGroup();
        Rect ubdcam   = new Rect(offset, Screen.height - ORDERS_BAR_HEIGHT*2-5, ORDERS_BAR_WIDTH, ORDERS_BAR_HEIGHT);
        Rect dronecam = new Rect(offset, Screen.height - ORDERS_BAR_HEIGHT, ORDERS_BAR_WIDTH, ORDERS_BAR_HEIGHT);
        if (selectionHighlight != null) {
            GUI.DrawTexture(ubdcam, selectionHighlight);
        }
        if (droneCamera != null)
        {
            GUI.DrawTexture(dronecam, droneCamera);
        }
    }

    private void DrawOrdersBar_bak() {
        int offset = MINIMAP_WIDTH  + INFO_BAR_WIDHT;

        GUI.color = Color.white;
        GUI.skin = ordersSkin;
        GUI.BeginGroup(new Rect(0, Screen.height - ORDERS_BAR_HEIGHT, Screen.width * 0.8f, ORDERS_BAR_HEIGHT));

        int orderWidth = Screen.width;
        //if (player.isPIPActive())
        //{
        //    orderWidth = ORDERS_BAR_WIDTH;
        //}

        GUI.Box(new Rect(offset, 0, ORDERS_BAR_WIDTH, ORDERS_BAR_HEIGHT), "");

        GUI.EndGroup();
        //if (player.getSelectedObjects().Count > 0)
        //{
        //List<WorldObject> objs = player.getSelectedObjects();
        for (int i = 0; i < 1; i++)
            {
                WorldObject obj = player.GetComponentInParent<Drone>();
                string text = obj.objectName;
                text += "  - loc: (" + obj.transform.position.x + ", " + obj.transform.position.z + "), H: " + obj.transform.position.y;
                if (obj is Drone)
                {
                    Drone unit = (Drone)obj;
                    text += " - battery: " + unit.currentBattery;
                }
                if (GUI.Button(new Rect(offset + 5, Screen.height - ORDERS_BAR_HEIGHT + i * LINE_HEIGHT + 5, ACTION_BTN_WIDTH, ACTION_BTN_HEIGHT), "Landing"))
                {
                    Drone unit = (Drone)obj;
                    //unit.LoadCell();
                    unit.Land();
                }
                if (GUI.Button(new Rect(offset + 5 , Screen.height - ORDERS_BAR_HEIGHT + i * LINE_HEIGHT + 5 + ACTION_BTN_HEIGHT +5, ACTION_BTN_WIDTH, ACTION_BTN_HEIGHT), "Take Off"))
                {
                    Drone unit = (Drone)obj;
                    unit.TakeOff();
                }

                if (GUI.Button(new Rect(offset + 5, Screen.height - ORDERS_BAR_HEIGHT + i * LINE_HEIGHT + 5 + ACTION_BTN_HEIGHT + 5+ ACTION_BTN_HEIGHT +5, ACTION_BTN_WIDTH, ACTION_BTN_HEIGHT), "Recharge"))
                {
                    Drone unit = (Drone)obj;
                    unit.Recharge();
                }
            }
        
        //}
    }

    public bool MouseInBounds() {
		//Screen coordinates start in the lower-left corner of the screen
		//not the top-left of the screen like the drawing coordinates do
		Vector3 mousePos = Input.mousePosition;
		bool insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width;
		bool insideHeight = mousePos.y >= SELECTION_BAR_HEIGHT && mousePos.y <= Screen.height - RESOURCE_BAR_HEIGHT;
		//bool insideHeight = mousePos.y >= 0 && mousePos.y <= Screen.height;
		bool insideMinimap = this.MouseInBoundsMinimap ();
		bool insideOrderBar = this.MouseInBoundsOrderBar ();
		bool insidePIPCamera = this.MouseInBoundsPIP ();
		bool inBounds = insideWidth && insideHeight && !insideMinimap && !insideOrderBar && !insidePIPCamera;

		return inBounds;
	}

	public bool MouseInBoundsMinimap(){
		Vector3 mousePos = Input.mousePosition;
		bool insideWidth = mousePos.x >= 0 && mousePos.x <= MINIMAP_WIDTH;
		bool insideHeight = mousePos.y >= 0 && mousePos.y < MINIMAP_HEIGHT;
		return insideWidth && insideHeight;
	}

	private bool MouseInBoundsOrderBar(){
		Vector3 mousePos = Input.mousePosition;
		bool insideWidth = (mousePos.x >= MINIMAP_WIDTH+ SELECTION_BAR_WIDTH + INFO_BAR_WIDHT) && mousePos.x <= Screen.width;
		bool insideHeight = mousePos.y >= 0 && mousePos.y <ORDERS_BAR_HEIGHT;
		return insideWidth && insideHeight;
	}

	public bool MouseInBoundsPIP(){
		Vector3 mousePos = Input.mousePosition;
		bool insideWidth = (mousePos.x >= MINIMAP_WIDTH+ SELECTION_BAR_WIDTH + INFO_BAR_WIDHT+ORDERS_BAR_WIDTH) && mousePos.x <= Screen.width;
		bool insideHeight = mousePos.y >= 0 && mousePos.y < MINIMAP_HEIGHT;
		return insideWidth && insideHeight;
	}


	public Rect GetPlayingArea() {
		return new Rect(0, RESOURCE_BAR_HEIGHT, Screen.width - ORDERS_BAR_HEIGHT, Screen.height - RESOURCE_BAR_HEIGHT);
	}

	private void SwitchDayNight(){
		//sun.SetActive(this.dayNightToggle);
	}

	//Render Selection
	private void DrawMouseDragSelectionBox(){
        if (startClick != -Vector3.one)
        {
            GUI.color = new Color(1, 1, 1, 0.5f);
            GUI.DrawTexture(selection, selectionHighlight);
        }
    }

	private void MouseDragSelection(){
        if (Input.GetMouseButtonDown(0) && !MouseInBoundsMinimap())
        {
            startClick = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startClick = -Vector3.one;
        }

        if (Input.GetMouseButton(0) && startClick != -Vector3.one)
        {
            selection = new Rect(startClick.x, InvertMouseY(startClick.y), Input.mousePosition.x - startClick.x, InvertMouseY(Input.mousePosition.y) - InvertMouseY(startClick.y));
            if (selection.width < 0)
            {
                selection.x += selection.width;
                selection.width = -selection.width;
            }
            if (selection.height < 0)
            {
                selection.y += selection.height;
                selection.height = -selection.height;
            }
        }
    }

	private void DrawPIPBar(){
        if (player.getSelectedObjects().Count > 0)
        {
            WorldObject wo = player.getSelectedObjects()[0];
            if (wo is Drone)
            {
                Drone drone = (Drone)wo;
                int offset_w = MINIMAP_WIDTH + ORDERS_BAR_WIDTH + INFO_BAR_WIDHT + SELECTION_BAR_WIDTH + 3;
                int offset_h = Screen.height - MINIMAP_HEIGHT;

                if (drone.getCameraFront().depth == Drone.PIP_DEPTH_ACTIVE)
                {
                    if (GUI.Button(new Rect(offset_w, offset_h, PIP_BTN_WIDTH, PIP_BTN_HEIGHT), drone_cam_front))
                    {
                        drone.togglePIPCamera();
                    }
                }
                else if (drone.getCameraDown().depth == Drone.PIP_DEPTH_ACTIVE)
                {
                    if (GUI.Button(new Rect(offset_w, offset_h, PIP_BTN_WIDTH, PIP_BTN_HEIGHT), drone_cam_down))
                    {
                        drone.togglePIPCamera();
                    }
                }
            }
        }
    }

	public static float InvertMouseY(float y){
		return Screen.height - y;
	}

	public static void DrawOutline(Rect position, string text, GUIStyle style, Color outColor, Color inColor){
        GUIStyle backupStyle = style;
        style.normal.textColor = outColor;
        position.x--;
        GUI.Label(position, text, style);
        position.x += 2;
        GUI.Label(position, text, style);
        position.x--;
        position.y--;
        GUI.Label(position, text, style);
        position.y += 2;
        GUI.Label(position, text, style);
        position.y--;
        style.normal.textColor = inColor;
        GUI.Label(position, text, style);
        style = backupStyle;
    }

}

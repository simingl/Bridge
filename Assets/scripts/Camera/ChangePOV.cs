using UnityEngine;
using System.Collections;
using RTS;
//short-cut-key
public class ChangePOV : MonoBehaviour {
	
	public Camera activeCamera;
	public Camera camMain;
	private Player player;
	private Vector3 camMainPosition;
	private Quaternion camMainRotation;
	private Quaternion inValidQuaternion = new Quaternion(0f, 0f, 0f, 1f);
    private SceneManager sceneManager;
    private HUD hud;
    public int worldSimulationSpeedVar;
        // Use this for initialization
    void Start () {
        player = GetComponent<Player> ();
		camMain = Camera.main;
		this.camMainPosition=Vector3.zero;
		this.camMainRotation=this.inValidQuaternion;
        sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>();
        hud = player.GetComponentInChildren<HUD>();
        worldSimulationSpeedVar = 1;

    }

	public void switchCamera(RTS.CameraType type){
		if (type == RTS.CameraType.Camera_Main) {
			if (activeCamera != null) {
				this.activeCamera = null;
				this.camMain.transform.position = this.camMainPosition;
				this.camMain.transform.rotation = this.camMainRotation;
				this.camMainPosition = Vector3.zero;
				this.camMainRotation = this.inValidQuaternion;
			}
		} else {
			this.backupMainCameraPosition();
			this.activeCamera = this.getActiveCamera(type);
		}
	}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            this.switchCamera(RTS.CameraType.Camera_First_View);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            this.switchCamera(RTS.CameraType.Camera_Third_View);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            this.switchCamera(RTS.CameraType.Camera_Hover_View);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            this.switchCamera(RTS.CameraType.Camera_Main);
        }

        if (this.activeCamera)
            this.copyCameraPosition(camMain, this.activeCamera);

        
        MainCameraMove();               //short-cut-key AWSD for RTS camera movement
        DroneShortCutKey();             //0-9 select first 10 in n.
        ShortCutKeyShowAllCameras();
        ShortCutKeyClearAllCameras();
        BlurSelectedDronesCameras();
        ClearAllDronesBlurCameras();    
        WorldSimulationSpeedFunc();     // keypad + for simulation speed up, - for simulation speed down
        DroneDied();                    // make drone die; for test;
    }

    private void DroneShortCutKey()
    {
        int droneCounter = sceneManager.getAllDrones().Length;
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (droneCounter != 0)
            {
                player.setSelectedObject(sceneManager.getAllDrones()[0]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (droneCounter > 1)
            {
                player.setSelectedObject(sceneManager.getAllDrones()[1]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (droneCounter > 2)
            {
                player.setSelectedObject(sceneManager.getAllDrones()[2]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (droneCounter > 3)
            {
                player.setSelectedObject(sceneManager.getAllDrones()[3]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (droneCounter > 4)
            {
                player.setSelectedObject(sceneManager.getAllDrones()[4]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (droneCounter > 5)
            {
                player.setSelectedObject(sceneManager.getAllDrones()[5]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (droneCounter > 6)
            {
                player.setSelectedObject(sceneManager.getAllDrones()[6]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (droneCounter > 7)
            {
                player.setSelectedObject(sceneManager.getAllDrones()[7]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (droneCounter > 8)
            {
                player.setSelectedObject(sceneManager.getAllDrones()[8]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (droneCounter > 9)
            {
                player.setSelectedObject(sceneManager.getAllDrones()[9]);
            }
        }
    }

    private void ShortCutKeyShowAllCameras()
    {
        if(Input.GetKeyDown(KeyCode.F5))
        {
            hud.ShowAllCameras();
        }
    }

    private void ShortCutKeyClearAllCameras()
    {
        if (Input.GetKeyDown(KeyCode.F6))
        {
            hud.ClearAllCameras();
        }
    }

    private void BlurSelectedDronesCameras()
    {
        if (Input.GetKeyDown(KeyCode.F7))
        {
            foreach (Drone drone in sceneManager.getAllDrones())
            {
                if (drone.isSelected())
                {
                    drone.malFunctionCameraEnable();
                }
            }
        }
    }

    private void DroneDied()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {            
            foreach(Drone drone in sceneManager.getAllDrones())
            {                
                if(drone.isSelected())
                {
                    Debug.Log("Dieing func is called");
                    drone.Dieing();
                }
            }
        }
    }

    private void ClearAllDronesBlurCameras()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            foreach (Drone drone in sceneManager.getAllDrones())
            {
                drone.malFunctionCameraClear();
            }
        }
    }

    private void WorldSimulationSpeedFunc()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (worldSimulationSpeedVar > 1)
            {
                worldSimulationSpeedVar /= 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if(worldSimulationSpeedVar < 64)
            {
                worldSimulationSpeedVar *= 2;
            }            
        }
    }

    private void MainCameraMove()
    {
        Vector3 movement = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.J))
        {
            movement.x -= ResourceManager.ScrollSpeed;
        }
        else if (Input.GetKey(KeyCode.L))
        {
            movement.x += ResourceManager.ScrollSpeed;
        }

        if (Input.GetKey(KeyCode.K))
        {
            movement.z -= ResourceManager.ScrollSpeed;
        }
        else if (Input.GetKey(KeyCode.I))
        {
            movement.z += ResourceManager.ScrollSpeed;
        }
        //away from ground movement
        Camera.main.orthographicSize -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");
        float minCameraSize = ResourceManager.MinCameraSize;
        float maxCameraSize = ResourceManager.MaxCameraSize;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minCameraSize, maxCameraSize);
        //movement += Camera.main.transform.forward * ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");;

        //calculate desired camera position based on received input
        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = origin;
        destination.x += movement.x;
        destination.z += movement.z;

        //limit away from ground movement to be between a minimum and maximum distance
        destination.x = Mathf.Clamp(destination.x, ResourceManager.MinCameraWidth, ResourceManager.MaxCameraWidth);
        destination.z = Mathf.Clamp(destination.z, ResourceManager.MinCameraLength, ResourceManager.MaxCameraLength);

        //if a change in position is detected perform the necessary update
        if (destination != origin)
        {
            Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed * 10);
        }

        Camera.main.GetComponent<CameraMain>().ClampCam();

    }

    private void copyCameraPosition(Camera main, Camera target){
		main.gameObject.transform.position = target.gameObject.transform.position;
		main.gameObject.transform.rotation = target.gameObject.transform.rotation;
	}

	private Camera getActiveCamera(RTS.CameraType ct){
		if (player.getSelectedObjects ().Count > 0) {
			WorldObject obj = player.getSelectedObjects () [0];
			Camera[] cameras = obj.gameObject.GetComponentsInChildren<Camera> ();
			foreach (Camera cam in cameras) {
				if (ct == RTS.CameraType.Camera_First_View && cam.tag == RTS.Tags.CAM_FIRST_VIEW) { 
					return cam;
				} else if (ct == RTS.CameraType.Camera_Hover_View && cam.tag == RTS.Tags.CAM_HOVER_VIEW) {
					return cam;
				}
			}
		}
		return null;
	}

	private void backupMainCameraPosition(){
		if (camMainPosition == Vector3.zero) {
			this.camMainPosition = camMain.transform.position;
		}
		float angle = Quaternion.Angle(camMainRotation, inValidQuaternion);
		if (angle == 0) {
			this.camMainRotation = camMain.transform.rotation;
		}
	}	
}

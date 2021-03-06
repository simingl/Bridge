﻿using UnityEngine;
using System.Collections;
using RTS;

public class WorldObject : MonoBehaviour {
	public WorldObjectType type;
	public int scoreValue = 1000;
	public string objectName;

	protected Player player;
	public enum STATUS {LANDED, IDLE, TAKEOFF, LANDING, CRASHING, MOVING, DEAD, CHARGING};
	public enum TASK {NULL, RECHARGING, CRASH, ROUTE}

	protected Bounds selectionBounds;

	protected Rect playingArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

	protected bool _isSelectable;


	public STATUS currentStatus;
	public TASK currentTask;

	private Vector3 lastPosition = Vector3.zero;
	private float lastUpdated = 0.0f;


    protected virtual void Awake() {
		currentStatus = STATUS.MOVING;
		currentTask = TASK.NULL;

		selectionBounds = ResourceManager.InvalidBounds;
		CalculateBounds();
    }

    protected virtual void Start () {
		this._isSelectable = true;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
		//this.playingArea = player.hud.GetPlayingArea ();
	}
	
	protected virtual void Update () {

    }



    protected virtual void OnGUI() {
		if (this.isSelected()) {
			DrawSelection ();
		}
	}

    public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller)
    {
        //ignore non-selectable objects
        if (!this._isSelectable)
        {
            player.cleanSelectedObject();
            return;
        }
    }

    public virtual void StopMove(){

	}

	public void DrawSelection() {
        GUI.skin = ResourceManager.SelectBoxSkin;
        Rect selectBox = WorkManager.CalculateSelectionBox(selectionBounds, playingArea);
        GUI.BeginGroup(playingArea);
        DrawSelectionBox(selectBox);
        GUI.EndGroup();
    }

   

    public void CalculateBounds() {
		selectionBounds = new Bounds(transform.position, Vector3.zero);
		foreach(Renderer r in GetComponentsInChildren< Renderer >()) {
			if(r.GetType().Name == "MeshRenderer" && r.ToString().IndexOf("group_top")>=0){
				selectionBounds.Encapsulate(r.bounds);
			}
		}
	}

	protected virtual void DrawSelectionBox(Rect selectBox) {
		GUI.Box(selectBox, "");
	}

	public bool isSelected(){
        return false;
		//return player.isSelected (this);
	}

	public void centerMainCamera(){
		Vector3 camPos = this.transform.position;
		camPos.y = Camera.main.transform.position.y;
		Camera.main.transform.position = camPos;
	}

	public bool isSelectable(){
		return _isSelectable;
	}

	public void OnCollisionExit(Collision collisionInfo){
		this.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
	} 

	public void SetLayerRecursively( GameObject obj, int oldLayer, int newLayer )
	{
		if (obj.layer == oldLayer) {
			obj.layer = newLayer;
		}
		foreach(Transform child in obj.transform )
		{
			SetLayerRecursively( child.gameObject, oldLayer, newLayer );
		}
	}

	public Vector3 FindHitPoint(Camera cam, Transform tr) {
		Ray ray = cam.ScreenPointToRay(tr.position);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)) return hit.point;
		return ResourceManager.InvalidPosition;
	}
	public bool isDead(){
		return this.currentStatus == STATUS.DEAD;
	}
    
}

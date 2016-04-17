using UnityEngine;
using System.Collections;

public class UBDMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //transform.Translate( 0, 0, 0.05f * Time.deltaTime);
        transform.Translate(Vector3.left * Time.deltaTime * 0.05f);
    }
}

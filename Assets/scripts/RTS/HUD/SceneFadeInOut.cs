using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour {

	public float fadeSpeed = 1.5f;
	private bool sceneStarting = true;

	private Image image;

    //private ChangePOV changePOV;
	void Awake(){
		image = this.GetComponent<Image> ();
		image.rectTransform.sizeDelta = new Vector2( Screen.width, Screen.height);
		//image.pixelInset = new Rect (0f, 0f, Screen.width, Screen.height);
	}
    void Start()
    {
        //changePOV = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ChangePOV>();
    }

	void Update(){
		if (sceneStarting) {
			StartScene();
		}
	}

	void FadeToClear(){
		image.color = Color.Lerp (image.color, Color.clear, fadeSpeed * Time.deltaTime/**changePOV.worldSimulationSpeedVar*/); 
	}

	void FadeToBlack(){
		image.color = Color.Lerp (image.color, Color.black, fadeSpeed * Time.deltaTime/**changePOV.worldSimulationSpeedVar*/); 
	}

	void StartScene(){

		FadeToClear ();
		if (image.color.a <= 0.15f) {
			this.image.color = Color.clear;
			this.image.enabled = false;
			sceneStarting = false;
		}
	}

	public void EndScene(){
		image.enabled = true;
		FadeToBlack ();
		if (image.color.a >= 0.95f) {
			Application.LoadLevel(0);
		}
	}
}

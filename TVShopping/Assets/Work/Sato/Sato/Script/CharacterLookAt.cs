using UnityEngine;
using System.Collections;

public class CharacterLookAt : MonoBehaviour {
	//
	// カメラの方向にキャラクター（target）が向くようにしたいぜ
	//

	ChangeCamera _target_camera;

	// Use this for initialization
	void Start() {

		_target_camera = GameObject.FindObjectOfType<ChangeCamera>();
		Vector3 target = -_target_camera.CurrentCamera.transform.position;
		target.y = transform.position.y;
		this.transform.LookAt(target);  
	
	}
		
	// Update is called once per frame
	void Update () {
		Vector3 target = -_target_camera.CurrentCamera.transform.position;
		target.y = transform.position.y;
		this.transform.LookAt(target);  
	
	}
}
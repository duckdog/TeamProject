using UnityEngine;
using System.Collections;

public class ChangeCamera : MonoBehaviour {
	//
	// 複数のカメラのオンオフを行います
	// 現在使用中のカメラの情報もここで渡します
	//

	[SerializeField]//カメラ管理用格納庫
	Camera[] SubCamera;
	Vector3[] _camera_pos;

	int _camera_number;
	private int _set_number;
	public int _SetNumber {

		get{ return _set_number; }

		set {
			if (value != -1 && value  != _camera_number) {

				SubCamera [_camera_number].enabled = false;

				_camera_number = value;
				SubCamera [_camera_number ].enabled = true;
				CurrentCamera = SubCamera [_camera_number ];

			}
		}
	}

	public Camera CurrentCamera{ get; set;}

	// Use this for initialization
	void Awake () {
		_set_number = 0;
		_camera_number = 0;
		CurrentCamera = SubCamera [0];
		SubCamera [0].enabled = true;//MainCameraからスタート
		SubCamera [1].enabled = false;
		SubCamera [2].enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
	
		//FixMe:Debug用
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
		
			//写してるカメラをオフ
			SubCamera [_camera_number].enabled = false;
			if (_camera_number < SubCamera.Length - 1) {
				_camera_number++;
			} else {
				_camera_number = 0;
			}
			SubCamera [_camera_number].enabled = true;
			CurrentCamera = SubCamera [_camera_number];
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			CameraTarget ();
		}


	}
	//デバッグ用 :カメラをチェンジ
	void SetCamera(int set_camera_number = 0){

		SubCamera [_camera_number].enabled = false;
		SubCamera [set_camera_number].enabled = true;

	}
	//Target
	void CameraTarget(){
		//Debug.用
		SubCamera [_camera_number].transform.LookAt(GameObject.FindGameObjectWithTag("MainGoods").transform);
	}


}

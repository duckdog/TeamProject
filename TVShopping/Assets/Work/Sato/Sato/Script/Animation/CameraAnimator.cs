using UnityEngine;
using System.Collections;

public class CameraAnimator : MonoBehaviour {

	public enum Animation
	{
		Zoom = 1,
		BounceZoom = 2,
		L_SlideIn = 3,
		R_SlideIn = 4,
		Shake = 5,

		Null = -1,

	}
	Easing _easing;
	ChangeCamera _camera;
	float timer;
	Vector3 _center_pos = new Vector3(0.0f,4.5f,-11.03f);
	Vector3 _screen_view_pos = new Vector3(0.0f,7.49f,-11.03f);
	float zoom_range = 5.0f;
	float slide_range = 5.0f;
	float animation_total_time = 2.5f;
	// Use this for initialization
	void Start () {
		timer = 0;
		_easing = GameObject.FindObjectOfType<Easing>();
		_camera = GameObject.FindObjectOfType<ChangeCamera> ();
		//StartCoroutine (RandomShake(_camera.CurrentCamera));
		//StartCoroutine(SlideIn(5.0f,_camera.CurrentCamera,true));
		StartCoroutine (R_HighFromCameraToTaget(3.0f,_camera.CurrentCamera));
	}
	

	//受けっとたアニメーションのフラグを立てる。※立てるだけなので、終了判定などは一切しておりません。
	public void StartAnimation(CameraAnimator.Animation animation,Camera camera)
	{
	
		switch (animation) 
		{
		case Animation.Zoom:
			StartCoroutine (BehindZoom(animation_total_time,camera));
			break;

		case Animation.BounceZoom:
			StartCoroutine (BehindZoom(animation_total_time,camera,true));
			break;

		case Animation.R_SlideIn:
			StartCoroutine(SlideIn(animation_total_time,_camera.CurrentCamera));
			break;

		case Animation.L_SlideIn:
			StartCoroutine(SlideIn(animation_total_time,_camera.CurrentCamera,true));
			break;
		case Animation.Shake:
			StartCoroutine(RandomShake(camera));
			break;

		}

	}

	//ゲームスタート時に、カメラが右上方向から、降りてきます.
	//Tips:二回使うことができません。 めちゃくちゃ使いづらいんです、はい。
	IEnumerator R_HighFromCameraToTaget(float total_time,Camera camera)
	{
		float time = 0;
		//Vector3 pos = camera.transform.localPosition;

		GameObject target = GameObject.FindGameObjectWithTag ("CameraTarget");
		Vector3 min_pos = camera.transform.localPosition;
		float half_time = total_time * 0.5f;
		//無理やりINOUTEXP
		while(time <= half_time){
		
			time += Time.deltaTime;

				camera.transform.localPosition =
				new Vector3 (
					(float)_easing.InExp(time,half_time, _center_pos.x,min_pos.x),
					(float)_easing.InExp(time,half_time, _center_pos.y,min_pos.y),
					camera.transform.localPosition.z);

			transform.LookAt(target.transform);
			yield return null;

		}
		time = 0;
		min_pos = camera.transform.localPosition;
		while(time  <= half_time){

			time += Time.deltaTime;

			camera.transform.localPosition =
				new Vector3 (
					(float)_easing.OutExp(time, half_time, _center_pos.x,min_pos.x),
					(float)_easing.OutExp(time,half_time, _center_pos.y,min_pos.y),
					camera.transform.localPosition.z);

			transform.LookAt(target.transform);
			yield return null;

		}
		camera.transform.localPosition = _center_pos;

		yield return null;
	}

	//現在のカメラ位置から、少し離れたところにカメラを移動し、元の位置によっていきます。
	public IEnumerator BehindZoom(float total_time,Camera camera,bool isbounce = false)
	{
		float time = 0;
		//元の位置から、定めた数値にいったんカメラを引く
		float target_pos_z = camera.transform.localPosition.z;

		camera.transform.localPosition -= new Vector3(0,0,zoom_range);

		//通常のズーム
		if (!isbounce) {
			while (time <= total_time) {

				time += Time.deltaTime;

				camera.transform.localPosition =
				new Vector3 (
					camera.transform.localPosition.x,
					camera.transform.localPosition.y,
						(float)_easing.InOutQuart(time, total_time,target_pos_z, camera.transform.localPosition.z));

				yield return null;

			}
		}
		else //バウンドズームをおこないます
		{
			while (time <= total_time) {

				time += Time.deltaTime;

				camera.transform.localPosition =
					new Vector3 (
						camera.transform.localPosition.x,
						camera.transform.localPosition.y,
						(float)_easing.OutBounce(time,total_time,target_pos_z,target_pos_z - zoom_range));

				Debug.Log (time);
				yield return null;
			}

		}

		//終了
		yield return null;

	}



	//Tips:カメラを横からずらして写します,※基本は右からスライドイン、引数のフラグで左からも可能
	public IEnumerator SlideIn(float total_time,Camera camera,bool is_right = false)
	{
		float time = 0;
		//元の位置から、定めた数値にいったんカメラを横にずらす
		Vector3 max_pos = camera.transform.localPosition;

		{
			//デフォルトで、右からスライド
			if (!is_right) {

				camera.transform.localPosition += new Vector3(slide_range,0,0);
				float min_pos_x = camera.transform.localPosition.x;
				while (time <= total_time) {

					time += Time.deltaTime;

					camera.transform.localPosition =
						new Vector3 (
							(float)_easing.OutExp (time, total_time, max_pos.x, min_pos_x),
							max_pos.y,
							max_pos.z);

					yield return null;

				}
			}
			//引数からフラグがたったら左からスライド
			else 
			{
				camera.transform.localPosition -= new Vector3(slide_range,0,0);
				float min_pos_x = camera.transform.localPosition.x;
				while (time <= total_time) {

					time += Time.deltaTime;

					camera.transform.localPosition =
						new Vector3 (
							(float)_easing.OutExp (time, total_time, max_pos.x, min_pos_x),
							max_pos.y,
							max_pos.z);

					yield return null;

				}
			}
		}
		//終了
		yield return null;

	}


	//Tips:カメラをゆらします。※実際に乱数は使いません.揺らし続けることはできません
	public IEnumerator RandomShake(Camera camera)
	{
	
		float time = 0;
		//元の位置から、定めた数値にいったんカメラを横にずらす
		float _shake_x = 2;
		float _shake_y = 2;
		Vector2 add = Vector2.zero;
		Vector3 origin_pos = camera.transform.localPosition;
		//適当な時間で抜ける
		while (time < 1) {
		
			time += Time.deltaTime;

			camera.transform.localPosition = origin_pos;
			camera.transform.localPosition += new Vector3(_shake_x,_shake_y,0);
			_shake_x *= -0.83f;
			_shake_y *= -0.83f;

			yield return null;

		}
		camera.transform.localPosition = origin_pos;
		//終了
		yield return null;

	}
}

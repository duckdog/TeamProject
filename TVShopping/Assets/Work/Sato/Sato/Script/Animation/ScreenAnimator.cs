//
//
//  視聴者の声として、
//
//
using UnityEngine;
using System.Collections;

public class ScreenAnimator : MonoBehaviour {

	Easing _Easing;
	[SerializeField]
	public bool _open_screen_animation = false;
	[SerializeField]
	public bool _close_screen_animation = false;
	[SerializeField]
	float _add_range = -5.0f;
	[SerializeField]
	float _total_time = 3.0f;
	float _timer = 0;
	float _defalt_pos;


	// Use this for initialization
	void Start () {
		_defalt_pos = transform.position.y;
		_Easing = GameObject.FindObjectOfType<Easing> ();

	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (_open_screen_animation) {
			StartCoroutine (OpenScreenAnimation());
			_open_screen_animation = false;
		} 

		if (_close_screen_animation) {
			StartCoroutine (CloseScreenAnimation());
			_close_screen_animation = false;
		} 
	}


	//i以下、スクリーンがででくるアニメーションと、閉まるアニメーション
	public IEnumerator OpenScreenAnimation()
	{

		while (_timer < _total_time) {

			_timer += Time.deltaTime;
			transform.position = new Vector3 (transform.position.x,
				(float)_Easing.OutBounce (
					_timer,
					_total_time,
					_defalt_pos + _add_range,
					_defalt_pos), transform.position.z);

			yield return null;

		}

		_timer = 0;
		yield return null;
	}


	public IEnumerator CloseScreenAnimation()
	{
		while (_timer < _total_time) {

			_timer += Time.deltaTime;
			transform.position = 
				new Vector3 (transform.position.x,
				(float)_Easing.OutBounce (_timer,_total_time,_defalt_pos,_defalt_pos + _add_range),
					transform.position.z);

			yield return null;
		}

		_close_screen_animation = false;
		_timer = 0;
		yield return null;
	
	}



}

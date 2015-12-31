using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FlyingFace : MonoBehaviour {

	// アベリィが怒りのあまりに,
	// ジョニーの顔を吹っ飛ばすアニメーションです
	// 
	//

	//参照
	ChangeCamera _Camera;
	CameraAnimator _camera_animator;
	Easing _Easing;
	CruckAnimator _CruckAnimator;
	WaitFade _WaitFade;
	SpriteRenderer _sprite_renderer;
	[SerializeField]
	public Sprite[] _images;
	ScenarioSetter _scenario;
	CharacterAnimator _character_animator;
	//アニメーション時間、速度
	float _speed = 3.0f;
	[SerializeField]
	float _total_time = 1.0f;
	float _timer;
	Vector3 _defalt_face_pos;
	Vector3 _move_range;
	Vector3 _target_pos;
	GameObject _abery;
	Vector3 _abery_defalt_face_pos;
	Vector3 _jony_pos;

	[SerializeField]
	float _fall_range = 10;
	public bool _do_flying_face;
	public bool _do_flying_wig;
	private bool _do_fall;
	[SerializeField]
	float _fall_time = 3.0f;
	[SerializeField]
	float _fade_start_time = 1.0f;
	float _fade_total_time = 1.0f;
	float _wait_fade_time = 1.0f;
	// Use this for initialization
	void Awake () {

		_Easing = GameObject.FindObjectOfType<Easing> ();
		_Camera = GameObject.FindObjectOfType<ChangeCamera>();
		_camera_animator = GameObject.FindObjectOfType<CameraAnimator> ();
		_CruckAnimator = GameObject.FindObjectOfType<CruckAnimator> ();
		_scenario = GameObject.FindObjectOfType<ScenarioSetter> ();
		_WaitFade = GameObject.FindObjectOfType<WaitFade> ();
		_jony_pos = (GameObject.FindGameObjectWithTag ("Jony")).transform.localPosition;
		_abery = (GameObject.FindGameObjectWithTag ("Abery")) as GameObject;
		_sprite_renderer = GetComponent<SpriteRenderer> ();



		_sprite_renderer.enabled = false;
		_do_flying_face = false;
		_do_flying_wig = false;
		_do_fall = false;

		_timer = 0.0f;
		_move_range = new Vector3 (30,5,30);
		_abery_defalt_face_pos = _abery.transform.localPosition;
	}

	// Update is called once per frame
	void Update () {

		if (_do_flying_face) 
		{
			//シナリオを進まないようにして、.悲しいアニメーション開始
			_scenario.SetRoute = ScenarioSetter.Route.NULL;
			_sprite_renderer.sprite = _images [0];
			StartCoroutine (FaceFlyingtoCamera());

			//一度だけ通る
			_do_flying_face = false;
		}
		if (_do_flying_wig) 
		{
			//シナリオを進まないようにして、.悲しいアニメーション開始
			_scenario.SetRoute = ScenarioSetter.Route.NULL;
			_sprite_renderer.sprite = _images [1];
			StartCoroutine (WigFlyingtoCamera());
			//一度だけ通る
			_do_flying_wig = false;

		}
	}

	private IEnumerator WigFlyingtoCamera()
	{
		float add_x = 0;
		float time = 0;
		float total_time = 0.5f;
		float move_min = _abery.transform.localPosition.x;
		float move_max = move_min + 3;

		CharacterAnimator jony_animation = (GameObject.FindGameObjectWithTag ("Jony")).GetComponent<CharacterAnimator> ();
		CharacterAnimator abey_animation = (GameObject.FindGameObjectWithTag ("Abery")).GetComponent<CharacterAnimator> ();
		abey_animation._current_state = CharacterAnimator.State.Angry;

		//忍び寄るアベリィ....
		while(time < total_time)
		{
			time += Time.deltaTime;
			add_x = _Easing.OutExp(time,total_time,move_max,move_min);

			Vector3 move_pos = new Vector3 (add_x,_abery.transform.localPosition.y,_abery.transform.localPosition.z);
			_abery.transform.localPosition = move_pos; 

			yield return null;
		}

		//飛ぶ用の顔出現
		time = 0;
		_defalt_face_pos = transform.localPosition;
		_target_pos = _Camera.CurrentCamera.transform.position;
		_sprite_renderer.enabled = true;
		jony_animation._current_state = CharacterAnimator.State.Angry;//ここをハゲに

		//かつらを飛ばします!
		while (time < 2) {
		
			//カメラにむかって突進!!!!!!!
			time += Time.deltaTime;
			transform.position = new Vector3 (
				(float)_Easing.InQuad (time, _total_time, _target_pos.x, transform.position.x),
				(float)_Easing.InQuad (time, _total_time, _target_pos.y, transform.position.y),
				(float)_Easing.InQuad (time, _total_time, _target_pos.z, transform.position.z));

			//一旦他のしょりへ　
			yield return null;

		}

		time = 0;
		//もとにもどるアベリィ....
		while(time < 1)
		{
			time += Time.deltaTime;
			add_x = _Easing.OutExp(time,total_time,move_min,move_max);

			Vector3 move_pos = new Vector3 (add_x,_abery.transform.localPosition.y,_abery.transform.localPosition.z);
			_abery.transform.localPosition = move_pos; 

			yield return null;
		}

		//シナリオ読み込みを再開
		transform.localPosition = _defalt_face_pos;
		_sprite_renderer.enabled = false;
		_scenario.BackToOldScenerioRoute ();
		yield return null;

	}

	private IEnumerator FaceFlyingtoCamera()
	{
		float add_x = 0;
		float time = 0;
		float total_time = 0.5f;
		float move_min = _abery.transform.localPosition.x;
		float move_max = move_min + 3;

		CharacterAnimator jony_animation = (GameObject.FindGameObjectWithTag ("Jony")).GetComponent<CharacterAnimator> ();
		CharacterAnimator abey_animation = (GameObject.FindGameObjectWithTag ("Abery")).GetComponent<CharacterAnimator> ();
		abey_animation._current_state = CharacterAnimator.State.Angry;

		//忍び寄るアベリィ....
		while(time < total_time)
		{
			time += Time.deltaTime;
			add_x = _Easing.OutExp(time,total_time,move_max,move_min);

			Vector3 move_pos = new Vector3 (add_x,_abery.transform.localPosition.y,_abery.transform.localPosition.z);
			_abery.transform.localPosition = move_pos; 

			yield return null;
		}

		//飛ぶ用の顔出現
		time = 0;
		_defalt_face_pos = transform.localPosition;
		_target_pos = _Camera.CurrentCamera.transform.position;
		_sprite_renderer.enabled = true;
		jony_animation._current_state = CharacterAnimator.State.Hate;

		//顔を飛ばします!
		while (true) {

			//カメラにぶつかったら、落ちるアニメーションへ遷移
			if (_do_fall) yield break;

			//カメラにむかって突進!!!!!!!
			time += Time.deltaTime;
			transform.position = new Vector3 (
				(float)_Easing.OutQuad (time, _total_time, _target_pos.x, transform.position.x),
				(float)_Easing.OutQuad (time, _total_time, _target_pos.y, transform.position.y),
				(float)_Easing.OutQuad (time, _total_time, _target_pos.z, transform.position.z));
				
			//一旦他のしょりへ　
			yield return null;

		}

	}

	void OnTriggerEnter(Collider other) {

		if (!_do_fall) {
			_do_fall = true;
			_CruckAnimator._doCrack = true;
			StartCoroutine (FallFace ());
			StartCoroutine (_camera_animator.RandomShake (_Camera.CurrentCamera));
		}
	}


	private IEnumerator FallFace()
	{
		float time = 0;

		//画面にぶつかった直後に、顔がずるずると落ちます。
		while (true) {
			time += Time.deltaTime;

			transform.position = new Vector3 (
				transform.position.x,
				(float)_Easing.InQuad (time, _fall_time, _target_pos.y - _fall_range, transform.position.y),
				transform.position.z);

			//フェード開始
			if (time >= _fade_start_time) {
				//1秒かけて、フェード
				StartCoroutine (FadeToFade(_fade_total_time));

				_do_fall = false;

				yield break;
			}

			//シナリオ読み込みを再開
			_scenario.BackToOldScenerioRoute ();
			yield return null;
		}

	}

	private IEnumerator FadeToFade(float total_time)
	{
		//fade_time分の時間をかけて、フェードOUT
		float time = 0;
		StartCoroutine (_WaitFade.FadeInOutScreenImage (total_time));
		CharacterAnimator abey_animation = (GameObject.FindGameObjectWithTag ("Abery")).GetComponent<CharacterAnimator> ();
		//フェードアウト終了後、遅延時間分だけ、フェードインをおくらせる。
		while (time < total_time + _wait_fade_time) {

			time += Time.deltaTime;
			yield return null;
		
		}

		//フェードイン　いろいろと初期位置にもどす.
		time = 0;
		_abery.transform.localPosition = _abery_defalt_face_pos;
		abey_animation._current_state = CharacterAnimator.State.Smile;
		_sprite_renderer.enabled = false;
		_CruckAnimator._doCrack = false;
		StartCoroutine (_WaitFade.FadeInOutScreenImage (total_time));

		//フェードイン終了後,
		while (time < total_time) {

			time += Time.deltaTime;
			yield return null;
		}

		//シナリオ読み込みを再開
		Init ();

	}


	void Init(){

		_timer = 0.0f;
		_do_fall = false;
		_do_flying_face = false;
		_CruckAnimator._doCrack = false;
		_WaitFade._DoFade = false;
		transform.localPosition = _defalt_face_pos;
		//シナリオ読み込みを再開
		_scenario.BackToOldScenerioRoute ();



	}
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharacterAnimator : MonoBehaviour
{

	//
	//[キャラクターアニメーション]
	//
	//  現時点ではデバッグ作業が多いため,
	//  わざとpublicで宣言しています。
	//  シリアライズにすればいいじゃん、とかいう人はまぁ。ごめんて
	//

	//sin,cos計算用に円周率を固定.雑把に.14まで。
	private const float RADIUS = 3.14F;
	GameMainManager _game_manager;
	Easing _easing;
	SaveJson _save_json;
	FlyingFace _flyig_face;

	//表情(画像)パターン
	public enum State
	{

		Normal = 0,             //通常時
		Smile = 1,                  //笑顔
		Happened = 2,               //驚き
		Angry = 3,
		CoolFace = 4,
		Happiness = 5,
		Hate = 6,//Jony is saddly
		Sexy = 7,//Jony is saddly

		Last,                   //番兵
	}
	//アニメーションパターン
	public enum Animation
	{
	
		UpScaling = 0,              //縦に拡縮
		Hopping = 1,                //跳ねる
		RotatoToChangeState = 2,    //回転して表情（画像）変更

		//以下、特殊アニメーション
		JonyFlyFace = 3,			//特殊アニメーション　顔がとびます


		Last,           //番兵

	}

	public State _current_state;
	public State _next_state;
	public Animation _current_animation;
	public Animation _next_animation;

	//アニメーションが遷移した時間を一時的に保存する場所
	public float _transition_time = 0.0f;

	//Animation Jsondate 格納リスト
	private List<SaveJson.TransitionInfo> _transition_index = new List<SaveJson.TransitionInfo>();
	[SerializeField]
	private bool DoRoadedAnimarionJsonDate;
	[SerializeField]
	private GameMainManager.Root _root;
	private float _transiton_time;
	private bool _do_initialize_scenario = false; 
	private int _current_scenario_number = 0;
	private bool _do_save_scenario = false;
	private bool _do_scenario = false;
	private float  _save_current_time = 0.0f;
	private float _debug_current_time = 0.0f;




	//キャラクターの画像格納と、表示画像領域
	public Sprite[] _charater_index;
	private SpriteRenderer _character;


	//アニメーションスピード
	//時間
	//デフォルトサイズ.
	public float _animation_timer;
	public float _animation_counter = 0.0f;
	public Vector2 _defalt_scale;
	public Vector3 _defalt_position;

	//拡縮
	public float _scaling_limit;
	public float _scaling_speed;
	//跳ねる
	public float _hopping_limit = 30.0f;
	public float _total_hopping_time = 1.0f;
	public int _hopping_count = 0;
	//半回転
	private const float _defalt_rotate = 0.0f;
	private const float _rotate_limit = 90.0f;
	[Range(0.0f, 1.0f)]
	public float _total_rotate_time = 1.0F;
	private bool _is_reverse;


	//初期化
	void Awake()
	{

		//gamemanager参照
		_game_manager = GameObject.FindObjectOfType<GameMainManager> ();
		_flyig_face = GameObject.FindObjectOfType<FlyingFace> ();

		//easing初期化
		_easing = GameObject.FindObjectOfType<Easing>();
		_save_json = GameObject.FindObjectOfType<SaveJson>();

		//sprite初期化
		_character = GetComponent<SpriteRenderer>();
		_character.sprite = _charater_index[(int)_current_state];

		//デフォルトのスケールを保存.
		_defalt_scale = new Vector2(transform.localScale.x, transform.localScale.y);
		_defalt_position = transform.position;

		//アニメーションデータを読み込み
		if (DoRoadedAnimarionJsonDate) {
			_transition_index = _save_json.LoadAnimation(_root);
			_do_scenario = true;
		}
	}


	// 毎フレーム呼び出し
	void Update()
	{

		//アニメーションタイマーが０じゃない場合
		//同じアニメーションを繰り替えす.
		//カウンターを更新
		_animation_timer += Time.deltaTime;

		//アニメーションのシナリオを再生
		if (_do_scenario)
		{

			//ゲームの更新時間
			_debug_current_time += _game_manager.MainGameTimer;
			PlayScenario();

		}

		//シナリオ保存開始
		if (_do_save_scenario)
		{
			//セーブが始まった時間を更新させる.

			_save_current_time += Time.deltaTime;
		}


		//表情変更
		_character.sprite = ChangeSprite(_current_state);


		switch (_current_animation)
		{

		///////拡縮 ※正確には縮んでもとに戻るようにしてあります.///////////////
		case Animation.UpScaling:

			UpScaling();

			break;

			///////跳ねる/////////////////////////////////////////////////////////
		case Animation.Hopping:

			Hopping();

			break;

			//////状態遷移のため、回転.////////////////////////////////////////////
		case Animation.RotatoToChangeState:

			StateChangeAnimation();

			break;

		case Animation.JonyFlyFace:

			_flyig_face._do_flying_face = true;
			_current_animation = Animation.UpScaling;

			break;

		}


	}

	//状態遷移(画像を変更)
	private Sprite ChangeSprite(State state)
	{

		return _charater_index[(int)state];

	}


	//拡縮アニメーション
	protected void UpScaling(Animation next_animation = Animation.UpScaling)
	{

		transform.localScale = new Vector2(_defalt_scale.x,
			_defalt_scale.y
			- (Mathf.Abs
				(Mathf.Sin(_scaling_speed * _animation_timer * RADIUS / 180.0f))
				* _scaling_limit));

		//他のアニメーションに遷移
		if (transform.localScale.y >= _defalt_scale.y - 0.01f)
		{
			transform.localScale = _defalt_scale;
			_current_animation = _next_animation;
		}

	}


	//ジャンプ
	protected void Hopping(Animation next_animation = Animation.UpScaling)
	{
		//カウンターを更新
		_animation_counter += Time.deltaTime;


		{
			float half_time = _total_hopping_time * 0.5f;
			//jump!※っぽく
			/*   if (_animation_counter < half_time)
                transform.position = new Vector2(_defalt_position.x,
                                                (float)_easing.InOutQuart(_animation_counter, half_time,
						_hopping_limit+_defalt_position.y, _defalt_position.y));
            //着地まで
            if (_animation_counter >= half_time && _animation_counter < _total_hopping_time)
                transform.position = new Vector2(_defalt_position.x,
                                                (float)_easing.InOutQuart(_animation_counter - half_time, half_time,
						_defalt_position.y, _hopping_limit+_defalt_position.y));
*/
			if (_animation_counter < half_time)
				transform.position = new Vector3(_defalt_position.x,
					(float)_easing.InOutQuart(_animation_counter, half_time,
						_hopping_limit+_defalt_position.y, transform.position.y)
					,_defalt_position.z);

			//着地まで
			if (_animation_counter >= half_time && _animation_counter < _total_hopping_time)
				transform.position = new Vector3(_defalt_position.x,
					(float)_easing.InOutQuart(_animation_counter - half_time, half_time,
						_defalt_position.y,transform.position.y),_defalt_position.z);

			//到達時間に達したらリセット
			if (_animation_counter >= _total_hopping_time)
			{
				//他のアニメーションに遷移
				//りせっと
				_animation_counter = 0;
				transform.position = _defalt_position;
				_current_animation = _next_animation;
			}
		}



	}


	//半回転して画像（表情）を変更
	protected void StateChangeAnimation(Animation next_animation = Animation.UpScaling)
	{

		//カウンターを更新
		_animation_counter += Time.deltaTime;


		//半回転開始
		if (_animation_counter < _total_rotate_time && !_is_reverse)
		{
			//限界値まで、いーじんぐで加算
			/*
            transform.eulerAngles = new Vector3(0,
                                                (float)_easing.InQuad(_animation_counter, _total_rotate_time,
                                                                      _rotate_limit, _defalt_rotate),
                                                0);
            */
			transform.localScale = new Vector2((float)_easing.InQuad(_animation_counter, _total_rotate_time,0, _defalt_scale.x),_defalt_scale.y);

		}

		//半回転終了後、逆半回転のフラグを立てる
		if (_animation_counter >= _total_rotate_time && !_is_reverse)
		{

			//折り返し
			_is_reverse = true;
			//表情（画像の差し替え
			_current_state = _next_state;
			//アニメーション用のカウンターをリセット
			_animation_counter = 0.0f;

		}

		//逆半回転のフラグがたったら回転開始
		if (_is_reverse && _animation_counter < _total_rotate_time)
		{

			//total_time分の時間をかけて元のｙ軸に戻す
			/*transform.eulerAngles = new Vector3(0,
                                                (float)_easing.OutQuad(_animation_counter, _total_rotate_time,
                                                                       _defalt_rotate, _rotate_limit),
                                                0);
*/
			transform.localScale = new Vector2((float)_easing.InQuad(_animation_counter, _total_rotate_time,_defalt_scale.x,0),_defalt_scale.y);


		}//元の位置にもどったら,位置ずれを修正、その後他のアニメーションに移る.
		else if (_is_reverse && _animation_counter >= _total_rotate_time)
		{

			//初期化
			_is_reverse = false;
			_animation_counter = 0;

			//細かい位置ずれ用調整.
			transform.eulerAngles = Vector3.zero;

			//他のアニメーションに遷移
			_current_animation = _next_animation;

		}
	}



	//拡縮アニメーション
	protected void JonyFlyingFace(Animation next_animation = Animation.UpScaling)
	{

		transform.localScale = new Vector2(_defalt_scale.x,
			_defalt_scale.y
			- (Mathf.Abs
				(Mathf.Sin(_scaling_speed * _animation_timer * RADIUS / 180.0f))
				* _scaling_limit));

		//他のアニメーションに遷移
		if (transform.localScale.y >= _defalt_scale.y - 0.01f)
		{
			transform.localScale = _defalt_scale;
			_current_animation = _next_animation;
		}

	}
	/////////////////////////////////////////////////////////////////
	//以下Canvas用　Debugコマンド//////////////////////////////////////////
	/////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////


	//アニメーションボタン
	public void UpscaleButton()
	{
		_current_animation = Animation.UpScaling;
		transform.position = _defalt_position;
		//セーブ中だったら
		if (_do_save_scenario)
			_transition_index.Add(new SaveJson.TransitionInfo(_save_current_time,_current_animation,_current_state));



	}
	public void HoppingButton()
	{

		_current_animation = Animation.Hopping;
		transform.localScale = _defalt_scale;
		//セーブ中だったら
		if (_do_save_scenario)
			_transition_index.Add(new SaveJson.TransitionInfo(_save_current_time,_current_animation, _current_state));

		//      Debug.Log("Time:" + Time.time + "AnimationTypeNumber:" + (int)_current_animation);

	}


	//表情遷移
	public void NormalButton()
	{

		_next_state = State.Normal;
		if (_next_state != _current_state) _current_animation = Animation.RotatoToChangeState;
		//セーブ中だったら
		if (_do_save_scenario)
			_transition_index.Add(new SaveJson.TransitionInfo(_save_current_time,_current_animation, _next_state));

		//  Debug.Log("Time:" + Time.time + "AnimationTypeNumber:" + (int)_current_animation);

	}
	public void SmileButton()
	{

		_next_state = State.Smile;
		if (_next_state != _current_state) _current_animation = Animation.RotatoToChangeState;
		//セーブ中だったら
		if (_do_save_scenario)
			_transition_index.Add(new SaveJson.TransitionInfo(_save_current_time, _current_animation, _next_state));

		//  Debug.Log("Time:" + Time.time + "AnimationTypeNumber:" + (int)_current_animation);

	}

	public void HappendButton()
	{

		_next_state = State.Happened;
		if (_next_state != _current_state) _current_animation = Animation.RotatoToChangeState;
		//セーブ中だったら
		if (_do_save_scenario)
			_transition_index.Add(new SaveJson.TransitionInfo(_save_current_time, _current_animation, _next_state));

		// Debug.Log("Time:" + Time.time + "AnimationTypeNumber:" + (int)_current_animation);

	}





	public void PlayScenario()
	{

		//Debug.Log("シナリオプレイ中");

		//list領域以上行かないように制御.
		if (_current_scenario_number < _transition_index.Count - 1)
		{

			//シナリオを一度だけ、更新　
			if (_do_initialize_scenario &&  _debug_current_time >= _transition_index[0]._TransitionTime)
			{
				//遷移するときの時間
				//その時に遷移するアニメーションと表情
				_transition_time =
					_transition_index[_current_scenario_number + 1]._TransitionTime
					-(_transition_index[_current_scenario_number]._TransitionTime);

				_current_animation = (Animation)_transition_index[_current_scenario_number]._Animation;
				_next_state = (State)_transition_index[_current_scenario_number]._State;

				_do_initialize_scenario = false;


			}           

			//アニメーション遷移の時間が０になるまで、減算
			_transition_time -= Time.deltaTime;


			//次のアニメーションに遷移
			if(_transition_time <= 0 && _debug_current_time >= _transition_index[0]._TransitionTime)
			{

				//次の配列番号に遷移

				_current_scenario_number++;
				_do_initialize_scenario = true;

			}

		}

	}

	//セーブ開始
	public void SaveStartScenarioButton()
	{

		_do_save_scenario = true;
		_do_scenario = false;
		_do_initialize_scenario = false;

	}
	//セーブストップ
	public void SaveStopScenarioButton()
	{

		//saveを終了 
		_do_save_scenario = false;
		_save_json.AddAnimation (_transition_index);
		_save_json.SavedAnimation (_root);
		_transition_index.Add(new SaveJson.TransitionInfo(-1, _current_animation, _current_state));

	}

	//アニメーションのシナリオを再生
	public void DoScenarioButton()
	{
		//シナリオ再生
		_do_scenario = true;
		_do_initialize_scenario = true;

	}




}
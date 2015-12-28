using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ScenarioSetter : MonoBehaviour {

	//
	// シナリオ、カメラ演出、キャラクターアニメーションのデータを
	// まとめて更新いたします。尚、データもとは、CSVデータです。
	// Todo:データ量が多いので、どうしても長くなってます、直せたらリファ


	//他クラス参照類
	CharacterAnimator Jony;
	CharacterAnimator Abery;
	ExtraAnimator _extra_animator;
	ScenarioText _scenario_text;
	ChangeCamera _view_camera;
	CVManager _cv_reference;

	public enum Route
	{
		Main = 0,
		A = 1,
		B = 2,
		C = 3,
		
		NULL =-1,//Minigameに遷移の予定
		
	}
	
	
	//テキスト一行分に格納されるデータ一覧
	//テキストデータ、※表示時間、次に表示するルート,アニメーション命令
	//追加予定:効果音、CV等の命令
	public struct Scenariodate
	{
		
		public string _text_date;
		public float _time;
		public Route _next_route;
		public CharacterAnimator.Animation _jony_animation;
		public CharacterAnimator.Animation _abery_animation;
		public CharacterAnimator.State _jony_state;
		public CharacterAnimator.State _abery_state;
		public int _camera_number;
		public ExtraAnimator.Animation _extra_animation;
		public Scenariodate(string text_date,float time ,Route next_route = Route.Main,
		                    CharacterAnimator.Animation jony_animation = CharacterAnimator.Animation.UpScaling,
		                    CharacterAnimator.Animation abery_animation = CharacterAnimator.Animation.UpScaling,
		                    
		                    CharacterAnimator.State jony_state = CharacterAnimator.State.Normal,
							CharacterAnimator.State abery_state = CharacterAnimator.State.Normal,
							int camera_number = -1,
							ExtraAnimator.Animation extra_animation = ExtraAnimator.Animation.NULL)
		{
			this._text_date = text_date;
			this._next_route = next_route;
			this._time = time;

			this._jony_animation  = jony_animation;
			this._jony_state      = jony_state;
			this._abery_animation = abery_animation;
			this._abery_state     = abery_state;
			this._camera_number   = camera_number;
			this._extra_animation = extra_animation;
		}

	}
	
	
	//メインシナリオ
	//テキスト格納用
	List<Scenariodate> _Main = new List<Scenariodate>();
	List<Scenariodate> _A = new List<Scenariodate>();
	List<Scenariodate> _B = new List<Scenariodate>();
	List<Scenariodate> _C = new List<Scenariodate>();

	Route _next_route;
	public Route SetRoute{ get{ return _next_route;} set{ _next_route = value;}}
	float _timer;


	string _current_text;
	int[,] _current_text_number = new int[4,1];
	public int CurrentTextNumber_A{
		get {return _current_text_number[(int)Route.A,0]; }
		set { _current_text_number [(int)Route.A, 0] = value;}
	}
	public int CurrentTextNumber_B{
		get {return _current_text_number[(int)Route.B,0]; }
		set { _current_text_number [(int)Route.B, 0] = value;}
	}
	public int CurrentTextNumber_C{
		get {return _current_text_number[(int)Route.C,0]; }
		set { _current_text_number [(int)Route.C, 0] = value;}
	}

	//フォント用変数
	public GUISkin s_Skin;
	GUIStyle Style;
	GUIStyleState State;
	public float[] timer;
	int gameroot = 0;


	// Use this for initialization
	void Start () {

		Jony = (GameObject.FindGameObjectWithTag ("Jony")).GetComponent<CharacterAnimator>();
		Abery = GameObject.FindGameObjectWithTag ("Abery").GetComponent<CharacterAnimator>();
		_extra_animator = GameObject.FindObjectOfType<ExtraAnimator> ();
		_scenario_text = GameObject.FindObjectOfType<ScenarioText> ();
		_view_camera = GameObject.FindObjectOfType<ChangeCamera> ();
		_cv_reference = GameObject.FindObjectOfType<CVManager> ();	
		Style = new GUIStyle();
		State = new GUIStyleState();
		
		//CSVデータから、ルートごとに分けてテキストデータ等を読み込む
		var MasterTable = new CSVMasterTable();
		MasterTable.Load();
		foreach (var Master in MasterTable.All)
		{
			//一度データを取り出す。
			Scenariodate data = new  Scenariodate (
				Master.Scenario, Master.WatchTime, (Route)Master.NextRoute,
				Master.JonyAnimation, Master.AberyAnimation,
				Master.JonyState, Master.AberyState, Master.CameraNumber,
				Master.ExtraAnimation);

			//ルートにあわせて保存
			switch ((Route)Master.CurrentRoute) {
				 
			case Route.Main:
				
				_Main.Add (data);

				break;
			case Route.A:
				
				_A.Add(data);
				
				break;
			case Route.B:
				
				_B.Add(data);
				
				break;
			case Route.C:
				
				_C.Add(data);
				
				break;
				
			}
			
		}
		
		_A.Add(new Scenariodate("ENDTEXT",0,Route.Main));
		_B.Add(new Scenariodate("ENDTEXT",0,Route.Main));
		_C.Add(new Scenariodate("ENDTEXT",0,Route.Main));

		UpdateScenerio (Route.Main);
		_next_route = Route.Main;
		
	}
	
	
	// Update is called once per frame
	void Update () {

		if(!_cv_reference._isPlay && _next_route != Route.NULL){

			//テキストデータを更新	
			UpdateScenerio (_next_route);
	

		}
		//ミニゲーム等で、テキスト表示を一旦中止.
		//デバッグキーがなくなったら、スイッチ分の中に入れるべき
		if (_next_route == Route.NULL && _timer <= 0) {
			
		
			_current_text = "";
			//Fix: α版用デバッグキー　右クリックで、メインシナリオに遷移
			if (Input.GetMouseButtonDown (1)) {
				_next_route = Route.Main;
				UpdateScenerio (_next_route);
			}
			
			if (Input.GetKeyDown(KeyCode.A)) {
				_next_route = Route.A;
				UpdateScenerio (_next_route);
				
				//シナリオをスキップ//次の分岐のテキストまで配列番号を更新
				while (true) {
					if (_B [CurrentTextNumber_B]._next_route != Route.B) {

						CurrentTextNumber_B++;
						break;
					}
					CurrentTextNumber_B++;
				};
				while (true) {
					if (_C [CurrentTextNumber_C]._next_route != Route.C) {

						CurrentTextNumber_C++;
						break;
					}
					CurrentTextNumber_C++;
				};

				
			}

			if (Input.GetKeyDown(KeyCode.B)) {
				_next_route = Route.B;
				UpdateScenerio (_next_route);
				//シナリオをスキップ//次の分岐のテキストまで配列番号を更新
				while (true) {
					if (_A [CurrentTextNumber_A]._next_route != Route.A) {

						CurrentTextNumber_A++;
						break;
					}
					CurrentTextNumber_A++;
				};
				while (true) {
					if (_C [CurrentTextNumber_C]._next_route != Route.C) {

						CurrentTextNumber_C++;
						break;
					}
					CurrentTextNumber_C++;
				};
			}

			if (Input.GetKeyDown(KeyCode.C)) {
				_next_route = Route.C;
				UpdateScenerio (_next_route);

				//シナリオをスキップ//次の分岐のテキストまで配列番号を更新
				while (true) {
					if (_A [CurrentTextNumber_A]._next_route != Route.A) {

						CurrentTextNumber_A++;
						break;
					}
					CurrentTextNumber_A++;
				};
				while (true) {
					if (_B [CurrentTextNumber_B]._next_route != Route.B) {

						CurrentTextNumber_B++;
						break;
					}
					CurrentTextNumber_B++;
				};
			}
			

		}
		
		
		
	}
	
	//シナリオ、アニメーション、カメラ位置情報を更新
	void UpdateScenerio(Route route)
	{
		Scenariodate data = new Scenariodate();
		switch (route) {

		case Route.Main:
			 
			data = _Main [_current_text_number [(int)Route.Main, 0]];
			_current_text_number [(int)Route.Main, 0]++;
			break;
		case Route.A:
			data = _A [_current_text_number [(int)route, 0]];
			_current_text_number [(int)Route.A, 0]++;
			break;
		case Route.B:
			data = _B [_current_text_number [(int)route, 0]];
			_current_text_number [(int)Route.B, 0]++;
			break;
		case Route.C:
			data = _C [_current_text_number [(int)route, 0]];
			_current_text_number [(int)Route.C, 0]++;
			break;
		case Route.NULL:
			break;
		}
		//ルートが決まってない場合以外更新
		if (route != Route.NULL) {
			_current_text = data._text_date;
			_scenario_text._Text.text = data._text_date;
			_timer = data._time;
			_next_route = data._next_route;
			//(CharacterAnimator.Animation) のキャストはいらなくなるかも
			Abery._current_animation = (CharacterAnimator.Animation)data._abery_animation;
			Abery._next_state = (CharacterAnimator.State)data._abery_state;
			Jony._current_animation = (CharacterAnimator.Animation)data._jony_animation;
			Jony._next_state = (CharacterAnimator.State)data._jony_state;
			_view_camera._SetNumber = data._camera_number;
			_extra_animator.PlayingExtraAnimation (data._extra_animation);
			_cv_reference.CVSoundPlay ();
		}
	}
	
	IEnumerator WaitTimeAndGo()
	{
		for (int i = 0; i < timer.Length;i++ )
		{
			Debug.Log(timer);
			yield return new WaitForSeconds(timer[i]);
			//text_number++;
		}
	}
	

}

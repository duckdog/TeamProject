using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CVManager : MonoBehaviour {

	ScenarioSetter _scenario;

	List<AudioClip> _Main_cv_source;
	List<AudioClip> _A_cv_source;
	List<AudioClip> _B_cv_source;
	List<AudioClip> _C_cv_source;

	AudioSource _audio;
	private float _volume;
	public float _CV_Volume {
		get{ return _volume; }
		set { 
			if (value >= 1)
				_volume = 1;
			else if (value < 0)
				_volume = 0;
			else
				_volume = value;	
		}

	}
	int _Main_current_number = 0;
	int _A_current_number = 0;
	int _B_current_number = 0;
	int _C_current_number = 0;
	private bool _is_play;
	public bool _isPlay{ get{ return _is_play;}set{_is_play = value;}}
	[SerializeField]
	float _delay_time;
	float _delaytime_limit;
	[SerializeField]
	int _Amount;


	public void Init() 
	{
	
		//初期化
		_scenario = GameObject.FindObjectOfType<ScenarioSetter> ();
		_Main_cv_source = new List<AudioClip> ();
		_A_cv_source = new List<AudioClip> ();
		_B_cv_source = new List<AudioClip> ();
		_C_cv_source = new List<AudioClip> ();
		_audio = GetComponent<AudioSource> ();

		//CVデータの読み込み test
		/*for (int i = 0; i < _Amount; i++) {
			_Main_cv_source.Add (Resources.Load<AudioClip> ("CV_Data/Main_" + i.ToString()));
		}*/

		//消さない！CVデータが届き次第実装　ファイル名を”ルート名_”+"番号"で定義してもらおう！
		//TODO:なお、うまくいくかわからないので、試してガッテン
		Debug.Log ( _scenario._Main_TextLength);
				for (int i = 0; i < _scenario._Main_TextLength; i++) {
		_Main_cv_source.Add (Resources.Load<AudioClip> ("CV_Data/Main_" + i.ToString()));
		}
		for (int i = 0; i < _scenario._A_TextLength; i++) {
			_A_cv_source.Add (Resources.Load<AudioClip> ("CV_Data/A_" + i.ToString()));
		}
		for (int i = 0; i < _scenario._B_TextLength; i++) {
			_B_cv_source.Add (Resources.Load<AudioClip> ("CV_Data/B_" + i.ToString()));
		}
		/*for (int i = 0; i < _scenario._C_TextLength; i++) {
			_C_cv_source.Add (Resources.Load<AudioClip> ("CV_Data/C_" + i.ToString()));
		}*/


		_Main_current_number = 0;
		_delaytime_limit = 1;
		_delay_time = _delaytime_limit;
		_is_play = false;
		//Test

	}
		
	//再生
	public void CVSoundPlay(int text_number,ScenarioSetter.Route route = ScenarioSetter.Route.Main){

		//テキストの番号とルート番号に合わせて、CV再生
		/*switch(route){
		case ScenarioSetter.Route.Main:
		
			_audio.clip = _Main_cv_source [text_number ];
			break;
		case ScenarioSetter.Route.A:
			_audio.clip = _A_cv_source [text_number];
			break;
		case ScenarioSetter.Route.B:
			_audio.clip = _B_cv_source [text_number];
			break;
		case ScenarioSetter.Route.C:
			_audio.clip = _C_cv_source [text_number];
			break;
		}*/

		//Test用
		switch(route){
		case ScenarioSetter.Route.Main:

			_audio.clip = _Main_cv_source [text_number % _Main_cv_source.Count];
			break;
		case ScenarioSetter.Route.A:
			_audio.clip = _A_cv_source [text_number% _A_cv_source.Count];
			break;
		case ScenarioSetter.Route.B:
			_audio.clip = _B_cv_source [text_number% _B_cv_source.Count];
			break;
		case ScenarioSetter.Route.C:
			_audio.clip = _C_cv_source [text_number% _C_cv_source.Count];
			break;
		}
		_audio.Play ();
		_isPlay = true;
	}

	// Update is called once per frame
	void Update () {
		//声がなり終わったら。
		if (!_audio.isPlaying) {
			_delay_time -= Time.deltaTime;
			if (_delay_time <= 0) {
				_isPlay = false;
				_delay_time = _delaytime_limit;
			}
		}
	}
}

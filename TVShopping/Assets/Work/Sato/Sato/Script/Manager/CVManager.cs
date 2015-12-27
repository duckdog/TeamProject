using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CVManager : MonoBehaviour {

	List<AudioClip> _cv_source;
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
	int _current_number = 0;
	private bool _is_play;
	public bool _isPlay{ get{ return _is_play;}set{_is_play = value;}}
	[SerializeField]
	float _delay_time;
	float _limit;
	[SerializeField]
	int _Amount;
	// Use this for initialization
	void Awake () {
	
		//初期化
		_cv_source = new List<AudioClip> ();
		_audio = GetComponent<AudioSource> ();
		for (int i = 0; i < _Amount; i++) {
			_cv_source.Add (Resources.Load<AudioClip> ("CV_Data/" + i.ToString()));

		}
		_current_number = 0;
		_limit = 1;
		_delay_time = _limit;
		_is_play = false;
		//Test

	}
		
	//再生
	public void CVSoundPlay(){

		if (_current_number < _Amount) 
		{

			_audio.clip = _cv_source [_current_number];
			_audio.Play ();
			_isPlay = true;
			//次のサウンドに更新
			_current_number++;

		} 
		else //WARNING!!: デバッグ処理です
		{
			//test このエルスは消します
			_current_number = 0;
			_audio.clip = _cv_source [_current_number];
			_audio.Play ();
			_isPlay = true;
			//次のサウンドに更新
			_current_number++;
		}
	}

	// Update is called once per frame
	void Update () {
		//声がなり終わったら。
		if (!_audio.isPlaying) {
			_delay_time -= Time.deltaTime;
			if (_delay_time <= 0) {
				_isPlay = false;
				_delay_time = _limit;
			}
		}
	}
}

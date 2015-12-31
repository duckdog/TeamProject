using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ExtraAnimator : MonoBehaviour {

	public enum Animation{
	
		ScreenOpen = 1,
		ScreenClose = 2,


		NULL = -1,
	}
	Animation _animation;
	Dictionary<Animation,bool> _animation_dic;
	public Animation SetExtraAnimation{

		get{return _animation;}
		set
		{
			_animation = value;

		}


	}

	ScreenAnimator _screen_animation;
	// Use this for initialization
	void Awake () {

		_screen_animation = GameObject.FindObjectOfType<ScreenAnimator> ();

	}

	//受けっとたアニメーションのフラグを立てる。※立てるだけなので、終了判定などは一切しておりません。
	public void PlayingExtraAnimation(ExtraAnimator.Animation animation)
	{
	

		switch (animation) 
		{
		case Animation.ScreenOpen:

			_screen_animation._open_screen_animation = true;

			break;

		case Animation.ScreenClose:

			_screen_animation._close_screen_animation = true;

			break;

		}

	}
}

//
//
// カメラにジョニーの顔が当たったて（謎） 
// レンズがひび割れるアニメーションです。
//
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CruckAnimator : MonoBehaviour {

	Image _Crack;
	AudioSource _CrackSound;
	private bool _do_crack;
	public bool _doCrack
	{
		get{ return _do_crack;}
		set{ _do_crack = value;
			doCrack (_do_crack);
		}

	}
	void Awake()
	{
		_Crack = GetComponent<Image> ();
		_Crack.enabled = false;
		_do_crack = false;
	}

	void doCrack(bool flag)
	{
		_Crack.enabled = flag;
	
	}

    // Update is called once per frame
	void Update () {
	
	}
}

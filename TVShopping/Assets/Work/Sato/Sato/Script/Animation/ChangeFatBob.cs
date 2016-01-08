using UnityEngine;
using System.Collections;

public class ChangeFatBob : MonoBehaviour {


	Easing _Easing;
	public SpriteRenderer[] _bobs;
	// Use this for initialization
	void Awake () {

		_Easing = GameObject.FindObjectOfType<Easing> ();



	}

	public IEnumerator BobChangeFatAnimation()
	{
		float[] time = new float[2]{0,0};
		float[] alpfa = new float[2]{0,0};;
		float total_time = 1;

		while (time[1] <= total_time) {



			//マッチョフェードアウト
			time[0] += Time.deltaTime;
			alpfa[0] = (float)_Easing.InExp (time[0],total_time,0,1);
			_bobs[0].color = new Color (1,1,1,alpfa[0]);
			//デブボブ
			if (alpfa [0] <= 0.60f) {

				time [1] += Time.deltaTime;

				alpfa [1] = (float)_Easing.OutExp (time[1], total_time, 1, 0);
				_bobs [1].color = new Color (1,1,1,alpfa[1]);
			}

			yield return null;

		}
		alpfa [1] = 1;
		_bobs [1].color = new Color (1,1,1,alpfa[1]);
		yield return null;
	}

	public void DoAnimation()
	{
		StartCoroutine (BobChangeFatAnimation());

	}
	// Update is called once per frame
	void Update () {
	
	}
}

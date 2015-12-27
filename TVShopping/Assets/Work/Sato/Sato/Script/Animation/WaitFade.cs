using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class WaitFade : MonoBehaviour {
	//
	//少々お待ちください画像
	//表示のシステム。
	//
	//

	//他クラス参照
	Easing _Easing;

	//以下、フェード時間、リソース情報等の雑多な変数
	[SerializeField]
	float _fade_total_time = 1.0f;
	[SerializeField]
	Image[] _fade_images;
	[SerializeField]
	Text _text;
	float _image_alfa = 0.0f;
	[SerializeField]
	bool _do_fade;
	public bool _DoFade
	{
		get{ return _do_fade;}
		set{_do_fade = value; }
	}

	// Use this for initialization
	void Start () {

		//アルファ値を０にして、見えない状態にしておく.
		_image_alfa = 0.0f;
		_Easing = GameObject.FindObjectOfType<Easing> ();
		foreach (var image in _fade_images) {
			image.enabled = true;
			image.color = new Color (1,1,1,_image_alfa);
		}
		_text.enabled = true;
		_text.color =  new Color (1,1,1,_image_alfa);

	}
	
	// Update is called once per frame
	void Update () {
	
		//フラグがたったら
		//if (_do_fade) {
			//テスト用
			//IEnumerator a = FadeInOutScreenImage ();
			//a.MoveNext();
			//_do_fade = false;
			//StartCoroutine(FadeInOutScreenImage());


		//}

	}

	//FadeInシステムをコルーチン化してみたインゴット
	public IEnumerator FadeInOutScreenImage(float fade_total_time)
	{
		float alfa_min = _image_alfa;
		float alfa_max;
		float time = 0;

		//アルファ値が0以上なら、フェードイン
		//それ以外なら、フェードアウト
		if (alfa_min == 0) 
		{
			alfa_max = 1;
		} 
		else 
		{
			alfa_max = 0;
		}
			
		Debug.Log ("フェードを開始いたします");
		//フェードイン、またはフェードアウトを開始
		while (time < fade_total_time)
		{
			//カウンタを更新
			time += Time.deltaTime;

			//アルファ値がターゲット値になるまで
			_image_alfa = (float)_Easing.InQuad (time, fade_total_time,alfa_max, alfa_min);

			//フェードに使われる全イメージ、テキストのアルファ値を更新
			_text.color = new Color (0, 0, 0, _image_alfa);
			foreach (var image in _fade_images) {
				image.color = new Color (1, 1, 1, _image_alfa);
			}


			//一旦抜ける
			yield return null;
		
		}
		//微量なずれを調整、処理終了
		_image_alfa = alfa_max;
		Debug.Log ("フェードを終了致します");
		yield return null;

	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreText : MonoBehaviour {

    //スコア参照用
    ScoreManager _score_reference;

    //score text
    private Text _score_text;
    private float _current_score_value;
    private float _score_value;


    //視聴率色　（低、中（ノルマ）、高視聴率）未実装
   /* Color _low_rating = Color.black;
    Color __Midium_rating = Color.green;
    Color _high_rating = Color.cyan;
    */




    // Use this for initialization
    void Start () {

        _score_reference = GameObject.FindObjectOfType<ScoreManager>();
        _score_text = GetComponent<Text>();
        
        //デフォルト値
		_current_score_value = 0.001f;
        ////Debug 用　test値
        
        //Score textを描画
        //_score_text.text = _current_score_value.ToString();

    }

    // Update is called once per frame
    void Update () {

        //スコア取得 //(毎回取得してるので無駄、あとでリファ)
        _score_value = _score_reference.ScoreValue;


        //表示してるスコアが実際のスコアと差があったら
        if (_current_score_value != _score_value) {

            //徐々にスコアを加算
			_current_score_value += 0.075f;
             
            //表示スコアが実際のスコアと同じ、またはそれ以上になったら
            if (_current_score_value >= _score_value) {
                
                //ずれがないよう調整しておく
                _current_score_value = _score_value;

            }

			//Score textを描画
            _score_text.text = _current_score_value.ToString();
        }
        
	}


    
}

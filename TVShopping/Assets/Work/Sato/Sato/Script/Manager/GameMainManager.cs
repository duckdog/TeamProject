using UnityEngine;
using System.Collections;

public class GameMainManager : MonoBehaviour {

    //進行時間
    private float _main_timer = 0;
    public float MainGameTimer
    { get { return _main_timer;}}
    //ミニゲーム等のイベント（画面が変わるような）ときの時間停止
    //または一時停止.
    private bool _do_stop_game;
    public bool doStopGame {get{ return _do_stop_game; }
                            set { _do_stop_game = value;} }


    //分岐
    public enum Root {
    
		Main,
		AbeMain,//α用
		JonMain,//アルファ用

        RootA,
        RootB,
		RootC,






    }
    public Root CurrentRoot{ get; set; }





	// Use this for initialization
	void Start () {
		_main_timer = 0;
        //StartCoroutine(WaitForSeconds());

    }
	
	// Update is called once per frame
	void Update () {
        
        //ミニゲーム中以外はタイマーを更新
        if(_do_stop_game)
        {
            //更新
            _main_timer += Time.deltaTime;

        }


        // クリックでリザルトへ遷移(※書き換えＯＫ！！※)
        if (Input.GetMouseButtonDown(0))
        {
			// Application.LoadLevel("Result");
        }

    }
    
    
    
    /*
    IEnumerator WaitForSeconds() {
        Debug.Log("0:" + Time.deltaTime);
        yield return new WaitForSeconds(1.0f);
        Debug.Log("1:" + Time.deltaTime);
        yield return new WaitForSeconds(1.0f);
        Debug.Log("2:" + Time.deltaTime);
        yield return new WaitForSeconds(1.0f);
        Debug.Log("3:" + Time.deltaTime);
        yield return new WaitForSeconds(1.0f);
     
    }*/

}

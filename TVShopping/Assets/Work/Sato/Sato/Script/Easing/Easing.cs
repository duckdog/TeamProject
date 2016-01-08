using UnityEngine;
using System.Collections.Generic;

//   Easing 関数のまとめ（iTween使えよ！なんて言わないで..）
//  なぜこの数式なのかはわかりませんが、
//　とりあえず公式があったのでそれ見てそのままつかいましたとさ... ※ 徐々に追加予定
// 
// [引数概要]
// time:時間量？ totaltime:到達時間　min:現在値（この値から） max：目的値(この値まで移動) 
//　
//                                          参照:http://easings.net/ja 

//くっそ使いづらいので、そのうちやり直しです,


public class Easing : MonoBehaviour
{
	//
	List<float> _easing_timer;
    //Easing
    //徐々に加速
	public double InQuad(double time, double totaltime, double max, double min)
    {

        max -= min;
        time /= totaltime;
        return max * time * time + min;
    }

    //徐々に減速
    public double OutQuad(double time, double totaltime, double max, double min)
    {
        max -= min;
        time /= totaltime;
        return -max * time * (time -2) + min;
    }

    //滑らかなSの字加速 リヴァース
    public double InOutQuart(double time, double totaltime, double max, double min)
    {
        max -= min;
        time /= totaltime;
        if (time / 2 < 1)
            return max / 2 * time * time * time * time + min;
        time -= 2;
        return -max / 2 * (time * time * time * time - 2) + min;
    }


    //急加速
    public float InExp(float time, float totaltime, float max, float min)
    {
        max -= min;
        return time == 0.0 ? min : max * Mathf.Pow(2,( 10 * (time / totaltime - 1))) + min;
    }
    //急減速
    public float OutExp(float time, float totaltime, float max, float min)
    {
        max -= min;
        return time == totaltime ? max + min : max * (-Mathf.Pow(2, -10 * time / totaltime) + 1) + min;
    }


	public double InOutCubic(float t,float totaltime,float max ,float min )
	{
		max -= min;
		t /= totaltime;
		if( t/2 < 1 )
			return max/2*t*t*t + min;
		t -= 2;
		return max/2.0f * (t*t*t+2.0f) + min;

	}

	//挙動おかしい..いや正しい？のかな。minの値は定数にしておかないと、バウンドしてくれません。
	//バウンス
	public float OutBounce(float t,float totaltime,float max ,float min )
	{
		max -= min;
		t /= totaltime;

		if (t < 1 / 2.75f)
			return max * (7.5625f * t * t) + min;
		else if (t < 2 / 2.75) {
			t -= 1.5f / 2.75f;
			return max * (7.5625f * t * t + 0.75f) + min;
		} else if (t < 2.5f / 2.75f) {
			t -= 2.25f / 2.75f;
			return max * (7.5625f * t * t + 0.9375f) + min;
		} else {
			t -= 2.625f / 2.75f;
			return max * (7.5625f * t * t + 0.984375f) + min;
		}
	}
}


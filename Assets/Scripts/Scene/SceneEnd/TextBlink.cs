using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour
{
    private Text text;
    public float speed;

    private void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine(FadeInAndOutLoop(text));
    }

    

    private IEnumerator FadeInAndOutLoop(Text obj)
    {
        //标志位，是正向还是反向
        bool forword = true;
        while(true)
        {
            yield return null;
            if (forword)
            {
                var r = obj.color.r;
                var g = obj.color.g;
                var b = obj.color.b;
                var a = obj.color.a + Time.deltaTime * speed;
                obj.color = new Color(r, g, b, a);
                if(obj.color.a>=1)
                {
                    forword = false;
                }
            }
            else
            {
                var r = obj.color.r;
                var g = obj.color.g;
                var b = obj.color.b;
                var a = obj.color.a - Time.deltaTime * speed;
                obj.color = new Color(r, g, b, a);
                if(obj.color.a<=0)
                {
                    forword = true;
                }
            }
        }
    }
}

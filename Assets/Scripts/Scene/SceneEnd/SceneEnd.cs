using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneEnd : MonoBehaviour
{
    public float yPos;
    public GameObject thanks;
    public GameObject returnMain;
    public float moveSpeed;
    public float speed;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return Thanks();
        returnMain.SetActive(true);
        yield return FadeIn(returnMain.GetComponent<Text>());
        yield return WaitAnyKey();
        SceneManager.LoadScene("Assets/Scenes/SceneStart/SceneStart.unity");
    }

    public IEnumerator Thanks()
    {
        while (thanks.GetComponent<RectTransform>().localPosition.y < yPos)
        {
            yield return null;
            thanks.transform.localPosition = new Vector3(0, thanks.transform.localPosition.y + Time.deltaTime * moveSpeed,0);
            Debug.Log(thanks.transform.localPosition);
        }
    }

    public IEnumerator FadeIn(Text obj)
    {
        while (obj.color.a < 1)
        {
            yield return null;
            var r = obj.color.r;
            var g = obj.color.g;
            var b = obj.color.b;
            var a = obj.color.a + Time.deltaTime * speed;
            obj.color = new Color(r, g, b, a);
        }
    }

    public IEnumerator WaitAnyKey()
    {
        while(true)
        {
            yield return null;
            if(Input.anyKeyDown)
            {
                break;
            }
        }
    }
}


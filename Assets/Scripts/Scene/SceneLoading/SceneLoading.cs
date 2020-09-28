using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    public Slider loadingBar;
    private AsyncOperation loading;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(PlayerPrefs.GetString("NextScene"));
        var nextScrene = PlayerPrefs.GetString("NextScene").Split('/');
        var sceneIndex = nextScrene[nextScrene.Length - 1].Remove(0, 5);
        sceneIndex = sceneIndex.Remove(sceneIndex.Length - 6);
        var nextSceneID = int.Parse(sceneIndex);
        if (nextSceneID > PlayerPrefs.GetInt("SceneCount"))
        {
            PlayerPrefs.SetString("NextScene", "Assets/Scenes/SceneEnd/SceneEnd.unity");
        }
        loading = SceneManager.LoadSceneAsync(PlayerPrefs.GetString("NextScene"));
    }

    // Update is called once per frame
    void Update()
    {
        loadingBar.value = loading.progress * loadingBar.maxValue;
    }
}

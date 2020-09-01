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
        loading = SceneManager.LoadSceneAsync(PlayerPrefs.GetString("NextScene"));
    }

    // Update is called once per frame
    void Update()
    {
        loadingBar.value = loading.progress * loadingBar.maxValue;
    }
}

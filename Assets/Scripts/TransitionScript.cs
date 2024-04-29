using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("Scene Transitioner").GetComponentInChildren<Animator>();
    }

    public void BackToMain(string sceneName)
    {
        StartCoroutine(SceneChanger(sceneName));
    }

    IEnumerator SceneChanger(string sceneName)
    {
        anim.Play("Fade_In_Out");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }

    public void QuitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}

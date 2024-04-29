using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehavior : MonoBehaviour
{
    public Animator anim;

    private static MenuBehavior instance_;

    public static MenuBehavior Instance { get { return instance_; } }

    private void Awake()
    {
        if(instance_ != null && instance_ != this)
        {
            Destroy(this.gameObject);
        }
        else 
        { 
            instance_ = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

    }

}

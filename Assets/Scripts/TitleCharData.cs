using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// タイトル画面でキャラクターコンポーネント取得するためのスクリプト
public class TitleCharData : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    public void OnQuitButton()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

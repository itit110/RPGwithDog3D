using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ボタンを押すとSceneの遷移
public class TitleSceneManager : MonoBehaviour
{
    GameObject QuitButton;

    private void Start()
    {
        
        if(QuitButton) OnQuitButton();
    }
    public void OnStartButton()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void OnQuitButton()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

}

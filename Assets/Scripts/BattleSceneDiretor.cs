using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSceneDiretor : MonoBehaviour
{
    public GameObject QuitButton;
    public GameObject RetryBtn;
    public GameObject TitleBtn;

    public void DieAfterUI()
    {
        RetryBtn.SetActive(true);
        TitleBtn.SetActive(true);
        QuitButton.SetActive(true);
    }

    public void OnRetryButton()
    {
        Debug.Log("Retry pressed");
        SceneManager.LoadScene("BattleScene");
    }

    public void OnTitleButton()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void OnQuitButton()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

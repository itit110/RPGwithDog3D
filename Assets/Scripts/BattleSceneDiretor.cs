using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSceneDiretor : MonoBehaviour
{
    public GameObject QuitButton;
    public GameObject RetryButton;
    public GameObject TitleButton;

    public void DieAfterUI()
    {
        RetryButton.SetActive(true);
        TitleButton.SetActive(true);
        QuitButton.SetActive(true);
    }

    public void OnRetryButton()
    {
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

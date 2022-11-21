using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Button resumeBtn = null;
    [SerializeField]
    private Button quitBtn = null;
    // Start is called before the first frame update
    void Start()
    {
        resumeBtn.onClick.AddListener(ResumeGame);
        quitBtn.onClick.AddListener(QuitGame);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ShowPauseMenu()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

}

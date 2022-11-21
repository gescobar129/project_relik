using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Animator transition = null;
    [SerializeField]
    private float transitionTime = 1f;

    public void LoadLevel(int levelIndex)
    {
        StartCoroutine(StartLevelTransition(levelIndex));
    }

    private IEnumerator StartLevelTransition(int levelIndex)
    {
        transition.SetTrigger("Close");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}

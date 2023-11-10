using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private GameObject loadingPopup;
    private void Update()
    {

    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void OpenGame()
    {
        SceneManager.LoadScene(1);
    }
}

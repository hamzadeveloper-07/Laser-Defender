using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float sceneLoadDelay =1.5f;
    ScoreKeeper scoreKeeper;

    void Awake(){
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void LoadGame(){
        scoreKeeper.ResetScore();
        SceneManager.LoadScene("Game");
    }

    public void LoadMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameOver(){
        StartCoroutine(WaitAndDelay("GameOver", sceneLoadDelay));
    }

    public void QuitGame(){
        Debug.Log("Quiting Game...");
        Application.Quit();
    }

    IEnumerator WaitAndDelay(string sceneName, float delay){
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}

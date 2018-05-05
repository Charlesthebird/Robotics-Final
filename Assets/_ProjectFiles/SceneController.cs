using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public GameObject winObj;
    public GameObject loseObj;
    private void Awake()
    {
        winObj.SetActive(false);
        loseObj.SetActive(false);
    }


    public bool upPressed = false;
    public bool downPressed = false;
    public bool leftPressed = false;
    public bool rightPressed = false;
    public void UButtonDown() { upPressed = true;
        Debug.Log("UP PRESSED!");
    }
    public void UButtonUp() { upPressed = false; }
    public void DButtonDown() { downPressed = true; }
    public void DButtonUp() { downPressed = false; }
    public void LButtonDown() { leftPressed = true; }
    public void LButtonUp() { leftPressed = false; }
    public void RButtonDown() { rightPressed = true; }
    public void RButtonUp() { rightPressed = false; }

    public void ResetScene()
    {
        SceneManager.LoadScene(0);
    }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject rewindText = null;
    [SerializeField] private GameObject losePanel = null, winPanel = null, pausePanel = null;

    public void ChangeState(bool rewind)
    {
        rewindText.SetActive(rewind);
    }

    public void OnLose()
    {
        losePanel.transform.DOLocalMoveX(0, 0.25f).OnComplete(() => Time.timeScale = 0);
        
    }

    public void OnWin()
    {
        winPanel.transform.DOLocalMoveX(-135, 0.25f).OnComplete(()=>Time.timeScale = 0);
    }

    public void OnPause()
    {
        pausePanel.transform.DOLocalMoveY(-135, 0.25f).OnComplete(() => Time.timeScale = 0);
    }

    public void OnMenuButtonClick()
    {
        Time.timeScale = 1;
        SoundManager.Instance.Click();
        SceneManager.LoadScene("Menu");
    }
    public void OnContinueButtonClick()
    {
        Time.timeScale = 1;
        pausePanel.transform.DOLocalMoveY(900, 0.25f);
        SoundManager.Instance.Click();
    }
    public void OnRetryButtonClick()
    {
        Time.timeScale = 1;
        SoundManager.Instance.Click();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnNextLevelButtonClick()
    {
        Time.timeScale = 1;
        SoundManager.Instance.Click();
        if (SceneManager.GetActiveScene().buildIndex+1 < SceneManager.sceneCount)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

using Chronos;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject rewindText = null,pauseText = null;
    [SerializeField] private GameObject losePanel = null, winPanel = null, pausePanel = null;
    [SerializeField] private GameObject scanLines = null;
    [SerializeField] private GameObject winFirstSelect = null, pauseFirstSelect = null, loseFirstSelect = null;
    private Clock entities;


    private void Start()
    {
        ChangeSelected();
        entities = Timekeeper.instance.Clock("Entities");
    }


    public void ChangeSelected(GameObject selected = null)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected);
        SoundManager.Instance.Slide();
    }

    public void ChangeState(bool rewind)
    {
        rewindText.SetActive(rewind);
    }

    public void OnLose()
    {
        SoundManager.Instance.Slide();
        SoundManager.Instance.Lose();
        scanLines.SetActive(true);
        entities.localTimeScale = 0;
        losePanel.transform.DOLocalMoveX(-135, 0.25f);
        ChangeSelected(loseFirstSelect);
    }

    public void OnWin()
    {
        SoundManager.Instance.Slide();
        SoundManager.Instance.Win();
        scanLines.SetActive(true);
        entities.localTimeScale = 0;
        winPanel.transform.DOLocalMoveX(-135, 0.25f);
        ChangeSelected(winFirstSelect);
    }

    public void OnPause()
    {
        SoundManager.Instance.Slide();
        pauseText.SetActive(true);
        scanLines.SetActive(true);
        GameManager.Instance.SetPaused(true);
        entities.localTimeScale = 0;
        pausePanel.transform.DOLocalMoveY(0, 0.25f);
        ChangeSelected(pauseFirstSelect);
    }

    public void OnMenuButtonClick()
    {
        entities.localTimeScale = 1;
        SoundManager.Instance.Click();
        SceneManager.LoadScene("Menu");
    }
    public void OnContinueButtonClick()
    {
        scanLines.SetActive(false);
        pauseText.SetActive(false);
        GameManager.Instance.SetPaused(false);
        entities.localTimeScale = 1;
        pausePanel.transform.DOLocalMoveY(2000, 0.25f);
        ChangeSelected();
        SoundManager.Instance.Click();
    }
    public void OnRetryButtonClick()
    {
        entities.localTimeScale = 1;
        SoundManager.Instance.Click();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnNextLevelButtonClick()
    {
        entities.localTimeScale = 1;
        SoundManager.Instance.Click();
        if (SceneManager.GetActiveScene().buildIndex+1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    
}

using Chronos;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject rewindText = null,pauseText = null;
    [SerializeField] private GameObject losePanel = null, winPanel = null, pausePanel = null;
    [SerializeField] private GameObject scanLines = null;
    [SerializeField] private SelectorUI retrySelector = null, nextSelector = null, pauseSelector = null;
    private SelectorUI _currentSelector = null;
    private Clock entities;


    private void Start()
    {
        ChangeSelector();
        entities = Timekeeper.instance.Clock("Entities");
    }

    private void ChangeSelector(SelectorUI changeTo = null)
    {
        retrySelector.gameObject.SetActive(false);
        nextSelector.gameObject.SetActive(false);
        pauseSelector.gameObject.SetActive(false);

        if (changeTo != null)
        {
            SoundManager.Instance.Slide();
            changeTo.gameObject.SetActive(true);
            _currentSelector = changeTo;
        }
    }
    public void ChangeState(bool rewind)
    {
        rewindText.SetActive(rewind);
    }

    public void OnLose()
    {
        scanLines.SetActive(true);
        entities.localTimeScale = 0;
        ChangeSelector(retrySelector);
        losePanel.transform.DOLocalMoveX(-135, 0.25f).OnComplete(() => Time.timeScale = 0);
    }

    public void OnWin()
    {
        scanLines.SetActive(true);
        entities.localTimeScale = 0;
        ChangeSelector(nextSelector);
        winPanel.transform.DOLocalMoveX(-135, 0.25f).OnComplete(()=>Time.timeScale = 0);
    }

    public void OnPause()
    {
        pauseText.SetActive(true);
        scanLines.SetActive(true);
        entities.localTimeScale = 0;
        ChangeSelector(pauseSelector);
        pausePanel.transform.DOLocalMoveY(0, 0.25f);
    }

    public void OnMenuButtonClick()
    {
        Time.timeScale = 1;
        SoundManager.Instance.Click();
        SceneManager.LoadScene("Menu");
    }
    public void OnContinueButtonClick()
    {
        scanLines.SetActive(false);
        pauseText.SetActive(false);
        ChangeSelector();
        entities.localTimeScale = 1;
        pausePanel.transform.DOLocalMoveY(2000, 0.25f);
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

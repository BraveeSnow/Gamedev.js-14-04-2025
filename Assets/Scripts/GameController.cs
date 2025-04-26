using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    internal static GameController Instance;
    private static float _dateAngerRate = 0.1F;
    private static float _scoreConvRate = 0.1F;
    private static float _audioTransitionDelay = 0.01F;

    // Game object references
    [Header("Game Object References")]
    [SerializeField] private GameObject _visualNovelCanvas;
    [SerializeField] private GameObject _kitchenView;
    [SerializeField] private GameObject _gameOverPanel;
    private AudioSource _dateAudio;
    private AudioSource _kitchenAudio;
    private Image _balanceBar;
    private Image _balanceBarFill;

    // State
    [Header("Sprites")]
    [SerializeField] private Sprite _balanceHappy;
    [SerializeField] private Sprite _balanceNeutral;
    [SerializeField] private Sprite _balanceAngry;

    private void Start()
    {
        Instance = this;

        _dateAudio = _visualNovelCanvas.GetComponent<AudioSource>();
        _kitchenAudio = _kitchenView.GetComponent<AudioSource>();
        _balanceBar = GameObject.Find("BalanceBar").GetComponent<Image>();
        _balanceBarFill = GameObject.Find("BalanceBarFill").GetComponent<Image>();
    }

    private void Update()
    {
        _balanceBarFill.fillAmount -= _dateAngerRate * Time.deltaTime;

        // Update bar image when neccessary
        if (_balanceBarFill.fillAmount > 0.67F)
        {
            _balanceBar.sprite = _balanceHappy;
        }
        else if (_balanceBarFill.fillAmount > 0.33F)
        {
            _balanceBar.sprite = _balanceNeutral;
        }
        else
        {
            _balanceBar.sprite = _balanceAngry;
        }

        if (_balanceBarFill.fillAmount <= 0)
        {
            SignalGameOver();
        }
    }

    public void SwitchScreen()
    {
        Canvas vnCanvas = _visualNovelCanvas.GetComponent<Canvas>();
        vnCanvas.enabled = !vnCanvas.enabled;
        StartCoroutine(TransitionAudio());
    }

    public void RegisterPlayerScore(int score)
    {
        _balanceBarFill.fillAmount += score * _scoreConvRate;
    }

    private void SignalGameOver()
    {
        _dateAudio.mute = true;
        _kitchenAudio.mute = true;
        _gameOverPanel.SetActive(true);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator TransitionAudio()
    {
        AudioSource toIncrease;
        AudioSource toDecrease;

        if (_dateAudio.mute)
        {
            toIncrease = _dateAudio;
            toDecrease = _kitchenAudio;
        }
        else
        {
            toIncrease = _kitchenAudio;
            toDecrease = _dateAudio;
        }

        toIncrease.mute = false;

        while (toDecrease.volume > 0)
        {
            toIncrease.volume += 0.01F;
            toDecrease.volume -= 0.01F;
            yield return new WaitForSeconds(_audioTransitionDelay);
        }

        toDecrease.mute = true;
    }
}

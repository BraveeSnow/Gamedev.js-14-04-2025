using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    internal static GameController Instance;
    private static float _dateAngerRate = 0.01F;
    private static float _audioTransitionDelay = 0.01F;

    // Game object references
    [SerializeField] private GameObject _visualNovelCanvas;
    [SerializeField] private GameObject _kitchenView;
    private AudioSource _dateAudio;
    private AudioSource _kitchenAudio;
    private Image _balanceBar;

    // State
    [SerializeField] private Texture2D _balanceHappy;
    [SerializeField] private Texture2D _balanceNeutral;
    [SerializeField] private Texture2D _balanceAngry;

    private void Start()
    {
        Instance = this;

        _dateAudio = _visualNovelCanvas.GetComponent<AudioSource>();
        _kitchenAudio = _kitchenView.GetComponent<AudioSource>();
        _balanceBar = GameObject.Find("BalanceBarFill").GetComponent<Image>();
    }

    private void Update()
    {
        _balanceBar.fillAmount -= _dateAngerRate * Time.deltaTime;
    }

    public void SwitchScreen()
    {
        Canvas vnCanvas = _visualNovelCanvas.GetComponent<Canvas>();
        vnCanvas.enabled = !vnCanvas.enabled;
        StartCoroutine(TransitionAudio());
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

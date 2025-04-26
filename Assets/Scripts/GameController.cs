using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    internal static GameController Instance;
    private static float _audioTransitionDelay = 0.01F;

    // Game object references
    [SerializeField] private GameObject _visualNovelCanvas;
    [SerializeField] private GameObject _kitchenView;
    private AudioSource _dateAudio;
    private AudioSource _kitchenAudio;

    private void Start()
    {
        Instance = this;

        _dateAudio = _visualNovelCanvas.GetComponent<AudioSource>();
        _kitchenAudio = _kitchenView.GetComponent<AudioSource>();
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

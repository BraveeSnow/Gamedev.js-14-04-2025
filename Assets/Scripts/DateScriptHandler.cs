using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using System.Collections;

public class DateScriptHandler : MonoBehaviour
{
    // Data
    private List<Script> _dateScripts;
    private Dictionary<string, List<string>> _excuses;

    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private List<GameObject> _responseButtons;

    [Header("Adjustable Fields")]
    [SerializeField]
    [Range(0.01F, 0.05F)] private float _textSpeed;
    [SerializeField]
    [Range(1F, 5F)] private float _responseDelay;

    // State
    private Script _currentScript;

    private void Start()
    {
        // Load dialogue from DateScript.json
        TextAsset dialogueJson = Resources.Load<TextAsset>("DateScript");
        _dateScripts = JsonConvert.DeserializeObject<List<Script>>(dialogueJson.text);

        // Load excuses from Excuses.json
        TextAsset excusesJson = Resources.Load<TextAsset>("Excuses");
        _excuses = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(excusesJson.text);

        ShowResponseButtons(false);
        LoadRandomScript();
    }

    public void LoadRandomScript()
    {
    begin:
        Script script = _dateScripts[Random.Range(0, _dateScripts.Count)];
        if (script.Equals(_currentScript))
        {
            goto begin;
        }

        _currentScript = script;
        _currentScript.responses = _currentScript.responses.OrderBy(_ => Random.value).ToList();
        //_dialogueText.text = _currentScript.prompt;
        StartCoroutine(AnimateText(_currentScript.prompt, true));

        for (int i = 0; i < _responseButtons.Count; i++)
        {
            _responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = _currentScript.responses[i].response;
        }
    }

    // Callback for user response buttons, do not directly call
    // Choice is 0-indexed
    public void SelectChoice(int choice)
    {
        ShowResponseButtons(false);

        int score = _currentScript.responses[choice].score;
        StartCoroutine(AnimateText(score > 0 ? _currentScript.goodResponse : _currentScript.badResponse, false));

        // TODO: make score impact balance bar
    }

    private void ShowResponseButtons(bool toShow)
    {
        _responseButtons.ForEach((button) => button.SetActive(toShow));
    }

    private IEnumerator AnimateText(string text, bool isPrompt)
    {
        _dialogueText.text = "";

        foreach (char c in text)
        {
            _dialogueText.text += c;
            yield return new WaitForSeconds(_textSpeed);
        }

        if (isPrompt)
        {
            ShowResponseButtons(true);
        }
        else
        {
            StartCoroutine(DelayUntilLoadNextScript());
        }
    }

    private IEnumerator DelayUntilLoadNextScript()
    {
        yield return new WaitForSeconds(_responseDelay);
        LoadRandomScript();
    }

    [System.Serializable]
    private class Script
    {
        public string prompt;
        public string attribute;
        public List<Response> responses;
        public string goodResponse;
        public string badResponse;
    }

    [System.Serializable]
    private class Response
    {
        public string response;
        public int score;
    }
}

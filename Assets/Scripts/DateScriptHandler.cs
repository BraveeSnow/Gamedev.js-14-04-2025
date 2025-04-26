using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;

public class DateScriptHandler : MonoBehaviour
{
    // Data
    private List<Script> _dateScripts;
    private Dictionary<string, List<string>> _excuses;

    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private List<GameObject> _responseButtons;
    [SerializeField] private List<GameObject> _excuseButtons;

    [Header("Adjustable Fields")]
    [SerializeField]
    [Range(0.01F, 0.05F)] private float _textSpeed;
    [SerializeField]
    [Range(1F, 5F)] private float _responseDelay;

    // State
    private Script _currentScript;
    private Excuse[] _currentExcuses = new Excuse[4];
    private string _previousAttribute = null;

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
        _previousAttribute = _currentScript.attribute;
        StartCoroutine(AnimateText(score > 0 ? _currentScript.goodResponse : _currentScript.badResponse, false));

        // TODO: make score impact balance bar
    }

    // Callback for excuse button, do not directly call
    public void ExcusePlayer()
    {
        // Toggle between response panel and excuse panel
        if (_excuseButtons[0].activeInHierarchy)
        {
            _excuseButtons[0].transform.parent.gameObject.SetActive(false);
            _responseButtons[0].transform.parent.gameObject.SetActive(true);
            return;
        }
        else
        {
            _responseButtons[0].transform.parent.gameObject.SetActive(false);
            _excuseButtons[0].transform.parent.gameObject.SetActive(true);
        }

        // refresh excuse array
        for (int i = 0; i < _currentExcuses.Count(); i++)
        {
            _currentExcuses[i] = null;
        }
        
        // Load the correct excuse into array
        if (_previousAttribute != null)
        {
            string correctExcuse = _excuses[_previousAttribute][Random.Range(0, _excuses[_previousAttribute].Count)];
            _currentExcuses[Random.Range(0, _currentExcuses.Count())] = new Excuse()
            {
                excuse = correctExcuse,
                attribute = _previousAttribute
            };
        }

        // Build the rest of the array
        for (int i = 0; i < _currentExcuses.Count(); i++)
        {
            // skip correct excuse if exists
            if (_currentExcuses[i] != null)
            {
                continue;
            }

            // place wrong excuses
            string wrongAttribute = _excuses.Keys.ToList()[Random.Range(0, _excuses.Count())];
            while (wrongAttribute == _previousAttribute)
            {
                wrongAttribute = _excuses.Keys.ToList()[Random.Range(0, _excuses.Count())];
            }

            _currentExcuses[i] = new Excuse()
            {
                excuse = _excuses[wrongAttribute][Random.Range(0, _excuses[wrongAttribute].Count())],
                attribute = wrongAttribute
            };
        }

        // Copy excuses over to buttons
        for (int i = 0; i < _currentExcuses.Count(); i++)
        {
            _excuseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = _currentExcuses[i].excuse;
        }
    }

    public void ConfirmExcuse(int choice)
    {
        // TODO: handle balance score

        GameController.Instance.SwitchScreen();
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
            // kinda bad design but leaving it here since there's only two cases
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

    private class Excuse
    {
        public string excuse { get; set; }
        public string attribute { get; set; }
    }
}

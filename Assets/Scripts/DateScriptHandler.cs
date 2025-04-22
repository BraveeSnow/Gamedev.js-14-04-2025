using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;

public class DateScriptHandler : MonoBehaviour
{
    private List<Script> _dateScripts;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private List<GameObject> _responseButtons;

    private Script _currentScript;

    private void Start()
    {
        TextAsset json = Resources.Load<TextAsset>("DateScript");
        _dateScripts = JsonConvert.DeserializeObject<List<Script>>(json.text);
        
        ShowResponseButtons(false);
        LoadRandomScript();
    }

    public void LoadRandomScript()
    {
        _currentScript = _dateScripts[Random.Range(0, _dateScripts.Count)];
        _dialogueText.text = _currentScript.prompt;

        for (int i = 0; i < _responseButtons.Count; i++)
        {
            _responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = _currentScript.responses[i].response;
        }

        ShowResponseButtons(true);
    }

    private void ShowResponseButtons(bool toShow)
    {
        _responseButtons.ForEach((button) => button.SetActive(toShow));
    }

    [System.Serializable]
    private class Script
    {
        public string prompt;
        public List<Response> responses;
        [JsonProperty("good")] public string goodResponse;
        [JsonProperty("bad")] public string badResponse;
    }

    [System.Serializable]
    private class Response
    {
        public string response;
        public int score;
    }
}

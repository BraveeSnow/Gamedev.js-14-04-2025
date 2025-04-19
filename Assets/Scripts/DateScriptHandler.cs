using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DateScriptHandler : MonoBehaviour
{
    [SerializeField] private List<Script> dateScripts;

    private void Start()
    {
        TextAsset json = Resources.Load<TextAsset>("DateScript");
        dateScripts = JsonConvert.DeserializeObject<List<Script>>(json.text);
    }

    [Serializable]
    private class Script
    {
        public string prompt;
        public List<Response> responses;
        [JsonProperty("good")] public string goodResponse;
        [JsonProperty("bad")] public string badResponse;
    }

    [Serializable]
    private class Response
    {
        public string response;
        public int score;
    }
}

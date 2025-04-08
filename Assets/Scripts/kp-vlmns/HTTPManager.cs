using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;        
using UnityEngine.Networking; 
using TMPro;

[System.Serializable]
public class MediaResponse
{
    public List<MediaItem> media;
}

[System.Serializable]
public class MediaItem
{
    public int index;
    public string filename;
}

public class HTTPManager : MonoBehaviour
{
    [Header("UI References")]
    public Button playButton;
    public Button pauseButton;
    public Button seekButton;
    public Button listButton;
    public Button loadButton;

    public TMP_InputField seekMsInput;
    public TMP_InputField loadIndexInput;
    public TMP_Text statusText;

    // If your Crow server is running on localhost:18080, use this base URL:
    public string baseUrl = "http://10.211.181.134:18080";

    void Start()
    {
        // Register button click listeners
        if (playButton != null)  playButton.onClick.AddListener(OnPlayButtonClicked);
        if (pauseButton != null) pauseButton.onClick.AddListener(OnPauseButtonClicked);
        if (seekButton != null)  seekButton.onClick.AddListener(OnSeekButtonClicked);
        if (listButton != null)  listButton.onClick.AddListener(OnListButtonClicked);
        if (loadButton != null)  loadButton.onClick.AddListener(OnLoadButtonClicked);
        loadIndexInput.placeholder.GetComponent<TextMeshProUGUI>().text = "index (e.g. 0 for first media)";
        seekMsInput.placeholder.GetComponent<TextMeshProUGUI>().text = "ms (e.g. 10000 for 10 seconds)";
    }

    // PLAY
    public void OnPlayButtonClicked()
    {
        StartCoroutine(CallPlayEndpoint());
    }

    private IEnumerator CallPlayEndpoint()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(baseUrl + "/play"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError
                || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                UpdateStatus("Play request error: " + webRequest.error);
            }
            else
            {
                UpdateStatus("Play request successful: " + webRequest.downloadHandler.text);
            }
        }
    }

    // PAUSE
    public void OnPauseButtonClicked()
    {
        StartCoroutine(CallPauseEndpoint());
    }

    private IEnumerator CallPauseEndpoint()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(baseUrl + "/pause"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError
                || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                UpdateStatus("Pause request error: " + webRequest.error);
            }
            else
            {
                UpdateStatus("Pause request successful: " + webRequest.downloadHandler.text);
            }
        }
    }

    // SEEK
    public void OnSeekButtonClicked()
    {
        // Get ms from the input field
        if (seekMsInput != null && !string.IsNullOrEmpty(seekMsInput.text))
        {
            StartCoroutine(CallSeekEndpoint(seekMsInput.text));
        }
        else
        {
            UpdateStatus("Please enter a valid ms value before seeking.");
        }
    }

    private IEnumerator CallSeekEndpoint(string msValue)
    {
        string url = baseUrl + "/seek?ms=" + msValue;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError
                || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                UpdateStatus("Seek request error: " + webRequest.error);
            }
            else
            {
                UpdateStatus("Seek request successful: " + webRequest.downloadHandler.text);
            }
        }
    }

    // LIST
    public void OnListButtonClicked()
    {
        StartCoroutine(CallListEndpoint());
    }

    private IEnumerator CallListEndpoint()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(baseUrl + "/list"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError
                || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                UpdateStatus("List request error: " + webRequest.error);
            }
            else
            {
                // Parse the JSON
                string json = webRequest.downloadHandler.text;
                MediaResponse mediaResponse = JsonUtility.FromJson<MediaResponse>(json);

                if (mediaResponse != null && mediaResponse.media != null)
                {
                    // Display or store the list
                    string display = "Available Media:\n";
                    foreach (var media in mediaResponse.media)
                    {
                        display += $"Index: {media.index}, Filename: {media.filename}\n";
                    }
                    UpdateStatus(display);
                }
                else
                {
                    UpdateStatus("Could not parse media list.");
                }
            }
        }
    }

    // LOAD
    public void OnLoadButtonClicked()
    {
        // Get index from the input field
        if (loadIndexInput != null && !string.IsNullOrEmpty(loadIndexInput.text))
        {
            StartCoroutine(CallLoadEndpoint(loadIndexInput.text));
        }
        else
        {
            UpdateStatus("Please enter a valid index before loading.");
        }
    }

    private IEnumerator CallLoadEndpoint(string indexValue)
    {
        string url = baseUrl + "/load?index=" + indexValue;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError
                || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                UpdateStatus("Load request error: " + webRequest.error);
            }
            else
            {
                UpdateStatus("Load request successful: " + webRequest.downloadHandler.text);
            }
        }
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
        else
        {
            Debug.Log(message);
        }
    }

}

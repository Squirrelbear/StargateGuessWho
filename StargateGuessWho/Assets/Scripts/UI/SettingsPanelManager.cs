using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SettingsPanelManager : MonoBehaviour
{
    [SerializeField]
    private string previousServer;
    [SerializeField]
    private string currentServer;

    [SerializeField]
    private UnityEngine.UI.InputField serverInputField;

    [SerializeField]
    private UnityEngine.UI.Text testConnectionText;

    [SerializeField]
    private NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        previousServer = PlayerPrefs.GetString("targetserver", "");
        currentServer = previousServer;
        serverInputField.text = previousServer;
    }

    public void handleServerChanged(string dummyValue)
    {
        string newValue = serverInputField.text;
        testConnectionText.text = "Server Changed: Press Test Connection";
        currentServer = newValue;
        PlayerPrefs.SetString("targetserver", newValue);
        networkManager.setServerURL(newValue);
    }

    public void testConnectionToServer()
    {
        testConnectionText.text = "Starting test connect...";
        StartCoroutine(TestWebDataConnection(currentServer));
    }

    public void resetToLastServer()
    {
        serverInputField.text = previousServer;
        handleServerChanged(previousServer);
    }

    IEnumerator TestWebDataConnection(string address)
    {
        UnityWebRequest www = UnityWebRequest.Get(address + "?action=testconnection");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Something went wrong: " + www.error);
            testConnectionText.text = "Failed Test: " + www.error;
        }
        else
        {
            // Check that the server gives the correct response otherwise it will just indicate
            // that the server is communicating over HTTP.
            SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse(www.downloadHandler.text);

            if (node.HasKey("success"))
            {
                testConnectionText.text = "Connection Success!";
            }
            else
            {
                testConnectionText.text = "Connection Failed. Game server not found.";
            }
        }
    }
}

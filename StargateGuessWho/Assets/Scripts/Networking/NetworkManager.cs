using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class NetworkManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetWebData("http://localhost:7000", "?action=createPlayer&playerName=Peter"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ProcessServerResponse(string rawResponse)
    {
        JSONNode node = JSONNode.Parse(rawResponse);
        Debug.Log("Name: " + node["playerName"] + "\n" + "Auth: " + node["playerAuth"]);
    }

    IEnumerator GetWebData(string address, string query)
    {
        UnityWebRequest www = UnityWebRequest.Get(address + query);
        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Something went wrong: " + www.error);
        } 
        else
        {
            Debug.Log(www.downloadHandler.text);

            ProcessServerResponse(www.downloadHandler.text);
        }
    }
}

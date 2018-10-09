using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
// using static root; 

public class collisiondetection : MonoBehaviour {

	// Use this for initialization
   public string api_key = "";
  // public TextMesh text_mesh;
	void Start () {

	}

  [System.Serializable]
  public class Claim
  {
    public string policy_id;
    public string claim_id;
    public string claim_number;
   }

  [System.Serializable]
  public class AppData
  {

  }

  [Serializable]
  public class Param
  {
    public Param (string _key, string _value) {
      key = _key;
      value = _value;
    }
    public string key;
    public string value;
  }

  private void OnCollisionEnter(Collision collision)
  {
    OpenClaim();
    // root.GetComponent<root>().OpenClaim();
  }

  public void OpenClaim() {
    Debug.Log("pre0");
    List<Param> parameters = new List<Param>();
    parameters.Add(new Param("policy_id", "47e55ad3-5fa7-48c4-80a9-ce1ef51ff6ae"));
    parameters.Add(new Param("incident_type", "Theft"));
    parameters.Add(new Param("incident_cause", "Bad things happened"));
    parameters.Add(new Param("incident_date", "2017-10-16T10:12:02.872Z"));
    parameters.Add(new Param("requested_amount", "1300000"));


    Debug.Log("pre1");
    StartCoroutine(CallAPIClaim("https://sandbox.root.co.za/v1/insurance/claims", parameters));
  }

  IEnumerator CallAPIClaim(String url, List<Param> parameters)
  {

    Debug.Log("1");
    string auth = api_key + ":";
    auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
    auth = "Basic " + auth;

    Debug.Log("2");
    WWWForm form = new WWWForm();

    foreach (var param in parameters) {
      form.AddField(param.key, param.value);
    }

    UnityWebRequest www = UnityWebRequest.Post(url, form);
    www.SetRequestHeader("AUTHORIZATION", auth);
    yield return www.Send();

    if (www.isNetworkError || www.isHttpError)
    {

      Debug.Log(www.downloadHandler.text);
    }
    else
    {
      Debug.Log(www.downloadHandler.text);
      Claim claim = JsonUtility.FromJson<Claim>(www.downloadHandler.text);

      // String text = "Claim ID: " + claim.claim_id;
      String text = "Claim ID: " + claim.claim_number;
      Debug.Log("Claim upload complete!");
      Debug.Log(text);
      // text_mesh.text = text;
    }
    yield return true;
  }
	
	// Update is called once per frame
	void Update () {
		
	}
}

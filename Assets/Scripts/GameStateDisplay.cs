using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateDisplay : MonoBehaviour {
	// Update is called once per frame
	void Update ()
	{
	    GetComponent<Text>().text = Manager.GameManager.State.ToString();
	}
}

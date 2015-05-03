using UnityEngine;
using System.Collections;

public class StartGameButton : MonoBehaviour {

	public void StartTheGame() {
		Application.LoadLevel("Level1");
	}
}

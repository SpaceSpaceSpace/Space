using UnityEngine;
using System.Collections;

public class HoldUpHey : InteractableScript {

	public override void OnInteract()
	{
		UI_Manager.instance.DisplayPauseScreen(true);
	}
}

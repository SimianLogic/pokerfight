using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ScreenSourceDirection
{
	Top,
	Bottom,
	Left,
	Right,
	Instant  //no animation
}

public class Pokerfight : MonoBehaviour 
{

	private GameScreen currentScreen;
	private BoardScreen board;
	void Start () {
		//init
		FutileParams fparams = new FutileParams (true, true, true, true);
		fparams.AddResolutionLevel(1024.0f, 1.0f, 1.0f, ""); //ipad, 1024x768
		fparams.origin = new Vector2 (0.0f, 0.0f);

		Futile.instance.Init (fparams);

		//load our art
		Futile.atlasManager.LoadAtlas("all_cards");
		Futile.atlasManager.LoadAtlas("board_layout");

		//font
		Futile.atlasManager.LoadFont("monaco","monaco_36", "monaco_36", 0.0f, -18.0f);

		board = new BoardScreen ();
		loadScreen (board);
	}

	//adapted a bit from the Banana demo in Futile
	public void loadScreen(GameScreen screen, ScreenSourceDirection direction=ScreenSourceDirection.Instant)
	{
		if(currentScreen == null)
		{
			//if we're first, only option is INSTANT
			screen.willShow();
			currentScreen = screen;
			Futile.stage.AddChild(screen);
			screen.didShow();
			return;
		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}

}

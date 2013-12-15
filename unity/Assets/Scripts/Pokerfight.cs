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

	//screens
	private BoardScreen board;
	private MenuScreen menu;
	private BattleOverlay battle;
	
	private bool hasBattle; //are we showing the battle alert?

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
		Futile.atlasManager.LoadFont("monaco","monaco_36", "monaco_36", 0.0f, -30.0f);

		board = new BoardScreen ();
		board.onGameOver += handleGameOver;

		menu = new MenuScreen ();
		menu.startHandler += onMenuStart;
		loadScreen (menu);
	
		battle = new BattleOverlay();
		battle.onContinue += onBattleContinue;
	}

	public void handleGameOver(Character player)
	{
		//TODO: cache our best heroes for later...
		//TODO: move us to game over screen...

		//come back to the menu: new character!
		battle.RemoveFromContainer();
		hasBattle = false;
		
		menu.player.randomize();
		menu.player.setSword (1);
		menu.player.setShield (1);
		loadScreen (menu);
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

		if (direction == ScreenSourceDirection.Instant) 
		{
			Futile.stage.RemoveChild (currentScreen);
			screen.willShow();
			currentScreen = screen;
			Futile.stage.AddChild(screen);
			screen.didShow();
			return;
		}
	}

	public void onMenuStart(Character player)
	{
		if(hasBattle) return;
		
		menu.AddChild(battle);
		hasBattle = true;
		
		battle.player.mimic (menu.player);
		battle.enemy.randomize();
	}
	
	public void onBattleContinue()
	{
		if(currentScreen == menu)
		{
			board.player.mimic (menu.player);
			board.enemy.mimic (battle.enemy);
			
			loadScreen (board, ScreenSourceDirection.Instant);			
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}

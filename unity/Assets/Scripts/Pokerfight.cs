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
	private RecapScreen recap;

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
		board.onCombat += handleCombat;
		board.onYouWin += handleYouWin;

		menu = new MenuScreen ();
		menu.startHandler += onMenuStart;

		battle = new BattleOverlay();
		battle.onContinue += onBattleContinue;
		
		recap = new RecapScreen();
		
		loadScreen(menu);
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
	
	
	
	//------------- BOARD SCREEN LISTENERS
	public void handleCombat(Character attacker)
	{
		board.AddChild (battle);
		
		//update levels and healths and such
		battle.player.mimic (board.player);
		battle.enemy.mimic (board.enemy);
		battle.refresh ();
		
		//important to pass battle's characters for equality checks
		if(attacker == board.player)
		{
			battle.playSequence(battle.player, battle.enemy);
		}else{
			battle.playSequence (battle.enemy, battle.player);
		}
	}
	
	
	public void handleGameOver()
	{
		//TODO: cache our best heroes for later...
		//TODO: move us to game over screen...

		//come back to the menu: new character!
		battle.RemoveFromContainer();
		
		recap.winner.mimic (board.enemy);
		recap.loser.mimic (board.player);
		recap.showLose();
		
		Futile.stage.AddChild(recap);
		
		recap.onContinue -= onRecapNextFight;
		recap.onContinue += onRecapToMenu;
	}
	
	public void handleYouWin()
	{
		//come back to the menu: new character!
		battle.RemoveFromContainer();
		
		recap.winner.mimic (board.player);
		recap.loser.mimic (board.enemy);
		recap.showWin();
		
		Futile.stage.AddChild(recap);
		
		recap.onContinue += onRecapNextFight;
		recap.onContinue -= onRecapToMenu;
	}

	
	//------------- MENU LISTENERS
	public void onMenuStart(Character player)
	{	
		menu.buttons["start"].isEnabled = false;
		menu.AddChild(battle);
		
		battle.healthFlashy.RemoveFromContainer();
		battle.player.mimic (menu.player);
		battle.enemy.randomize();
	}
	
	//------------- RECAP ALERT LISTENERS
	public void onRecapToMenu()
	{
		recap.RemoveFromContainer();
		menu.buttons["start"].isEnabled = true;
		
		menu.player.randomize();
		menu.player.setSword (1);
		menu.player.setShield (1);
		menu.player.health = 100;
		menu.player.maxHealth = 100;
		menu.player.attack = 10;
		menu.player.defense = 10;
		
		loadScreen (menu);
	}
	
	//eventually this would take you to the store instead
	public void onRecapNextFight()
	{
		recap.RemoveFromContainer();
		Futile.stage.AddChild(battle);
		
		board.nextFight();
		battle.healthFlashy.RemoveFromContainer();
		battle.player.mimic (menu.player);
		battle.enemy.randomize();
		battle.refresh();
	}
	
	//------------- BATTLE ALERT LISTENERS
	public void onBattleContinue()
	{
		battle.RemoveFromContainer();
		
		if(currentScreen == menu)
		{
			board.player.mimic (menu.player);
			board.enemy.mimic (battle.enemy);
			
			loadScreen (board, ScreenSourceDirection.Instant);			
		}
		
		if(currentScreen == board)
		{
			board.player.mimic (battle.player);
			board.enemy.mimic (battle.enemy);
			board.refresh();
			
			board.willShow();
			board.didShow();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(currentScreen == board)
		{
			board.update();
		}
	}

}

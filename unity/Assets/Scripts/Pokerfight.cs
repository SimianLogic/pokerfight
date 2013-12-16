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
	
	
	private Character hof1;
	private Character hof2;
	private Character hof3;
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
		
		initHof();
	}
	
	public void initHof()
	{
		//MAGIC NUMBER
//		hof1 = new Character();
//		hof2 = new Character();
//		hof3 = new Character();
//		
//		hof1.scale = 2.0f;
//		hof2.scale = 2.0f;
//		hof3.scale = 2.0f;
//		
//		float box_size = 170;
//		float padding = (box_size-hof1.width)/2;
//		
//		player.x = positions ["player_1"].x + padding;
//		player.y = positions ["player_1"].y - padding - player.height;
		
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
//		int p1_kills = PlayerPrefs.GetInt ("HOF_1_Kills");
//		int p2_kills = PlayerPrefs.GetInt ("HOF_2_Kills");
//		int p3_kills = PlayerPrefs.GetInt ("HOF_3_Kills");
//		
//		//no time to do proper sorting...
//		if(board.player.kills > p1_kills)
//		{
//			saveHOF(board.player, 1);
//		}else if(board.player.kills > p2_kills){
//			saveHOF(board.player, 2);
//		}else if(board.player.kills > p3_kills){
//			saveHOF(board.player, 3);
//		}

		//come back to the menu: new character!
		battle.RemoveFromContainer();
		
		recap.winner.mimic (board.enemy);
		recap.loser.mimic (board.player);
		recap.showLose();
		
		Futile.stage.AddChild(recap);
		
		recap.onContinue -= onRecapNextFight;
		recap.onContinue += onRecapToMenu;
	}
	
	public void saveHOF(Character player, int slot)
	{
		PlayerPrefs.SetInt("HOF_" + slot + "_Kills", player.kills);
		PlayerPrefs.SetString("HOF_" + slot + "_Data", player.toString());
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

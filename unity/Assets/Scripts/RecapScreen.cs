using UnityEngine;
using System.Collections;

public class RecapScreen : GameScreen
{
	public event ContinueHandler onContinue;
	
	public Character winner;
	public Character loser;

	public FSprite youWin;
	public FSprite youLose;
	public FSprite coin;
	public RecapScreen() : base("battle_complete_screen")
	{
		FSprite modal = new FSprite("modal");
		this.AddChildAtIndex(modal, 0);
		modal.width = 1024;
		modal.height = 768;
		modal.x = 512;
		modal.y = 384;
		
	
		youWin = new FSprite("You_Won_the_Fight");
		youWin.x = positions["You_Won_the_Fight"].x + youWin.width/2;
		youWin.y = positions["You_Won_the_Fight"].y - youWin.height/2;
		
		youLose = new FSprite("Game_Over!");
		youLose.x = positions["Game_Over!"].x + youLose.width/2;
		youLose.y = positions["Game_Over!"].y - youLose.height/2;
		
		
		coin = new FSprite("coin");
		coin.anchorX = 0.0f;
		coin.anchorY = 1.0f;
		coin.x = positions["coin"].x;
		coin.y = positions["coin"].y;
		
		this.buttons ["continue"].SignalPress += continueHandler;
		
		
		winner = new Character();
		
		//MAGIC NUMBER
		winner.scale = 4.0f;
		
		winner.x = positions ["winner_anchor"].x - winner.width/2;
		winner.y = positions ["winner_anchor"].y - winner.height/2;
		
		
		loser = new Character();
		
		loser.scale = 4.0f;
		loser.rotation = 90.0f;
		
		loser.x = positions ["loser_anchor"].x - loser.height/2;
		loser.y = positions ["loser_anchor"].y + loser.width/2;
		
		this.AddChild (loser);
		this.AddChild (winner);
		
	}
	
	public void showWin()
	{
		this.AddChild (youWin);
		this.AddChild (coin);
		
		int purse = RXRandom.Range (50,150);
		winner.coins += purse;
		
		setText ("reward", purse + "");
		setText ("reward_copy", "Your Reward");
		
		youLose.RemoveFromContainer();
	}
	
	public void showLose()
	{
		youWin.RemoveFromContainer();
		coin.RemoveFromContainer();
		this.AddChild(youLose);
		
		setText ("reward", loser.kills + "");
		setText ("reward_copy","Kill Count");
	}
	
	public void continueHandler(FButton button)
	{
		if(onContinue != null)
		{
			onContinue();
		}
	}
}


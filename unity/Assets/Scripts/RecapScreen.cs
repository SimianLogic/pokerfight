using UnityEngine;
using System.Collections;

public class RecapScreen : GameScreen
{
	public event ContinueHandler onContinue;
	
	public Character winner;
	public Character loser;

	public FSprite youWin;
	public FSprite youLose;
	public RecapScreen() : base("battle_complete_screen")
	{
		FSprite modal = new FSprite("modal");
		this.AddChildAtIndex(modal, 0);
		modal.width = 1024;
		modal.height = 768;
		modal.x = 512;
		modal.y = 384;

		youWin = images["You_Won_the_Fight"];
		youLose = images["Game_Over!"];
		
		images["winner_anchor"].RemoveFromContainer();
		images["loser_anchor"].RemoveFromContainer();
		images["coin"].RemoveFromContainer();
				
		youWin.RemoveFromContainer();
		youLose.RemoveFromContainer();

		this.buttons ["continue"].SignalPress += continueHandler;
		
		winner = new Character();
		
		//MAGIC NUMBER
		winner.scale = 4.0f;
		
		winner.x = positions["winner_anchor"].x;
		winner.y = positions["winner_anchor"].y;
		
		
		loser = new Character();
		
		loser.scale = 4.0f;
		loser.rotation = 90.0f;
		
		loser.x = positions["loser_anchor"].x;
		loser.y = positions["loser_anchor"].y;
		
		this.AddChild (loser);
		this.AddChild (winner);
		
	}
	
	public void showWin()
	{
		this.AddChild(youWin);
		this.AddChild(images["coin"]);
		
		int purse = RXRandom.Range(50,150);
		winner.coins += purse;
		
		setText ("reward", purse + "");
		setText ("reward_copy", "Your Reward");
		
		youLose.RemoveFromContainer();
	}
	
	public void showLose()
	{
		youWin.RemoveFromContainer();
		images["coin"].RemoveFromContainer();
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


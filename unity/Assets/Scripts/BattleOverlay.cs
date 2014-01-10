using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void ContinueHandler();

public class BattleOverlay : GameScreen
{
	public event ContinueHandler onContinue;
	
	private Character attacker;
	public Character player;
	public Character enemy;
	
	public FSprite playerSlash;
	public FSprite enemySlash;
	public Dictionary<PokerHand, FSprite> topHands;
	public Dictionary<PokerHand, FSprite> bottomHands;
	
	public FSprite modal;
	public FLabel healthFlashy;
	
	public string[] superlatives;
	
	public BattleOverlay():base("battle_screen")
	{
	
		modal = new FSprite("modal");
		this.AddChildAtIndex(modal, 0);
		modal.width = 1024;
		modal.height = 768;
		
		superlatives = new string[] {"Royal_Flush", "Straight_Flush", "Four_of_a_Kind", "Full_House", "Flush", "Straight", "Three_of_a_Kind", "Two_Pair", "One_Pair", "JUNK"};
		foreach(string title in superlatives)
		{
			this.RemoveChild(images[title]);
		}
		this.RemoveChild(images["p1_slash"]);
		this.RemoveChild(images["p2_slash"]);
		this.RemoveChild(images["defense_anchor"]);
		this.RemoveChild(images["attack_anchor"]);
				
		player = new Character();
		this.AddChild (player);

		player.scale = 2.0f;
		
		player.x = positions ["battle_player_1"].x;
		player.y = positions ["battle_player_1"].y;		
		
		enemy = new Character();
		this.AddChild (enemy);
		
		enemy.scale = 2.0f;
		
		enemy.x = positions ["battle_player_2"].x;
		enemy.y = positions ["battle_player_2"].y;
		
		addProgressBar("p1Health", "p1_health_progress_fill");
		addProgressBar("p2Health", "p2_health_progress_fill");
		addProgressBar("p1Attack", "p1_attack_progress_fill");
		addProgressBar("p1Defense", "p1_defense_progress_fill");
		
		this.buttons ["continue"].SignalPress += continueHandler;
		
		//initialize all our superlatives
		topHands = new Dictionary<PokerHand, FSprite>();
		bottomHands = new Dictionary<PokerHand, FSprite>();
		for(int i = 0; i <= (int)PokerHand.RoyalFlush; i++)
		{
			PokerHand hand = (PokerHand)i;
			FSprite top_sprite = new FSprite(Card.stringForHand(hand));
			top_sprite.x = positions["attack_anchor"].x;
			top_sprite.y = positions["attack_anchor"].y;
			topHands[hand] = top_sprite;
			
			FSprite bot_sprite = new FSprite(Card.stringForHand(hand));
			bot_sprite.x = positions["defense_anchor"].x;
			bot_sprite.y = positions["defense_anchor"].y;
			bottomHands[hand] = bot_sprite;
		}
		
		healthFlashy = new FLabel("monaco", "-99");
		
		playerSlash = new FSprite("p1_slash");
		playerSlash.anchorX = 0.0f;
		playerSlash.anchorY = 1.0f;
		
		enemySlash = new FSprite("p2_slash");
		enemySlash.anchorX = 0.0f;
		enemySlash.anchorY = 1.0f;
		
		refresh();
	}
	
	public void refresh()
	{
		setText ("p2_defense_level", enemy.defenseLevel + "");
		setText ("p2_attack_level", enemy.attackLevel + "");
		setText ("p1_defense_level", player.defenseLevel + "");
		setText ("p1_attack_level",player.attackLevel + "");
		
		progress["p1Health"].scaleX = player.health / (float)player.maxHealth;
		progress["p2Health"].scaleX = enemy.health / (float)enemy.maxHealth;
		
		//p1Attac, p1Defense...
	}
	
	public void playSequence(Character attacker, Character defender)
	{
		healthFlashy.RemoveFromContainer();
		this.attacker = attacker;
		
		Debug.Log ("ATTACKER IS WIELDING " + attacker.attackStance + "/" + attacker.defenseStance);
		Debug.Log ("DEFENDER IS WIELDING " + defender.attackStance + "/" + defender.defenseStance);
		
		showHands(attacker.attackStance, defender.defenseStance);
	}
	
	public void continueHandler(FButton button)
	{
		if(onContinue != null)
		{
			onContinue();
		}
	}
	
	public void showSlash()
	{
		if(attacker == player)
		{
			slashRight();
		}else{
			slashLeft();
		}
	}
	
	//long term, should add another anchor point to be the end point so there's less magic numbers
	public void slashRight()
	{
		this.AddChild(playerSlash);
		playerSlash.scale = 0.5f;
		playerSlash.x = positions["p1_slash"].x;
		playerSlash.y = positions["p1_slash"].y;
		
		Go.to (playerSlash, 0.2f, new TweenConfig()
			.floatProp ("x",positions["p1_slash"].x + 400)
			.floatProp ("y", positions["p1_slash"].y - 100)
			.floatProp ("scale",1.5f)
			.onComplete (thisTween => {
				this.RemoveChild(playerSlash);
				showDamage();
			})
		);
	}
	
	public void slashLeft()
	{
		this.AddChild(enemySlash);
		enemySlash.scale = 0.5f;
		enemySlash.x = positions["p2_slash"].x;
		enemySlash.y = positions["p2_slash"].y;
		
		Go.to (enemySlash, 0.2f, new TweenConfig()
			.floatProp ("x",positions["p2_slash"].x - 400)
			.floatProp ("y", positions["p2_slash"].y - 100)
			.floatProp ("scale",1.5f)
			.onComplete (thisTween => {
				this.RemoveChild (enemySlash);
				showDamage();
			})
		);
	}
	
	public float damageMultiplierForDefense(PokerHand hand)
	{
		if(hand == PokerHand.RoyalFlush) return 0.02f;
		if(hand == PokerHand.StraightFlush) return 0.04f;
		if(hand == PokerHand.FourOfAKind) return 0.08f;
		if(hand == PokerHand.FullHouse) return 0.17f;
		if(hand == PokerHand.Flush) return 0.20f;
		if(hand == PokerHand.Straight) return 0.33f;
		if(hand == PokerHand.ThreeOfAKind) return 0.50f;
		if(hand == PokerHand.TwoPair) return 0.75f;
		if(hand == PokerHand.OnePair) return 1.00f;
		
		//JUNK
		return 2.0f;
	}
	
	public float damageMultiplierForAttack(PokerHand hand)
	{
		if(hand == PokerHand.RoyalFlush) return 50f;
		if(hand == PokerHand.StraightFlush) return 25f;
		if(hand == PokerHand.FourOfAKind) return 12f;
		if(hand == PokerHand.FullHouse) return 6f;
		if(hand == PokerHand.Flush) return 5f;
		if(hand == PokerHand.Straight) return 3f;
		if(hand == PokerHand.ThreeOfAKind) return 2f;
		if(hand == PokerHand.TwoPair) return 1.5f;
		if(hand == PokerHand.OnePair) return 1.00f;
		
		//JUNK
		return 0.5f;		
	}
	
	public void showDamage()
	{
		int damage;
		if(attacker == player)
		{
		
			damage = (int)(damageMultiplierForAttack(player.attackStance) * damageMultiplierForDefense(enemy.defenseStance) * player.attack);
			Debug.Log ("DEAL " + damage + " TO ENEMY");
			enemy.health = Mathf.Max (0, enemy.health - damage);
		}else{
			damage = (int)(damageMultiplierForAttack(enemy.attackStance) * damageMultiplierForDefense(player.defenseStance) * player.attack);
			Debug.Log ("DEAL " + damage + " TO PLAYER");
			player.health = Mathf.Max (0, player.health - damage);
		}
		
		this.AddChild (healthFlashy);
		healthFlashy.text = "-" + damage;
		healthFlashy.scale = 1.0f;
		healthFlashy.x = positions["defense_anchor"].x;
		healthFlashy.y = positions["defense_anchor"].y - healthFlashy.textRect.height/2;
		Go.to (healthFlashy, 0.1f, new TweenConfig().floatProp("scale",1.5f).setEaseType (EaseType.BackOut));
		
		refresh();
	}
	
	public void showHands(PokerHand top, PokerHand bot)
	{
		Debug.Log (top + " vs " + bot);
		
		this.AddChild (topHands[top]);
		this.AddChild (bottomHands[bot]);
		
		//set our initial state in caes we were used before...
		topHands[top].alpha = 1.0f;
		bottomHands[bot].alpha = 0.0f;
		
		topHands[top].scale = 0.5f;
		bottomHands[top].scale = 0.5f;
		
		Go.to (topHands[top], 0.25f, new TweenConfig().floatProp ("scale",1.0f)
			.setEaseType(EaseType.BackOut)
			.onComplete(firstTween => {
				bottomHands[bot].alpha = 1.0f;
				Go.to (bottomHands[bot], 0.1f, new TweenConfig().floatProp ("scale",1.0f)
					.setEaseType(EaseType.BackOut)
					.onComplete( secondTween => { 
						Go.to (topHands[top], 0.25f, new TweenConfig().floatProp ("alpha",0.0f).setDelay (0.5f));
						Go.to (bottomHands[bot], 0.1f, new TweenConfig().floatProp ("alpha",0.0f).setDelay (0.5f)
							.onComplete(thirdTween => {
								this.RemoveChild (topHands[top]);
								this.RemoveChild (topHands[bot]);
								showSlash();
							})
				       );
				    })
				);
			})
		);
	}
}


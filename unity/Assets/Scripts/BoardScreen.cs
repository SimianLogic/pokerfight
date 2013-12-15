using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void GameOverEventHandler();
public delegate void CombatHandler(Character attacker);

public class BoardScreen : GameScreen
{
	private FContainer playerHand;
	private List<Card> deck;
	
	private Card[] attack;
	private Card[] defense;

	private Card activeCard;

	public Character player;
	public Character enemy;
	
	public event GameOverEventHandler onGameOver;
	public event CombatHandler onCombat;

	public BoardScreen() : base("background|0|0:dealt_card|516|484:deck|364|484:Attack|446|54:Defense|436|423:player_1|8|518:p1_sword|190|550:p1_shield|186|640:p1_sword_level|4|695:p1_shield_level|86|703:p1_attack_level_label|31|697:p1_defense_level_label|123|697:p1_heart|186|703:p1_initiative_icon|165|492:player_2|845|518:p2_sword|803|550:p2_shield|799|640:p2_sword_level|843|695:p2_shield_level|923|703:p2_attack_level_label|868|697:p2_defense_level_label|960|697:p2_heart|799|703:p2_initiative_icon|778|492:defense_5|777|264:defense_4|622|264:defense_3|414|264:defense_2|262|264:defense_1|110|264:attack_5|777|54:attack_4|622|54:attack_3|414|110:attack_2|262|54:attack_1|110|54:text_p2_initiative|691|502|000000|Monaco|right|36|88|30:text_p2_attack|691|571|000000|Monaco|right|36|88|30:text_p2_defense|691|646|000000|Monaco|right|36|88|30:text_p2_health|692|716|000000|Monaco|right|36|87|30:text_p2_attack_level|869|716|000000|Monaco|left|36|43|30:text_p2_defense_level|959|716|000000|Monaco|left|36|43|30:progress_2_bg|691|536:progress_2_fill|694|539:text_p1_initiative|235|501|000000|Monaco|left|36|88|30:text_p1_attack|235|570|000000|Monaco|left|36|88|30:text_p1_defense|235|645|000000|Monaco|left|36|88|30:text_p1_health|235|716|000000|Monaco|left|36|88|30:text_p1_attack_level|33|716|000000|Monaco|left|36|43|30:text_p1_defense_level|123|716|000000|Monaco|left|36|43|30:progress_1_bg|235|536:progress_1_fill|238|539:btn_surrender_down|399|693:btn_surrender_up|399|693")
	{

		//build the deck
		deck = new List<Card>();
		string[] suits = new string[4] {"spade", "club", "diamond","heart"};
		for (int i = 0; i < 4; i++) 
		{
			for(int j = 1; j <= 13; j++)
			{
				Card card = new Card(suits[i], j);
				deck.Add(card);
			}
		}

		FSprite background = new FSprite ("board_layout");
		background.x = 512;
		background.y = 384;
		this.AddChildAtIndex (background, 0);

		this.buttons ["surrender"].SignalPress += surrenderHandler;

		player = new Character();
		this.AddChild (player);
		
		//MAGIC NUMBER
		float box_size = 170;
		player.scale = 2.0f;
		
		float padding = (box_size-player.width)/2;
		
		player.x = positions ["player_1"].x + padding;
		player.y = positions ["player_1"].y - padding - player.height;
		
		
		enemy = new Character();
		this.AddChild (enemy);
		
		enemy.scale = 2.0f;
		
		enemy.x = positions ["player_2"].x + padding;
		enemy.y = positions ["player_2"].y - padding - player.height;
		
		addProgressBar ("p1_init", "progress_1_fill", "progress_1_fill");
		addProgressBar ("p2_init", "progress_2_fill", "progress_1_fill");
		
		progress["p1_init"].scaleX = 0.4f;
		progress["p2_init"].scaleX = 0.7f;
	}
	
	override public void willShow()
	{
		//TODO: make sure all our labels are filled with the right values and put in the right place...
		refresh ();
	}
	
	public void refresh()
	{
		setText("p1_health",player.health + "");
		setText("p2_health",enemy.health + "");
		
		setText("p1_defense_level",player.defenseLevel + "");		
		setText("p1_attack_level",player.attackLevel + "");
		setText("p2_defense_level",enemy.defenseLevel + "");
		setText("p2_attack_level",enemy.attackLevel + "");
		
		
		//BOGUS McBOGUSON
		setText("p1_attack","24");	
		setText("p1_defense","4");
		setText("p1_initiative","30");
		
		setText("p2_attack",""+RXRandom.Range (4,24));
		setText("p2_defense",""+RXRandom.Range (4,24));
		setText("p2_initiative","45");
		
	}
	
	// Use this for initialization
	override public void didShow ()
	{
		reshuffle ();
		deck.RemoveAll(item => item == null);
		
		Debug.Log ("DECK HAS " + deck.Count + " CARDS");
		dealAttackCard ();
	}

	void surrenderHandler(FButton button)
	{
		if(onGameOver != null)
		{
			onGameOver();
		}
	}

	private TweenConfig dealAttackConfig;
	void dealAttackCard()
	{
		if(attack[2] == null)
		{
			Debug.Log ("DEAL ATTACK CARD");
		}else{
			Debug.Log ("ALREADY HAVE A ATTACK CARD");
			return;
		}
		
		Card first_attack = deck [0];
		deck.RemoveAt (0);
		attack [2] = first_attack;
		
		this.AddChild (first_attack);
		first_attack.show ();
		
		//		first_attack.rotation = 90;
		//		first_defense.rotation = 90;
		
		first_attack.x = positions ["deck"].x + first_attack.width/2;
		first_attack.y = positions ["deck"].y - first_attack.height/2;
		
		if(dealAttackConfig == null)
		{
			dealAttackConfig = new TweenConfig ()
				.floatProp ("rotation", 90.0f)
				.floatProp("x", positions ["attack_3"].x + first_attack.height / 2)
				.floatProp("y", positions ["attack_3"].y - first_attack.width / 2)
				.setEaseType(EaseType.BackOut)
				.onComplete(thisTween => {
					dealDefenseCard();
				});
		}
		
		Go.to (first_attack, 0.5f,dealAttackConfig);
	}
	
	private TweenConfig dealDefenseConfig;
	void dealDefenseCard()
	{
		if(defense[2] == null)
		{
			Debug.Log ("DEAL DEFENSE CARD");
		}else{
			Debug.Log ("ALREADY HAVE A DEFENSE CARD");
			return;
		}
		Card first_defense = deck [0];
		deck.RemoveAt (0);
		defense [2] = first_defense;
		
		this.AddChild (first_defense);
		first_defense.show ();
		
		first_defense.x = positions ["deck"].x + first_defense.width/2;
		first_defense.y = positions ["deck"].y - first_defense.height/2;
		
		if(dealDefenseConfig == null)
		{
			dealDefenseConfig = new TweenConfig ()
				.floatProp ("rotation", 90.0f)
					.floatProp("x", positions ["defense_3"].x + first_defense.height / 2)
					.floatProp("y", positions ["defense_3"].y - first_defense.width / 2)
					.setEaseType(EaseType.BackOut)
					.onComplete(thisTween => {
						Debug.Log ("TWEEN DEAL CARD");
						dealCard();
					});
		}
		Go.to(first_defense, 0.5f, dealDefenseConfig);
	}
	
	private TweenConfig dealCardConfig;
	void dealCard()
	{	
		Debug.Log ("DEAL CARD");
		activeCard = deck [0];
		deck.RemoveAt(0);
		
		this.AddChild (activeCard);
		activeCard.rotation = 0;
		activeCard.x = positions ["deck"].x + activeCard.width/2;
		activeCard.y = positions ["deck"].y - activeCard.height/2;
		activeCard.show ();
		
		if(dealCardConfig == null)
		{
			dealCardConfig = new TweenConfig ()
				.floatProp("x",positions ["dealt_card"].x + activeCard.width/2)
					.floatProp("y",positions ["dealt_card"].y - activeCard.height/2)
					.setEaseType(EaseType.BackIn);
		}
		
		Go.to(activeCard, 0.25f, dealCardConfig);
		
		activeCard.draggable = true;
		activeCard.dropHandler += cardDroppedHandler;//new CardDroppedEventHandler(cardDroppedHandler);
	}
	
	void reshuffle() 
	{
		//todo: remove any cards that are on stage
		if(attack != null)
		{
			foreach (Card card in attack) 
			{
				card.RemoveFromContainer();
				deck.Add(card);
			}
			foreach (Card card in defense) 
			{
				card.RemoveFromContainer();
				deck.Add(card);
			}
		}

		if(activeCard != null)
		{
			deck.Add (activeCard);
		}
		
		attack = new Card[5];
		defense = new Card[5];
		
		deck.Shuffle ();
		
		Debug.Log ("DECK HAS " + deck.Count + " CARDS");
	}
	
	void cardDroppedHandler(Card card)
	{
		Vector2 hit_test = new Vector2 (card.x, card.y);
		activeCard = null;
		card.draggable = false;
		
		for(int i = 0; i < 5; i++)
		{
			Rect attack_rect = new Rect(positions["attack_" + (i+1)].x, positions["attack_" + (i+1)].y - card.height, card.width, card.height);
			Debug.Log (attack_rect + " vs " + hit_test);
			if(attack_rect.Contains(hit_test))
			{
				if(attack[i] == null)
				{
					attack[i] = card;
					Go.to(card, 0.1f, new TweenConfig().floatProp("x",positions ["attack_" + (i+1)].x + card.width/2).floatProp("y",positions ["attack_" + (i+1)].y - card.height/2).setEaseType(EaseType.ExpoIn));
					
					card.dropHandler -= cardDroppedHandler;
					
					if(deck.Count > 42)
					{
						dealCard();
					}else{
						Debug.Log ("DECK HAS " + deck.Count);
						resolveAttack();
					}
					return;
				}
			}
			
			Rect defense_rect = new Rect(positions["defense_" + (i+1)].x, positions["defense_" + (i+1)].y - card.height, card.width, card.height);
			if(defense_rect.Contains(hit_test))
			{
				if(defense[i] == null)
				{
					defense[i] = card;
					Go.to(card, 0.1f, new TweenConfig().floatProp("x",positions ["defense_" + (i+1)].x + card.width/2).floatProp("y",positions ["defense_" + (i+1)].y - card.height/2).setEaseType(EaseType.ExpoIn));
					
					card.dropHandler -= cardDroppedHandler;
					
					if(deck.Count > 42)
					{
						Debug.Log ("DECK HAS " + deck.Count);
						dealCard();
					}else{
						resolveAttack();
					}
					return;
				}
			}
			
		}
		
		activeCard = card;
		card.draggable = true;
		Go.to(card, 0.25f, new TweenConfig().floatProp("x",positions ["dealt_card"].x + activeCard.width/2).floatProp("y",positions ["dealt_card"].y - activeCard.height/2).setEaseType(EaseType.ExpoIn));
	}
	
	void resolveAttack()
	{
		player.attackStance = Card.classifyHand(attack);
		player.defenseStance = Card.classifyHand(attack);
		
		if(onCombat != null)
		{
			onCombat(player);
		}
	}

}


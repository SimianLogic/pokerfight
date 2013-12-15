using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardScreen : GameScreen
{
	private FContainer playerHand;
	private List<Card> deck;
	
	private Card[] attack;
	private Card[] defense;

	private Card activeCard;

	public BoardScreen() : base("background|0|0:dealt_card|516|501:deck|364|501:Attack|444|54:Defense|434|423:text_p1_initiative|235|501|000000|Monaco|left|36|88|30:text_p1_attack|235|570|000000|Monaco|left|36|88|30:text_p1_defense|235|645|000000|Monaco|left|36|88|30:text_p1_health|235|716|000000|Monaco|left|36|201|30:player_1|11|526:p1_sword|190|550:p1_shield|186|640:p1_sword_level|4|695:p1_shield_level|86|703:p1_attack_level_label|31|697:p1_defense_level_label|123|697:p1_heart|186|703:p1_initiative|165|492:text_p1_attack_level|33|716|000000|Monaco|left|36|43|30:text_p1_defense_level|123|716|000000|Monaco|left|36|43|30:text_p2_initiative|691|502|000000|Monaco|right|36|88|30:text_p2_attack|691|571|000000|Monaco|right|36|88|30:text_p2_defense|691|646|000000|Monaco|right|36|88|30:text_p2_health|579|716|000000|Monaco|right|36|200|30:player_2|848|526:p2_sword|803|550:p2_shield|799|640:p2_sword_level|843|695:p2_shield_level|923|703:p2_attack_level_label|868|697:p2_defense_level_label|960|697:p2_heart|799|703:p2_initiative|778|492:text_p2_attack_level|869|716|000000|Monaco|left|36|43|30:text_p2_defense_level|959|716|000000|Monaco|left|36|43|30:defense_5|777|264:defense_4|622|264:defense_3|414|264:defense_2|262|264:defense_1|110|264:attack_5|777|54:attack_4|622|54:attack_3|414|110:attack_2|262|54:attack_1|110|54")
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


	}

	override public void willShow()
	{
		//TODO: make sure all our labels are filled with the right values and put in the right place...
	}
	// Use this for initialization
	override public void didShow ()
	{
		reshuffle ();
		dealAttackCard ();
	}

	private TweenConfig dealAttackConfig;
	void dealAttackCard()
	{
		Card first_attack = deck [0];
		deck.RemoveAt (0);
		attack [2] = first_attack;
		
		Futile.stage.AddChild (first_attack);
		first_attack.show ();
		
		//		first_attack.rotation = 90;
		//		first_defense.rotation = 90;
		
		first_attack.x = positions ["deck"].x + first_attack.width/2;
		first_attack.y = positions ["deck"].y - first_attack.height/2;
		
		if(dealAttackConfig == null)
		{
			dealAttackConfig = new TweenConfig ()
				.setDelay (1.0f)
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
		Card first_defense = deck [0];
		deck.RemoveAt (0);
		defense [2] = first_defense;
		
		Futile.stage.AddChild (first_defense);
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
						dealCard();
					});
		}
		Go.to(first_defense, 0.5f, dealDefenseConfig);
	}
	
	private TweenConfig dealCardConfig;
	void dealCard()
	{
		if(activeCard != null)
		{
			activeCard.draggable = false;
			activeCard.dropHandler -= cardDroppedHandler;
		}
		
		activeCard = deck [0];
		deck.RemoveAt(0);
		
		Futile.stage.AddChild (activeCard);
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
				deck.Add(card);
			}
			foreach (Card card in defense) 
			{
				deck.Add(card);
			}
		}
		
		attack = new Card[5];
		defense = new Card[5];
		
		deck.Shuffle ();
		
		Debug.Log ("DECK HAS " + deck.Count + " CARDS");
	}
	
	void cardDroppedHandler(Card card)
	{
		Vector2 hit_test = new Vector2 (card.x, card.y);
		
		for(int i = 0; i < 5; i++)
		{
			Rect attack_rect = new Rect(positions["attack_" + (i+1)].x, positions["attack_" + (i+1)].y - activeCard.height, activeCard.width, activeCard.height);
			Debug.Log (attack_rect + " vs " + hit_test);
			if(attack_rect.Contains(hit_test))
			{
				if(attack[i] == null)
				{
					attack[i] = activeCard;
					Go.to(card, 0.1f, new TweenConfig().floatProp("x",positions ["attack_" + (i+1)].x + activeCard.width/2).floatProp("y",positions ["attack_" + (i+1)].y - activeCard.height/2).setEaseType(EaseType.ExpoIn));
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
			
			Rect defense_rect = new Rect(positions["defense_" + (i+1)].x, positions["defense_" + (i+1)].y - activeCard.height, activeCard.width, activeCard.height);
			if(defense_rect.Contains(hit_test))
			{
				if(defense[i] == null)
				{
					defense[i] = activeCard;
					Go.to(card, 0.1f, new TweenConfig().floatProp("x",positions ["defense_" + (i+1)].x + activeCard.width/2).floatProp("y",positions ["defense_" + (i+1)].y - activeCard.height/2).setEaseType(EaseType.ExpoIn));
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
		
		Go.to(card, 0.25f, new TweenConfig().floatProp("x",positions ["dealt_card"].x + activeCard.width/2).floatProp("y",positions ["dealt_card"].y - activeCard.height/2).setEaseType(EaseType.ExpoIn));
	}
	
	void resolveAttack()
	{
		Debug.Log ("KILL!");
	}

}


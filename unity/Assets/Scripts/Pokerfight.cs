using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pokerfight : MonoBehaviour {


	private FContainer playerHand;
	private List<Card> deck;
	
	private Card[] attack;
	private Card[] defense;

	private static string metadata = "background|0|0:dealt_card|258|250:deck|182|250:Attack|222|27:Defense|217|211:text_p1_initiative|117|250|000000|Monaco|left|18|44|15:text_p1_attack|117|285|000000|Monaco|left|18|44|15:text_p1_defense|117|322|000000|Monaco|left|18|44|15:text_p1_health|117|358|000000|Monaco|left|18|100|15:player_1|5|263:p1_sword|95|275:p1_shield|93|320:p1_sword_level|2|347:p1_shield_level|43|351:p1_attack_level_label|15|348:p1_defense_level_label|61|348:p1_heart|93|351:p1_initiative|82|246:text_p1_attack_level|16|358|000000|Monaco|left|18|21|15:text_p1_defense_level|61|358|000000|Monaco|left|18|21|15:text_p2_initiative|345|251|000000|Monaco|right|18|44|15:text_p2_attack|345|285|000000|Monaco|right|18|44|15:text_p2_defense|345|323|000000|Monaco|right|18|44|15:text_p2_health|289|358|000000|Monaco|right|18|100|15:player_2|424|263:p2_sword|401|275:p2_shield|399|320:p2_sword_level|421|347:p2_shield_level|461|351:p2_attack_level_label|434|348:p2_defense_level_label|480|348:p2_heart|399|351:p2_initiative|389|246:text_p2_attack_level|434|358|000000|Monaco|left|18|21|15:text_p2_defense_level|479|358|000000|Monaco|left|18|21|15:defense_5|388|132:defense_4|311|132:defense_3|207|132:defense_2|131|132:defense_1|55|132:attack_5|388|27:attack_4|311|27:attack_3|207|55:attack_2|131|27:attack_1|55|27";
	private Dictionary<string, Vector2> positions;
	private Dictionary<string, FLabel> labels;

	private Card activeCard;

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
		Futile.stage.AddChild (background);

		processMetadata ();

		reshuffle ();
		dealAttackCard ();
	}

	void processMetadata()
	{
		positions = new Dictionary<string,Vector2> ();

		string[] objects = metadata.Split(":"[0]);
		labels = new Dictionary<string, FLabel> ();
		foreach(string obj in objects)
		{
			string[] data = obj.Split("|"[0]);
			string type = data[0].Split("_"[0])[0];

			int x = System.Int32.Parse(data[1])*2;
			int y = 768 - System.Int32.Parse(data[2])*2;

			if(type == "text")
			{
				FLabel label = new FLabel("monaco","00");
				Debug.Log ("Intializing label " + data[0].Substring (5));
				Futile.stage.AddChild(label);

				int label_width = System.Int32.Parse(data[7])*2;
				Debug.Log (data[5] + " <--- " + data[0]);
				if(data[5] == "center")
				{
					//leave the anchors put
					x += label_width/2;
				}else if(data[5] == "left"){
					label.anchorX = 0.0f;
				}else if(data[5] == "right"){
					label.anchorX = 1.0f;
					x += label_width;
				}

				label.x = x;
				label.y = y;

				labels[data[0].Substring(5)] = label;
				positions[data[0].Substring(5)] = new Vector2(x,y);
			}else{
				//x,y in this point are assuming y is at the top left...
				positions[data[0]] = new Vector2(x,y);
			}
		}


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
	
	// Update is called once per frame
	void Update () {
	
	}

}

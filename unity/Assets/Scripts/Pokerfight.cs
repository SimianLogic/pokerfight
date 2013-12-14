using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pokerfight : MonoBehaviour {


	private FContainer playerHand;
	private List<Card> deck;
	
	private List<Card>attack;
	private List<Card>defense;

	private static string metadata = "background|0|0:dealt_card|258|250:deck|182|250:text_p1_initiative|117|251|000000|Monaco|center|18|43|15:text_p1_attack|117|285|000000|Monaco|center|18|43|15:text_p1_defense|117|323|000000|Monaco|center|18|43|15:text_p1_health|115|359|000000|Monaco|left|18|100|15:player_1|5|263:p1_sword|95|275:p1_shield|93|320:p1_sword_level|2|347:p1_shield_level|43|351:p1_attack_level_label|15|348:p1_defense_level_label|61|348:p1_heart|93|351:p1_initiative|82|246:text_p1_attack_level|16|358|000000|Monaco|left|18|21|15:text_p1_defense_level|61|358|000000|Monaco|left|18|21|15:text_p2_initiative|345|251|000000|Monaco|center|18|43|15:text_p2_attack|345|285|000000|Monaco|center|18|43|15:text_p2_defense|345|323|000000|Monaco|center|18|43|15:text_p2_health|296|359|000000|Monaco|left|18|100|15:player_2|424|263:p2_sword|401|275:p2_shield|399|320:p2_sword_level|421|347:p2_shield_level|461|351:p2_attack_level_label|434|348:p2_defense_level_label|480|348:p2_heart|399|351:p2_initiative|389|246:text_p2_attack_level|434|358|000000|Monaco|left|18|21|15:text_p2_defense_level|479|358|000000|Monaco|left|18|21|15:defense_5|388|132:defense_4|311|132:defense_3|207|132:defense_2|131|132:defense_1|55|132:attack_5|388|27:attack_4|311|27:attack_3|207|55:attack_2|131|27:attack_1|55|27";
	private Dictionary<string, Vector2> positions;

	private Card activeCard;
	private List<Card> dealtCards;
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
		Futile.atlasManager.LoadFont("monaco","monaco_36", "monaco_36", 0.0f, 18.0f);

		//build the deck
		deck = new List<Card>();
		dealtCards = new List<Card>();
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
		dealHand ();
		dealCard ();
	}

	void processMetadata()
	{
		positions = new Dictionary<string,Vector2> ();

		string[] objects = metadata.Split(":"[0]);
		foreach(string obj in objects)
		{
			Debug.Log ("LOOK AT " + obj);
			string[] data = obj.Split("|"[0]);
			Debug.Log (data[0] + " => (" + data[1] + "," + data[2] + ")");
			int x = System.Int32.Parse(data[1])*2;
			int y = 768 - System.Int32.Parse(data[2])*2;

			//x,y in this point are assuming y is at the top left...
			positions[data[0]] = new Vector2(x,y);
		}
	}

	void dealHand()
	{
		Card first_attack = deck [0];
		deck.RemoveAt (0);
		Card first_defense = deck [0];
		deck.RemoveAt (0);

		Futile.stage.AddChild (first_attack);
		Futile.stage.AddChild (first_defense);

		first_attack.rotation = 90;
		first_defense.rotation = 90;

		first_attack.x = positions ["attack_3"].x + first_attack.height/2;
		first_attack.y = positions ["attack_3"].y - first_attack.width/2;

		first_defense.x = positions ["defense_3"].x + first_defense.height/2;
		first_defense.y = positions ["defense_3"].y - first_attack.width/2;

		first_defense.show ();
		first_attack.show ();

		dealtCards.Add (first_attack);
		dealtCards.Add (first_defense);

//		Go.to(_closeButton, 0.5f, new TweenConfig().setDelay(0.0f).floatProp("scale",1.0f).setEaseType(EaseType.BackOut));
	}

	void dealCard()
	{
		//TODO: clean up any listeners on current activeCard
		activeCard = deck [0];
		deck.RemoveAt (0);

		Futile.stage.AddChild (activeCard);
		activeCard.x = positions ["dealt_card"].x + activeCard.width/2;
		activeCard.y = positions ["dealt_card"].y - activeCard.height/2;
		activeCard.show ();

		dealtCards.Add (activeCard);


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

		attack = new List<Card> ();
		defense = new List<Card> ();
		deck.Shuffle ();

		Debug.Log ("DECK HAS " + deck.Count + " CARDS");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}

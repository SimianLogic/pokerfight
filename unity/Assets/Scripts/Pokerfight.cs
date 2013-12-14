using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pokerfight : MonoBehaviour {


	private FContainer playerHand;
	private List<Card> deck;

	void Start () {
		//init
		FutileParams fparams = new FutileParams (true, true, true, true);
		fparams.AddResolutionLevel(1024.0f, 1.0f, 1.0f, ""); //ipad, 1024x768
		fparams.origin = new Vector2 (0.5f, 0.5f);

		Futile.instance.Init (fparams);

		//load our art
		Futile.atlasManager.LoadAtlas("all_cards");

		//build the deck
		deck = new List<Card>();
		string[] suits = new string[4] {"spade", "club", "diamond","heart"};
		for (int i = 0; i < 4; i++) 
		{
			for(int j = 1; j <= 13; j++)
			{
				Card card = new Card(suits[i], j);
				deck.Add(card);
				Futile.stage.AddChild (card);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

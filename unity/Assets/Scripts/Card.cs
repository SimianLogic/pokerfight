using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public delegate void CardDroppedEventHandler(Card card);

public enum PokerHand
{
	Junk,
	OnePair,
	TwoPair,
	ThreeOfAKind,
	Straight,
	Flush,
	FullHouse,
	FourOfAKind,
	StraightFlush,
	RoyalFlush
}

public class Card : FSprite, FSingleTouchableInterface
{
	public event CardDroppedEventHandler dropHandler;
	
	private FAtlasElement back;
	private FAtlasElement front;

	public bool draggable;
	public string suit;
	public int value;
	public Card(string suit, int value): base("card_back")
	{
		this.suit = suit;
		this.value = value;
		this.draggable = false;
	
		//magic numbers... these come from the photoshop layout.
		//TODO: make this dynamic
		this.width = 145;
		this.height = 201;

		FAtlasManager am = Futile.atlasManager;
		front = am.GetElementWithName (buildCard (suit, value));
		back = am.GetElementWithName ("card_back");

		this.EnableSingleTouch ();
	}

	public void show()
	{
		this.element = front;
	}

	public void hide()
	{
		this.element = back;
	}

	private static string buildCard(string suit, int value)
	{
		return suit + "_" + value;
	}



	//TOUCH HANDLERS
	private Vector2 lastTouch;
	private bool isDragging = false;
	public bool HandleSingleTouchBegan(FTouch touch) 
	{
		if (!draggable){ // don't trap touches when paused, this way our replay button can be touched
			return false;
		}

		if (!this.GetTextureRectRelativeToContainer().Contains (touch.position)) 
		{
			return false;
		}

		lastTouch = touch.position;
		isDragging = true;

		return true;
	}
	
	public void HandleSingleTouchMoved(FTouch touch)
	{

		if (!isDragging) return;

		float deltaX = touch.position.x - lastTouch.x;
		float deltaY = touch.position.y - lastTouch.y;

		this.x += deltaX;
		this.y += deltaY;
		
		lastTouch = touch.position;
	}
	
	public void HandleSingleTouchEnded(FTouch touch)
	{
		isDragging = false;
		if (dropHandler != null) 
		{
			dropHandler(this);
		}
	}
	
	public void HandleSingleTouchCanceled(FTouch touch)
	{
		isDragging = false;
		if (dropHandler != null) 
		{
			dropHandler(this);
		}
	}
	
	public static string stringForHand(PokerHand hand)
	{
		if(hand == PokerHand.OnePair) return "One_Pair";
		if(hand == PokerHand.TwoPair) return "Two_Pair";
		if(hand == PokerHand.ThreeOfAKind) return "Three_of_a_Kind";
		if(hand == PokerHand.Straight) return "Straight";
		if(hand == PokerHand.Flush) return "Flush";
		if(hand == PokerHand.FullHouse) return "Full_House";
		if(hand == PokerHand.FourOfAKind) return "Four_of_a_Kind";
		if(hand == PokerHand.StraightFlush) return "Straight_Flush";
		if(hand == PokerHand.RoyalFlush) return "Royal_Flush";
		
		return "JUNK";
	}
	public static PokerHand randomHand()
	{
		return (PokerHand)RXRandom.Range((int)PokerHand.Junk, (int)PokerHand.RoyalFlush + 1);
	}
	
	public static PokerHand classifyHand(Card[] hand)
	{
		List<Card> hand_list = new List<Card>(hand);
		List<Card> sorted_hand = hand_list.OrderBy(x => x.value).ToList();
		
		bool isRoyal = sorted_hand[0].value == 10; //in combination with isFlush and isStraight
		bool isFlush = true;
		bool isStraight = true;
		
		Card lastCard = null;
		List<int> counts = new List<int>();
		int tally = 1;
		int pairs = 0;
		int trips = 0;
		int quads = 0;
		foreach(Card card in sorted_hand)
		{
			if(lastCard == null)
			{
				lastCard = card;
				continue;
			}
			
			Debug.Log ("COMPARE " + card.value + " vs " + lastCard.value);
			if(card.value == lastCard.value)
			{
				tally++;
			}else{
				Debug.Log ("HAD " + tally + " " + card.suit + "_" + card.value);
				counts.Add (tally);
				if(tally == 2) pairs++;
				if(tally == 3) trips++;
				if(tally == 4) quads++;
				tally = 1;
			}
			
			if(card.suit != lastCard.suit) isFlush = false;
			if(card.value == 1)
			{
				if(lastCard.value != 13) isStraight = false;
			}else{
				if(lastCard.value + 1 != card.value) isStraight = false;
			}
			
			lastCard = card;
		}
		if(tally == 2) pairs++;
		if(tally == 3) trips++;
		if(tally == 4) quads++;
		Debug.Log ("HAD " + tally + sorted_hand[4].suit + "_" + sorted_hand[4].value);
		
		
		if(isStraight && isFlush && isRoyal) return PokerHand.RoyalFlush;
		if(isStraight && isFlush) return PokerHand.StraightFlush;
		if(quads == 1) return PokerHand.FourOfAKind;
		if(trips == 1 && pairs == 1) return PokerHand.FullHouse;
		if(isFlush) return PokerHand.Flush;
		if(isStraight) return PokerHand.Straight;
		if(trips == 1) return PokerHand.ThreeOfAKind;
		if(pairs == 2) return PokerHand.TwoPair;
		if(pairs == 1) return PokerHand.OnePair;
				
		return PokerHand.Junk;		
	}

}


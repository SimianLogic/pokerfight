using UnityEngine;
using System.Collections;

public delegate void CardDroppedEventHandler(Card card);

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
}


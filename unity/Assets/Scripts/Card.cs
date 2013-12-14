using UnityEngine;
using System.Collections;

public class Card : FSprite
{

	private FAtlasElement back;
	private FAtlasElement front;
	public Card(string suit, int value): base("card_back")
	{

		FAtlasManager am = Futile.atlasManager;
		front = am.GetElementWithName (buildCard (suit, value));
		back = am.GetElementWithName ("card_back");

		//default to front
		//this.element = front;
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
}


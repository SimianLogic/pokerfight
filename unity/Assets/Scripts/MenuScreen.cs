using UnityEngine;
using System;
using System.Collections;

public delegate void StartEventHandler(Character player);

public class MenuScreen : GameScreen
{
	public Character player;
	public event StartEventHandler startHandler;
	public MenuScreen() : base("main_menu")
	{
		this.buttons ["randomize"].SignalPress += randomizeHandler;
		this.buttons ["start"].SignalPress += startButtonHandler;

		player = new Character();
		player.setShield (1);
		player.setSword (1);
		player.health = 100;
		player.maxHealth = 100;
		player.attack = 10;
		player.defense = 10;
		this.AddChild (player);


		player.scale = 2.0f;
		//MAGIC NUMBER
		float box_size = 160;
		float padding = (box_size-player.width)/2;
		
		player.x = positions ["player"].x;
		player.y = positions ["player"].y;
	}

	public void randomizeHandler(FButton button)
	{
		player.randomizeLook ();
	}

	public void startButtonHandler(FButton button)
	{
		if(startHandler != null)
		{
			startHandler(player);
		}
	}

	override public void willShow()
	{
		
	}

	// Use this for initialization
	override public void didShow ()
	{

	}
}


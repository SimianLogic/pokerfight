using UnityEngine;
using System.Collections;

public delegate void StartEventHandler(Character player);

public class MenuScreen : GameScreen
{
	public Character player;
	public event StartEventHandler startHandler;
	public MenuScreen() : base("background|0|0+Hall_of_Fame|173|346+player|43|-130+Your_Hero|320|-72+btn_start_down|319|-184+btn_start_up|319|-184+btn_randomize_down|43|-251+btn_randomize_up|43|-251+instructions|-334|-214+big_card|-337|151+player_1|-49|222+p1_sword_level|-126|103+p1_shield_level|-41|102+p1_attack_level_label|-86|120+p1_defense_level_label|6|120+text_p1_attack_level|-88|93|000000|Monaco|left|18|43|30+text_p1_kills|-40|49|000000|Monaco|center|18|177|30+text_p1_defense_level|2|93|000000|Monaco|left|18|43|30+player_2|177|222+p2_sword_level|100|103+p2_shield_level|185|102+p2_attack_level_label|140|120+p2_defense_level_label|232|120+text_p2_attack_level|138|93|000000|Monaco|left|18|43|30+text_p2_kills|176|49|000000|Monaco|center|18|177|30+text_p2_defense_level|228|93|000000|Monaco|left|18|43|30+player_3|398|222+p3_sword_level|321|103+p3_shield_level|406|102+p3_attack_level_label|361|120+p3_defense_level_label|453|120+text_p3_attack_level|359|93|000000|Monaco|left|18|43|30+text_p3_kills|402|49|000000|Monaco|center|18|177|30+text_p3_defense_level|449|93|000000|Monaco|left|18|43|30")
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


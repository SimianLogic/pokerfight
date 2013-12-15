using UnityEngine;
using System.Collections;

public class Character : MetaContainer
{
	public static int SIZE = 64;

	public static int NUM_FACES = 1;
	public static int NUM_HEADS = 8;
	public static int NUM_HATS = 25;
	public static int NUM_SHIRTS = 8;
	public static int NUM_PANTS = 8;
	public static int NUM_SHOES = 9;
	public static int NUM_SHIELDS = 36;
	public static int NUM_SWORDS = 37;
	public static int NUM_BOWS = 24;

	public FSprite face;
	public FSprite head;
	public FSprite hat;
	public FSprite shirt;
	public FSprite pants;
	public FSprite shoes;
	public FSprite sword;
	public FSprite bow;
	public FSprite shield;

	public int faceId;
	public int headId;
	public int hatId;
	public int shirtId;
	public int pantsId;
	public int shoesId;
	public int bowId;
	public int swordId;
	public int shieldId;
	
	public int health;
	public int coins;
	public int attackLevel;
	public int defenseLevel;

	public Character(int face_id=1, int head_id=0, int hat_id=-1, int torso_id=0, int legs_id=0, int feet_id=0, int bow_id=-1, int sword_id=-1, int shield_id=-1) : base()
	{
		this.metadata = "face_1|27|12:head_1|22|5:head_2|22|5:head_3|22|5:head_4|22|5:head_5|22|5:head_6|22|5:head_7|22|5:head_8|22|5:head_9|22|5:head_10|22|5:head_11|22|5:head_12|22|5:torso_1|7|30:torso_2|7|30:torso_3|7|30:torso_4|7|30:torso_5|7|30:torso_6|7|30:torso_7|7|30:torso_8|7|30:legs_1|25|47:legs_2|25|47:legs_3|25|47:legs_4|25|47:legs_5|25|47:legs_6|25|47:legs_7|25|47:legs_8|25|47:hat_1|24|8:hat_2|24|8:hat_3|24|8:hat_4|24|8:hat_5|24|8:hat_6|24|8:hat_7|24|8:hat_8|20|1:hat_9|20|1:hat_10|20|1:hat_11|20|1:hat_12|20|1:hat_13|20|1:hat_14|20|1:hat_15|20|1:hat_16|24|8:hat_17|22|0:hat_18|20|0:hat_19|20|0:hat_20|20|0:hat_21|20|0:hat_22|20|0:hat_23|20|0:hat_24|20|0:hat_25|20|0:feet_1|20|57:feet_2|20|57:feet_3|20|57:feet_4|20|57:feet_5|20|57:feet_6|20|57:feet_7|20|57:feet_8|20|57:feet_9|25|57:sword_1|5|19:sword_2|5|19:sword_3|5|19:sword_4|5|19:sword_5|5|19:sword_6|5|19:sword_7|5|19:sword_8|5|19:sword_9|5|19:sword_10|5|19:sword_11|5|19:sword_12|5|19:sword_13|5|19:sword_14|5|13:sword_15|5|4:sword_16|5|13:sword_17|5|4:sword_18|5|13:sword_19|5|4:sword_20|5|13:sword_21|5|4:sword_22|5|13:sword_23|5|4:sword_24|5|13:sword_25|5|4:sword_26|5|13:sword_27|5|4:sword_28|5|13:sword_29|5|4:sword_30|5|13:sword_31|5|4:sword_32|5|13:sword_33|5|4:sword_34|5|13:sword_35|5|4:sword_36|5|13:sword_37|5|4:shield_1|47|36:shield_2|43|35:shield_3|41|30:shield_4|47|36:shield_5|43|35:shield_6|41|30:shield_7|47|36:shield_8|43|35:shield_9|41|30:shield_10|47|36:shield_11|43|35:shield_12|41|30:shield_13|47|36:shield_14|43|35:shield_15|41|30:shield_16|47|36:shield_17|43|35:shield_18|41|30:shield_19|47|36:shield_20|43|35:shield_21|41|30:shield_22|47|36:shield_23|43|35:shield_24|41|30:shield_25|47|36:shield_26|43|35:shield_27|41|30:shield_28|47|36:shield_29|43|35:shield_30|41|30:shield_31|47|36:shield_32|43|35:shield_33|41|30:shield_34|47|36:shield_35|43|35:shield_36|41|30:bow_1|9|29:bow_2|9|26:bow_3|9|19:bow_4|9|29:bow_5|9|26:bow_6|9|19:bow_7|9|29:bow_8|9|26:bow_9|9|19:bow_10|9|29:bow_11|9|26:bow_12|9|19:bow_13|9|29:bow_14|9|26:bow_15|9|19:bow_16|9|29:bow_17|9|26:bow_18|9|19:bow_19|9|29:bow_20|9|26:bow_21|9|19:bow_22|9|29:bow_23|9|26:bow_24|9|19";
		processMetadata (SIZE);
		
		attackLevel = 1;
		defenseLevel = 1;

		//the order is important here for layering
		setHead (head_id);
		setHat (hat_id);
		setFace (face_id);

		setShirt (torso_id);
		setPants (legs_id);
		setShoes (feet_id);
		
		if(bow_id == -1 && sword_id == -1)
		{
			if(RXRandom.Float() > 0.5)
			{
				setSword(-1);
				setBow(0);
			}else{
				setBow(-1);
				setSword(0);
			}
		}else{
			setBow(bow_id);
			setSword(sword_id);
		}
		setShield(shield_id);
	}

	public Character clone()
	{
		Character bob = new Character ();
		bob.mimic (this);
		return bob;
	}

	public void mimic(Character target)
	{
		setHead (target.headId);
		setHat (target.hatId);
		setFace (target.faceId);
		
		setShirt (target.shirtId);
		setPants (target.pantsId);
		setShoes (target.shoesId);
		
		setBow(target.bowId);
		setSword(target.swordId);
		setShield(target.shieldId);
		
		health = target.health;
		coins = target.coins;
		attackLevel = target.attackLevel;
		defenseLevel = target.defenseLevel;
	}

	public float height{
		get{ return SIZE * this.scale;}
	}
	public float width{
		get{ return SIZE * this.scale;}
	}

	
	public void randomize()
	{
		randomizeLook ();
		randomizeGear ();
	}
	
	public void randomizeGear()
	{
		if(RXRandom.Float() > 0.5)
		{
			setSword(-1);
			setBow(0);
		}else{
			setBow(-1);
			setSword(0);
		}
		setShield(-1);
	}
	
	public void randomizeLook()
	{
		setHead (0);
		setHat (-1);
		setShirt (0);
		setPants (0);
		setShoes (0);
	}
	
	public void setBow(int id)
	{
		//-1 is random, 0 is no hat
		if (id == -1) 
		{
			setBow (RXRandom.Range(0,NUM_BOWS+1));
			return;
		}
		
		bowId = id;
		
		if(id == 0)
		{
			if(bow == null)
			{
				return;
			}else{
				//don't remove it...we want to preserve layer order
				bow.alpha = 0.0f;
				return;
			}
		}
		
		setSword (0);
		
		if (bow == null) 
		{
			bow = new FSprite("bow_" + id);
			this.AddChild(bow);
		}else{
			FAtlasManager am = Futile.atlasManager;
			bow.element = am.GetElementWithName ("bow_" + id);
		}
		
		//hats have different sizes, need to reposition each time
		bow.x = positions["bow_" + id].x + bow.width/2;
		bow.y = positions["bow_" + id].y - bow.height/2;
		bow.alpha = 1.0f;
	}
	
	public void setSword(int id)
	{
		//-1 is random, 0 is no hat
		if (id == -1) 
		{
			setSword (RXRandom.Range(0,NUM_SWORDS+1));
			return;
		}
		
		swordId = id;
		
		if(id == 0)
		{
			if(sword == null)
			{
				return;
			}else{
				//don't remove it...we want to preserve layer order
				sword.alpha = 0.0f;
				return;
			}
		}
		
		setBow (0);
		
		if (sword == null) 
		{
			sword = new FSprite("sword_" + id);
			this.AddChild(sword);
		}else{
			FAtlasManager am = Futile.atlasManager;
			sword.element = am.GetElementWithName ("sword_" + id);
		}
		
		//hats have different sizes, need to reposition each time
		sword.x = positions["sword_" + id].x + sword.width/2;
		sword.y = positions["sword_" + id].y - sword.height/2;
		sword.alpha = 1.0f;
	}
	
	
	public void setShield(int id)
	{
		//-1 is random, 0 is no hat
		if (id == -1) 
		{
			setShield (RXRandom.Range(0,NUM_SHIELDS+1));
			return;
		}
		
		shieldId = id;
		
		if(id == 0)
		{
			if(shield == null)
			{
				return;
			}else{
				//don't remove it...we want to preserve layer order
				shield.alpha = 0.0f;
				return;
			}
		}
		
		if (shield == null) 
		{
			shield = new FSprite("shield_" + id);
			this.AddChild(shield);
		}else{
			FAtlasManager am = Futile.atlasManager;
			shield.element = am.GetElementWithName ("shield_" + id);
		}
		
		//hats have different sizes, need to reposition each time
		shield.x = positions["shield_" + id].x + shield.width/2;
		shield.y = positions["shield_" + id].y - shield.height/2;
		shield.alpha = 1.0f;
	}
	

	public void setFace(int id)
	{
		if (id == 0) 
		{
			setFace (RXRandom.Range(1,NUM_FACES+1));
			return;
		}

		faceId = id;

		if (face == null) 
		{
			face = new FSprite("face_" + id);
			this.AddChild (face);

			face.x = positions["face_" + id].x + face.width/2;
			face.y = positions["face_" + id].y - face.height/2;

		}else{
			FAtlasManager am = Futile.atlasManager;
			face.element = am.GetElementWithName ("face_" + id);
		}
	}

	public void setHead(int id)
	{
		if (id == 0) 
		{
			setHead (RXRandom.Range(1,NUM_HEADS+1));
			return;
		}
		
		headId = id;

		if (head == null) 
		{
			head = new FSprite("head_" + id);
			this.AddChild(head);

			head.x = positions["head_" + id].x + head.width/2;
			head.y = positions["head_" + id].y - head.height/2;

		}else{
			FAtlasManager am = Futile.atlasManager;
			head.element = am.GetElementWithName ("head_" + id);
		}
	}

	public void setHat(int id)
	{
		//-1 is random, 0 is no hat
		if (id == -1) 
		{
			setHat (RXRandom.Range(0,NUM_HATS+1));
			return;
		}

		hatId = id;

		if(id == 0)
		{
			if(hat == null)
			{
				return;
			}else{
				//don't remove it...we want to preserve layer order
				hat.alpha = 0.0f;
				return;
			}
		}

		if (hat == null) 
		{
			hat = new FSprite("hat_" + id);
			this.AddChild(hat);
		}else{
			FAtlasManager am = Futile.atlasManager;
			hat.element = am.GetElementWithName ("hat_" + id);
		}

		//hats have different sizes, need to reposition each time
		hat.x = positions["hat_" + id].x + hat.width/2;
		hat.y = positions["hat_" + id].y - hat.height/2;
		hat.alpha = 1.0f;
	}

	public void setShirt(int id)
	{
		if (id == 0) 
		{
			setShirt (RXRandom.Range(1,NUM_SHIRTS+1));
			return;
		}

		shirtId = id;

		if (shirt == null) 
		{
			shirt = new FSprite("torso_" + id);
			this.AddChild(shirt);

			shirt.x = positions["torso_" + id].x + shirt.width/2;
			shirt.y = positions["torso_" + id].y - shirt.height/2;

		}else{
			FAtlasManager am = Futile.atlasManager;
			shirt.element = am.GetElementWithName ("torso_" + id);
		}
	}
	public void setPants(int id)
	{
		if (id == 0) 
		{
			setPants (RXRandom.Range(1,NUM_PANTS+1));
			return;
		}

		pantsId = id;

		if (pants == null) 
		{
			pants = new FSprite("legs_" + id);
			this.AddChild(pants);

			pants.x = positions["legs_" + id].x + pants.width/2;
			pants.y = positions["legs_" + id].y - pants.height/2;

		}else{
			FAtlasManager am = Futile.atlasManager;
			pants.element = am.GetElementWithName ("legs_" + id);
		}
	}

	public void setShoes(int id)
	{
		if (id == 0) 
		{
			setShoes (RXRandom.Range(1,NUM_SHOES+1));
			return;
		}

		shoesId = id;

		if (shoes == null) 
		{
			shoes = new FSprite("feet_" + id);
			this.AddChild(shoes);

			shoes.x = positions["feet_" + id].x + shoes.width/2;
			shoes.y = positions["feet_" + id].y - shoes.height/2;

		}else{
			FAtlasManager am = Futile.atlasManager;
			shoes.element = am.GetElementWithName ("feet_" + id);
		}
	}
}


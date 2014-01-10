using UnityEngine;
using System.Collections;

public class Character : FContainer
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
	
	public int kills;
	public int maxHealth;
	public int health;
	
	public int attack;
	public int defense;
	public int initiative;
	
	public int coins;
	public int attackLevel;
	public int defenseLevel;
	public PokerHand attackStance;
	public PokerHand defenseStance;

	public Character(int face_id=1, int head_id=0, int hat_id=-1, int torso_id=0, int legs_id=0, int feet_id=0, int bow_id=-1, int sword_id=-1, int shield_id=-1) : base()
	{
		setDefaults();
		
		//the order is important here for layering
		setHead(head_id);
		setHat(hat_id);
		setFace(face_id);

		setShirt(torso_id);
		setPants(legs_id);
		setShoes(feet_id);
		
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
	
	private void setDefaults()
	{
		kills = 0;
		
		attack = RXRandom.Range(7,13);
		defense = RXRandom.Range(6,15);
		initiative = 40;
		
		health = defense*10;
		maxHealth = health;
		
		coins = 0;
		attackLevel = 1;
		defenseLevel = 1;
		
		attackStance = PokerHand.Junk;
		defenseStance = PokerHand.Junk;
	}

	public Character clone()
	{
		Character bob = new Character();
		bob.mimic(this);
		return bob;
	}
	
	public string toString()
	{
		return headId + "_" + hatId + "_" + faceId + "_" + shirtId + "_" + pantsId + "_" + shoesId + "_" + bowId + "_" + swordId + "_" + shieldId + "_";
	}
	
	public void fromString(string save)
	{
		string[] data = save.Split("_"[0]);
		
		setHead(System.Int32.Parse(data[0]));
		setHat(System.Int32.Parse(data[1]));
		setFace(System.Int32.Parse(data[2]));
		
		setShirt(System.Int32.Parse(data[3]));
		setPants(System.Int32.Parse(data[4]));
		setShoes(System.Int32.Parse(data[5]));
		
		setBow(System.Int32.Parse(data[6]));
		setSword(System.Int32.Parse(data[7]));
		setShield(System.Int32.Parse(data[8]));
	}

	public void mimic(Character target)
	{
		setHead(target.headId);
		setHat(target.hatId);
		setFace(target.faceId);
		
		setShirt(target.shirtId);
		setPants(target.pantsId);
		setShoes(target.shoesId);
		
		setBow(target.bowId);
		setSword(target.swordId);
		setShield(target.shieldId);
		
		kills = target.kills;
		maxHealth = target.maxHealth;
		health = target.health;
		coins = target.coins;
		
		attack = target.attack;
		defense = target.defense;
		initiative = target.initiative;
		
		attackLevel = target.attackLevel;
		defenseLevel = target.defenseLevel;
		attackStance = target.attackStance;
		defenseStance = target.defenseStance;
	}

	public float height{
		get{ return SIZE * this.scale;}
	}
	public float width{
		get{ return SIZE * this.scale;}
	}

	
	public void randomize()
	{
		randomizeLook();
		randomizeGear();
		setDefaults();
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
		setHead(0);
		setHat(-1);
		setShirt(0);
		setPants(0);
		setShoes(0);
	}
	
	public void setBow(int id)
	{
		//-1 is random, 0 is no hat
		if(id == -1) 
		{
			setBow(RXRandom.Range(0,NUM_BOWS+1));
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
		
		setSword(0);
		
		if(bow == null) 
		{
			bow = new FSprite("bow_" + id);
			this.AddChild(bow);
		}else{
			FAtlasManager am = Futile.atlasManager;
			bow.element = am.GetElementWithName("bow_" + id);
		}

		bow.alpha = 1.0f;
	}
	
	public void setSword(int id)
	{
		//-1 is random, 0 is no hat
		if(id == -1) 
		{
			setSword(RXRandom.Range(0,NUM_SWORDS+1));
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
		
		setBow(0);
		
		if(sword == null) 
		{
			sword = new FSprite("sword_" + id);
			this.AddChild(sword);
		}else{
			FAtlasManager am = Futile.atlasManager;
			sword.element = am.GetElementWithName("sword_" + id);
		}
		
		sword.alpha = 1.0f;
	}
	
	
	public void setShield(int id)
	{
		//-1 is random, 0 is no hat
		if(id == -1) 
		{
			setShield(RXRandom.Range(0,NUM_SHIELDS+1));
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
		
		if(shield == null) 
		{
			shield = new FSprite("shield_" + id);
			this.AddChild(shield);
		}else{
			FAtlasManager am = Futile.atlasManager;
			shield.element = am.GetElementWithName("shield_" + id);
		}
		
		shield.alpha = 1.0f;
	}
	

	public void setFace(int id)
	{
		if(id == 0) 
		{
			setFace(RXRandom.Range(1,NUM_FACES+1));
			return;
		}

		faceId = id;

		if(face == null) 
		{
			face = new FSprite("face_" + id);
			this.AddChild(face);
		}else{
			FAtlasManager am = Futile.atlasManager;
			face.element = am.GetElementWithName("face_" + id);
		}
	}

	public void setHead(int id)
	{
		if(id == 0) 
		{
			setHead(RXRandom.Range(1,NUM_HEADS+1));
			return;
		}
		
		headId = id;

		if(head == null) 
		{
			head = new FSprite("head_" + id);
			this.AddChild(head);
		}else{
			FAtlasManager am = Futile.atlasManager;
			head.element = am.GetElementWithName("head_" + id);
		}
	}

	public void setHat(int id)
	{
		//-1 is random, 0 is no hat
		if(id == -1) 
		{
			setHat(RXRandom.Range(0,NUM_HATS+1));
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

		if(hat == null) 
		{
			hat = new FSprite("hat_" + id);
			this.AddChild(hat);
		}else{
			FAtlasManager am = Futile.atlasManager;
			hat.element = am.GetElementWithName("hat_" + id);
		}
		hat.alpha = 1.0f;
	}

	public void setShirt(int id)
	{
		if(id == 0) 
		{
			setShirt(RXRandom.Range(1,NUM_SHIRTS+1));
			return;
		}

		shirtId = id;

		if(shirt == null) 
		{
			shirt = new FSprite("torso_" + id);
			this.AddChild(shirt);
		}else{
			FAtlasManager am = Futile.atlasManager;
			shirt.element = am.GetElementWithName("torso_" + id);
		}
	}
	public void setPants(int id)
	{
		if(id == 0) 
		{
			setPants(RXRandom.Range(1,NUM_PANTS+1));
			return;
		}

		pantsId = id;

		if(pants == null) 
		{
			pants = new FSprite("legs_" + id);
			this.AddChild(pants);

		}else{
			FAtlasManager am = Futile.atlasManager;
			pants.element = am.GetElementWithName("legs_" + id);
		}
	}

	public void setShoes(int id)
	{
		if(id == 0) 
		{
			setShoes(RXRandom.Range(1,NUM_SHOES+1));
			return;
		}

		shoesId = id;

		if(shoes == null) 
		{
			shoes = new FSprite("feet_" + id);
			this.AddChild(shoes);

		}else{
			FAtlasManager am = Futile.atlasManager;
			shoes.element = am.GetElementWithName("feet_" + id);
		}
	}
}


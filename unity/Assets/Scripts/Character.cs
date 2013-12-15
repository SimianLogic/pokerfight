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

	public FSprite face;
	public FSprite head;
	public FSprite hat;
	public FSprite shirt;
	public FSprite pants;
	public FSprite shoes;

	public Character(int face_id=1, int head_id=0, int hat_id=-1, int torso_id=0, int legs_id=0, int feet_id=0) : base()
	{
		this.metadata = "face_1|27|12:head_1|22|5:head_2|22|5:head_3|22|5:head_4|22|5:head_5|22|5:head_6|22|5:head_7|22|5:head_8|22|5:head_9|22|5:head_10|22|5:head_11|22|5:head_12|22|5:torso_1|7|30:torso_2|7|30:torso_3|7|30:torso_4|7|30:torso_5|7|30:torso_6|7|30:torso_7|7|30:torso_8|7|30:legs_1|25|47:legs_2|25|47:legs_3|25|47:legs_4|25|47:legs_5|25|47:legs_6|25|47:legs_7|25|47:legs_8|25|47:hat_1|24|8:hat_2|24|8:hat_3|24|8:hat_4|24|8:hat_5|24|8:hat_6|24|8:hat_7|24|8:hat_8|20|1:hat_9|20|1:hat_10|20|1:hat_11|20|1:hat_12|20|1:hat_13|20|1:hat_14|20|1:hat_15|20|1:hat_16|24|8:hat_17|22|0:hat_18|20|0:hat_19|20|0:hat_20|20|0:hat_21|20|0:hat_22|20|0:hat_23|20|0:hat_24|20|0:hat_25|20|0:feet_1|20|57:feet_2|20|57:feet_3|20|57:feet_4|20|57:feet_5|20|57:feet_6|20|57:feet_7|20|57:feet_8|20|57:feet_9|25|57";
		processMetadata (SIZE);

		//the order is important here for layering
		setHead (head_id);
		setHat (hat_id);
		setFace (face_id);

		setShirt (torso_id);
		setPants (legs_id);
		setShoes (feet_id);
	}

	public float height{
		get{ return SIZE * this.scale;}
	}
	public float width{
		get{ return SIZE * this.scale;}
	}

	public void randomize()
	{
		setHead (0);
		setHat (-1);
		setShirt (0);
		setPants (0);
		setShoes (0);
	}

	public void setFace(int id)
	{
		if (id == 0) 
		{
			setFace (RXRandom.Range(1,NUM_FACES));
			return;
		}

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
			setHead (RXRandom.Range(1,NUM_HEADS));
			return;
		}

		Debug.Log ("HEAD " + id);
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
			setHat (RXRandom.Range(0,NUM_HATS));
			return;
		}

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

		Debug.Log ("HAT " + id);
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
			setShirt (RXRandom.Range(1,NUM_SHIRTS));
			return;
		}

		Debug.Log ("SHIRT " + id);
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
			setPants (RXRandom.Range(1,NUM_PANTS));
			return;
		}

		Debug.Log ("PANTS " + id);
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
			setShoes (RXRandom.Range(1,NUM_SHOES));
			return;
		}

		Debug.Log ("SHOES " + id);
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


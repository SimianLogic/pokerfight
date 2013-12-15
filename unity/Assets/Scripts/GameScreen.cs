using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScreen : FContainer
{

	public string metadata;
	public Dictionary<string, Vector2> positions;
	public Dictionary<string, FLabel> labels;
	
	public GameScreen(string metadata)
	{	
		this.metadata = metadata;
		processMetadata ();
	}

	void processMetadata()
	{
		positions = new Dictionary<string,Vector2> ();
		
		string[] objects = metadata.Split(":"[0]);
		labels = new Dictionary<string, FLabel> ();
		foreach(string obj in objects)
		{
			string[] data = obj.Split("|"[0]);
			string type = data[0].Split("_"[0])[0];
			
			int x = System.Int32.Parse(data[1]);
			int y = 768 - System.Int32.Parse(data[2]);
			
			if(type == "text")
			{
				FLabel label = new FLabel("monaco","00");
				Debug.Log ("Intializing label " + data[0].Substring (5));
				this.AddChild(label);
				
				int label_width = System.Int32.Parse(data[7]);
				Debug.Log (data[5] + " <--- " + data[0]);
				if(data[5] == "center")
				{
					//leave the anchors put
					x += label_width/2;
				}else if(data[5] == "left"){
					label.anchorX = 0.0f;
				}else if(data[5] == "right"){
					label.anchorX = 1.0f;
					x += label_width;
				}
				
				label.x = x;
				label.y = y;
				
				labels[data[0].Substring(5)] = label;
				positions[data[0].Substring(5)] = new Vector2(x,y);
			}else{
				//x,y in this point are assuming y is at the top left...
				positions[data[0]] = new Vector2(x,y);
			}
		}
		
		
	}

	// Use this for initialization
	virtual public void willShow ()
	{
		
	}
	// Use this for initialization
	virtual public void didShow ()
	{
		
	}


}


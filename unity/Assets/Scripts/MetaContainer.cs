using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MetaContainer : FContainer
{
	public int rootWidth;
	public int rootHeight;
	
	public Dictionary<string, Vector2> positions;
	public Dictionary<string, FLabel> labels;
	public Dictionary<string, FButton> buttons;
	public Dictionary<string, FSprite> progress;
	public Dictionary<string, FSprite> images;	

	public MetaContainer(string metadata_filename) : base()
	{	
		string text = (Resources.Load("Metadata/" + metadata_filename, typeof(TextAsset)) as TextAsset).text;
		processMetadata(text);
	}
	
	public void setText(string field, string text)
	{
		labels[field].text = text;
		labels[field].x = positions[field].x;
		labels[field].y = positions[field].y;
	}
	
	internal void addProgressBar(string name, string fill_name)
	{		
		FSprite progress_bar = images[fill_name];

		progress_bar.anchorX = 0.0f;
		progress_bar.x -= progress_bar.width/2;
		
		progress[name] = progress_bar;
	}
	
	internal void processMetadata(string metadata)
	{
		//master list of all name -> coordinates
		positions = new Dictionary<string,Vector2>();
		
		//lists of our buttons, labels, progress bars, and images
		labels = new Dictionary<string, FLabel>();
		buttons = new Dictionary<string, FButton>();
		progress = new Dictionary<string, FSprite>();
		images = new Dictionary<string, FSprite>();

		string[] objects = metadata.Split("+"[0]);

		foreach(string obj in objects)
		{
			string[] data = obj.Split("|"[0]);
			string type = data[0].Split("_"[0])[0];
			
			//these two don't have an x & a y!
			if(data[0] == "root_width"){
				rootWidth = System.Int32.Parse(data[1]);
				continue;
			}else if(data[0] == "root_height"){
				rootHeight = System.Int32.Parse(data[1]);				
				continue;
			}
			
			int x = System.Int32.Parse(data[1]);
			int y = System.Int32.Parse(data[2]);
			
			Debug.Log(data[0] + " - " + x + "," + y);

			if(type == "btn"){
				if(data[0].IndexOf("_down") >= 0)
				{
					//no need to process a button twice!
					continue;
				}

				string button_name = data[0].Replace("btn_","").Replace("_up","");
				FButton button = new FButton(data[0], "btn_" + button_name + "_down");

				this.AddChild(button);
				button.x = x;
				button.y = y;

				buttons[button_name] = button;
				positions[button_name] = new Vector2(x,y);
			}else if(type == "text"){
			  //TODO: font size
			  //TODO: font color
			  //TODO: font selection (MOAR FONT)
			  
				FLabel label = new FLabel("monaco","00");
				this.AddChild(label);
				
				int label_width = System.Int32.Parse(data[7]);
				label.anchorY = 0.0f;

				if(data[5] == "center")
				{
          //all good! default is centered
				}else if(data[5] == "left"){
					label.anchorX = 0.0f;
	        		x -= label_width / 2;
				}else if(data[5] == "right"){
					label.anchorX = 1.0f;
					x += label_width / 2;
				}
				
				label.x = x;
				label.y = y;
				
				labels[data[0].Substring(5)] = label;
				positions[data[0].Substring(5)] = new Vector2(x,y);
			}else{
				//x,y in this point are assuming y is at the top left...
				positions[data[0]] = new Vector2(x,y);
			
				FSprite sprite = new FSprite(data[0]);
				this.AddChild(sprite);

				Debug.Log("   -> anchorX = " + sprite.anchorX);
				
				sprite.x = x;
				sprite.y = y;
				
				images[data[0]] = sprite;
				positions[data[0]] = new Vector2(x,y);
			}
		}
		
		
	}


}


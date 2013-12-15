using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MetaContainer : FContainer
{

	public string metadata;
	public Dictionary<string, Vector2> positions;
	public Dictionary<string, FLabel> labels;
	public Dictionary<string, FButton> buttons;
	public Dictionary<string, FSprite> progress;

	public MetaContainer() : base()
	{
		//the subclass constructor will provide the metadata and call processMetadata...
		//this is a kludgy way to let the Character set maxHeight to 64 instead of 768
	}

	public MetaContainer(string metadata)
	{	
		this.metadata = metadata;
		processMetadata (768);
	}
	
	public void setText(string field, string text)
	{
		labels[field].text = text;
		labels[field].x = positions[field].x;
		labels[field].y = positions[field].y;
		
		Debug.Log ("put " + field + " at (" + positions[field] + ")");
	}
	
	internal void addProgressBar(string name, string position_name, string fill_name)
	{
		FSprite progress_bar = new FSprite(fill_name);
		progress_bar.anchorX = 0.0f;
		
		this.AddChild(progress_bar);
		
		progress_bar.x = positions[position_name].x;
		progress_bar.y = positions[position_name].y - progress_bar.height/2;
		
		progress[name] = progress_bar;
	}
	
	internal void processMetadata(int maxHeight)
	{
		positions = new Dictionary<string,Vector2>();
		labels = new Dictionary<string, FLabel>();
		buttons = new Dictionary<string, FButton>();
		progress = new Dictionary<string, FSprite>();

		string[] objects = metadata.Split(":"[0]);

		foreach(string obj in objects)
		{
			string[] data = obj.Split("|"[0]);
			string type = data[0].Split("_"[0])[0];
			
			int x = System.Int32.Parse(data[1]);
			int y = maxHeight - System.Int32.Parse(data[2]);

			if(type == "btn"){
				if(data[0].IndexOf("_down") >= 0)
				{
					//no need to process a button twice!
					continue;
				}

				string button_name = data[0].Replace ("btn_","").Replace("_up","");
				FButton button = new FButton(data[0], "btn_" + button_name + "_down");

				this.AddChild(button);
				button.x = x + button.sprite.width/2;
				button.y = y - button.sprite.height/2;

				buttons[button_name] = button;
				positions[button_name] = new Vector2(x,y);
			}else if(type == "text"){
				FLabel label = new FLabel("monaco","00");
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


}


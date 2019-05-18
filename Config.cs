using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

using ReLogic.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
//using Terraria.ModLoader.Config;
//using Terraria.ModLoader.Config.UI;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace DrownedMod
{
    public static class Drowned_Config
    {
		public static string Initial_Flood = "yes";
		public static string FFT = "no";
		public static string FFL = "no";
		public static string FFR = "no";
		public static string FFTL = "no";
		public static string FFTR = "no";
		public static string type = "Water";
		//public static string worldSize = "Large";
		public static int ID = 0;
		public static bool fileExistis = false;
		public static string vers;
		static string ConfigPath = Path.Combine(Main.SavePath, "Mod Configs/TFC's Mods", "Drowned Mod.txt");
		static Preferences Configuration = new Preferences(ConfigPath);
		
		public static bool Load()
        {
            bool success = ReadConfig();

            if(!success)
            {
                ErrorLogger.Log("Check config to see if it's how you want it.");
				CreateConfig();
            }
			
			return success;
        }
		
		public static void CreateConfig()
        {
			if(Configuration.Load())
			{
				
			}
				
			if(!Configuration.Load() || !ReadConfig())
			{
				Configuration.Clear();
				
				Configuration.Put("Should the world flood upon creation yes/no.", Initial_Flood);
				//Configuration.Put("Enables or disables the inital flood (for if your world is already drowned) (You do not need to change this to prevent your world from flooding twice).", "");
				//Configuration.Put("Initial Flood", );
				
				//Configuration.Put("", "");
				//Configuration.Put(" ", "");
				/*
				Configuration.Put("Should the game flood the world from the specified area yes/no.", "These might cause lag. Or mabey they just won't work.");
				Configuration.Put("Flood From Top of the world", FFT);
				Configuration.Put("Flood From Left of the world", FFL);
				Configuration.Put("Flood From Right of the world", FFR);
				Configuration.Put("Flood From Top Left Corner of the world", FFTL);
				
				Configuration.Put("", "");
				Configuration.Put("", "");
				*/
				Configuration.Put("Fluid to fill the world with (Water/Honey/Lava)", type);
				//Configuration.Put("Water, Honey, Lava", "");
				//Configuration.Put("Fluid", type);
				//Configuration.Put("  ", "");
				//Configuration.Put("   ", "");
				
				//Configuration.Put("Large, Medium or Small.", "");
				//Configuration.Put("World size", worldSize);
				
				//Configuration.Put("", "");
				//Configuration.Put("", "");
				
				//Configuration.Put("DO NOT TOUCH", "!!!!");
				Configuration.Put("Version", "1.1.2.3");
				
				Configuration.Save();
			}
			
		}
		
		static bool ReadConfig()
		{
			if(Configuration.Load())
			{
				Configuration.Get<string>("Initail Flood", ref Initial_Flood);
				Configuration.Get<string>("Flood From Top of the world", ref FFT);
				Configuration.Get<string>("Flood From Left of the world", ref FFL);
				Configuration.Get<string>("Flood From Right of the world", ref FFR);
				Configuration.Get<string>("Flood From Top Left Corner of the world", ref FFTL);
				Configuration.Get<string>("Flood From Top Right Corner of the world", ref FFTR);
				Configuration.Get<string>("Fluid", ref type);
				//Configuration.Get<string>("World size", ref worldSize);
				Configuration.Get<string>("Version", ref vers);
			}
			
			if (type == "Water")
			{
				ID = 0;
			} else if (type == "Honey")
			{
				ID = 2;
			} else if (type == "Lava")
			{
				ID = 1;
			}// else {
			//	ID = 2;
			//}
			
			if (vers == "1.1.2.3")
			{
				return true;
			} else
			{
				CreateConfig();
				return false;
			}
		}
	}
}
//Directory.CreateDirectory
//
//AddResource(ModConfig, (Assembly.GetExecutingAssembly() + "ModLoader"), TFCsStuffV10.json)
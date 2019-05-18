using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;

using ReLogic.Graphics;

using Terraria.UI;
using Terraria.IO;
using Terraria.GameContent.UI;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using Terraria.GameContent.Generation;


namespace DrownedMod
{
	public class Startup : ModWorld
	{
		public int right;
		public int bubble;
		
		public void ChangeSpawn()
		{
			if (Drowned_Config.ID == 1)
			{
				Main.spawnTileX -= 0;
				Main.spawnTileY += 4;
			}
		}
				
		public bool IsTileSolid(int x, int y)
		{
			return 
				Main.tile[x, y].active() &&
				Main.tile[x, y].topSlope() ||
				Main.tile[x, y].bottomSlope() ||
				Main.tile[x, y].leftSlope() ||
				Main.tile[x, y].rightSlope() ||
				Main.tile[x, y].halfBrick() ||
				Main.tileSolid[(int)Main.tile[x, y].type]
			;
		}
		
		public override void PostWorldGen()
		{
			bubble = Main.maxTilesY - 203;
			right = Main.maxTilesX - 2;
			
			for (int y = 2; y < bubble; y++)
			{
				for (int x = 2; x <= right; x++)
				{
					RemoveRails(x, y);
					FillATile(Drowned_Config.ID, x, y);
				}
			}
			for (int y = 2; y < bubble; y++)
			{
				for (int x = 2; x <= right; x++)
				{
					switch (Drowned_Config.ID)
					{
						case 0:
							LiquidInteraction(0, 1, 56, x, y);
							LiquidInteraction(0, 2, 229, x, y);
							break;
						case 1:
							LiquidInteraction(1, 0, 56, x, y);
							LiquidInteraction(1, 2, 230, x, y);
							break;
						case 2:
							LiquidInteraction(2, 0, 229, x, y);
							LiquidInteraction(2, 1, 230, x, y);
							break;
					}
				}
			}
			for (int x = 2; x <= right; x++)
			{
				if (!Main.tile[x, bubble].active())
				{
					WorldGen.PlaceTile(x, bubble, 379);
				}
				if (Main.tile[x, bubble].liquid > 0 || Main.tile[x, bubble].active())
				{
					Main.tile[x, bubble].ClearEverything();
					Main.tile[x, bubble].liquid = 0;
					WorldGen.PlaceTile(x, bubble, 379);
				}
			}
			ChangeSpawn();
		}
		
		public void FillATile(int fluidID, int x, int y)
		{
			if (Main.tile[x, y].liquid > 0)
			{
				if (Main.tile[x, y].liquidType() == fluidID)
				{
					Main.tile[x, y].liquid = 255;
					WorldGen.SquareTileFrame(x, y, false);
					return;
				}
			}
			else if (!IsTileSolid(x, y))
			{
				Main.tile[x, y].liquidType(fluidID);
				Main.tile[x, y].liquid = 255;
				WorldGen.SquareTileFrame(x, y, false);
				return;
			}
			else if (!Main.tile[x, y].active())
			{
				Main.tile[x, y].liquidType(fluidID);
				Main.tile[x, y].liquid = 255;
				WorldGen.SquareTileFrame(x, y, false);
				return;
			}
			else if (Main.tile[x, y].type == 19) 
			{
				Main.tile[x, y].liquidType(fluidID);
				Main.tile[x, y].liquid = 255;
				return;
			}
		}

		public void RemoveRails(int x, int y)
		{
			if (Main.tile[x, y].type == 314)
			{
				//Main.tile[x, y].ClearTile();
				if (Main.tile[x, y + 1].liquid > 0)
				{
					Main.tile[x, y].liquidType(Main.tile[x, y + 1].liquidType());
					Main.tile[x, y].liquid = 255;
				}
				if (Main.tile[x + 1, y].liquid > 0)
				{
					Main.tile[x, y].liquidType(Main.tile[x + 1, y].liquidType());
					Main.tile[x, y].liquid = 255;
				}
				if (Main.tile[x - 1, y].liquid > 0)
				{
					Main.tile[x, y].liquidType(Main.tile[x - 1, y].liquidType());
					Main.tile[x, y].liquid = 255;
				}
				if (Main.tile[x, y - 1].liquid > 0)
				{
					Main.tile[x, y].liquidType(Main.tile[x, y - 1].liquidType());
					Main.tile[x, y].liquid = 255;
				}
			}
		}

		public void LiquidInteraction(int fluidID, int oppositeFluid, int interactionBlock, int x, int y)
		{
			if (Main.tile[x, y].liquidType() == oppositeFluid) 
			{
				if 
				(
					Main.tile[x, y - 1].liquidType() == fluidID &&
					Main.tile[x, y - 1].liquid > 0
				)
				{
					if (Main.tile[x, y].type == 314)
					{
						Main.tile[x, y - 1].liquid = 0;
						Main.tile[x, y - 1].ClearTile();
						WorldGen.PlaceTile(x, y - 1, interactionBlock);
					}
					else
					{
						Main.tile[x, y].liquid = 0;
						Main.tile[x, y].ClearTile();
						WorldGen.PlaceTile(x, y, interactionBlock);
					}
				}
				if
				(
					Main.tile[x - 1, y].liquidType() == fluidID &&
					Main.tile[x - 1, y].liquid > 0
				)
				{
					if (Main.tile[x, y].type == 314)
					{
						Main.tile[x - 1, y].liquid = 0;
						Main.tile[x - 1, y].ClearTile();
						WorldGen.PlaceTile(x - 1, y, interactionBlock);
					}
					else
					{
						Main.tile[x, y].liquid = 0;
						Main.tile[x, y].ClearTile();
						WorldGen.PlaceTile(x, y, interactionBlock);
					}
				}
				if
				(
					Main.tile[x + 1, y].liquidType() == fluidID &&
					Main.tile[x + 1, y].liquid > 0
				)
				{
					if (Main.tile[x, y].type == 314)
					{
						Main.tile[x + 1, y].liquid = 0;
						Main.tile[x + 1, y].ClearTile();
						WorldGen.PlaceTile(x + 1, y, interactionBlock);
					}
					else
					{
						Main.tile[x, y].liquid = 0;
						Main.tile[x, y].ClearTile();
						WorldGen.PlaceTile(x, y, interactionBlock);
					}
				}
				if
				(
					Main.tile[x, y + 1].liquidType() == fluidID &&
					Main.tile[x, y + 1].liquid > 0
				)
				{
					if (Main.tile[x, y].type == 314)
					{
						Main.tile[x, y + 1].liquid = 0;
						Main.tile[x, y + 1].ClearTile();
						WorldGen.PlaceTile(x, y + 1, interactionBlock);
					}
					else
					{
						Main.tile[x, y].liquid = 0;
						Main.tile[x, y].ClearTile();
						WorldGen.PlaceTile(x, y, interactionBlock);
					}
				}
			}
		}
	}
	
	public class Text : ModPlayer
	{
		//private int bubbleL = 2197;
		private int bubble = Main.maxTilesY - 200;
		private string wS;
		//private bool Tele = false;
		//private int bubbleS = 998;

		public override void SetControls()
		{
			
						/*
						if ((Main.maxTilesX) == 8400)
						{
							wS = "Large";
						} else if ((Main.maxTilesX) == 6400)
						{
							wS = "Medium";
						} else if ((Main.maxTilesX) == 4200)
						{
							wS = "Small";
						}
						*/
						
			/*Main.NewText("");
			Main.NewText("");
			Main.NewText("");
			Main.NewText("");
			Main.NewText("tX:" + (int)(player.position.X/16));
			Main.NewText("tY:" + (int)(player.position.Y/16));
			Main.NewText("C:" + Main.rockLayer);
			Main.NewText("mTX:" + Main.maxTilesX);
			Main.NewText("mTY:" + Main.maxTilesY);
			//Main.NewText("dFBTBL:" + (Main.maxTilesY - bubbleL));
			//Main.NewText("dFBTBS:" + (Main.maxTilesY - bubbleS));
			Main.NewText("Right:" + (Main.maxTilesX - 2));
			Main.NewText("Bottom:" + (Main.maxTilesY));
			Main.NewText("Heck:" + (Main.maxTilesY - 204));*/
			
			//Main.NewText("tX:" + (int)(player.position.X/16));
			//Main.NewText("tY:" + (int)(player.position.Y/16));
			//Main.NewText("Bottom:" + (Main.maxTilesY));
			//Main.NewText("Heck?:" + (Main.maxTilesY - 203));
			//Main.NewText("Right:" + (Main.maxTilesX));
			//Main.NewText("Done?:" + (startup.done));
			//Main.NewText("Ply.World Size?:" + (wS));
			//Main.NewText("MaxY/Bubble Ratio:" + ((float)Main.maxTilesY/996f));
			
			
			//Drowned_Config.Load();
			if (player.position.Y < 2197)
			{
				if (Drowned_Config.FFL == "yes")
				{
					int k = (int)player.position.Y/16;
					int i = 0;
					if(Main.tile[i, k].liquid > 0)
					{
						if(Main.tile[i, k].liquidType() == Drowned_Config.ID)
						{
							Main.tile[i, k].liquidType(Drowned_Config.ID);
							Main.tile[i, k].liquid = 255;
							WorldGen.SquareTileFrame(i, k, true);
							NetMessage.SendTileSquare(-1, i, k, 1);
						}
					} else if(!Main.tile[i, k].active() || Main.tile[i, k].inActive() || Main.tile[i, k].collisionType == 3) {
						Main.tile[i, k].liquidType(Drowned_Config.ID);
						Main.tile[i, k].liquid = 255;
						WorldGen.SquareTileFrame(i, k, true);
						NetMessage.SendTileSquare(-1, i, k, 1);
					}
				}
				
				if (Drowned_Config.FFR == "yes")
				{
					int k = (int)player.position.Y;
					int i = 8398;
					if(Main.tile[i, k].liquid > 0)
					{
						if(Main.tile[i, k].liquidType() == Drowned_Config.ID)
						{
							Main.tile[i, k].liquidType(Drowned_Config.ID);
							Main.tile[i, k].liquid = 255;
							WorldGen.SquareTileFrame(i, k, true);
							NetMessage.SendTileSquare(-1, i, k, 1);
						}
					} else if(!Main.tile[i, k].active() || Main.tile[i, k].inActive() || Main.tile[i, k].collisionType == 3) {
						Main.tile[i, k].liquidType(Drowned_Config.ID);
						Main.tile[i, k].liquid = 255;
						WorldGen.SquareTileFrame(i, k, true);
						NetMessage.SendTileSquare(-1, i, k, 1);
					}
				}
			}
		}
		
		public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
		{
			if (Drowned_Config.ID == 0 || Drowned_Config.ID == 2)
			{
				Item a = new Item();
				a.SetDefaults(291);
				a.stack = 1;
				items.Add(a);
			}
			if (Drowned_Config.ID == 2)
			{
				Item b = new Item();
				b.SetDefaults(290);
				b.stack = 1;
				items.Add(b);
			}
			if (Drowned_Config.ID == 1)
			{
				Item c = new Item();
				c.SetDefaults(288);
				c.stack = 1;
				items.Add(c);
			}
		}
		
		//	tele = PlayerInput.Triggers.Current.KeyStatus[Tele];
			
		//if (tele)
		//{
		//	player.position = Main.MouseWorld;
		//}
			
	}
		
	//[GlobalMod]
	public class MNPC : GlobalNPC
	{
		public override void NPCLoot(NPC npc)
		{
			if (npc.type == 63)
			{
				
				/*Item.NewItem
				(
					(int)npc.position.X, //spawnpos
					(int)npc.position.Y, //spawnpos
					npc.width /2, //spawn offset
					npc.height /2, //spawn offset
					23, //ID
					Main.rand.Next(3,5), //count?
					false, //idk
					0 //idk
				);*/
				Item.NewItem(npc.getRect(), 23, Main.rand.Next(3,5));
				
			} else if (npc.type == 64 || npc.type == 242 || npc.type == 103)
			{
				
				/*Item.NewItem
				(
					(int)npc.position.X, //spawnpos
					(int)npc.position.Y, //spawnpos
					npc.width /2, //spawn offset
					npc.height /2, //spawn offset
					23, //ID
					Main.rand.Next(1,3), //count?
					false, //idk
					0 //idk
				);*/
				Item.NewItem(npc.getRect(), 23, Main.rand.Next(1,3));
				
			} else if (npc.type == 4)
			{
				
				Item.NewItem
				(
					(int)npc.position.X, //spawnpos
					(int)npc.position.Y, //spawnpos
					npc.width /2, //spawn offset
					npc.height /2, //spawn offset
					268, //ID
					1, //count?
					false, //idk
					0 //idk
				);
				
			} else if (npc.type == 4)
			{
				if (Main.rand.Next(0,1000) == 0)
				{
					Item.NewItem
					(
						(int)npc.position.X, //spawnpos
						(int)npc.position.Y, //spawnpos
						npc.width /2, //spawn offset
						npc.height /2, //spawn offset
						268, //ID
						1, //count?
						false, //idk
						0 //idk
					);
				}
				
			}
		}
	}
	//TerrariaOverhaul
	public class MItem : GlobalItem
	{
		public override void PostUpdate(Item item)
		{
			Mod oH = ModLoader.GetMod("TerrariaOverhaul");
			int i = (int)item.position.X/16;
			int k = (int)item.position.Y/16;
			if (oH != null)
			{
				if (Main.tile[i, k].liquid > 0)
				item.velocity.Y -= 1;
				item.velocity.Y *= -1;
				item.velocity.Y += 1;
			}
		}
	}
}
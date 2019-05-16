using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;
using System;

namespace DrownedMod
{
	public class Drowned : ModWorld
	{
		public override void PostWorldGen()
		{
			for (int y = 3; y < Main.maxTilesY - 150; y++)
			{
				for (int x = 3; x <= Main.maxTilesX - 3; x++)
				{
					if (!Main.tile[x, y].active() && Main.tile[x, y].liquid == 0)
					{
						Main.tile[x, y].liquidType(0);
						Main.tile[x, y].liquid = 255;
						WorldGen.SquareTileFrame(x, y, false);
					}
				}
			}
			for (int x = 3; x < Main.maxTilesX - 3; x++)
			{
				if (!Main.tile[x, Main.maxTilesY - 150].active())
				{
					WorldGen.PlaceTile(x, Main.maxTilesY - 150, 379);
				}
			}
		}
	}
}
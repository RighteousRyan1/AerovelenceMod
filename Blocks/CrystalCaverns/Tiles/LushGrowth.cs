using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using AerovelenceMod.Items.Placeable.CrystalCaverns;

namespace AerovelenceMod.Blocks.CrystalCaverns.Tiles
{
    public class LushGrowth : ModTile
    {
		public static int _type;
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			SetModTree(new CrystalTree());
			Main.tileMerge[Type][mod.TileType("CavernStone")] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileBlockLight[Type] = true;
			AddMapEntry(new Color(105, 211, 029));
			drop = mod.ItemType("LushGrowthItem");
			TileID.Sets.Grass[Type] = true;
			TileID.Sets.NeedsGrassFraming[Type] = true;
			TileID.Sets.NeedsGrassFramingDirt[Type] = mod.TileType("CavernStone");
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (!effectOnly)
			{
				fail = true;
				Main.tile[i, j].type = (ushort)mod.TileType("CavernStone");
				WorldGen.SquareTileFrame(i, j, true);
				Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.Marble, 0f, 0f, 0, new Color(121, 121, 121), 1f);
			}
		}
		public static bool PlaceObject(int x, int y, int type, bool mute = false, int style = 0, int alternate = 0, int random = -1, int direction = -1)
		{
			TileObject toBePlaced;
			if (!TileObject.CanPlace(x, y, type, style, direction, out toBePlaced, false))
			{
				return false;
			}
			toBePlaced.random = random;
			if (TileObject.Place(toBePlaced) && !mute)
			{
				WorldGen.SquareTileFrame(x, y, true);
			}
			return false;
		}
		public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return mod.TileType("CrystalSapling");
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0f;
			g = 0.250f;
			b = 0.050f;
		}
	}
}
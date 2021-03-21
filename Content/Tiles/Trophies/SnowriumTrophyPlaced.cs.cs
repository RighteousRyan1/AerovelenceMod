using AerovelenceMod.Content.Items.Placeables.Trophies;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace AerovelenceMod.Content.Tiles.Trophies
{
	public class SnowriumTrophyPlaced : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			dustType = 7;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Snowrium Trophy");
			AddMapEntry(new Color(120, 85, 60), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 48, 48, ModContent.ItemType<SnowriumTrophy>());

		}
	}
}
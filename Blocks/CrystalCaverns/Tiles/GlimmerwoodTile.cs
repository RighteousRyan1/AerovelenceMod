using Terraria;
using Terraria.ModLoader;
using AerovelenceMod.Items.Placeable.CrystalCaverns;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace AerovelenceMod.Blocks.CrystalCaverns.Tiles
{
    public class GlimmerwoodTile : ModTile
    {
        public override void SetDefaults()
        {
			mineResist = 2.5f;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = false;
            Main.tileLighted[Type] = false;
			AddMapEntry(new Color(068, 077, 098));
			dustType = 59;
			soundType = SoundID.Tink;
			drop = ModContent.ItemType<Glimmerwood>();

        }
    }
}
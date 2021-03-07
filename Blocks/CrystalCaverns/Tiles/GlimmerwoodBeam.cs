using Terraria;
using Terraria.ModLoader;
using AerovelenceMod.Items.Placeable.CrystalCaverns;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace AerovelenceMod.Blocks.CrystalCaverns.Tiles
{
    public class GlimmerwoodBeam : ModTile
    {
        public override void SetDefaults()
        {
			mineResist = 2.5f;
			minPick = 59;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            AddMapEntry(new Color(061, 079, 110));
			dustType = 59;
			soundType = SoundID.Tink;
			drop = ModContent.ItemType<GlimmerwoodBeamItem>();

        }
    }
}
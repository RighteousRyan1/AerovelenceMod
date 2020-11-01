using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AerovelenceMod.Items.Weapons.Ranged
{
    public class AcidicBlaster : ModItem
    {
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acidic Blaster");
		}
        public override void SetDefaults()
        {
			item.shootSpeed = 24f;
			item.crit = 8;
            item.damage = 12;
            item.ranged = true;
            item.width = 32;
            item.height = 32;
            item.useTime = 20;
            item.useAnimation = 20;
			item.UseSound = SoundID.Item5;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 4;
            item.value = Item.sellPrice(0, 0, 35, 20);
            item.rare = ItemRarityID.Green;
			item.shoot = ProjectileID.StyngerShrapnel;
            item.autoReuse = true;
        }
    }
}
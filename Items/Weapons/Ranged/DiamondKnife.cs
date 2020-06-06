using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AerovelenceMod.Items.Weapons.Ranged
{
    public class DiamondKnife : ModItem
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Knife");
		}
        public override void SetDefaults()
        {
			item.UseSound = SoundID.Item1;
			item.crit = 8;
            item.damage = 12;
            item.melee = true;
            item.width = 60;
            item.height = 32;
            item.useTime = 17;
			item.useAnimation = 17;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = Item.sellPrice(0, 25, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.autoReuse = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("DiamondKnifeProjectile");
            item.shootSpeed = 16f;
		}
		public override void AddRecipes()  //How to craft this item
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FallenStar, 10);
			recipe.AddIngredient(ItemID.Diamond, 3);
            recipe.AddIngredient(ItemID.IronBar, 4);
            recipe.AddTile(TileID.Anvils);   //at work bench
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
	
namespace AerovelenceMod.Items.Weapons.Ranged
{
    public class DiamondKnifeProjectile : ModProjectile
    {
		public bool e;
		public float rot = 0.5f;
		public int i;
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 38;
            projectile.friendly = true;
            projectile.penetrate = 3;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 120;
        }
        public override void AI()
        {
			i++;
			if (i % 2 == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width / 2, projectile.height / 2, 132);
			}
			projectile.alpha += 2;
			projectile.velocity *= 0.99f;
			if (!e)
			{
				projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
				e = true;
			}
			projectile.rotation += rot;
			rot *= 0.99f;
        }
    }
}
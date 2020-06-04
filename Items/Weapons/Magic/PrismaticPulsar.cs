using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AerovelenceMod.Projectiles;
using AerovelenceMod;
using AerovelenceMod.Items.Ores.PreHM.Frost;

namespace AerovelenceMod.Items.Weapons.Magic
{
    public class PrismaticPulsar : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            DisplayName.SetDefault("Prismatic Pulsar");
            Tooltip.SetDefault("Burns with the fury of the lost souls");
        }
        public override void SetDefaults()
        {
            item.crit = 11;
            item.damage = 82;
            item.magic = true;
            item.width = 28;
            item.height = 30;
            item.useTime = 65;
            item.useAnimation = 65;
            item.UseSound = SoundID.Item21;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 6;
            item.value = 10000;
            item.rare = ItemRarityID.Purple;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("HomingWisp");
            item.shootSpeed = 40f;
        }
        public static Vector2[] randomSpread(float speedX, float speedY, int angle, int num)
        {
            var posArray = new Vector2[num];
            float spread = (float)(angle * 0.075);
            float baseSpeed = (float)System.Math.Sqrt(speedX * speedX + speedY * speedY);
            double baseAngle = System.Math.Atan2(speedX, speedY);
            double randomAngle;
            for (int i = 0; i < num; ++i)
            {
                randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                posArray[i] = new Vector2(baseSpeed * (float)System.Math.Sin(randomAngle), baseSpeed * (float)System.Math.Cos(randomAngle));
            }
            return (Vector2[])posArray;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2[] speeds = randomSpread(speedX, speedY, 5, 5);
            for (int i = 0; i < 5; ++i)
            {
                type = Main.rand.Next(new int[] { type, ModContent.ProjectileType<HomingWisp>(), ModContent.ProjectileType<HomingWispPurple>() });
                Projectile.NewProjectile(position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ModContent.ItemType<FrostRay>(), 1);
            modRecipe.AddIngredient(ItemID.StaffofEarth, 1);
            modRecipe.AddIngredient(ItemID.SpectreStaff, 1);
            modRecipe.AddIngredient(ItemID.ShadowbeamStaff, 1);
            modRecipe.AddIngredient(ItemID.Ectoplasm, 40);
            modRecipe.AddIngredient(ItemID.ShroomiteBar, 15);
            modRecipe.AddTile(TileID.MythrilAnvil);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
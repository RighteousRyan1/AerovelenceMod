using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AerovelenceMod.Content.Items.Weapons.Magic
{
    public class CrystalSpray : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Spray");
            Tooltip.SetDefault("Casts homing water streams to chase your foes\nAqua Sceptre++");
        }

        public override void SetDefaults()
        {
            item.mana = 6;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 16;
            item.useTime = 8;
            item.knockBack = 5f;
            item.width = 38;
            item.crit = 5;
            item.height = 10;
            item.damage = 78;
            item.scale = 1f;
            item.shoot = ModContent.ProjectileType<CrystalSprayProjectile>();
            item.shootSpeed = 12.5f;
            item.UseSound = SoundID.Item13;
            item.noMelee = true;
            item.rare = ItemRarityID.Yellow;
            item.value = 27000;
            item.magic = true;
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.AquaScepter, 1);
            modRecipe.AddIngredient(ItemID.SpectreStaff, 1);
            modRecipe.AddIngredient(ItemID.HallowedBar, 15);
            modRecipe.AddTile(TileID.MythrilAnvil);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }


    partial class CrystalSprayProjectile : ModProjectile
    {
        private void ApplyTrailFx()
        {
            Projectile proj = projectile;

            //proj.velocity.Y += 0.2f;

            for (int dusts = 0; dusts < 1; dusts++)
            {
                int castAheadDist = 6;
                var pos = new Vector2(
                    proj.position.X + castAheadDist,
                    proj.position.Y + castAheadDist
                );

                for (int subDusts = 0; subDusts < 3; subDusts++)
                {
                    float dustCastAheadX = proj.velocity.X / 3f * (float)subDusts;
                    float dustCastAheadY = proj.velocity.Y / 3f * (float)subDusts;

                    int dustIdx = Dust.NewDust(
                        Position: pos,
                        Width: proj.width - castAheadDist * 2,
                        Height: proj.height - castAheadDist * 2,
                        Type: 172,
                        SpeedX: 0f,
                        SpeedY: 0f,
                        Alpha: 100,
                        newColor: default(Color),
                        Scale: 1.2f
                    );

                    Main.dust[dustIdx].noGravity = true;
                    Main.dust[dustIdx].velocity *= 0.3f;
                    Main.dust[dustIdx].velocity += proj.velocity * 0.5f;

                    Dust dust = Main.dust[dustIdx];
                    dust.position.X = dust.position.X - dustCastAheadX;
                    dust.position.Y = dust.position.Y - dustCastAheadY;
                }

                if (Main.rand.Next(8) == 0)
                {
                    int dustIdx = Dust.NewDust(
                        Position: pos,
                        Width: proj.width - castAheadDist * 2,
                        Height: proj.height - castAheadDist * 2,
                        Type: 172,
                        SpeedX: 0f,
                        SpeedY: 0f,
                        Alpha: 100,
                        newColor: default(Color),
                        Scale: 0.75f
                    );
                    Main.dust[dustIdx].velocity *= 0.5f;
                    Main.dust[dustIdx].velocity += proj.velocity * 0.5f;
                }
            }
        }
    }

    partial class CrystalSprayProjectile : ModProjectile
    {
        private static int SpectreStaffAiStyle;
        private static int AquaSceptreAiStyle;



        ////////////////

        public override string Texture => "Terraria/Projectile_" + ProjectileID.WaterStream;



        ////////////////

        public override void SetStaticDefaults()
        {
            this.DisplayName.SetDefault("Crystal Spray");

            var spectreStaffProj = new Projectile();
            spectreStaffProj.SetDefaults(ProjectileID.LostSoulFriendly);
            var aquaSceptreProj = new Projectile();
            aquaSceptreProj.SetDefaults(ProjectileID.WaterStream);

            CrystalSprayProjectile.SpectreStaffAiStyle = spectreStaffProj.aiStyle;
            CrystalSprayProjectile.AquaSceptreAiStyle = aquaSceptreProj.aiStyle;
        }

        public override void SetDefaults()
        {
            //this.projectile.CloneDefaults( ProjectileID.WaterStream );  // clones aqua sceptre unless aiStyle changes?

            //this.projectile.aiStyle = 0;	// does nothing
            //this.projectile.aiStyle = CrystalSprayProjectile.SpectreStaffAiStyle;	// both aiStyle and aiType needed?
            //this.aiType = ProjectileID.LostSoulFriendly;

            this.projectile.width = 12;
            this.projectile.height = 12;
            this.projectile.tileCollide = true;
            this.projectile.friendly = true;         //Can the projectile deal damage to enemies?
            this.projectile.hostile = false;         //Can the projectile deal damage to the player?
            this.projectile.magic = true;
            this.projectile.alpha = 255;
            this.projectile.timeLeft = 600;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            this.projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            this.projectile.tileCollide = true;          //Can the projectile collide with tiles?
            //this.projectile.penetrate = -1;           //How many monsters the projectile can penetrate.
            this.projectile.extraUpdates = 1;   // 2 = aqua sceptre
        }


        ////////////////

        public override bool PreAI()
        {
            //Dust.QuickDust( this.projectile.Center, Color.Red );
            this.ApplyTrailFx();
            this.RunHomingAI();
            return false;
        }
        /*public override bool PreDrawExtras( SpriteBatch spriteBatch ) {
			var tempAI = this.projectile.ai.Clone();
			var tempLocalAI = this.projectile.localAI.Clone();

			this.projectile.type = ProjectileID.WaterStream;
			this.projectile.aiStyle = CrystalSprayProjectile.AquaSceptreAiStyle;
			//this.projectile.CloneDefaults( ProjectileID.WaterStream );

			this.projectile.ai = (float[])tempAI;
			this.projectile.localAI = (float[])tempLocalAI;

			return base.PreDrawExtras( spriteBatch );
		}

		public override void PostDraw( SpriteBatch spriteBatch, Color lightColor ) {
			var tempAI = this.projectile.ai.Clone();
			var tempLocalAI = this.projectile.localAI.Clone();

			this.projectile.type = ModContent.ProjectileType<CrystalSprayProjectile>();
			this.projectile.aiStyle = CrystalSprayProjectile.SpectreStaffAiStyle;
			//this.projectile.CloneDefaults( ModContent.ProjectileType<CrystalSprayProjectile>() );

			this.projectile.ai = (float[])tempAI;
			this.projectile.localAI = (float[])tempLocalAI;
		}*/
    }

    partial class CrystalSprayProjectile : ModProjectile
    {
        private void RunHomingAI()
        {
            Projectile proj = this.projectile;

            float projPosMidX = proj.position.X + (float)(proj.width / 2);
            float projPosMidY = proj.position.Y + (float)(proj.height / 2);
            float closestNpcPosX = proj.Center.X;
            float closestNpcPosY = proj.Center.Y;
            float closestNpcDistBothAxis = 400f;
            bool targetNpcFound = false;

            for (int npcWho = 0; npcWho < 200; npcWho++)
            {
                NPC npc = Main.npc[npcWho];
                if (!npc.CanBeChasedBy(proj, false))
                {
                    continue;
                }
                if (proj.Distance(npc.Center) >= closestNpcDistBothAxis)
                {
                    continue;
                }
                if (!Collision.CanHit(proj.Center, 1, 1, npc.Center, 1, 1))
                {
                    continue;
                }

                float npcPosMidX = npc.position.X + (float)(npc.width / 2);
                float npcPosMidY = npc.position.Y + (float)(npc.height / 2);

                float bothAxisDist = Math.Abs(projPosMidX - npcPosMidX) + Math.Abs(projPosMidY - npcPosMidY);
                if (bothAxisDist < closestNpcDistBothAxis)
                {
                    closestNpcDistBothAxis = bothAxisDist;
                    closestNpcPosX = npcPosMidX;
                    closestNpcPosY = npcPosMidY;
                    targetNpcFound = true;
                }
            }

            if (!targetNpcFound)
            {
                return;
            }

            Vector2 projPosMid = new Vector2(projPosMidX, projPosMidY);
            float closestNpcDistX = closestNpcPosX - projPosMid.X;
            float closestNpcDistY = closestNpcPosY - projPosMid.Y;
            float closestNpcDist = (float)Math.Sqrt((closestNpcDistX * closestNpcDistX) + (closestNpcDistY * closestNpcDistY));
            closestNpcDist = 6f / closestNpcDist;
            closestNpcDistX *= closestNpcDist;
            closestNpcDistY *= closestNpcDist;

            proj.velocity.X = ((proj.velocity.X * 20f) + closestNpcDistX) / 21f;
            proj.velocity.Y = ((proj.velocity.Y * 20f) + closestNpcDistY) / 21f;
        }
    }
}
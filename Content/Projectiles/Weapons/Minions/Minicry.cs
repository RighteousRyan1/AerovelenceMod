using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AerovelenceMod.Content.Projectiles.Weapons.Minions
{

	public class Minicry : ModProjectile
	{
		int i;

        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Mini-Cry");
			Main.projFrames[projectile.type] = 6;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}
        public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Spazmamini);
			projectile.width = 44;
			projectile.height = 26;
			projectile.minion = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.netImportant = true;
			aiType = ProjectileID.Spazmamini;
			projectile.alpha = 0;
			projectile.penetrate = -10;
			projectile.timeLeft = 18000;
			projectile.minionSlots = 1;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.penetrate == 0)
				projectile.Kill();

			return false;
		}

		public override bool? CanCutTiles()
		{
			return true;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		public override void AI()
		{
			bool flag64 = projectile.type == mod.ProjectileType("Minicry");
			Player player = Main.player[projectile.owner];
			AeroPlayer modPlayer = player.GetModPlayer<AeroPlayer>();
			if (flag64)
			{
				if (player.dead)
					modPlayer.Minicry = false;

				if (modPlayer.Minicry)
					projectile.timeLeft = 2;

			}
			i++;
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.frameCounter++;
			if (projectile.frameCounter % 10 == 0)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 6)
					projectile.frame = 0;
			}

			if (i % 2 == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width / 2, projectile.height / 2, 132);
			}
		}
	}
}
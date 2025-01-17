using System;
using AerovelenceMod.Common.Globals.Worlds;
using AerovelenceMod.Content.Dusts;
using AerovelenceMod.Content.Items.TreasureBags;
using AerovelenceMod.Content.Projectiles.NPCs.Bosses.CrystalTumbler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

//using AerovelenceMod.Items.BossBags;

namespace AerovelenceMod.Content.NPCs.Bosses.CrystalTumbler
{
	[AutoloadBossHead]
	public class CrystalTumbler : ModNPC
	{
		// AI state management of the Crystal Tumbler.
		private enum CrystalTumblerState
		{
			IdleRoll = 0,
			ProjectileSpawn = 1,
			SuperDash = 2,
			Electricity = 3,
			RockRain = 4,
			Jump = 5,
			Teleport = 6
		}

		/// <summary>
		/// Manages the current AI state of the Crystal Tumbler.
		/// Gets and sets npc.ai[0] as tracker.
		/// </summary>
		private CrystalTumblerState State
		{
			get => (CrystalTumblerState)npc.ai[0];
			set => npc.ai[0] = (float)value;
		}

		/// <summary>
		/// Manages several AI state attack timers.
		/// Gets and sets npc.ai[1] as tracker.
		/// </summary>
		private float AttackTimer
		{
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}

		/// <summary>
		/// Boss-specific jump timer manager, to disallow frequent jumping.
		/// Gets and sets npc.ai[2] as tracker.
		/// </summary>
		private float JumpTimer
		{
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}

		/// <summary>
		/// Returns a value between 0.0 - 1.0 based on current life.
		/// </summary>
		float LifePercentLeft => (npc.life / (float)npc.lifeMax);

		public bool P;
		public int spinTimer;
		public bool Phase2;
		public bool Teleport = false;
		public bool doingDash = false;
		int i;
		bool teleported = false;
		int t;
		public int counter = 0;
		public int counter2 = 0;

		public override void SetDefaults()
		{
			npc.width = 120;
			npc.height = 128;
			npc.value = Item.buyPrice(0, 5, 60, 45);

			npc.alpha = 0;
			npc.damage = 30;
			npc.defense = 15;
			npc.lifeMax = 3500;
			npc.knockBackResist = 0f;

			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = false;
			npc.noTileCollide = false;

			bossBag = ModContent.ItemType<CrystalTumblerBag>();

			npc.HitSound = SoundID.NPCHit41;
			npc.DeathSound = SoundID.NPCDeath44;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/CrystalTumbler");
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.defense = 18;
			npc.damage = 45;  //boss damage increase in expermode
			npc.lifeMax = 5000;  //boss life scale in expertmode
		}

		public override bool PreAI()
		{
			npc.TargetClosest(true);
			Player target = Main.player[npc.target];

			Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY);

			if (target.dead)
			{
				npc.velocity.Y += 0.09f;
				npc.timeLeft = 300;
				npc.noTileCollide = true;
			}

			npc.noTileCollide = npc.noGravity = false;

			// No second phase implemented yet.
			/*if (npc.life <= npc.lifeMax / 2)
			{
				AttackTimer = 0;
				return;
			}*/

			// Idle roll/follow state. Very basic movement, with a timer for a random attack state.
			//if (State != CrystalTumblerState.IdleRoll) { Main.NewText(State); }
			if (State == CrystalTumblerState.IdleRoll)
			{
				doingDash = false;
				RollingMove(target);
				counter++;
				npc.Opacity *= 1.1f;

				if (++AttackTimer >= 200)
				{
					AttackTimer = 0;
					npc.netUpdate = true;

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{

						if ((npc.Center.Y - 50 > target.Center.Y || npc.Center.Y + 50 < target.Center.Y) && Main.rand.NextBool(2))
						{
							State = CrystalTumblerState.Teleport;
							npc.netUpdate = true;
							// Jump *only* when below the target, standing on solid ground, and with a bit of randomness.


							if (npc.velocity.Y == 0 && npc.Center.Y - 50 > target.Center.Y /*&& Main.rand.NextBool(2)*/)
							{

								State = CrystalTumblerState.Jump;
								npc.netUpdate = true;
							}
						}
						else if (Math.Abs(target.Center.Y - npc.Center.Y) <= 50 && Main.rand.NextBool(2))
						{

							State = CrystalTumblerState.SuperDash;
							npc.netUpdate = true;
						}
						else
						{
							int randomState = Main.rand.Next(3);
							if (randomState == 0)
							{
								State = CrystalTumblerState.ProjectileSpawn;
								npc.netUpdate = true;
							}
							else if (randomState == 1)
							{
								State = CrystalTumblerState.Electricity;
								npc.netUpdate = true;
							}
							else
							{
								State = CrystalTumblerState.RockRain;
								npc.netUpdate = true;
							}
						}
					}
				}
			}

			// Attack state. Spawns three projectiles on the NPC, which hover for a (few) second(s), before shooting towards the target.
			else if (State == CrystalTumblerState.ProjectileSpawn)
			{
				// Spawn a projectile every 10 ticks (and on the first tick this state is active).
				// TODO: Eldrazi - Multiplayer support?
				if (AttackTimer++ == 0)
				{
					Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 69, 0.75f);
					Projectile.NewProjectile(npc.Center, default, ModContent.ProjectileType<TumblerBoulder1>(), 12, 1f, Main.myPlayer, 0);
					npc.netUpdate = true;
				}
				else if (AttackTimer == 10)
				{
					Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 69, 0.75f);
					Projectile.NewProjectile(npc.Center, default, ModContent.ProjectileType<TumblerBoulder1>(), 12, 1f, Main.myPlayer, 1);
					npc.netUpdate = true;
				}
				else if (AttackTimer == 20)
				{
					Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 69, 0.75f);
					Projectile.NewProjectile(npc.Center, default, ModContent.ProjectileType<TumblerBoulder1>(), 12, 1f, Main.myPlayer, 2);
					npc.netUpdate = true;
					AttackTimer = 0;
					State = CrystalTumblerState.IdleRoll;
				}
				RollingMove(target);
			}

			// Attack state. Starts speedy roll in place. After a set amount of ticks, releases/dashes with a high velocity and frequent jumps/bounces.
			else if (State == CrystalTumblerState.Jump)
			{
				/*Main.NewText("Newcheck");
				Main.NewText(target.Center.Y < npc.Center.Y - 60);
				Main.NewText(npc.Center.Y - 60 + "   -60 y");
				Main.NewText(target.Center.Y);*/

				if (target.Center.Y < npc.Center.Y - 60)
				{
					npc.velocity.Y -= Main.rand.Next(15) + 12;
					for (int num325 = 0; num325 < 20; num325++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, npc.velocity.X, npc.velocity.Y, 0, default, 1);
						npc.netUpdate = true;
					}
					State = CrystalTumblerState.IdleRoll;
				}
				else if (target.Center.Y > npc.Center.Y)
				{
					State = CrystalTumblerState.IdleRoll;
				}
				npc.netUpdate = true;
			}

			else if (State == CrystalTumblerState.SuperDash)
			{
				t++;
				CheckPlatform(target);
				CheckTilesNextTo(target, 5f);
				// Rotating in place state.
				if (++AttackTimer <= 180)
				{
					doingDash = true;
					npc.velocity.X *= 0.95f;
					npc.rotation += AttackTimer / 180 * npc.direction;
					float speed = 5f;
					Vector2 velocity = new Vector2(speed, speed).RotatedByRandom(MathHelper.ToRadians(360));
					if (t % 25 == 0 && !Main.expertMode == true)
					{
						if (Main.rand.NextBool(2))
						{
							Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<TumblerSpike1>(), 15, 0f, Main.myPlayer, 0f, 0f);
							npc.netUpdate = true;
						}
						else
						{
							Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<TumblerSpike2>(), 15, 0f, Main.myPlayer, 0f, 0f);
							npc.netUpdate = true;
						}
					}
					else if (t % 15 == 0 && Main.expertMode == true)
					{
						int randomProj = Main.rand.Next(3);
						if (randomProj == 0)
						{
							Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<TumblerSpike1>(), 20, 10f, Main.myPlayer, 0f, 0f);
							npc.netUpdate = true;
						}
						else if (randomProj == 1)
						{
							Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<TumblerSpike2>(), 15, 10f, Main.myPlayer, 0f, 0f);
							npc.netUpdate = true;
						}
						else
						{
							Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<TumblerHomingShard>(), 20, 0f, Main.myPlayer, 0f, 0f);
							npc.netUpdate = true;
						}
					}
					// If the timer is at 180 (or 3 seconds), set the velocity towards the NPCs direction.
					if (AttackTimer == 180)
					{
						npc.netUpdate = true;
						npc.velocity.X = npc.direction * 16f;
						npc.netUpdate = true;
					}

				}

				// Fast horizontal movement and frequent jumps.
				else
				{
					Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 76, 0.75f);
					if (npc.velocity.Y == 0 && target.Center.Y < npc.Center.Y - 100f)
					{
						TryJump(Main.rand.Next(4, 7), 60);
					}
					JumpTimer--;
					var player = Main.player[npc.target];
					if (player.Center.X > npc.Center.X)
					{
						npc.velocity.X += 0.3f;
						npc.netUpdate = true;
					}
					else if (player.Center.X < npc.Center.X)
					{
						npc.velocity.X -= 0.3f;
						npc.netUpdate = true;
					}
					doingDash = false; //idk
				}



				// After 480 ticks (300 ticks or 5 seconds after the 'Rotating in place' state), go back to idle rolling.
				if (AttackTimer >= 480)
				{
					npc.netUpdate = true;

					AttackTimer = 0;
					State = CrystalTumblerState.IdleRoll;
				}
				npc.rotation += npc.velocity.X * 0.025f;
			}

			else if (State == CrystalTumblerState.Electricity)
			{
				var player = Main.player[npc.target];
				Vector2 vector8 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height * 0.5f));
				if (AttackTimer++ == 0)
				{
					Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 93, 0.75f);
					float Speed = 1 * 0.99f;
					int damage = Main.expertMode ? 20 : 10;// if u want to change this, 15 is for expert mode, 10 is for normal mod
					int type = mod.ProjectileType("TumblerOrb");
					float rotation = (float)Math.Atan2(vector8.Y - (player.position.Y + (player.height * 0.5f)), vector8.X - (player.position.X + (player.width * 0.5f)));
					Projectile.NewProjectile(vector8.X, vector8.Y, (float)(Math.Cos(rotation) * Speed * -1), (float)(Math.Sin(rotation) * Speed * -1), type, damage, 0f, 0);
					npc.netUpdate = true;
				}
				AttackTimer = 0;
				State = CrystalTumblerState.IdleRoll;
				RollingMove(target);
			}

			else if (State == CrystalTumblerState.Teleport)
			{
				Vector2 teleportPosition = target.position - Vector2.UnitY * 370;

				if (!Collision.SolidCollision(teleportPosition, npc.width, npc.height))
				{
					AttackTimer++;
					counter2++;
					if (AttackTimer >= 100)
					{
						npc.Opacity *= 0.97f;
						npc.netUpdate = true;
					}
					if (AttackTimer == 100)
					{
						Projectile.NewProjectile(target.position, default, ModContent.ProjectileType<TeleportCharge>(), 12, 1f, Main.myPlayer, 1);
						npc.netUpdate = true;
					}
					if (teleported == true)
					{
						if (counter2 == 500)
						{
							npc.noTileCollide = false;
							counter2 = 0;
						}
						teleported = false;
					}
					if (AttackTimer >= 200)
					{
						npc.position.Y = teleportPosition.Y;
						npc.position.X = target.position.X;
						npc.noTileCollide = true;
						teleported = true;
						AttackTimer = 0;
						npc.netUpdate = true;
						Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 94, 0.75f);
						for (float i = 0; i < 360; i += 0.5f)
						{
							float ang = (float)(i * Math.PI) / 180;

							float x = (float)(Math.Cos(ang) * 150) + npc.Center.X;
							float y = (float)(Math.Sin(ang) * 150) + npc.Center.Y;

							Vector2 vel = Vector2.Normalize(new Vector2(x - npc.Center.X, y - npc.Center.Y)) * 15;

							int dustIndex = Dust.NewDust(new Vector2(x - 3, y - 3), 6, 6, 54, vel.X, vel.Y);
							Main.dust[dustIndex].noGravity = true;
							npc.netUpdate = true;
						}
						State = CrystalTumblerState.IdleRoll;
					}
					RollingMove(target);
				}
				else
				{
					npc.netUpdate = true;
					State = CrystalTumblerState.IdleRoll;
				}
			}

			else if (State == CrystalTumblerState.RockRain)
			{
				Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 74, 0.75f);

				for (int i = -3; i <= 3; i++)
				{
					if (Main.rand.NextBool(2))
					{
						Projectile.NewProjectile(target.Center.X + i * 20, target.Center.Y - 380, i * 2, -2, ModContent.ProjectileType<TumblerSpike1>(), 30, 0f, Main.myPlayer, 0f, 0f);
						npc.netUpdate = true;
					}
					else
					{
						Projectile.NewProjectile(target.Center.X + i * 20, target.Center.Y - 380, i * 2, -2, ModContent.ProjectileType<TumblerSpike2>(), 30, 0f, Main.myPlayer, 0f, 0f);
						npc.netUpdate = true;
					}
				}

				State = CrystalTumblerState.IdleRoll;
			}

			return (false);
		}

		private void CheckPlatform(Player player)
		{
			i++;
			bool onplatform = true;

			for (int i = (int)npc.position.X; i < npc.position.X + npc.width; i += npc.width / 4)
			{
				Tile tile = Framing.GetTileSafely(new Point((int)npc.position.X / 16, (int)(npc.position.Y + npc.height + 8) / 16));
				if (!TileID.Sets.Platforms[tile.type])
				{
					onplatform = false;
				}
			}
			if (onplatform && (npc.position.Y + npc.height + 20 < player.Center.Y))
			{
				if (i % 70 == 0)
				{
					npc.noTileCollide = true;
				}
				else
				{
					npc.noTileCollide = false;
				}
			}
		}

		private void CheckTilesNextTo(Player player, float desiredspeed)
		{
			bool doJump = false;

			for (int i = (int)npc.position.X; i < npc.position.X + npc.width; i += npc.width / 4)
			{
				if (npc.velocity.X < desiredspeed && player.Center.Y < npc.Center.Y)
				{
					doJump = true;
				}
			}
			if (doJump && !doingDash)
			{
				if (npc.velocity.Y == 0 && npc.velocity.X == 0)
				{
					npc.velocity.Y -= Main.rand.Next(9) + 7;
					for (int num325 = 0; num325 < 20; num325++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, npc.velocity.X, npc.velocity.Y, 0, default, 1);
					}
					State = CrystalTumblerState.IdleRoll;
				}
			}

		}

        public override void BossLoot(ref string name, ref int potionType)
        {
			DownedWorld.DownedCrystalTumbler = true;

			if (Main.netMode == NetmodeID.Server)
				NetMessage.SendData(MessageID.WorldData);
        }

        public override void NPCLoot()
		{
			if (Main.expertMode)
                npc.DropBossBags();

			if (!Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LesserHealingPotion, Main.rand.Next(4, 12));
				switch (Main.rand.Next(0, 7))
				{
					case 0:
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CavernMauler"));
						break;
					case 1:
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CavernousImpaler"));
						break;
					case 2:
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CrystallineQuadshot"));
						break;
					case 3:
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PrismThrasher"));
						break;
					case 4:
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PrismPiercer"));
						break;
					case 5:
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DiamondDuster"));
						break;
					case 6:
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DarkCrystalStaff"));
						break;
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int k = 0; k < 20; k++)
                    Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Sparkle>(), npc.velocity.X, npc.velocity.Y, 0, Color.White, 1);

                for (int i = 0; i < 7; i++)
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TumblerGore" + i));
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = ModContent.GetTexture("AerovelenceMod/Content/NPCs/Bosses/CrystalTumbler/Glowmask");
			spriteBatch.Draw(texture, npc.Center - Main.screenPosition, npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2f, npc.scale, SpriteEffects.None, 0);
		}

        public override void BossHeadRotation(ref float rotation) => rotation = npc.rotation;

        #region AI Methods

		/// <summary>
		/// Very basic movement state.
		/// Roll speed depends on current NPC life (from 2 to 14 at 0 life).
		/// </summary>
		/// <param name="player"></param>
		private void RollingMove(Player player)
		{
			// Movement.

			float desiredSpeed = 2 + 12 * (1 - LifePercentLeft);
			CheckPlatform(player);
			CheckTilesNextTo(player, desiredSpeed);
			if (player.Center.X > npc.Center.X)
			{
				if (npc.velocity.X < desiredSpeed)
				{
					npc.velocity.X += (0.1f * (1 - LifePercentLeft + 1));
				}
			}
			else if (player.Center.X < npc.Center.X)
			{
				if (npc.velocity.X > -desiredSpeed)
				{
					npc.velocity.X -= (0.1f * (1 - LifePercentLeft + 1));
				}
			}
			npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -6, 6);

			// Rotation.
			npc.rotation += npc.velocity.X * 0.025f;

			// Jump.
			if (npc.velocity.Y == 0 && player.Center.Y < npc.Center.Y - 100f)
			{
				TryJump(Main.rand.Next(12) + 7, 180);
			}
			else if (JumpTimer > 0)
			{
				JumpTimer--;
			}

		}

		/// <summary>
		/// Attempts to jump if the JumpTimer is past its timeout.
		/// </summary>
		/// <param name="height"></param>
		/// <param name="cooldown"></param>
		private void TryJump(float height, int cooldown)
		{
			if (JumpTimer > 0)
			{
				return;
			}

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				npc.netUpdate = true;
				npc.velocity.Y -= height;
			}
			for (int num325 = 0; num325 < 20; num325++)
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, npc.velocity.X, npc.velocity.Y, 0, default, 1);

			JumpTimer = cooldown;
		}

		#endregion
	}
}
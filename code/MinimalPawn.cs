using Sandbox;

namespace NightSky
{
	class MinimalPawn : Player
	{
		private TimeSince timeSinceJumpReleased;
		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );
			Controller = new WalkController();
			Animator = new StandardPlayerAnimator();
			Camera = new FirstPersonCamera();
			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();
		}

		public override void OnKilled()
		{
			base.OnKilled();
			EnableDrawing = false;
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			// Viewpoint switching
			if ( Input.Pressed( InputButton.View ) )
			{
				if ( Camera is not FirstPersonCamera )
				{
					Camera = new FirstPersonCamera();
				}
				else
				{
					Camera = new ThirdPersonCamera();
				}
			}

			if ( Input.Released( InputButton.Jump ) )
			{
				if ( timeSinceJumpReleased < 0.3f )
				{
					Game.Current?.DoPlayerNoclip( cl );
				}

				timeSinceJumpReleased = 0;
			}

			if ( Input.Left != 0 || Input.Forward != 0 )
			{
				timeSinceJumpReleased = 1;
			}

		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace NightSky.Entities
{
	class SkyDome : RenderEntity
	{
		private const float _renderRadius = 32768.0f;
		private Material _skyMaterial;
		private VertexBuffer _vb;
		private bool _isRenderReady = false;

		public SkyDome() : base()
		{
			_skyMaterial = Material.Load( "materials/night_sky.vmat" );
			_vb = new();
			_vb.Init( true );
			GenerateSkyDome( _renderRadius );

			// Attempting to use a vertex buffer before rendering causes a game crash
			// we ensure our data is ready before trying to render!
			_isRenderReady = true;
		}

		public override void Spawn()
		{
			Transmit = TransmitType.Always;
			RenderBounds = BBox.FromHeightAndRadius( _renderRadius, _renderRadius );
		}

		private void GenerateSkyDome( float radius )
		{
			int sectorCount = 16;
			int stackCount = 16;

			float sectorStep = 2.0f * MathF.PI / sectorCount;
			float stackStep = MathF.PI / stackCount;

			// Create verticies
			for ( int i = 0; i <= stackCount; i++ )
			{
				float currentStackAngle = MathF.PI / 2.0f - i * stackStep;
				float xy = radius * MathF.Cos( currentStackAngle );
				float z = radius * MathF.Sin( currentStackAngle );

				for ( int j = 0; j <= sectorCount; j++ )
				{
					float sectorAngle = j * sectorStep;
					float x = xy * MathF.Cos( sectorAngle );
					float y = xy * MathF.Sin( sectorAngle );

					_vb.Add( new Vector3( x, y, z ), new Vector2( (float)j / (float)sectorCount, (float)i / (float)stackCount ) ); ;
				}
			}

			// Create triangles
			for ( int i = 0; i < stackCount; i++ )
			{
				int k1 = i * (sectorCount + 1);
				int k2 = k1 + sectorCount + 1;
				for ( int j = 0; j < sectorCount; ++j, ++k1, ++k2 )
				{
					// We specifically flip the winding order to render the
					// sphere on the inside instead of the outside.
					// Since we currently cannot control the culling render
					// state we have to do it this way
					if ( i != 0 )
					{
						_vb.AddRawIndex( k1 + 1 );
						_vb.AddRawIndex( k2 );
						_vb.AddRawIndex( k1 );
					}

					if ( i != (stackCount - 1) )
					{
						_vb.AddRawIndex( k2 + 1 );
						_vb.AddRawIndex( k2 );
						_vb.AddRawIndex( k1 + 1 );

					}
				}
			}
		}

		public override void DoRender( SceneObject obj )
		{
			if(!_isRenderReady)
			{
				return;
			}

			_vb.Draw( _skyMaterial );
		}
	}
}

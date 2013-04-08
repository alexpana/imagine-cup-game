using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace VertexArmy.Global.Controllers.Components
{
	public class CarControlComponent : BaseComponent
	{
		private readonly List<string> _lineJoints;
		private readonly List<float> _lineJointsSpeeds;
		private readonly SoundEffectInstance _engineSound;
		private float _oldDirection;

		public CarControlComponent( ICollection<string> lineJointNames, ICollection<float> speeds )
		{
			Data = new List<object>();
			Debug.Assert( lineJointNames.Count == speeds.Count );

			_type = ComponentType.CarControlComponent;
			_lineJoints = new List<string>();
			_lineJoints.AddRange( lineJointNames );
			_lineJointsSpeeds = new List<float>();
			_lineJointsSpeeds.AddRange( speeds );


			_engineSound = Platform.Instance.Content.Load<SoundEffect>( "sounds/engine_run" ).CreateInstance();
			_engineSound.IsLooped = true;
			_engineSound.Volume = 0.1f;
		}

		public override void InitEntity()
		{
			if ( Platform.Instance.Settings.IsMusicEnabled )
			{
				_engineSound.Play();
			}
		}

		public override void Update( GameTime dt )
		{
			if ( Platform.Instance.Settings.IsMusicEnabled )
			{
				_engineSound.Volume = 0.1f;
			}
			else
			{
				_engineSound.Volume = 0.0f;
			}

			float direction = 0f;
			if ( Keyboard.GetState().IsKeyDown( Keys.Left ) || Keyboard.GetState().IsKeyDown( Keys.A ) )
			{
				direction = -1f;
			}
			else if ( Keyboard.GetState().IsKeyDown( Keys.Right ) || Keyboard.GetState().IsKeyDown( Keys.D ) )
			{
				direction = 1f;
			}

			_engineSound.Pitch = Math.Min( Entity.MainBody.LinearVelocity.Length() / 10f, 1f );


			_oldDirection = direction;

			if ( Entity != null && Entity.PhysicsEntity.Enabled )
			{
				for ( int i = 0; i < _lineJoints.Count; i++ )
				{
					Entity.PhysicsEntity.SetLineJointMotorSpeed( _lineJoints[i], direction * _lineJointsSpeeds[i] );
				}
			}

		}

		public override void Clean()
		{
			_lineJoints.Clear();
			_lineJointsSpeeds.Clear();
			_engineSound.Stop();
		}
	}
}

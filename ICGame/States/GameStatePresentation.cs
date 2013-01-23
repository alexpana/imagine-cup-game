using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.States
{
	internal class GameStatePresentation : IGameState
	{
		private ContentManager _contentManager;
		/* Color/Texture variables */
		private Texture2D _texture;
		private Texture2D _ground;
		private Texture2D _loadBar;
		private Texture2D _fillBar;
		private Texture2D _loadBarRed;

		private SpriteFont _font;

		/* Guybrush variables */
		private int _lastDirection;
		private bool _isMoving;
		private Point _guyBrushPos = new Point( 300, 200 );

		private float _cloakIntensity = 1f;
		private float _cloakBattery = 1f;
		private bool _isCloaked;

		private bool _cloakTouched = true; // variable to keep correct toggle state

		private const float CloakIntensityMin = 0.05f;
		private const float CloakIntensityMax = 1.0f;
		private const float CloakDecreaseRate = 0.001f;
		private const float CloakIncreaseRate = 0.005f;
		private const float CloakSpeed = 0.1f;

		/* Animation variables */
		private Point _frameSize = new Point( 104, 151 );
		private Point _currentFrame = new Point( 0, 2 );
		private Point _sheetSize = new Point( 6, 3 );
		private const int AnimationFrameInterval = 10;

		private int _frameCount;

		private SpriteBatch _spriteBatch;
		private SpriteBatch _uiBatch;
		private SpriteBatch _staticSpriteBatch;

		private SoundEffect _soundCloakEnabled;
		private SoundEffectInstance _soundCloakEnabledInstance;

		// ===============================================================
		// Effects - Render To Texture
		// ===============================================================
		private RenderTarget2D _renderTarget1;
		private SpriteBatch _rttBatch;
		private Effect _effectSepia;

		public GameStatePresentation( ContentManager content )
		{
			_contentManager = content;
		}

		public void OnUpdate( GameTime dt )
		{
			_frameCount++;
			// Allows the game to exit
			if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed )
				StateManager.Instance.ChangeState( GameState.Menu );

			if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.Escape ) )
				StateManager.Instance.ChangeState( GameState.Menu );

			if ( !_cloakTouched && Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.E ) )
			{
				if ( !_isCloaked && _cloakBattery > 0.09f )
				{
					_isCloaked = true;
					_soundCloakEnabledInstance.Play();
				}
				else if ( _isCloaked )
				{
					_isCloaked = !_isCloaked;
					if ( _soundCloakEnabledInstance.State == SoundState.Playing )
					{
						_soundCloakEnabledInstance.Stop();
					}
				}
				_cloakTouched = true;
			}

			if ( _cloakTouched && Keyboard.GetState( PlayerIndex.One ).IsKeyUp( Keys.E ) )
			{
				_cloakTouched = false;
			}

			//key presses
			if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.Left ) )
			{
				_lastDirection = -1;
				_currentFrame.Y = 1;
				_guyBrushPos.X += -4;
				_isMoving = true;
			}
			else if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.Right ) )
			{
				_lastDirection = 1;
				_currentFrame.Y = 0;
				_guyBrushPos.X += 4;
				_isMoving = true;
			}

			if ( Keyboard.GetState( PlayerIndex.One ).IsKeyUp( Keys.Right ) && Keyboard.GetState( PlayerIndex.One ).IsKeyUp( Keys.Left ) )
			{
				if ( _lastDirection == -1 )
				{
					_lastDirection = 0;
					_currentFrame.Y = 2;
					_currentFrame.X = 1;
					_isMoving = false;
				}
				else if ( _lastDirection == 1 )
				{
					_lastDirection = 0;
					_currentFrame.Y = 2;
					_currentFrame.X = 0;
					_isMoving = false;
				}
			}

			// set cloak decrease/increase rate
			var decreaseRate = CloakDecreaseRate;
			var increaseRate = CloakIncreaseRate;
			if ( _isMoving )
			{
				decreaseRate *= 3;
			}

			// update battery
			if ( _isCloaked )
			{
				_cloakBattery -= decreaseRate;

				if ( _cloakBattery <= 0f )
					_isCloaked = false;
			}
			else if ( !_isCloaked && _cloakBattery < 1f )
			{
				_cloakBattery += increaseRate;

				if ( _cloakBattery > 1f )
				{
					_cloakBattery = 1f;
				}
			}

			//update intensity
			if ( _isCloaked && _cloakIntensity > CloakIntensityMin )
			{
				_cloakIntensity -= CloakSpeed;

				if ( _cloakIntensity < CloakIntensityMin )
				{
					_cloakIntensity = CloakIntensityMin;
				}
			}

			if ( !_isCloaked && _cloakIntensity < CloakIntensityMax )
			{
				_cloakIntensity += CloakSpeed;

				if ( _cloakIntensity > CloakIntensityMax )
				{
					_cloakIntensity = CloakIntensityMax;
				}
			}

			// animation update
			if ( _isMoving && _frameCount % AnimationFrameInterval == 0 )
			{
				++_currentFrame.X;
				if ( _currentFrame.X >= _sheetSize.X )
				{
					_currentFrame.X = 0;
				}

				_frameCount = AnimationFrameInterval;
			}
		}

		public void RenderScene()
		{
			const string output = "Press E to cloak/decloak! Use arrows to move.";

			_uiBatch.Begin( SpriteSortMode.FrontToBack, BlendState.AlphaBlend );

			_uiBatch.Draw( _cloakBattery < 0.3f ? _loadBarRed : _loadBar, new Rectangle( 10, 20, 250, 30 ), Color.White );

			_uiBatch.Draw( _fillBar, new Rectangle( 12, 22, ( int ) ( 246 * _cloakBattery ), 26 ), Color.White );
			_uiBatch.DrawString( _font, output, new Vector2( 540, 35 ), Color.Black, 0, _font.MeasureString( output ) / 2, 1.0f, SpriteEffects.None, 0.5f );
			_uiBatch.End();

			_staticSpriteBatch.Begin( SpriteSortMode.FrontToBack, BlendState.AlphaBlend );
			_staticSpriteBatch.Draw( _ground, new Rectangle( 0, 315, 1000, 500 ), Color.White );
			_staticSpriteBatch.End();

			_spriteBatch.Begin( SpriteSortMode.FrontToBack, BlendState.AlphaBlend );
			_spriteBatch.Draw( _texture, new Vector2( _guyBrushPos.X, _guyBrushPos.Y ),
				new Rectangle( _currentFrame.X * _frameSize.X,
					_currentFrame.Y * _frameSize.Y,
					_frameSize.X,
					_frameSize.Y ),
				Color.White * _cloakIntensity, 0, Vector2.Zero,
				1, SpriteEffects.None, 0 );
			_spriteBatch.End();
		}

		public void OnRender( GameTime dt )
		{
			Platform.Instance.Device.Clear( Color.SkyBlue );

			Platform.Instance.Device.SetRenderTarget( _renderTarget1 );
			RenderScene();
			Platform.Instance.Device.SetRenderTarget( null );

			_rttBatch.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, _isCloaked ? _effectSepia : null );
			_rttBatch.Draw( _renderTarget1, new Rectangle( 0, 0, 800, 600 ), Color.White );

			_rttBatch.End();
		}

		public void OnEnter()
		{
			_renderTarget1 = new RenderTarget2D(
				Platform.Instance.Device,
				Platform.Instance.Device.PresentationParameters.BackBufferWidth,
				Platform.Instance.Device.PresentationParameters.BackBufferHeight,
				false,
				Platform.Instance.Device.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24 );

			_rttBatch = new SpriteBatch( Platform.Instance.Device );

			// Effects
			_effectSepia = _contentManager.Load<Effect>( @"effects\sepia" );

			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch( Platform.Instance.Device );
			_staticSpriteBatch = new SpriteBatch( Platform.Instance.Device );
			_uiBatch = new SpriteBatch( Platform.Instance.Device );

			_font = _contentManager.Load<SpriteFont>( @"fonts\SpriteFont1" );

			_texture = _contentManager.Load<Texture2D>( @"images\animated\gb_walk" );

			_ground = new Texture2D( Platform.Instance.Device, 1, 1, false, SurfaceFormat.Color );
			_ground.SetData( new[] { Color.Brown } );

			_loadBar = new Texture2D( Platform.Instance.Device, 1, 1, false, SurfaceFormat.Color );
			_loadBar.SetData( new[] { Color.Purple } );

			_loadBarRed = new Texture2D( Platform.Instance.Device, 1, 1, false, SurfaceFormat.Color );
			_loadBarRed.SetData( new[] { new Color( 197, 5, 5 ) } );

			_fillBar = new Texture2D( Platform.Instance.Device, 1, 1, false, SurfaceFormat.Color );
			_fillBar.SetData( new[] { Color.Cyan } );

			_soundCloakEnabled = _contentManager.Load<SoundEffect>( @"sounds\cloak_enable" );
			_soundCloakEnabledInstance = _soundCloakEnabled.CreateInstance();

		}

		public void OnClose()
		{
			_contentManager.Unload( );
		}
	}
}

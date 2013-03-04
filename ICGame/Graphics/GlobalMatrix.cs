using System;
using Microsoft.Xna.Framework;


namespace VertexArmy.Graphics
{
	public enum EMatrix
	{
		World,
		View,
		Projection,
		Texture,
		Count
	};
	public sealed class GlobalMatrix
	{
		private static volatile GlobalMatrix _instance;
		private static object _syncRoot = new Object();
		public const int StackDepth = 8;
		private GlobalMatrix( )
		{
			_si = new int[(int)EMatrix.Count];
			_stacks = new Matrix[(int)EMatrix.Count][];

			for(int i = 0; i < (int)EMatrix.Count; ++i)
			{
				_stacks[i] = new Matrix[StackDepth];

				for(int j = 0; j < (int)EMatrix.Count; ++j)
				{
					_stacks[i][j] = Matrix.Identity;
				}
			
				_si[i] = 0;
			}
			_matWorld = Matrix.Identity;
			_matWorldInverseTranspose = Matrix.Identity;
			_matWorldViewProjection = Matrix.Identity;

			_matWorldF = false;
			_matWorldInverseTransposeF = false;
			_matWorldViewProjectionF = false;
		}

		public static GlobalMatrix Instance
		{
			get 
			{
				if ( _instance == null ) 
				{
					lock ( _syncRoot ) 
					{
						if ( _instance == null )
							_instance = new GlobalMatrix( );
					}
				}
				return _instance;
			}
		}

		public Matrix MatWorld
		{
			get
			{
				if(_matWorldF)
				{
					_matWorld = _stacks[( int ) EMatrix.World][_si[( int ) EMatrix.World]];
					_matWorldF = false;
				}
				return _matWorld;
			}
		}

		public Matrix MatWorldInverseTranspose
		{
			get 
			{ 
				if(_matWorldInverseTransposeF)
				{
					_matWorldInverseTranspose = Matrix.Invert( Matrix.Transpose( _stacks[( int ) EMatrix.World][_si[( int ) EMatrix.World]] ) );
					_matWorldInverseTransposeF = false;
				}
				return _matWorldInverseTranspose;
			}
		}

		public Matrix MatWorldViewProjection
		{
			get
			{
				if(_matWorldViewProjectionF)
				{
					_matWorldViewProjection =
						_stacks[(int)EMatrix.World][_si[(int)EMatrix.World]] *
						_stacks[(int)EMatrix.View][_si[(int)EMatrix.View]] *
						_stacks[( int ) EMatrix.Projection][_si[( int ) EMatrix.Projection]];
					_matWorldViewProjectionF = false;
				}
				return _matWorldViewProjection;
			}
		}

		private int[] _si;
		private Matrix[][] _stacks;


		private Matrix _matWorld;
		private Matrix _matWorldInverseTranspose;
		private Matrix _matWorldViewProjection;

		private bool _matWorldF;
		private bool _matWorldInverseTransposeF;
		private bool _matWorldViewProjectionF;

		public void PushMatrix ( EMatrix matrix )
		{
			if(_si[(int)matrix] < StackDepth - 1)
			{
				++_si[(int)matrix];
				_stacks[(int)matrix][_si[(int)matrix]] =
					_stacks[(int)matrix][_si[(int)matrix - 1]];
			}
		}

		public void PopMatrix ( EMatrix matrix )
		{
			if(_si[(int)matrix] > 0)
			{
				--_si[(int)matrix];
			}
			SetFlags(matrix);
		}

		private void SetFlags ( EMatrix matrix )
		{
			switch(matrix)
			{
				case EMatrix.World:
					_matWorldF = true;
					_matWorldInverseTransposeF = true;
					_matWorldViewProjectionF = true;
					break;
				case EMatrix.View:
					_matWorldViewProjectionF = true;
					break;
				case EMatrix.Projection:
					_matWorldViewProjectionF = true;
					break;
			}
		}

		public void LoadIdentity ( EMatrix matrix )
		{
			_stacks[(int)matrix][_si[(int)matrix]] = Matrix.Identity;
			SetFlags(matrix);
		}

		public void LoadMatrix( EMatrix matrix, Matrix mat )
		{
			_stacks[( int ) matrix][_si[( int ) matrix]] = mat;
			SetFlags( matrix );
		}

		public void MultMatrix( EMatrix matrix, Matrix mat )
		{
			_stacks[( int ) matrix][_si[( int ) matrix]] = mat * _stacks[( int ) matrix][_si[( int ) matrix]];
			SetFlags( matrix );
		}

		public Matrix GetMatrix( EMatrix matrix )
		{
			return _stacks[( int ) matrix][_si[( int ) matrix]];
		}
	}
}

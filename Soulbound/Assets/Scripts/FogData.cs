using System;
using UnityEngine;

//http://wiki.unity3d.com/index.php/Singleton
public class FogData : Singleton<FogData>
{
#if UNITY_EDITOR
	#region Actions
	public event Action OnChange;
	#endregion Actions	
#endif

	#region InspectorFields	
		
	[Header( "Boundaries" )]
	public Vector2 fogBack = new Vector2( 0f, 50f );
	public Vector2 fogFront = new Vector2( 0f, 1f );
	public Vector2 verticalSplit = new Vector2( 0f, 1f );

	[Header( "Fog Map" )]
	public bool useTexture = true;
	//Texture should be 4 pixels width, the order of columns should be backBottom, backTop, frontBottom, frontTop
	public Texture2D fogRampTexture;

	
	[Header( "Colors" )]
	public Gradient backBottom;
	public Gradient backTop;
	public Gradient frontBottom;
	public Gradient frontTop;
	
	public int colorsResolution = 100;
	#endregion InspectorFields

	#region PrivateFields
	private Texture2D _generated_texture;
	#endregion

	#region Accessors
	//This tell us if we should activate or deactivate the effect.
	//Not only enabling or disabling the component acts on the effect activation; 
	//for example, if fog back minimum is higher than maximum, the effect gets deactivated automatically.
	public bool Activated
	{ get; set; }
	#endregion Accessors

	#region Unity
#if UNITY_EDITOR
	//Calling something changed if the component is activated or deactivated, so we can see the effect right away on the scene
	private void OnEnable( )
	{
		if( OnChange != null )
		{
			OnChange( );
		}

		OnChange += _Changed;
	}

	private void OnDisable( )
	{
		if( OnChange != null )
		{
			OnChange( );
		}

		OnChange -= _Changed;
	}
#endif

	private void Awake( )
	{
		_UpdateFogFromSettings( );
    }
	#endregion Unity

	//This makes sense only in editor.
#if UNITY_EDITOR
	#region Events
	//This will be called from the editor script when something in the inspector changes
	public void CallOnChangeEvent( )
	{
		if( OnChange != null )
		{
			OnChange( );
		}
	}

	private void _Changed()
	{
		_UpdateFogFromSettings( );
	}
	#endregion Events
#endif

	#region LocalMethods
	//Redo the texture and recheck if the component should be activated or not
	private void _UpdateFogFromSettings( )
	{
		if( !useTexture )
		{
			_MakeTextureFromColors( );
			fogRampTexture = _generated_texture;
		}

		_UpdateActivated( );
	}
		
	private void _UpdateActivated( )
	{
		bool fog_back_min_bigger_than_max = fogBack.x >= fogBack.y;
		bool fog_front_min_bigger_than_max = fogFront.x >= fogFront.y;

		if( !fogRampTexture || fog_back_min_bigger_than_max || fog_front_min_bigger_than_max )
		{
			Activated = false;
		}
		else
		{
			Activated = true;
		}
	}

	private void _MakeTextureFromColors( )
	{
		if(!_generated_texture)
		{
			_generated_texture = new Texture2D( 4, colorsResolution, TextureFormat.ARGB32, false );
		}

		//Properties to make the texture work properly
		_generated_texture.wrapMode = TextureWrapMode.Clamp;
		_generated_texture.anisoLevel = 0;
		_generated_texture.filterMode = FilterMode.Point;

		//Correct order to make the columns in the texture. If we make a manual texture we should follow the same structure.
		Gradient[] colors = { backBottom, backTop, frontBottom, frontTop };

		//Create a 4 by colorsResolution pixel texture, running through all gradients with the order above.
		for( int j = 0; j < 4; j++ )
		{
			Gradient current = colors[j];

			for( int i = 0; i < colorsResolution; i++ )
			{
				_generated_texture.SetPixel( j, i, current.Evaluate( 1 - ( i / (float)colorsResolution ) ) );
			}
		}

		//Apply changes to the texture
		_generated_texture.Apply( );
	}
	#endregion LocalMethods
}
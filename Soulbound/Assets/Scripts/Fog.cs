using UnityEngine;

public class Fog : MonoBehaviour
{
	#region Const
	const string ON_KEYWORD = "_FOG_ON";
	const string OFF_KEYWORD = "_FOG_OFF";
	#endregion

	#region PrivateFields
	//Shader Property Ids. It's always better to use id's than to send strings to the shader.
	private int _01_fog_depth_verticals_prop;
	private int _fog_texture_prop;

	//We use this to update fog if an object changes Z position.
	private float _previous_z;

	private Renderer _rend;
	private Material[] materials;

	private bool previous_activation_state;
	#endregion PrivateFields

#region Unity
#if UNITY_EDITOR
	//We subscribe to OnChange to update Fog if the inspector had changes. This only makes sense in Editor.
	private void OnEnable( )
	{
		if( FogData.HasInstance )
		{
			FogData.Instance.OnChange += _UpdateFog;
		}
	}

	private void OnDisable( )
	{
		if( FogData.HasInstance )
		{
			FogData.Instance.OnChange -= _UpdateFog;
		}
	}
#endif

	private void Awake( )
	{
		_01_fog_depth_verticals_prop = Shader.PropertyToID( "_01FogDepthVerticals" );
		_fog_texture_prop = Shader.PropertyToID( "_FogTexture" );

		_rend = GetComponent<Renderer>( );
		materials = _rend.materials;

		_UpdateFog( );
	}

	private void Update( )
	{
		//If the z changed, update fog
		if( _previous_z != transform.position.z )
		{
			_UpdateFog( );
		}
	}

	private void LateUpdate( )
	{
		_previous_z = transform.position.z;
	}
#endregion Unity

#region LocalMethods	
	private bool _IsFogSetAndEnabled( )
	{
		return FogData.HasInstance && FogData.Instance.Activated && FogData.Instance.isActiveAndEnabled;
	}

	private void _UpdateFog( )
	{
		if( !_IsFogSetAndEnabled( ) )
		{
			SwitchEffectInShader( false );
		}
		else
		{
			FogData fog_data = FogData.Instance;
			float object_z = Mathf.Abs( transform.position.z );

			//If object is behind we consider front fog, otherwise we consider back fog
			Vector2 fog_limits = fog_data.fogBack;
			float depth_01 = 1;

			//If we our z position is less than 0, we use fog front.
			if( transform.position.z < 0 )
			{
				fog_limits = fog_data.fogFront;
				depth_01 *= -1;
			}

			//Send properties if object is in fog boundaries.
			bool in_fog_zone = object_z >= fog_limits.x;
			if( in_fog_zone )
			{
				Texture2D fog_tex = fog_data.fogRampTexture;
				Vector2 fog_vertical = fog_data.verticalSplit;

				//This calculates the absolute coordinates for depth depending on FogBack and FogFront, and Z position of the object.
				depth_01 *= Mathf.Clamp01( 1f / ( fog_limits.y - fog_limits.x ) * ( object_z - fog_limits.x ) );

				//Send these properties to the shader, compressed in a Vector3, plus the texture
				_SetShaderFogProperties( new Vector3( depth_01, fog_vertical.x, fog_vertical.y ), fog_tex );
			}
			else
			{
				//If we are out the fog zone, deactivate the shader.
				SwitchEffectInShader( false );
			}
		}
	}

	//This sends the properties to the shader. The Vector with 3 properties of fog boundaries and coordinates, and the texture.
	private void _SetShaderFogProperties( Vector3 fog_depth_verticals_, Texture2D fog_texture_ )
	{
		SwitchEffectInShader( true );

		foreach( Material material in materials )
		{			
			material.SetVector( _01_fog_depth_verticals_prop, fog_depth_verticals_ );
			material.SetTexture( _fog_texture_prop, fog_texture_ );
		}
	}

	//Just activate and deactivate the effect using multi compile, so it's not compiled if it's not necessary.
	public void SwitchEffectInShader( bool activate_ )
	{
		if(activate_ == previous_activation_state)
		{
			return;
		}

		foreach( Material material in materials )
		{

			if( activate_ )
			{
				material.DisableKeyword( OFF_KEYWORD );
				material.EnableKeyword( ON_KEYWORD );
			}
			else
			{
				material.DisableKeyword( ON_KEYWORD );
				material.EnableKeyword( OFF_KEYWORD );
			}
		}

		previous_activation_state = activate_;
    }
#endregion LocalMethods
}
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor( typeof( FogData ) )]
public class FogDataEditor : Editor
{
	//Boundaries
	private SerializedProperty fogBack_prop;
	private SerializedProperty fogFront_prop;
	private SerializedProperty fogVertical_prop;

	//Toggle
	private SerializedProperty useTexture_prop;

	//Texture
	private SerializedProperty fogRampTexture_prop;

	//Colors
	private SerializedProperty colorsResolution_prop;

	private SerializedProperty backBottom_prop;
	private SerializedProperty backTop_prop;
	private SerializedProperty frontBottom_prop;
	private SerializedProperty frontTop_prop;

	//target
	private FogData _data;

	#region Unity
	private void OnEnable( )
	{
		_data = target as FogData;

		//Boundaries
		fogBack_prop = serializedObject.FindProperty( "fogBack" );
		fogFront_prop = serializedObject.FindProperty( "fogFront" );
		fogVertical_prop = serializedObject.FindProperty( "verticalSplit" );

		//Toggle
		useTexture_prop = serializedObject.FindProperty( "useTexture" );

		//Texture
		fogRampTexture_prop = serializedObject.FindProperty( "fogRampTexture" );

		//Colors
		colorsResolution_prop = serializedObject.FindProperty( "colorsResolution" );

		backBottom_prop = serializedObject.FindProperty( "backBottom" );
		backTop_prop = serializedObject.FindProperty( "backTop" );
		frontBottom_prop = serializedObject.FindProperty( "frontBottom" );
		frontTop_prop = serializedObject.FindProperty( "frontTop" );
	}

	public override void OnInspectorGUI( )
	{
		serializedObject.Update( );

		EditorGUI.BeginChangeCheck( );

		//Boundaries
		EditorGUILayout.PropertyField( fogBack_prop );
		EditorGUILayout.PropertyField( fogFront_prop );
		EditorGUILayout.PropertyField( fogVertical_prop );

		//Toggle
		EditorGUILayout.PropertyField( useTexture_prop );

		//Hide and show depending on the toggle
		if( useTexture_prop.boolValue )
		{
			//Texture
			EditorGUILayout.PropertyField( fogRampTexture_prop );
		}
		else
		{
			//Colors
			EditorGUILayout.PropertyField( colorsResolution_prop );

			EditorGUILayout.PropertyField( backBottom_prop );
			EditorGUILayout.PropertyField( backTop_prop );
			EditorGUILayout.PropertyField( frontBottom_prop );
			EditorGUILayout.PropertyField( frontTop_prop );
		}

		if( EditorGUI.EndChangeCheck( ) )
		{
			serializedObject.ApplyModifiedProperties( );
			_data.CallOnChangeEvent( );
		}
	}
	#endregion Unity
}

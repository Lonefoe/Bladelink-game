using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T instance = null;

	// Returns the instance of this singleton.
	public static T Instance
	{
		get
		{
			if( instance == null )
			{
				instance = (T)FindObjectOfType( typeof( T ) );

				if( instance == null )
				{
					Debug.LogError( "An instance of " + typeof( T ) + " is needed in the scene, but there is none." );
				}
				else
				{
					T[] objects_of_type = FindObjectsOfType<T>( );
					if( objects_of_type.Length > 1 )
					{
						Debug.LogWarning( "There is " + objects_of_type.Length + " instances of the singlenton " + typeof( T ) + ". There should be only one." );
						foreach( T object_of_type in objects_of_type )
						{
							Debug.LogWarning( "", object_of_type );
						}
					}
				}
			}

			return instance;
		}
	}

	public static bool HasInstance
	{
		get
		{
			if( instance == null )
			{
				instance = (T)FindObjectOfType( typeof( T ) );
			}

			return instance == null ? false : true;
		}
	}
}
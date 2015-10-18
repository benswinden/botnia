using UnityEngine;
using System.Collections;

namespace Scripts
{
	public class DontDestroyOnLoad : MonoBehaviour {
	
		// Use this for initialization
		void Awake () {
			DontDestroyOnLoad(gameObject);
		}
		
		void Start()
		{
			Destroy (this);
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
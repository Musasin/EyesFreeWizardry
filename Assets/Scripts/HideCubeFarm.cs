using UnityEngine;
using System.Collections;

public class HideCubeFarm : MonoBehaviour {

	public GameObject hideCube;

	// Use this for initialization
	void Start () {
		for(int x = -9; x < 10; x++)
		{
			for(int z = -10; z < 11; z++)
			{
				Instantiate (hideCube, new Vector3(x, 2, z), Quaternion.identity);
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

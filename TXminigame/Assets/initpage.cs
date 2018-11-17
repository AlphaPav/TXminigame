using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initpage : MonoBehaviour {

    public GameObject pagePrefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        initPage(5);

    }
    
    private void initPage(int color)
    {
        Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), 0.321f, Random.Range(-10f, 10f));
        //Debug.Log(Random.Range(-100f, 77f));
        GameObject.Instantiate(pagePrefab, randomPosition,transform.rotation);
        
    }
}

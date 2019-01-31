using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleKiller : MonoBehaviour
{

	public float killTimer;
    // Start is called before the first frame update
    void Start()
    {
		killTimer *= 2;
        if(killTimer == 0f)
		{
			killTimer = gameObject.GetComponent<ParticleSystem>().main.duration * 2;
		}
    }

    // Update is called once per frame
    void Update()
    {
		killTimer -= Time.deltaTime;
		if (killTimer <= 0f)
			Destroy(gameObject);
    }


}

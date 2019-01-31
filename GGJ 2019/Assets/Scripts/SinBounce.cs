using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinBounce : MonoBehaviour
{

	// Start is called before the first frame update
	public float bounceHeight;
	public float bounceSpeed;
	public bool jumping = true;
	private float startY;



    void Start()
    {
		startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
		if (jumping)
		{
			transform.position = new Vector3(transform.position.x, startY + Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed)) * bounceHeight, transform.position.z);
		}
		else
		{
			transform.position = new Vector3(transform.position.x, startY, transform.position.z);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Vector3 target;
	public Transform bestCameraAngle;
	[SerializeField] private float followSmoothness;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

		
    }

	private void StateChecker()
	{
		/*
		 switch(GameManager.state)
		 {
			case(state.whatever)
			{
				fuckyou();
			}
			break;
		 }
		*/
	}

	public void FollowTarget()
	{
		Camera.main.transform.position = bestCameraAngle.position;
		Camera.main.transform.rotation = bestCameraAngle.rotation;

		if (Vector3.Distance(gameObject.transform.position, target) > 0.05f)
		{
			transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * followSmoothness);
		}
	}
}

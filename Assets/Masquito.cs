using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Masquito : MonoBehaviour
{

    private  const float PI = 3.1415926F;
    private float SpeedAngleConstant = 10F;

    //Speed Dependencies
    private float speed = 0F; // Move Length Per Second
    private float deltaSpeed = 0F;
    private float newSpeed = 0F;
    private float spScaler = 15F;
    private float baseSpeed = 0.8F;
    private float maxSpeed = 100F;

    //Direction Dependencies
    private float direction = 0.0F;//In Radias
    private float deltadir = 0.01F; // In Radias
    private float newdeltadir = 0.01F;
    private float deltadeltadir = 0.0F;
    private float dirScaler= 8F;//In Angle
    private float dirRange = 15F;//In Angle
    private float straightdelta = PI * 3F / 180F; //inrange

    private float AccuTime = 0;
    private float moveDuration = 1.0F;
    private float transTime = 1.0F; //the time for a masquito to change it's speed and direction.
    private float baseDuration = 0.1F;
    private float durScale = 0.5F;



    // Start is called before the first frame update
    void Start()
    {
        direction = transform.rotation.z;
        moveDuration = 0.1f;
        newSpeed = 0.1f;
        deltaSpeed = newSpeed - speed;
        AccuTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        AccuTime += Time.deltaTime;


        direction += deltadir;

        if (direction > 2 * PI)
        {
            direction -= 2 * PI;
        }
        else if (direction < 0)
        {
            direction += 2 * PI;
        }



        //Movement
        float x = transform.position.x;
        float y = transform.position.y;
        float deltaPortion = Time.deltaTime / moveDuration;

        speed += deltaSpeed * deltaPortion;
        deltadir += deltadeltadir * deltaPortion;

        y += Mathf.Sin(direction) * speed * Time.deltaTime;
        x += Mathf.Cos(direction) * speed * Time.deltaTime;

        transform.position = new Vector3(x, y, transform.position.z);
        transform.eulerAngles = new Vector3(0, 0, direction * 180 / PI);
        //if( direction > 1.57 && direction < 4.71)
        //{
        //    transform.localScale = new Vector3(1, -1, 1);
        //}
        //else
        //{
        //    transform.localScale = new Vector3(1, 1, 1);
        //}
       


        if (AccuTime >= moveDuration )
        {
            //Arrange New Duration
            moveDuration = Convert.ToSingle(GlobalVars.rand.NextDouble())*durScale + baseDuration;


           
            AccuTime = 0;
            //Bias Random Direction
            double selector = GlobalVars.rand.NextDouble();
            if (selector < 0.2)
            {
                //Do Nothing, Keep The Delta Direction
            }
            else if (selector < 0.2 + 0.1)
            {
                //Select the Inverse of Delta Direction.
                newdeltadir = -newdeltadir;
            }
            else
            {
                //Randomly Select New Delta Direction
                newdeltadir = (Convert.ToSingle(GlobalVars.rand.NextDouble() - 0.5) *2F * dirScaler) * PI / 180f;
                float deltadir_angle = newdeltadir * 180F / PI;
                if (deltadir_angle >dirRange )
                {
                    newdeltadir = 0;
                    //deltadir = (dirRange) * PI / 180F;
                }
                else if(deltadir_angle < -dirRange)
                {
                   newdeltadir = 0;
                    //deltadir = (-dirRange) * PI / 180F;
                }
            }
           
            //If It Tends to move straight
            if (Math.Abs(newdeltadir) < straightdelta)
            {
                newSpeed = Convert.ToSingle(GlobalVars.rand.NextDouble()) * spScaler + baseSpeed;
            }
            else 
            {
                newSpeed = SpeedAngleConstant * Math.Abs(newdeltadir)+5F;
            }
            if (newSpeed > maxSpeed) newSpeed = maxSpeed;
            deltaSpeed = newSpeed - speed;
            deltadeltadir = newdeltadir - deltadir;
            //newSpeed = baseSpeed + Convert.ToSingle(GlobalVars.rand.NextDouble()) * spScaler;

        }

    }
}

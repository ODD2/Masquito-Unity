using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Masquito : MonoBehaviour
{

    private  const double PI = 3.1415926F;
    private double SpeedAngleConstant = 10F;

    //Speed Dependencies
    private double speed = 0F; // Move Length Per Second
    private double deltaSpeed = 0F;
    private double newSpeed = 0F;
    private double spScaler = 15F;
    private double baseSpeed = 0.8F;
    private double maxSpeed = 100F;

    //Direction Dependencies
    private double direction = 0.0F;//In Radias
    private double deltadir = 0.01F; // In Radias
    private double newdeltadir = 0.01F;
    private double deltadeltadir = 0.0F;
    private double dirScaler= 8F;//In Angle
    private double dirRange = 15F;//In Angle
    private double straightdelta = PI * 3F / 180F; //inrange

    private double AccuTime = 0;
    private double moveDuration = 1.0F;
    private double transTime = 1.0F; //the time for a masquito to change it's speed and direction.
    private double baseDuration = 0.1F;
    private double durScale = 0.5F;



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
        double x = transform.position.x;
        double y = transform.position.y;
        double deltaPortion = Time.deltaTime / moveDuration;

        speed += deltaSpeed * deltaPortion;
        deltadir += deltadeltadir * deltaPortion;

        y += Mathf.Sin((float)direction) * speed * Time.deltaTime;
        x += Mathf.Cos((float)direction) * speed * Time.deltaTime;

        transform.position = new Vector3((float)x,(float)y, transform.position.z);
        transform.eulerAngles = new Vector3(0, 0, (float)(direction * 180 / PI));


        if (AccuTime >= moveDuration )
        {
            //Arrange New Duration
            moveDuration = GlobalVars.rand.NextDouble()*durScale + baseDuration;


           
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
                double deltadir_angle = newdeltadir * 180F / PI;
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
                newSpeed =GlobalVars.rand.NextDouble() * spScaler + baseSpeed;
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

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Masquito : MonoBehaviour
{

    //Location Variables
    private double x = 0;
    private double y = 0;

    //Environment Settings
    private const double GoBackDistance = 20;
    private const double PI = 3.1415926F;
    private double SpeedAngleConstant = 10F;

    //Speed Dependencies
    private double speed = 0F; // Move Length Per Second
    private double deltaSpeed = 0F;
    private double newSpeed = 0F;
    private double baseSpeed = 0.1F;
    private double maxSpeed = 20F;
    private double straightSpScaler = 30F;
    private double rotateSpScaler = 15F;
    private double maxAngleSpeed = 20F; //Test Purpose

    //Direction Dependencies
    private double direction = 0.0F;//In Radias
    private double deltadir = 0.01F; // In Radias
    private double newdeltadir = 0.01F;
    private double deltadeltadir = 0.0F;
    private double dirScaler= 20F;//In Angle
    private const double dirRange = 20F;//In Angle
    private const double dirRangeRad = dirRange * PI / 180F;
    private double straightdelta = PI * 3F / 180F; //inrange

    //Movement Dependencies
    private double moveDuration = 1.0F;
    private double transTime = 1.0F; //the time for a masquito to change it's speed and direction.
    private double baseDuration = 0.1F;
    private double durScale = 0.3F;

    //Counters
    private double AccuTime = 0;


    //Dangerous Dependencies
    public Vector3 lastCursorPosition = new Vector3(1000,1000, 10);
    public Vector3 cursor;
    private double dangerous3Position = 3;
    public double distance;
    public bool lastDangerous;
    public Sprite spriteOrigin;
    public Sprite spriteDangerous;


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

        direction = CheckRadias(direction + deltadir);



        //Movement
         x = transform.position.x;
         y = transform.position.y;
        double deltaPortion = Time.deltaTime / transTime;

        speed += deltaSpeed * deltaPortion;
        deltadir += deltadeltadir * deltaPortion;

        y += Mathf.Sin((float)direction) * speed * Time.deltaTime;
        x += Mathf.Cos((float)direction) * speed * Time.deltaTime;

        transform.position = new Vector3((float)x,(float)y, transform.position.z);
        transform.eulerAngles = new Vector3(0, 0, (float)(direction * 180 / PI));



        if (AccuTime >= moveDuration )
        {
            //Arrange New Behavior
            AccuTime = 0;
            if (Math.Pow(x * x + y * y, 0.5) > GoBackDistance)
            {  //Too far away from the center point
                //Try to stay close to the center point
                GoCenterBehavior();
            }
            else if (Dangerous3())
            {
                if (!lastDangerous)
                {
                    GetComponent<SpriteRenderer>().sprite = spriteDangerous;
                    ChangeToDangerous3();
                }
                lastDangerous = true;
            }
            else if (lastDangerous)
            {
                lastDangerous = false;
                GetComponent<SpriteRenderer>().sprite = spriteOrigin;
                ChangeToNormal();
            }
            else
            {
                NormalBehavior();
            }

            if (newSpeed > maxSpeed) newSpeed = maxSpeed;
            deltaSpeed = newSpeed - speed;

            if (newdeltadir > dirRangeRad) newdeltadir = dirRangeRad;
            else if (newdeltadir < -dirRangeRad) newdeltadir = -dirRangeRad;
            deltadeltadir = newdeltadir - deltadir;
        }
    }



    private double CheckRadias(double rad)
    {
        if(rad > 0)
        {
            rad = rad %(2* PI);
        }
        else{
            rad = -1 * Math.Abs(rad) %(2* PI);
        }
        return rad;
    }

    private void GoCenterBehavior()
    {
        moveDuration = 0.1;
        direction = CheckRadias(Math.Atan2(y, x) + PI);
        newdeltadir = 0;
        deltadir = 0;
        newSpeed = baseSpeed;
    }

    private void NormalBehavior()
    {
        moveDuration = GlobalVars.rand.NextDouble() * durScale + baseDuration;
        //Bias Random Direction
        double selector = GlobalVars.rand.NextDouble();
        if (selector < 0.2)
        {
            //Do Nothing, Keep The Delta Direction
        }
        else if (selector < 0.2 + 0.3)
        {
            //Select the Inverse of Delta Direction.
            newdeltadir = -newdeltadir;
        }
        else
        {
            //Randomly Select New Delta Direction
            newdeltadir = (Convert.ToSingle(GlobalVars.rand.NextDouble() - 0.5) * 2F * dirScaler) * PI / 180f;
            double deltadir_angle = newdeltadir * 180F / PI;
            if (deltadir_angle > dirRange)
            {
                newdeltadir = 0;
            }
            else if (deltadir_angle < -dirRange)
            {
                newdeltadir = 0;
            }
        }


        //Different speed in different modes.
        if (Math.Abs(newdeltadir) < straightdelta)
        {
            //If It Tends to move forward.
            newSpeed = GlobalVars.rand.NextDouble() * straightSpScaler + baseSpeed;
        }
        else
        {
            //If It Tends to turn.
            newSpeed = SpeedAngleConstant * Math.Abs(newdeltadir) + GlobalVars.rand.NextDouble() * rotateSpScaler + baseSpeed;
        }
    }

    void ChangeToNormal()
    {
        speed /= 1.3f;
        NormalBehavior();
    }

    void ChangeToDangerous3()
    {
        AccuTime = 0;
        speed *= 1.3f;
        Vector2 transToCursor = new Vector2(transform.position.x - cursor.x, transform.position.y - cursor.y);
        Vector2 dir = new Vector2(Mathf.Cos((float)direction), Mathf.Sin((float)direction));
        deltadir = Vector2.SignedAngle(dir, transToCursor + dir) / 180 * PI / 30;
        deltadeltadir = 0;
    }

    bool Dangerous3()
    {
        cursor = Input.mousePosition;
        cursor.z = 20;
        cursor = Camera.main.ScreenToWorldPoint(cursor);
        Vector3 pos = transform.position;
        pos.z = 20;
        distance = Math.Sqrt(Math.Pow(cursor.x - pos.x, 2) + Math.Pow((cursor.y - pos.y), 2));
        double lastDistance = Math.Sqrt(Math.Pow(lastCursorPosition.x - pos.x, 2) + Math.Pow((lastCursorPosition.y - pos.y), 2));
        lastCursorPosition = cursor;

        //if (distance < dangerous3Position && distance < lastDistance)
        //    return true;
        if (distance < dangerous3Position)
            return true;

        return false;
    }
}

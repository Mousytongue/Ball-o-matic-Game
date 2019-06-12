using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollision : MonoBehaviour {
    bool hit = false;
    float timer = 0;
    const int SPEED_THRESHOLD = 30;
    const int ROTATE_BACK_THRESHOLD = 80;
    const int PUSHBACK = 40;
    const int ROTATION_SPEED = 2;
    const float LIFESPAN_AFTER_HIT = 1.5f;
    AudioSource hitSound, clown3Sound, clown15Sound, dogSound;
    public GameObject smallExplosion;
    public GameObject player = null;
    const int GREEN_TARGET_POINTS = 5;
    const int RED_TARGET_POINTS = -15;
    const int POINTS_NOTICE_SIZE = 20;
    const int SMALL_STREAK_BONUS = 15;
    const int LARGE_STREAK_BONUS = 30;
    Color GREEN_HIT_COLOR = Color.green;
    Color RED_HIT_COLOR = Color.red;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        AudioSource[] audios = GetComponents<AudioSource>();
        hitSound = audios[0];
        clown3Sound = audios[1];
        clown15Sound = audios[2];
        dogSound = audios[3];
    }

    void Update ()
    {
        if (hit)
            FlopForward();
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Sphere") && collision.relativeVelocity.magnitude > SPEED_THRESHOLD)
        {
            if (hit == false)
            {
                //Pop the ball prevents multiple hits to count if we want
                Instantiate(smallExplosion, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                Destroy(collision.gameObject); 
                hit = true;

                if (gameObject.tag == "GreenTarget")
                {
                    player.GetComponent<BallMachineBehavior>().IncrementStreak(); //increase streak
                    int streak = player.GetComponent<BallMachineBehavior>().GetStreak(); //get the streak for easy checks

                    if(streak % 3 != 0) //Not going to receive the streak bonus
                    {
                        player.GetComponent<BallMachineBehavior>().UpdateScore(GREEN_TARGET_POINTS); //increase score
                        ShowPoints(GREEN_TARGET_POINTS, GREEN_HIT_COLOR); //shows hit points
                    }
                    else if (streak % 3 == 0 && streak % 15 != 0 && streak != 0) //streak mult of 3, not mult 15
                    {
                        clown3Sound.Play(0); //happy clown
                        player.GetComponent<BallMachineBehavior>().UpdateScore(SMALL_STREAK_BONUS); //increase score
                        ShowPoints(SMALL_STREAK_BONUS, GREEN_HIT_COLOR);

                    }
                    else if (streak % 15 == 0 && streak != 0) //streak 15
                    {
                        clown15Sound.Play(0); //mad clown
                        player.GetComponent<BallMachineBehavior>().UpdateScore(LARGE_STREAK_BONUS); //increase score
                        ShowPoints(LARGE_STREAK_BONUS, GREEN_HIT_COLOR);
                    }
                }
                else //RedTarget
                {
                    player.GetComponent<BallMachineBehavior>().UpdateScore(RED_TARGET_POINTS); //decrease score
                    ShowPoints(RED_TARGET_POINTS, RED_HIT_COLOR);
                    player.GetComponent<BallMachineBehavior>().ResetStreak(); //reset streak
                    
                    dogSound.Play(0); //dog yelp

                    GameObject mc = GameObject.FindGameObjectWithTag("MainControls");
                    mc.GetComponent<MainController>().IncreaseSpawnRate();
                }
            }
            player.GetComponent<BallMachineBehavior>().ShowScore();
            PushTargetBack();
            hitSound.Play(0);       //Smackin board sound     
        }
    }

    void PushTargetBack()
    {
        timer = 0;
        if (transform.parent.localRotation.x < ROTATE_BACK_THRESHOLD)
            transform.parent.eulerAngles += new Vector3(PUSHBACK, 0f, 0f);
    }

    void FlopForward()
    {
        timer += Time.deltaTime;
        transform.parent.eulerAngles -= new Vector3(ROTATION_SPEED, 0, 0);
        if (timer >= LIFESPAN_AFTER_HIT)
        {
            Destroy(transform.parent.gameObject);
            //Particles
        }
    }

    void ShowStreak(int streak)
    {
        //player.GetComponent<BallMachineBehavior>().pointsText. = "";
    }

    void ShowPoints(int points, Color col)
    {
        FloatingText pointsText = Resources.Load<FloatingText>("Prefabs/Prefab Objects/PopupPointsParent");
        FloatingText instance = Instantiate(pointsText);
        instance.transform.SetParent(GameObject.Find("MainCanvas").transform, false);
        instance.transform.localPosition = transform.position;
        //GameObject.Find("MainCanvas").transform.position = transform.localPosition;
        instance.SetSize(POINTS_NOTICE_SIZE); //Not working for some reason
        instance.SetColor(col); //Not working for some reason
        instance.SetText(points.ToString());
    }

}
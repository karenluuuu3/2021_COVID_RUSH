using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    public Transform player;
    static Animator anim;

    private float dis;
    private bool flg=false;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);
        dis = Vector3.Distance(player.position, this.transform.position);

        if (dis < 20 && angle < 45 && !flg)
        {
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                        Quaternion.LookRotation(direction), 0.1f);


            anim.SetBool("isIdle", false);
            this.transform.Translate(0, 0, 0.01f);
            anim.SetBool("isRunning", true);

            /*
            if (direction.magnitude > 5)
            {
                this.transform.Translate(0, 0, 0.01f);
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }
            */
            
        }
        else
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isRunning", false);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (dis > 20)// avoid pushing player
            {
                flg = false;
            }
            else
            {
                flg = true;
            }
            /*
            collectParticle.Play();
            GetComponent<AudioSource>().Play();// get
            Destroy(col.gameObject);
            mEventStore.Notify("onPickupItem", this, col.gameObject.tag);
            */
        }
    }
}

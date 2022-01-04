using UnityEngine;
using System.Collections;
using COVID_RUSH;

public class Player : MonoBehaviour {

		private Animator anim;
		private CharacterController controller;
		private EventStore mEventStore = EventStore.instance;

		public float speed = 600.0f;
		public float turnSpeed = 400.0f;
		private Vector3 moveDirection = Vector3.zero;
		public float gravity = 20.0f;
		public int playerState=0;   //0=idle	1=walk	2=jump

		[SerializeField] ParticleSystem collectParticle = null;
		public AudioClip clip;


		void Start () {
			controller = GetComponent <CharacterController>();
			anim = gameObject.GetComponentInChildren<Animator>();
			anim.SetInteger ("AnimationPar", playerState);
			AudioSource audio = GetComponent<AudioSource>();
			UpdateCompass();
		}

		void KeyEnventCon(){
			if (Input.GetKey ("s")) {
				if(playerState!=2)	playerState = 1;
				else{
					moveDirection.z = (transform.forward * Input.GetAxis("Vertical") * speed).z;
					moveDirection.x = (transform.forward * Input.GetAxis("Vertical") * speed).x;
				}
				//else moveDirection.z =-5;
			}
			else if (Input.GetKey ("w")){
				if(playerState!=2) playerState = 1;
				else{
					moveDirection.z = (transform.forward * Input.GetAxis("Vertical") * speed).z;
					moveDirection.x = (transform.forward * Input.GetAxis("Vertical") * speed).x;
				}
				//else moveDirection.z =5;
			}
			else if (Input.GetKey (KeyCode.Space)){
				if (playerState!=2){
					moveDirection.y=10;
					playerState = 2;
				}
			}  
			else if(Input.GetKeyUp ("w")||Input.GetKeyUp ("s")&&playerState!=2){
				moveDirection.z=0;
				playerState = 0;
			}
			
		}

		void MovementCon(){
			if(controller.isGrounded){
				moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
				if (playerState==2)	playerState = 0;	//if is jumping
				//anim.SetInteger ("AnimationPar", 0);
			}
			float turn = Input.GetAxis("Horizontal");
			transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
			controller.Move(moveDirection * Time.deltaTime);
			moveDirection.y -= gravity * Time.deltaTime;
		}

		private void UpdateCompass()
		{
			// Notify this event to update minimap & compass
			Vector2 newDirection = new Vector2(transform.forward.x, -transform.forward.z);
			mEventStore.Notify("onChangeForward", this, newDirection);

			// TODO: Replace fixed destination position
			Vector2 destinationDirection = new Vector2(45 - transform.position.x, -90 - transform.position.z);
			mEventStore.Notify("onChangeDestination", this, destinationDirection);
		}

		private bool IsObtainable(string tag)
		{
			return (tag == "Props_Vaccine") || (tag == "Props_Facemask") || (tag == "Props_Needle");
		}

		private void OnTriggerEnter(Collider col)
		{
			if (IsObtainable(col.gameObject.tag))
			{
				collectParticle.Play();
				GetComponent<AudioSource>().Play();// get
				Destroy(col.gameObject);
				mEventStore.Notify("onPickupItem", this, col.gameObject.tag);
			}
		}

		void Update (){
			// TODO: Uncomment this if you want to start with Start Scene
			// if (!GameManager.instance.IsGaming()) return;
			MovementCon();
			KeyEnventCon();
			UpdateCompass();
			anim.SetInteger ("AnimationPar", playerState);

		}

		
}

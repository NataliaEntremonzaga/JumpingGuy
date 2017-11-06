using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Animator animator;
	public GameObject game;
	public GameObject enemyGenerator;
	public AudioClip jumpClip;
	public AudioClip dieClip;
	public AudioClip pointClip;
	private AudioSource audioPlayer;
	private float startY;
	public ParticleSystem dust;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		audioPlayer = GetComponent<AudioSource> ();
		startY = transform.position.y;
	}

	// Update is called once per frame
	void Update () {
		bool isGrounded = transform.position.y == startY;
		bool gamePlaying = game.GetComponent<GameController> ().gameState == GameState.Playing;
		if (isGrounded) {
			if (gamePlaying) {
				if (Input.GetKeyDown ("up") || Input.GetMouseButtonDown (0) || Input.GetKeyDown ("space")) {
					UpdateState ("PlayerJump");
					//sonido player jump
					audioPlayer.clip = jumpClip;
					audioPlayer.Play ();
				}
			}
		}
	}

	public void UpdateState(string state = null){
		if(state !=null){
			animator.Play(state);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Enemy") {
			Debug.Log ("Me Muero¡¡¡");
			UpdateState ("PlayerDie");
			game.GetComponent<GameController> ().gameState = GameState.Ended;
			enemyGenerator.SendMessage ("CancelGenerator", true);
			game.SendMessage ("ResetTimeScale", 0.5f);

			//sonido player die
			game.GetComponent<AudioSource> ().Stop ();
			audioPlayer.clip = dieClip;
			audioPlayer.Play ();
			//dust.Stop ();
		} else if (other.gameObject.tag == "Point") {//Puntuacion del jugador
			game.SendMessage ("IncreasePoints");
			//sonido puntuacion
			audioPlayer.clip = pointClip;
			audioPlayer.Play ();
		}
	}
	void GameReady(){
		game.GetComponent<GameController> ().gameState = GameState.Ready;
	}

	void DustPlay(){
		dust.Play ();
	}

	void DustStop(){
		dust.Stop ();
	}
}

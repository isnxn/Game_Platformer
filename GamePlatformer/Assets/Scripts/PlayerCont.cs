using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCont : MonoBehaviour
{
	// animator dari player
	Animator anim;
	// rigidbody 2d dari player
	Rigidbody2D rigid;
	//untuk menyimpan state apakah karakter berada di ground
	public bool isGrounded = false;
	// untuk mengetahui arah hadap pada player
	public bool isFacingRight = true;
	// besar gaya untuk mengangkat karakter ke atas
	public float jumpForce = 200f;
	// besar gaya untuk mendorong karakter ke samping
	public float walkForce = 15f;
	// kecepatan maksimum dari karakter utama
	public float maxSpeed = 1.5f;
	// object peluru
	public GameObject projectile;
	// kecepatan peluru
	public Vector2 projectileVelocity = new Vector2 (50, 0);
	// jarak posisi peluru dari posisi player
	public Vector2 projectileOffset = new Vector2 (0.75f, -0.104f);
	// jeda waktu untuk menembak
	public float cooldown = 1f;
	//  memastikan untuk kapan dapat menembak
	bool isCanShoot = true;
	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator> ();
		rigid = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update ()
	{
		InputHandler ();
		anim.SetInteger ("Speed", (int)rigid.velocity.x);
	}
	void InputHandler ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			Fire ();
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			MoveLeft ();
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			MoveRight ();
		}
		if (Input.GetKeyDown (KeyCode.UpArrow) && isGrounded) {
			Jump ();
		}
	}
	void Fire(){
		// jika karakter dapat menembak
		if (isCanShoot) {
			anim.SetTrigger ("Shoot");
			//Membuat projectile baru
			GameObject bullet =  (GameObject)Instantiate(projectile,
				(Vector2)transform.position+projectileOffset*transform.localScale.x,Quaternion.identity);
			// mengatur kecepatan dari projectile
			Vector2 velocity = new Vector2(projectileVelocity.x * transform.localScale.x,
				projectileVelocity.y);
			bullet.GetComponent<Rigidbody2D> ().velocity = velocity;
			//Menyesuaikan scale dari projectile dengan scale karakter
			Vector3 scale = transform.localScale;
			bullet.transform.localScale = scale;
			StartCoroutine(CanShoot ());
		}
	}
	IEnumerator CanShoot(){
		isCanShoot = false;
		yield return new WaitForSeconds (cooldown);
		isCanShoot = true;
	}
	void MoveLeft ()
	{
		if (rigid.velocity.x * -1 < maxSpeed)
			rigid.AddForce (Vector2.left * walkForce);
		// membalik arah karakter apabila tidak menghadap ke kanan
		if (isFacingRight) {
			Flip ();
		}
	}
	void MoveRight ()
	{
		if (rigid.velocity.x * 1 < maxSpeed)
			rigid.AddForce (Vector2.right * walkForce);
		// membalik arah karakter apabila tidak menghadap ke kiri
		if (!isFacingRight) {
			Flip ();
		}
	}
	void Jump ()
	{
		rigid.AddForce (Vector2.up * jumpForce);
	}
	void Flip ()
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		isFacingRight = !isFacingRight;
	}
	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.CompareTag ("Ground")) {

			anim.SetBool ("IsGrounded", true);
			isGrounded = true;
		}
	}
	//digunakan untuk mengecek apakah Player masih diatas tanah atau tidak
	void OnCollisionStay2D (Collision2D col)
	{
		if (col.gameObject.CompareTag ("Ground")) {
			anim.SetBool ("IsGrounded", true);
			isGrounded = true;
		}
	}
	//digunakan untuk memberi tahu Player bahwa sudah tidak diatas tanah
	void OnCollisionExit2D (Collision2D col)
	{
		if (col.gameObject.CompareTag ("Ground")) {
			anim.SetBool ("IsGrounded", false);
			isGrounded = false;
		}
	}
}
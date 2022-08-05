using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedWeaponItem : WeaponItem
{
	public GameObject projectilePrefab;

	override public void Attack(GameObject origin, GameObject target, CombatantStats player)
	{

		//get dir of projectile
		Camera cam = Camera.main;
		Vector3 mouse = Mouse.current.position.ReadValue();
		mouse.z = 10;
		Vector2 worldMouse = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

		Vector2 vec = worldMouse - (Vector2)origin.transform.position;

        Projectile projectileScript = projectilePrefab.GetComponent<Projectile>();
        //projectileScript.damage = this.strength;
        projectileScript.damage = this.strength + ((int)(player.Strength / 2));
        projectileScript.dir = vec;
        projectileScript.origin = origin;
        GameObject projectile = Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity);
        /*GameObject projectile = Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity);
		Projectile projectileScript = projectile.GetComponent<Projectile>();*/
        //TODO TEST
        /*projectileScript.dir = vec;
		projectileScript.origin = origin;*/
    }
}

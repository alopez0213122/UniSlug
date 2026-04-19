using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;


public class EnemyAggro : MonoBehaviour
{
    [SerializeField] private EnemyShooting enemyShooting;
    [SerializeField] private CircleCollider2D aggroCollider;
    private void Start()
    {
        aggroCollider.radius = enemyShooting.GetAggroRadius();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            enemyShooting.SetAggro(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            enemyShooting.SetAggro(false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour {

    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    public float speed = 5f;
    public float spawnDelay = 0.5f;

    private bool movingRight = true;
    private float xMax;
    private float xMin;

    // Use this for initialization
	void Start ()
    {
        float distanceToCamera = this.transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary =Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
        xMin = leftBoundary.x;
        xMax = rightBoundary.x;

        SpawnUntilFull();
	}

    private void SpawnEnemies()
    {
        foreach (Transform child in transform)
        {
            // Spawn enemy
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;

            // Child enemy to Enemy Formation
            enemy.transform.parent = child;
        }
    }

    private void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition();

        if(freePosition)
        {
            // Spawn enemy
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;

            // Child enemy to Enemy Formation
            enemy.transform.parent = freePosition;
        }

        if(NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }        
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, new Vector3(width, height));
    }

    // Update is called once per frame
    void Update ()
    {
		if(movingRight)
        {
            this.transform.position += Vector3.right * speed * Time.deltaTime;
        } else {
            this.transform.position += Vector3.left * speed * Time.deltaTime;
        }

        float rightEdgeOfFormation = this.transform.position.x + (0.5f * width);
        float leftEdgeOfFormation = this.transform.position.x - (0.5f * width);

        if (leftEdgeOfFormation < xMin)
        {
            movingRight = true;
        } else if (rightEdgeOfFormation > xMax) {
            movingRight = false;
        }

        if(AllMembersDead())
        {
            //Debug.Log("Empty Formation");

            // Respawn enemies
            SpawnUntilFull();
        }
    }

    private Transform NextFreePosition()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }
        }
        return null;
    }

    private bool AllMembersDead()
    {
        foreach(Transform childPositionGameObject in transform)
        {
            if(childPositionGameObject.childCount > 0)
            {
                return false;
            }
        }

        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BallSpawner : NetworkBehaviour {

    public GameObject BallPrefab;
    public Transform SpawnPosition;
    public bool SpawnOnServerStart = true;
    public bool SpawnTestBall;

	// Use this for initialization
	public override void OnStartServer() {
        if (isServer && SpawnOnServerStart)
        {
            SpawnBall();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (SpawnTestBall)
        {
            SpawnBall();
            SpawnTestBall = false;
        }
	}

    void SpawnBall()
    {
        GameObject ball = (GameObject)Instantiate(BallPrefab, SpawnPosition.position, SpawnPosition.rotation);
        NetworkServer.Spawn(ball);
    }

    void SpawnBall(NetworkConnection networkConnection)
    {
        GameObject ball = (GameObject)Instantiate(BallPrefab, SpawnPosition.position, SpawnPosition.rotation);
        NetworkServer.SpawnWithClientAuthority(ball, networkConnection);
    }

    IEnumerator SpawnBallWhenReady(NetworkConnection netWorkConnection)
    {
        yield return new WaitUntil(() => netWorkConnection.isReady);
        SpawnBall(netWorkConnection);
    }
}

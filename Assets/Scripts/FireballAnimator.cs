using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code based vaguely on this:
//https://docs.unity3d.com/Manual/PartSysExplosion.html
public class FireballAnimator : MonoBehaviour {
    public float contactDelay=2.5f;
    public float explosionStartTime = 0.75f;
    public float explosionFinishTime = 0.25f;
    public Material burntTileMaterial;

    public GameObject fireball;
    public GameObject explosion;
    public HexCellObj parentTile;
    public LevelController levelController;
    public AIController aiController;
    public bool completed=false;
    List<MonsterStats> markedMonsters = new List<MonsterStats>();

    //Credit to user fafase on stackexchange for how to get child objects by name
    //http://answers.unity3d.com/questions/689549/find-child-objects-by-name.html
    void Awake () {
        foreach (Transform t in transform)
        {
            if (t.name == "ExplosionParticles")
            {
                explosion = t.gameObject;
            }
            else if (t.name == "FireballParticles")
            {
                fireball = t.gameObject;
            }
        }
        parentTile = transform.parent.gameObject.GetComponent<HexCellObj>();

        gameObject.transform.localScale = new Vector3(40, 1 / transform.parent.localScale.y, 40);
        gameObject.transform.position = gameObject.transform.position + new Vector3(0, parentTile.modelHeight/2, 0);


        aiController = GameObject.Find("AIController").GetComponent<AIController>() as AIController;
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>() as LevelController;
    }

	// Use this for initialization
	void Start () {

        Debug.Log("coroutime fireball!");
        StartCoroutine(Fireball());

        //Debug.Log("coroutime explosion start!");

        //Debug.Log("coroutime explosion end!");
	}

    //kill condition
    void Update() {
        if (completed)
        {
            Debug.Log("hasta la vista!");
            levelController.turnFrozen = false;
            Destroy(gameObject);
        }
    }

    //Summons the particles for the explosion, sets center tile extra crispy, and starts killing monsters.
    IEnumerator StartExplosion()
    {
        Debug.Log("explosion start");
        PathCell targetedCell = aiController[parentTile.q, parentTile.r, parentTile.h];

        foreach (Monster m in aiController.monsters)
        {
            PathCell monsterLoc = aiController.pathGrid[m.CurrentCell[0], m.CurrentCell[1], m.CurrentCell[2]];

            if (aiController.DistBetween(targetedCell, monsterLoc) <= 0)
            {
                m.gameObject.GetComponent<MonsterStats>().Health -= 50;
            }
            else if (aiController.DistBetween(targetedCell, monsterLoc) <= 1) //if we aren't in the immediate vicinity of the monsters, but they're in range of the fireball explosion...
            {
                markedMonsters.Add(m.gameObject.GetComponent<MonsterStats>()); //add the monster stats to the "you're already dead" list
            }
        }

        parentTile.gameObject.GetComponent<Renderer>().material = this.burntTileMaterial;

        fireball.SetActive(false);
        explosion.SetActive(true);
        ParticleSystem explosionParticles = explosion.GetComponent<ParticleSystem>();
        explosionParticles.Play();
        yield return new WaitForSeconds(explosionStartTime);

        Debug.Log("end explosion start");
        StartCoroutine(FinishExplosion());
    }

    //Kills monsters, sets outer tiles extra crispy, and ends explosion.
    IEnumerator FinishExplosion()
    {
        Debug.Log("begin explosion end");
        //now the monsters have caught up
        foreach (MonsterStats m in markedMonsters)
        {
            m.Health -= 50;
        }

        //now turn the remaining tiles in the AoE extra crispy
        PathCell[] surroundingCells = aiController.pathGrid.GetRadius(parentTile.q, parentTile.r, parentTile.h, 1, 5, true);
        foreach (PathCell cell in surroundingCells)
        {
            HexCellData cellData = levelController.levelGrid[cell.q, cell.r, cell.h];
            cellData.hexCellObject.gameObject.GetComponent<Renderer>().material = this.burntTileMaterial;
        }
        yield return new WaitForSeconds(explosionFinishTime);
        explosion.SetActive(false);
        Debug.Log("end explosion end");
        completed = true;

    }

    //Summons the particles for the fireball and waits until it hits
    IEnumerator Fireball()
    {
        fireball.SetActive(true);
        ParticleSystem fireballParticles = fireball.GetComponent<ParticleSystem>();
        fireballParticles.Play();
        Debug.Log("insanely overpowered fireball");
        yield return new WaitForSeconds(contactDelay);

        Debug.Log("fireball end");
        StartCoroutine(StartExplosion());
    }
}

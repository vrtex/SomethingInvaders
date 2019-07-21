using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;


// more like a game manager
public class WaveManager : MonoBehaviour
{
    private enum Direction
    {
        Right,
        Left,
        Down
    }

    public bool blockShootingAtStart = true;
    public PlayerController player;
    public EndZone endZone;

    public int currentMap = 0;
    public List<TextAsset> mapFiles;

    private List<GameObject> currentWave = new List<GameObject>();
    private List<GameObject> approachingShips = new List<GameObject>();
    private bool waveReady = false;

    private Direction currentDirection = Direction.Right;
    private Direction nextDirection;
    private float lastStep;
    public float stepInterval = 1.5f;
    public float stepSize = 1f;
    private float lastPause;
    private float missedStepTime = -1;

    public EnemyBound[] bounds = new EnemyBound[2];

    public GameObject playerUI;
    public GameObject gameOverScreen;

    public Vector2 scatterAreaMin;
    public Vector2 scatterAreaMax;

    private Coroutine stepCoroutine;

    void Start()
    {
        player.GetComponent<Health>().OnDepletion += (e, args) => EndGame();
        endZone.OnEnemyDetected += (e, args) => EndGame();
        PrepareWave();
    }

    void PrepareWave()
    {
        Regex regex = new Regex("\\((.*?), (.*?), (.*?)\\) (.*?)$", RegexOptions.Multiline);

        if(stepCoroutine != null)
        {
            StopCoroutine(stepCoroutine);
            stepCoroutine = null;
        }

        ProjectileController[] projectiles = FindObjectsOfType<ProjectileController>();


        foreach(var p in projectiles)
            p.End();

        player.SetCanShoot(!blockShootingAtStart);

        MatchCollection matches = regex.Matches(mapFiles[currentMap % mapFiles.Count].text);

        lastStep = Time.time - stepInterval;

        foreach(Match m in matches)
        {
            Vector3 target = new Vector3(float.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture), float.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture), float.Parse(m.Groups[3].Value, CultureInfo.InvariantCulture));
            EnemyParser.EnemyType type = (EnemyParser.EnemyType)System.Enum.Parse(typeof(EnemyParser.EnemyType), m.Groups[4].Value);

            GameObject nextEnemy = ObjectPool.SpawnObject(ObjectPool.enemy, new Vector3(Random.Range(-8, 8), 0, Random.Range(12, 15)));

            var controller = nextEnemy.GetComponent<EnemyController>();
            controller.SetTargetPosition(target);
            controller.SetType(type);
            controller.OnDestinationReached += ShipApproached;
            controller.waveManager = this;

            var mover = nextEnemy.GetComponent<Mover>();
            mover.speed = new Vector2(5, 5);
            mover.alignment = Mover.MoveAlignment.Align;
            mover.rotationOffset = 180f;

            nextEnemy.GetComponent<Health>().OnDepletion += KillEnemy;

            currentWave.Add(nextEnemy);
            approachingShips.Add(nextEnemy);
        }

    }

    private void ShipApproached(object sender, System.EventArgs e)
    {
        EnemyController controller = sender as EnemyController;
        approachingShips.Remove(controller.gameObject);
        controller.OnDestinationReached -= ShipApproached;
        controller.Ready();

        waveReady = approachingShips.Count == 0;

        controller.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);

        var mover = controller.gameObject.GetComponent<Mover>();
        mover.speed = new Vector2(1, 1);
        mover.alignment = Mover.MoveAlignment.Tilt;


        if(waveReady)
        {
            foreach(var bound in bounds)
                bound.gameObject.SetActive(true);

            player.SetCanShoot(true);
            stepCoroutine = StartCoroutine(Step());
        }
    }

    public void SetPaused(bool paused)
    {
        if(stepCoroutine != null && paused)
        {
            StopCoroutine(stepCoroutine);
            stepCoroutine = null;
            lastPause = Time.time;
        }

        if(waveReady && !paused)
        {
            float elapsedTime = lastPause - lastStep;
            missedStepTime = stepInterval - elapsedTime;
            stepCoroutine = StartCoroutine(Step());
        }

    }

    private void KillEnemy(object sender, Health.HealthEventArgs e)
    {
        Health health = sender as Health;
        currentWave.Remove(health.transform.root.gameObject);
        health.OnDepletion -= KillEnemy;

        var controller = health.GetComponent<EnemyController>();

        player.AddPoints(EnemyParser.GetEnemyInfo(controller.type).scoreGiven);

        if(currentWave.Count == 0)
        {
            ++currentMap;
            PrepareWave();
        }
    }

    public void Reverse()
    {
        if(currentDirection == Direction.Down)
            return;
        nextDirection = currentDirection == Direction.Left ? Direction.Right : Direction.Left;
        currentDirection = Direction.Down;
    }

    private IEnumerator Step()
    {
        while(true)
        {
            //yield return new WaitForSeconds(stepInterval);
            yield return new WaitForSeconds(missedStepTime < 0 ? stepInterval : missedStepTime);
            missedStepTime = -1;
            Vector2 step = new Vector2();

            switch(currentDirection)
            {
                case Direction.Left:
                    step = new Vector2(-stepSize, 0);
                    break;
                case Direction.Right:
                    step = new Vector2(stepSize, 0);
                    break;
                case Direction.Down:
                    step = new Vector2(0, -stepSize * 0.5f);
                    currentDirection = nextDirection;
                    break;
            }

            foreach(GameObject enemy in currentWave)
                enemy.GetComponent<EnemyController>().Step(step);

            lastStep = Time.time;
        }
    }

    private void Scatter()
    {
        if(stepCoroutine != null)
        {
            StopCoroutine(stepCoroutine);
            stepCoroutine = null;
        }

        foreach(EnemyBound bound in bounds)
            bound.gameObject.SetActive(false);

        foreach(GameObject e in currentWave)
        {
            EnemyController controller = e.GetComponent<EnemyController>();
            controller.Ready(false);
            controller.SetTargetPosition(new Vector3(Random.Range(scatterAreaMin.x, scatterAreaMax.x), 0,Random.Range(scatterAreaMin.y, scatterAreaMax.y)));

            Mover mover = e.GetComponent<Mover>();
            mover.speed *= 7;
            mover.alignment = Mover.MoveAlignment.Align;
        }
    }

    private void EndGame()
    {
        Scatter();
        endZone.gameObject.SetActive(false);
        playerUI.SetActive(false);
        gameOverScreen.SetActive(true);
        gameOverScreen.GetComponent<GameOverText>().Setup(player.currentScore, currentMap);
        player.gameObject.SetActive(false);
    }
}

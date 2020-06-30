using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace ECS
{
    public class GameManager : Singleton<GameManager>
    {
        public Button startButton;
        public Button stopButton;
        [Header("Equipo Azul")]
        public TMP_InputField blueTeamEntitiesSpawn;
        public TMP_InputField blueTeamReinforces;
        public TMP_InputField blueTeamTimeDelay;
        public TextMeshProUGUI blueTeamTotalUnits;
        public Spawner2 blueSpawn;
        [Header("Equipo Rojo")]
        public TMP_InputField redTeamEntitiesSpawn;
        public TMP_InputField redTeamReinforces;
        public TMP_InputField redTeamTimeDelay;
        public TextMeshProUGUI redTeamTotalUnits;
        public Spawner2 redSpawn;

        private void Update()
        {
            blueTeamTotalUnits.text = blueSpawn.TotalUnits.ToString();
            redTeamTotalUnits.text = redSpawn.TotalUnits.ToString();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        public void StartSimulation()
        {
            int spawn1, spawn2;
            spawn1 = int.Parse(blueTeamEntitiesSpawn.text);
            spawn2 = int.Parse(redTeamEntitiesSpawn.text);
            blueSpawn.Spawn(spawn1);
            redSpawn.Spawn(spawn2);
            startButton.interactable = false;
            stopButton.interactable = true;
            StartReinforces();
            BulletSpawner.Instance.StartSimulation();
        }
        public void StopSimulation()
        {
            blueSpawn.ClearCubes();
            redSpawn.ClearCubes();
            BulletSpawner.Instance.ClearBullets();
            startButton.interactable = true;
            stopButton.interactable = false;
            StopAllCoroutines();
        }

        private void StartReinforces()
        {
            int spawn1, spawn2;
            spawn1 = int.Parse(blueTeamReinforces.text);
            spawn2 = int.Parse(redTeamReinforces.text);
            float time1, time2;
            time1 = float.Parse(blueTeamTimeDelay.text);
            time2 = float.Parse(redTeamTimeDelay.text);
            StartCoroutine(SpawnReinforces(blueSpawn, spawn1, time1));
            StartCoroutine(SpawnReinforces(redSpawn, spawn2, time2));
        }

        public IEnumerator SpawnReinforces(Spawner2 spawner, int n, float t)
        {
            while (true)
            {
                yield return new WaitForSeconds(t);
                spawner.Spawn(n);
            }
            
            
        }
        

    }
}

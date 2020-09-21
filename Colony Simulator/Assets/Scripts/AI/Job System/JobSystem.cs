using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSystem : MonoBehaviour {

    public static JobSystem Instance;
    
    public List<Job> AllJobs => _availableJobs;

    private List<Job> _availableJobs = new List<Job>();
    private static float jobUpdateCooldown = 0.1f;


    private void Awake() {

        if(Instance != null) {

            Debug.LogError("Only one JobSystem can exist at a time", this);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {

        StartCoroutine(ProcessJobs());
    }

    public void AddJob(Job job) => _availableJobs.Add(job);
    public void DeleteJob(Job job) => _availableJobs.Remove(job);

    private IEnumerator ProcessJobs() {

        while (true) {
            yield return new WaitForSeconds(jobUpdateCooldown);

            if (_availableJobs.Count == 0)
                continue;
            
            List<JobHandlerComponent> availableWorkers = GetAvailableWorkers();

            if (availableWorkers.Count == 0)
                continue;

            for (int i = availableWorkers.Count - 1; i >= 0; i--) {

                JobHandlerComponent worker = availableWorkers[i];

                foreach(Job possibleJob in _availableJobs) {

                    if (worker.CanDoJob(possibleJob)) {
                        possibleJob.AssignWorker(worker);
                        _availableJobs.Remove(possibleJob);
                        break;
                    }
                }
            }
        }
    }

    private List<JobHandlerComponent> GetAvailableWorkers() {

        List<JobHandlerComponent> availableWorkers = new List<JobHandlerComponent>();

        foreach(Human colonist in GameManager.Instance.characterManager.colonists) {

            JobHandlerComponent worker = colonist.jobHandlerComponent;

            if (worker.IsAvailable)
                availableWorkers.Add(worker);
        }

        return availableWorkers;
    }
}

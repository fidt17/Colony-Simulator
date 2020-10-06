using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSystem : Singleton<JobSystem> {

    public List<Job> AllJobs => _allJobs;
    public List<Job> AvailableJobs => _availableJobs;

    private List<Job> _allJobs = new List<Job>();
    private List<Job> _availableJobs = new List<Job>();
    private const float jobUpdateCooldown = 0.1f;

    private void Start() => StartCoroutine(ProcessJobs());

    public void AddJob(Job job) {
        _allJobs.Insert(0, job);
        _availableJobs.Insert(0, job);
    }

    public void ReturnJob(Job job) {
        _availableJobs.Insert(0, job);
    }

    public void DeleteJob(Job job) {
        _allJobs.Remove(job);
        _availableJobs.Remove(job);
    }

    private IEnumerator ProcessJobs() {
        while (true) {
            yield return new WaitForSeconds(jobUpdateCooldown);

            if (_availableJobs.Count == 0) {
                continue;
            }
            
            List<JobHandlerComponent> availableWorkers = GetAvailableWorkers();
            if (availableWorkers.Count == 0) {
                continue;
            }

            foreach (JobHandlerComponent worker in availableWorkers) {
                for (int i = _availableJobs.Count - 1; i >= 0; i--) {
                    if (worker.CanDoJob(_availableJobs[i])) {
                        worker.AssignJob(_availableJobs[i]);
                        _availableJobs.RemoveAt(i);
                    }
                }
            }
        }
    }

    private List<JobHandlerComponent> GetAvailableWorkers() {
        List<JobHandlerComponent> availableWorkers = new List<JobHandlerComponent>();
        foreach(Human colonist in GameManager.Instance.characterManager.colonists) {
            JobHandlerComponent worker = colonist.jobHandlerComponent;
            if (worker.IsAvailable) {
                availableWorkers.Add(worker);
            }
        }
        return availableWorkers;
    }
}
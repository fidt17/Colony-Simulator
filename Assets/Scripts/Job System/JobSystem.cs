using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSystem : Singleton<JobSystem> {

    //List of every single job on the map
    //the ones which have been already assigned to a worker
    //and those that are not yet assigned to anyone
    public List<Job> AllJobs => _allJobs;
    
    public List<Job> AvailableJobs => _availableJobs;

    private List<JobHandlerComponent> _availableWorkers = new List<JobHandlerComponent>(); 
    private List<Job> _allJobs = new List<Job>();
    //List of jobs to which workers might be assigned.
    private List<Job> _availableJobs = new List<Job>();

    //Job system refreshes and checks if there are any worker that can be assigned to a job
    private const float jobUpdateCooldown = 0.1f;

    private void Start() {
        StartCoroutine(ProcessJobs());
    }

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

    public void AddWorker(JobHandlerComponent worker) {
        _availableWorkers.Add(worker);
        ProcessJobs();
    }

    public void RemoveWorker(JobHandlerComponent worker) => _availableWorkers.Remove(worker);

    public IEnumerator HideJobForSeconds(Job job, float seconds) {
        DeleteJob(job);
        yield return new WaitForSeconds(seconds);
        AddJob(job);
    }

    private IEnumerator ProcessJobs() {
        while (true) {
            yield return new WaitForSeconds(jobUpdateCooldown);
            if (_availableJobs.Count == 0 || _availableWorkers.Count == 0) {
                continue;
            }
            for (int j = _availableWorkers.Count - 1; j >= 0; j--) {
                JobHandlerComponent worker = _availableWorkers[j];
                for (int i = _availableJobs.Count - 1; i >= 0; i--) {
                    if (_availableJobs[i].CanDoJob(worker)) {
                        worker.AssignJob(_availableJobs[i]);
                        _availableJobs.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }

    #region Testing

    public bool HasWorker(JobHandlerComponent worker) {
        return _availableWorkers.Contains(worker);
    }

    #endregion
}
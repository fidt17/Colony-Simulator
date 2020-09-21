using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugJobSystem : WindowComponent {
    
    public ScrollViewScript jobsList;

    private void OnEnable() {

        jobsList.ClearViewport();

        foreach(Job job in JobSystem.Instance.AllJobs) {

            GameObject element = jobsList.AddElement();
            element.GetComponent<TMPro.TextMeshProUGUI>().text = "" + job.GetType();
        }

        if (JobSystem.Instance.AllJobs.Count == 0) {

            jobsList.AddElement().GetComponent<TMPro.TextMeshProUGUI>().text = "No jobs available.";
        }
    }
}

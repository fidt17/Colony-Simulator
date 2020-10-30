using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vegetation : StaticObject, IHarvestable, ICuttable {

    public bool HasCutJob { get; set; }

    public virtual Item Cut() {
        Destroy();
        return null;
    }
    
    public virtual void Harvest() => Destroy();

    public void HandleCutJobResult(object job, System.EventArgs e) {
        if (((Job.JobResultEventArgs) e).result == true
        || (((Job.JobResultEventArgs) e).wasJobCanceled == true)) {
            ((Job) job).JobResultHandler -= HandleCutJobResult;
            HasCutJob = false;
        }
    }
}
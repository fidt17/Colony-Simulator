using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetation : StaticObject, IHarvestable, ICuttable {
       
    #region IHarvestable

    public virtual void Harvest() => Destroy();

    #endregion

    #region ICuttable

    public bool HasCutJob { get; set; }

    public virtual Item Cut() {
        Destroy();
        return null;
    }

    public void HandleCutJobResult(object job, System.EventArgs e) {
        if ((e as Job.JobResultEventArgs).result == true
        || ((e as Job.JobResultEventArgs).wasJobCanceled == true)) {
            (job as Job).JobResultHandler -= HandleCutJobResult;
            HasCutJob = false;
        }
    }

    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutJob : StaticJob {
    
    public ICuttable Vegetation => _cuttable;

    protected ICuttable _cuttable;
    
    public CutJob(ICuttable cuttable, Vector2Int jobPosition, GameObject jobIcon = null) : base(jobPosition, jobIcon) {
        _cuttable = cuttable;
        _cuttable.HasCutJob = true;
        JobResultHandler += _cuttable.HandleCutJobResult;
    }

    protected override void AddJobIcon() => _jobIcon = (_jobIcon is null) ? Factory.Create("cut job", _jobPosition) : _jobIcon;

    protected override void PlanJob() {
        _task = new CutTask(_worker.MotionComponent, GetDestinationNode().Position, _cuttable) as ITask;
        _worker.CommandProcessor.AddTask(_task);
        _task.ResultHandler += OnJobFinish;
    }
}
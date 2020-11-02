using System;
using NUnit.Framework;
using UnityEngine;

namespace Tests {
	public class JobTests {

		private class TestJob : Job {

			public TestJob(Vector2Int jobPosition, GameObject jobIcon = null) : base(jobPosition, jobIcon) {
				
			}
			
			protected override void PlanJob() {
				throw new System.NotImplementedException();
			}
		}

		private bool _jobFinished;
		private bool _wasJobCanceled;

		private void OnJobFinish(object source, EventArgs e) {
			_jobFinished = true;
			_wasJobCanceled = ((Job.JobResultEventArgs) e).wasJobCanceled;
		}
		
		[Test]
		public void add_job() {
			TestJob job = new TestJob(Vector2Int.zero, null);
			JobSystem.GetInstance().AddJob(job);

			if (JobSystem.GetInstance().AllJobs.Contains(job) == false) {
				Assert.Fail();
			}

			if (JobSystem.GetInstance().AvailableJobs.Contains(job) == false) {
				Assert.Fail();
			}
			
			job.DeleteJob();
			Assert.Pass();
		}

		[Test]
		public void delete_job_from_job_system() {
			TestJob job = new TestJob(Vector2Int.zero, null);
			_jobFinished = false;
			job.JobResultHandler += OnJobFinish;
			JobSystem.GetInstance().AddJob(job);
			job.DeleteJob();
			
			//test removal from job system lists
			if (JobSystem.GetInstance().AllJobs.Contains(job) == true) {
				Assert.Fail();
			}
			
			//test that job result handler did invoke
			if (JobSystem.GetInstance().AvailableJobs.Contains(job) == true) {
				Assert.Fail();
			}
			
			//test that job finished with flag wasJobCanceled
			if (_wasJobCanceled == false || _jobFinished == false) {
				Assert.Fail();
			}

			Assert.Pass();
		}
		
	}
}
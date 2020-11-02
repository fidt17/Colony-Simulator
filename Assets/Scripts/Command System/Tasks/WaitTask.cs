public class WaitTask : Task {
	public WaitTask(float time) {
		AddCommand(new WaitCommand(time));
	}
}

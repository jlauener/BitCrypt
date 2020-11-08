using System.Collections;

class Coroutine
{
	public IEnumerator routine;
	public Coroutine waitForCoroutine;
	public bool finished = false;

	public void Stop()
	{
		finished = true;
	}

	public Coroutine(IEnumerator routine) { this.routine = routine; }
}

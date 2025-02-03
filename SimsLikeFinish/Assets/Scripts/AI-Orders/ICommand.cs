using System.Collections;

public interface ICommand
{
    public IEnumerator Execute();
    public void Finish();
    public void Cancel();
}

namespace CaptainCoder.WebConsole;

public class Console
{
    public Queue<string> OutBuffer { get; } = new();
    public Queue<string> InputBuffer { get; } = new();
    private List<Action<Console>> _observers = new();
    private List<Action> _clearObservers = new();


    private bool _isWaitingForInput = false;
    public bool IsWaitingForInput
    {
        get => _isWaitingForInput;
        private set
        {
            if (_isWaitingForInput == value) return; 
            _isWaitingForInput = value;
            Notify();
        }
    }

    public void Write(string value) {
        OutBuffer.Enqueue(value);
        Notify();
    }

    public void Clear() {
        _clearObservers.ForEach(o => o.Invoke());
    }

    public void WriteLine()
    {
        OutBuffer.Enqueue("\n");
        Notify();
    }

    public void WriteLine(string value)
    {
        OutBuffer.Enqueue(value + "\n");
        Notify();
    }

    public async Task<string?> ReadLine()
    {
        System.Console.WriteLine($"Input Buffer size: {InputBuffer.Count}");
        while (InputBuffer.Count == 0)
        {
            IsWaitingForInput = true;
            await Task.Delay(100);
        }
        IsWaitingForInput = false;
        string line = InputBuffer.Dequeue();
        OutBuffer.Enqueue(line + "\n");
        Notify();
        return line;
    }


    public void RegisterClear(Action observer)
    {
        _clearObservers.Add(observer); 
    }

    public void Register(Action<Console> observer)
    {
        _observers.Add(observer);
    }

    public void Notify()
    {
        _observers.ForEach(o => o.Invoke(this));
    }
}
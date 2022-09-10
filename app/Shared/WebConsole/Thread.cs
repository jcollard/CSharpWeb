namespace CaptainCoder.WebConsole;

public static class Thread 
{

    public static async Task Sleep(int millis) {
        await Task.Delay(millis);
    }

}
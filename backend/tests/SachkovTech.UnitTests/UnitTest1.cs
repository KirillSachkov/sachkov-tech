namespace SachkovTech.UnitTests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var semaphoreSlim = new SemaphoreSlim(3);

        var tasks = Enumerable.Range(0, 100).Select(async _ =>
        {
            await semaphoreSlim.WaitAsync();

            try
            {
                await AsyncMethod();
            }
            finally
            {
                semaphoreSlim.Release();
            }
        });

        await Task.WhenAll(tasks);
    }

    private async Task AsyncMethod()
    {
        Console.WriteLine("Before uploading");
        await Task.Delay(2000);
        Console.WriteLine("After uploading");
    }
}
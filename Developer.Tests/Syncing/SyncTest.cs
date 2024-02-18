using Bogus;
using Developer.Service.Syncing;
using Developer.Service.Utils;
using Xunit.Abstractions;

namespace Developer.Service.Tests.Syncing;

public class SyncTest
{
    private readonly ITestOutputHelper _output;
    private readonly Faker _faker = new();

    private readonly SyncDebug _syncDebug = new();

    /// <summary>
    /// Constructor
    /// </summary>
    public SyncTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void InitializeList_Success()
    {
        // Arrange
        var items = new List<string>(_faker.Lorem.Words(50));

        var itemsHash = new HashSet<string>();
        items.ForEach(item => itemsHash.Add(item));


        // Act
        var resultList = _syncDebug.InitializeList(items)!;

        
        //Assert
        Assert.Equal(items.Count, resultList.Count);

        resultList.ForEach(resultItem => Assert.True(itemsHash.Contains(resultItem), $"Cannot find item {resultItem}"));
    }

    [Fact]
    public async Task InitializeListAsync_Success()
    {
        // Arrange
        var items = new List<string>(_faker.Lorem.Words(50));

        var itemsHash = new HashSet<string>();
        items.ForEach(item => itemsHash.Add(item));


        // Act
        var resultList = await _syncDebug.InitializeListAsync(items)!;


        // Assert
        Assert.Equal(items.Count, resultList.Count);

        resultList.ForEach(resultItem => Assert.True(itemsHash.Contains(resultItem), $"Cannot find item {resultItem}"));
    }

    [Fact]
    public void ItemsOnlyInitializeOnce_Success()
    {
        // Arrange
        var count = 0;

        
        // Act
        var dictionary = _syncDebug.InitializeDictionary(i =>
        {
            Thread.Sleep(1);
            Interlocked.Increment(ref count);

            return i.ToString();
        });


        // Assert
        Assert.Equal(100, count);
        Assert.Equal(100, dictionary.Count);
    }

    [Theory]
    [InlineData(5000, 3)]
    [InlineData(100000, 5)]
    [InlineData(1000000, 20)]
    [InlineData(10000000, 50, Skip = "This test can take a while depending on processor.")]
    public void BigDataLoader_MultipleThreads_NoDuplicates_Items(int itemCount, int threadCount)
    {
        // Arrange
        var bigDataList = new List<string>();
        var bigDataHash = new HashSet<string>();

        for (var i = 0; i < itemCount; i++)
        {
            var bigData = Cryptography.GetKeyBase64String(64);

            bigDataList.Add(bigData);
            bigDataHash.Add(bigData);
        }


        // Act
        var bigDataDictionary = _syncDebug.BigDataLoader(bigDataList, threadCount);


        // Assert
        Assert.Equal(itemCount, bigDataDictionary.Count);

        Assert.Equal(itemCount, bigDataDictionary.Values.Distinct().Count());

        foreach (var bigDataValue in bigDataDictionary.Values)
        {
            Assert.True(bigDataHash.Contains(bigDataValue));
        }
    }

    [Theory]
    [InlineData(5000, 3)]
    [InlineData(100000, 5)]
    [InlineData(1000000, 20)]
    [InlineData(10000000, 50, Skip = "This test can take a while depending on processor.")]
    public void BigQueueLoader_MultipleThreads_NoDuplicates_Items(int itemCount, int threadCount)
    {
        // Arrange
        var bigDataQueue = new Queue<string>();

        for (var i = 0; i < itemCount; i++)
        {
            bigDataQueue.Enqueue(Cryptography.GetKeyBase64String(64));
        }

        var bigDataSourceQueue = new Queue<string>(bigDataQueue);


        // Act
        var bigDataResultQueue = _syncDebug.BigQueueLoader(bigDataQueue, threadCount);


        // Assert
        Assert.Equal(itemCount, bigDataResultQueue.Count);
        Assert.Equal(0, bigDataQueue.Count);

        while (bigDataResultQueue.Count > 0)
        {
            Assert.Equal(bigDataSourceQueue.Dequeue(), bigDataResultQueue.Dequeue());
        }
    }

    [Theory]
    [InlineData(5000, 5, 25)]
    [InlineData(100000, 10, 50)]
    [InlineData(250000, 15, 100)]
    [InlineData(500000, 20, 200)]
    [InlineData(1000000, 50, 500)]
    [InlineData(5000000, 100, 500)]
    [InlineData(10000000, 100, 1000, Skip = "This test can take a while depending on processor.")]
    public async Task PubSubMessageQueue_MultipleThreads_LoadsAllItems_Success(int itemCount, int threadCount, int batchSize)
    {
        // Arrange
        var dataSample = new List<string>();
        var dataSampleHash = new HashSet<string>();

        for (var i = 0; i < itemCount; i++)
        {
            var message = Cryptography.GetKeyBase64String(64);

            dataSample.Add(message);
            dataSampleHash.Add(message);
        }


        // Act
        var consumedDataList =
            await _syncDebug.PubSubMessageQueueAsync(dataSample, threadCount, batchSize, CancellationToken.None);


        // Assert
        Assert.Equal(itemCount, consumedDataList.Count);

        consumedDataList.ForEach(message => Assert.True(dataSampleHash.Contains(message), $"The message {message} is not contained in the data sample."));

        Assert.Equal(consumedDataList.Distinct().Count(), itemCount);
    }

}
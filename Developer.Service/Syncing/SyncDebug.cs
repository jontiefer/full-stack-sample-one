using System;
using System.Buffers.Text;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Developer.Service.Utils;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace Developer.Service.Syncing;

public class SyncDebug
{
    public List<string>? InitializeList(IEnumerable<string> items)
    {
        using ManualResetEvent resetEvent = new(false);
        
        var bag = new ConcurrentBag<string>();
        
        try
        {
            var result = Parallel.ForEach(items, item =>
            {
                // Perform processing
                Thread.Sleep(500);

                bag.Add(item);
            });

            return bag.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing list.  Message: {ex.Message}, Task Id: {Task.CurrentId}");

            return null;
        }
    }

    public async Task<List<string>?> InitializeListAsync(IEnumerable<string> items, CancellationToken token = default)
    {
        var bag = new ConcurrentBag<string>();

        try
        {
            await Parallel.ForEachAsync(items, token, async (item, cancellationToken) =>
            {
                //Perform Processing
                await Task.Delay(500, cancellationToken);
                
                bag.Add(item);
            });

            return bag.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing list.  Message: {ex.Message}, Task Id: {Task.CurrentId}");

            return null;
        }
    }

    public Dictionary<int, string> InitializeDictionary(Func<int, string> getItem)
    {
        var itemsToInitialize = Enumerable.Range(0, 100).ToList();

        var concurrentDictionary = new ConcurrentDictionary<int, string>();

        var currentIndex = -1;

        try
        {
            var threads = Enumerable.Range(0, 3)
                .Select(i => new Thread(() =>
                {
                    Debug.WriteLine($"Running thread: {Thread.CurrentThread.Name}");

                    try
                    {
                        while (currentIndex < itemsToInitialize.Count)
                        {
                            var itemIndex = Interlocked.Increment(ref currentIndex);

                            if (itemIndex < itemsToInitialize.Count)
                            {
                                var item = itemsToInitialize[itemIndex];

                                concurrentDictionary.AddOrUpdate(item, getItem, (_, s) => s);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            "Error occurred adding items to dictionary. " +
                            $"Thread Name: {Thread.CurrentThread.Name}, " +
                            $"Current Item Index: {currentIndex}, Error Message: {ex.Message}");
                    }
                }))
                .ToList();

            for(var threadIndex = 0; threadIndex < threads.Count; threadIndex++)
            {
                var thread = threads[threadIndex];

                thread.Name = $"DictionaryThread-{threadIndex}";

                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return concurrentDictionary.ToDictionary(kv => kv.Key, kv => kv.Value);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public Dictionary<int, string> BigDataLoader(List<string> bigDataList, int threadCount)
    {
        using var rng = RandomNumberGenerator.Create();

        var bigDataDictionary = new ConcurrentDictionary<int, string>();

        var itemCount = bigDataList.Count;

        using var listLoadCountdown = new CountdownEvent(itemCount);

        var currentIndex = -1;

        try
        {
            var threads = Enumerable.Range(1, threadCount)
                .Select(key => new Thread(() =>
                {
                    Debug.WriteLine($"Running thread: {Thread.CurrentThread.Name}");

                    try
                    {
                        while (!listLoadCountdown.IsSet)
                        {
                            var itemIndex = Interlocked.Increment(ref currentIndex);

                            if (itemIndex < itemCount)
                            {
                                if(!bigDataDictionary.TryAdd(itemIndex, bigDataList[itemIndex]))
                                {
                                    throw new InvalidOperationException(
                                        $"Item {bigDataList[itemIndex]} already exists in dictionary!");
                                }

                                listLoadCountdown.Signal();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            "Error occurred adding items to dictionary. " +
                            $"Thread Name: {Thread.CurrentThread.Name}, " +
                            $"Current Item Index: {currentIndex}, Error Message: {ex.Message}");

                        if(!listLoadCountdown.IsSet)
                            listLoadCountdown.Signal();
                    }
                }))
                .ToList();

            for (var threadIndex = 0; threadIndex < threads.Count; threadIndex++)
            {
                var thread = threads[threadIndex];

                thread.Name = $"BigDataThread-{threadIndex}";

                thread.Start();
            }

            listLoadCountdown.Wait();

            return bigDataDictionary.ToDictionary(kv => kv.Key, kv => kv.Value);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public Queue<string> BigQueueLoader(Queue<string> bigDataSourceQueue, int threadCount)
    {
        var bigDataResultQueue = new Queue<string>();

        var itemCount = bigDataSourceQueue.Count;

        using var queueLoadCountdown = new CountdownEvent(itemCount);

        using var queueLoadSemaphore = new SemaphoreSlim(1);

        try
        {
            var threads = Enumerable.Range(1, threadCount)
                .Select(key => new Thread(() =>
                {
                    Debug.WriteLine($"Running thread: {Thread.CurrentThread.Name}");

                    try
                    { 
                        while (!queueLoadCountdown.IsSet)
                        {
                            queueLoadSemaphore.Wait();

                            if (!bigDataSourceQueue.TryDequeue(out var bigData))
                            {
                                throw new InvalidOperationException(
                                    $"The queue is empty and cannot have an item removed.");
                            }

                            bigDataResultQueue.Enqueue(bigData);

                            queueLoadCountdown.Signal();

                            queueLoadSemaphore.Release();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            "Error occurred adding items to dictionary. " +
                            $"Thread Name: {Thread.CurrentThread.Name}, " +
                            $"Error Message: {ex.Message}");

                        if (!queueLoadCountdown.IsSet)
                            queueLoadCountdown.Signal();

                        queueLoadSemaphore.Release();
                    }
                }))
                .ToList();

            for (var threadIndex = 0; threadIndex < threads.Count; threadIndex++)
            {
                var thread = threads[threadIndex];

                thread.Name = $"BigDataThread-{threadIndex}";

                thread.Start();
            }

            queueLoadCountdown.Wait();

            return new Queue<string>(bigDataResultQueue);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    /// <summary>
    /// This function will emulate a Pub-Sub type message queueing system similar to SQS or RabbitMQ.  It utilizes the
    /// Observer pattern and operates concurrently producing and consuming messages with multiple threads for efficient
    /// processing.  In a distributed system this would emulate multiple servers publishing and consuming messages in
    /// batches on a network.
    /// </summary>
    /// <param name="dataSample">Sample of Data to load in the queue to emulate a message broker.</param>
    /// <param name="threadCount">Number of threads to concurrently produce and consume messages.</param>
    /// <param name="batchSize">The number of items in each batch to emulate to produce to the consumer.</param>
    /// <param name="token">Cancellation Token</param>
    /// <returns></returns>
    public async Task<List<string>> PubSubMessageQueueAsync(List<string> dataSample, int threadCount, int batchSize, CancellationToken token = default)
    {
        // Adjust to the capacity size of the channel to simulate memory of system hosting a message broker, producer, or consumer.
        const int channelCapacity = 100000;

        ConcurrentQueue<string> messageQueue = new(dataSample);

        var messageChannel = Channel.CreateBounded<List<string>>(
            new BoundedChannelOptions(channelCapacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            });

        using var producerCountdown = new CountdownEvent(dataSample.Count);
        using var consumerCountdown = new CountdownEvent(dataSample.Count);

        using var consumerSemaphore = new SemaphoreSlim(1, 1);
        
        var consumedMessages = new List<string>(dataSample.Count);

        var publishTasks = Enumerable.Range(1, threadCount)
            .Select(i => Task.Factory.StartNew(MessageProducer, TaskCreationOptions.LongRunning))
            .ToArray();

        var consumerTasks = Enumerable.Range(1, threadCount)
            .Select(i => Task.Factory.StartNew(MessageConsumer, TaskCreationOptions.LongRunning))
            .ToArray();


        await Task.WhenAll(publishTasks);
        await Task.WhenAll(consumerTasks);

        messageChannel.Writer.Complete();

        return consumedMessages;


        // Producer
        void MessageProducer()
        {
            while (!producerCountdown.IsSet)
            {
                var batch = new List<string>();

                for (var i = 0; i < batchSize; i++)
                {
                    if (messageQueue.TryDequeue(out var message))
                    {
                        batch.Add(message);
                    }
                    else
                    {
                        break;
                    }
                }

                if (batch.Count > 0)
                {
                    bool writeSuccess = false;

                    while(!writeSuccess)
                    {
                        writeSuccess = messageChannel.Writer.TryWrite(batch);

                        if(writeSuccess)
                            producerCountdown.Signal(batch.Count);
                        else
                        {
                            // Channel is blocked with more data than the consumer can process, so will put thread
                            // to sleep to give consumers time to catch up with high load.
                            Thread.Sleep(500);
                        }
                    }
                }
            }
        }

        // Consumer
        void MessageConsumer()
        {
            try
            {
                while (!consumerCountdown.IsSet)
                {
                    if (messageChannel.Reader.TryRead(out var batch))
                    {
                        try
                        {
                            consumerSemaphore.Wait(token);

                            consumedMessages.AddRange(batch);

                            consumerCountdown.Signal(batch.Count);
                        }
                        finally
                        {
                            consumerSemaphore.Release();
                        }
                    }
                }
            }
            catch (ChannelClosedException)
            {
                Debugger.Break();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error consuming from message queue.  Message: {ex.Message}");
            }
        }
    }

    
}
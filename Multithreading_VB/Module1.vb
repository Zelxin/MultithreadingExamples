Imports System.Threading
Module Module1

    Dim lock1 = New Object()
    Dim lock2 = New Object()

    Sub Main()
        Dim t1 = New Thread(New ThreadStart(AddressOf DeadLock1))
        Dim t2 = New Thread(New ThreadStart(AddressOf DeadLock2))
        t1.Start()
        t2.Start()
        'Dim t1 = New Thread(New ThreadStart(AddressOf FryEggs))
        't1.Start()
        'Console.WriteLine("Eggs Started")
        Console.ReadKey()
    End Sub
    Public Sub DeadLock1()
        SyncLock (lock1)
            Console.WriteLine("Thread 1 is locked")
            Thread.Sleep(500)
            SyncLock (lock2)
                Console.WriteLine("Thread 2 is locked")
            End SyncLock
        End SyncLock
    End Sub

    Public Sub DeadLock2()
        SyncLock (lock2)
            Console.WriteLine("Thread 2 is locked")
            Thread.Sleep(500)
            SyncLock (lock1)
                Console.WriteLine("Thread 1 is locked")
            End SyncLock
        End SyncLock
    End Sub

    Private Sub FryEggs()
        Thread.Sleep(TimeSpan.FromSeconds(5))
        Console.WriteLine("Eggs are done!")
    End Sub

End Module

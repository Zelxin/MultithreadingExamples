Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Threading
Class MainWindow
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
        RaiseEvent PropertyChanged(Me, e)
    End Sub

    Private _percentCompleted As Integer
    Public Property PercentCompleted() As Integer
        Get
            Return _percentCompleted
        End Get
        Set(ByVal value As Integer)
            _percentCompleted = value
            'This call is used to notify the WPF environment that the PercentCompleted Property has been altered and should be flag
            'Any bindings to update their values accordingly.
            Me.OnPropertyChanged(New PropertyChangedEventArgs("PercentCompleted"))
        End Set
    End Property


    Private _breakfastmessages As ObservableCollection(Of String)
    Public Property BreakfastMessages() As ObservableCollection(Of String)
        Get
            Return _breakfastmessages
        End Get
        Set(ByVal value As ObservableCollection(Of String))
            _breakfastmessages = value
        End Set
    End Property

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        'Marking the DataContext to Me means when our xaml is looking for binding targets it looks for properties within THIS class
        'You can bind to a seperate class and use that as a model for your bindings as well.
        Me.DataContext = Me
        BreakfastMessages = New ObservableCollection(Of String)()
    End Sub


    ''' <summary>
    ''' This function demonstrates the usefulness of the async/await pattern.
    ''' The FryEggsAsync function returns the Task(of Egg) which begins running immediately.
    ''' later on in the code the execution be awaited on which then gives returns the execution 
    ''' back to the calling thread(the ui thread in this case)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim EggsTask As Task(Of Egg) = FryEggsAsync(2)
        Dim baconTask As Task(Of Bacon) = FryBaconAsync(3)
        Dim toastTask As Task(Of Toast) = ToastBreadAsync(2, 4)

        Dim eggs As Egg = Await EggsTask
        PercentCompleted = 25 ' Updating using a binding works no matter which thread you use to call it. (as long as your binding is correct)
        pg_noBinding.Value = 25 ' Updating without a binding works as long as the call comes from the user interface thread.
        BreakfastMessages.Add("Eggs are done")
        Dim bacon As Bacon = Await baconTask
        PercentCompleted = 50
        pg_noBinding.Value = 50
        BreakfastMessages.Add("Bacon is done")
        Dim toast As Toast = Await toastTask
        PercentCompleted = 75
        pg_noBinding.Value = 75
        BreakfastMessages.Add("Toast is ready")

        Dim cup As Coffee = Await PourCoffeeAsync()
        BreakfastMessages.Add("Coffee is done.")
        BreakfastMessages.Add("Breakfast/lunch or dinner is ready!")
        PercentCompleted = 100
        pg_noBinding.Value = 100
    End Sub

    ''' <summary>
    ''' This function demonstrates using the user interface thread to do long running work
    ''' The functions called take multiple seconds to complete and cause the user interface to become unresponsive.
    ''' This also causes all of the Breakfast messages to be added to the UI at the same time. Which can be misleading/confusing to users.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_Click_FREEZEUI(sender As Object, e As RoutedEventArgs)

        Dim eggs = FryEggs(2)
        BreakfastMessages.Add("Eggs are done")
        Dim toast = ToastBread(2, 4)
        BreakfastMessages.Add("Toast is ready")
        Dim bacon = FryBacon(4)
        BreakfastMessages.Add("Bacon is done")
        Dim cup = PourCoffee()
        BreakfastMessages.Add("Coffee is done.")
        BreakfastMessages.Add("Breakfast/lunch or dinner is ready!")
    End Sub


    ''' <summary>
    ''' Toasts bread, it takes 5 seconds i guess.
    ''' </summary>
    ''' <param name="v1"></param>
    ''' <param name="toastiness">how toasty do you like it?</param>
    ''' <returns></returns>
    Private Function ToastBread(v1 As Integer, toastiness As Integer) As Toast
        Thread.Sleep(5000)
        Return New Toast()
    End Function

    Private Function FryBacon(v As Integer) As Bacon
        Thread.Sleep(5000)
        Return New Bacon
    End Function

    Private Function FryEggs(v As Integer) As Egg
        Dim index = 0
        While index < v
            Thread.Sleep(5000)
            index += 1
        End While
        Return New Egg()
    End Function

    Private Function PourCoffee() As Coffee
        Thread.Sleep(5000)
        Return New Coffee()
    End Function

    Private Async Function ToastBreadAsync(v1 As Integer, toastiness As Integer) As Task(Of Toast)
        Await Task.Delay(2000)
        Return New Toast()
    End Function

    Private Async Function FryBaconAsync(v As Integer) As Task(Of Bacon)
        Await Task.Run(Sub()
                           Thread.Sleep(5000)
                       End Sub)
        Return New Bacon
    End Function

    Private Async Function FryEggsAsync(v As Integer) As Task(Of Egg)
        Dim index = 0
        While index < v
            Await Task.Delay(2000)
            index += 1
        End While

        Return New Egg()
    End Function

    Private Async Function PourCoffeeAsync() As Task(Of Coffee)
        Await Task.Delay(2000)
        Return New Coffee()
    End Function

    Public Class Toast

    End Class
    Public Class Coffee

    End Class
    Public Class Egg

    End Class
    Public Class Bacon

    End Class
End Class

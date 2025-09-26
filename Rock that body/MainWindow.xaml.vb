Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Class MainWindow
    Implements INotifyPropertyChanged

    '-- Eventos
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    '-- Almacenamiento
        Private _text As String
        Private _pathmusic As Uri = New Uri(AppDomain.CurrentDomain.BaseDirectory & "\Rock That Body Short.mp3")

    '-- Propiedades
        Public Property Text As String
            Get
             Return _text
            End Get
            Set(value As String)
                SetProperty(_text, value)
            End Set
        End Property


    '-- Inicializacion del control
        Sub New()
            Me.DataContext = Me
            InitializeComponent()
        End Sub

        Private Async Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

            '-- Variables
                Dim ScreenSize             = GetCurrentScreenSize(New MainWindow)
                Dim ScreenWidth  As Double = ScreenSize.Width
                Dim ScreenHeight As Double = ScreenSize.Height
                Dim BaseWidth    As Double = Me.Width
                Dim BaseHeight   As Double = Me.Height
                Dim RatioX       As Double = ScreenWidth / BaseWidth
                Dim RatioY       As Double = ScreenHeight / BaseHeight
                Dim Scale        As Double = Math.Max(RatioX, RatioY)

            '-- Configuramos ventana
                Me.WindowStartupLocation = WindowStartupLocation.CenterScreen
                Me.WindowState = WindowState.Maximized
                Me.GridContainer.LayoutTransform = New ScaleTransform(Scale, Scale)
                
            '-- Creamos lista de textos
                Dim TextList As New List(Of TLine) From {
                    New TLine("Starting...", 0.05),
                    New TLine("¿Are you ready?", 0.05),
                    New TLine("I wanna da—", 0.06),
                    New TLine("I wanna dance in the lights", 0.05),
                    New TLine("I wanna ro—", 0.07),
                    New TLine("I wanna rock your body", 0.08),
                    New TLine("I wanna go", 0.08),
                    New TLine("I wanna go for a ride", 0.068),
                    New TLine("Hop in the music and", 0.07),
                    New TLine("Rock your body", 0.08),
                    New TLine("Rock your body", 0.069),
                    New TLine("Come on, come on", 0.035),
                    New TLine("Rock your body", 0.05),
                    New TLine("(Rock your body)", 0.03),
                    New TLine("Rock your body", 0.049),
                    New TLine("Come on, come on", 0.035),
                    New TLine("Rock your body", 0.08)
                }

            '-- Tiempo entre salto
                Dim DelayStep As Double() = {3, 2, 0.4, 1.6, 0.18, 1, 0.12, 1.5, 0.2, 0.2, 0.3, 0.2, 0.2, 0.3, 0.4, 0.3, 0.3}

            '-- Creamos reproductor de musica
                With Me.MediaE
                    .LoadedBehavior = MediaState.Play
                    .Volume = 1
                    .Source = _pathmusic
                End With
                
            '-- Arrancamos funciones
                Dim i   As Integer = 0
                Dim sp  As Integer = 0
                For Each line As TLine In TextList
                    Await TextDelay(line.Line, line.Delay)

                    sp = CInt(DelayStep(i) * 1000)
                    Await Task.Delay(sp)

                    i += 1
                Next

            '-- Finalizamos
                End

        End Sub
    
        Private Sub MainWindow_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
            If e.Key = Key.Escape Then
                End
            End If
        End Sub

    '-- Funciones
        Private Async Function TextDelay(Text As String, Optional Delay As Double = 0.05) As Task
            Me.Text = String.Empty
            For Each c As Char In Text
                Me.Text &= c
                Await Task.Delay(CInt(Delay * 1000))
            Next
        End Function

        Private Function GetCurrentScreenSize(window As Window) As (Width As Double, Height As Double)
            Dim source = PresentationSource.FromVisual(window)

            If source IsNot Nothing Then
                Dim dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11
                Dim dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22

                Return (SystemParameters.PrimaryScreenWidth * dpiX / 96.0,
                        SystemParameters.PrimaryScreenHeight * dpiY / 96.0)
            End If

            Return (SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight)
        End Function

        Protected Overloads Sub OnPropertyChanged(<CallerMemberName> Optional propertyName As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Protected Function SetProperty(Of T)(ByRef storage As T, value As T, <CallerMemberName> Optional propertyName As String = Nothing) As Boolean
            If EqualityComparer(Of T).Default.Equals(storage, value) Then Return False
            storage = value
            OnPropertyChanged(propertyName)
            Return True
        End Function

    '-- Modelo
        Public Class TLine
            Public Property Line As String
            Public Property Delay As Double
            Sub New(Line As String, Delay As Double)
                Me.Line = Line
                Me.Delay = Delay
            End Sub
        End Class

End Class
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows.Threading

Class MainWindow
    Implements INotifyPropertyChanged

    '-- Modelo
        Public Class TLine
            Public Property Id As Integer
            Public Property Line As String
            Public Property Delay As Integer
            Public Property DStep As Integer
            Public Property ForeB As Brush
            Public Property BackB As Brush
            Sub New(Id As Integer, Line As String, Delay As Double, DStep As Double, ForeB As Brush, BackB As Brush)
                Me.Id = Id
                Me.Line = Line
                Me.Delay = CInt(Delay * 1000)
                Me.DStep = CInt(DStep * 1000)
                Me.ForeB = ForeB
                Me.BackB = BackB
            End Sub
        End Class

    '-- Eventos
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    '-- Almacenamiento
        Private _text       As String = String.Empty
        Private _foreb      As Brush  = Brushes.White
        Private _backb      As Brush  = Brushes.Black
        Private _pathmusic  As Uri = New Uri(AppDomain.CurrentDomain.BaseDirectory & "\Rock That Body Short.mp3")

    '-- Propiedades
        Public Property Text As String
            Get
             Return _text
            End Get
            Set(value As String)
                SetProperty(_text, value)
            End Set
        End Property
        Public Property ForeB As Brush
            Get
                Return _foreb
            End Get
            Set(value As Brush)
                SetProperty(_foreb, value)
            End Set
        End Property
        Public Property BackB As Brush
            Get
                Return _backb
            End Get
            Set(value As Brush)
                SetProperty(_backb, value)
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
                Me.WindowState = WindowState.Maximized
                Me.GridContainer.LayoutTransform = New ScaleTransform(Scale, Scale)
                
            '-- Creamos lista de textos
                Dim TextList As New List(Of TLine) From {
                    New TLine(0 , "..........."                 , 0.05  , 3    , Brushes.White, Brushes.Black),
                    New TLine(1 , "¿Are you ready?"             , 0.05  , 2    , Brushes.White, Brushes.Black),
                    New TLine(2 , "I wanna da—"                 , 0.06  , 0.4  , Brushes.White, Brushes.Black),
                    New TLine(3 , "I wanna dance in the lights" , 0.05  , 1.6  , Brushes.Black, Brushes.White),
                    New TLine(4 , "I wanna ro—"                 , 0.07  , 0.18 , Brushes.White, Brushes.Black),
                    New TLine(5 , "I wanna rock your body"      , 0.08  , 1    , Brushes.White, Brushes.Black),
                    New TLine(6 , "I wanna go"                  , 0.08  , 0.18 , Brushes.White, Brushes.Black),
                    New TLine(7 , "I wanna go for a ride"       , 0.068 , 0.78 , Brushes.White, Brushes.Black),
                    New TLine(8 , "Hop in the music and"        , 0.07  , 1    , Brushes.White, Brushes.Black),
                    New TLine(9 , "Rock your body"              , 0.08  , 0.2  , Brushes.White, Brushes.Black),
                    New TLine(10, "Rock your body"              , 0.069 , 0.3  , Brushes.White, Brushes.Black),
                    New TLine(11, "Come on, come on"            , 0.035 , 0.3  , Brushes.White, Brushes.Black),
                    New TLine(12, "Rock your body"              , 0.05  , 0.3  , Brushes.White, Brushes.Black),
                    New TLine(13, "(Rock your body)"            , 0.03  , 0.2  , Brushes.Yellow, Brushes.Black),
                    New TLine(14, "Rock your body"              , 0.049 , 0.4  , Brushes.White, Brushes.Black),
                    New TLine(15, "Come on, come on"            , 0.035 , 0.4  , Brushes.White, Brushes.Black),
                    New TLine(16, "Rock your body"              , 0.08  , 0.3  , Brushes.White, Brushes.Black)
                }

            '-- Creamos reproductor de musica
                With Me.MediaE
                    .LoadedBehavior = MediaState.Play
                    .Volume         = 1
                    .Source         = _pathmusic
                End With
                
            '-- Arrancamos funciones
                For Each line As TLine In TextList
                    If line.Id = 3 Then
                        Await TextDelay(line.Line, line.Delay)
                        Dim TempForeB As Brush = Me.ForeB
                        Dim TempBackB As Brush = Me.BackB
                        Me.ForeB = line.ForeB
                        Me.BackB = line.BackB
                        Await Task.Delay(line.DStep)
                        Me.ForeB = TempForeB
                        Me.BackB = TempBackB

                    ElseIf line.Id = 13 Then
                        Dim TempForeB As Brush = Me.ForeB
                        Me.ForeB = line.ForeB
                        Await TextDelay(line.Line, line.Delay)
                        Await Task.Delay(line.DStep)
                        Me.ForeB = TempForeB

                    Else
                        Await TextDelay(line.Line, line.Delay)
                        Await Task.Delay(line.DStep)
                    End If

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
        Private Async Function TextDelay(Text As String, Delay As Integer) As Task
            Me.Text = String.Empty
            For Each c As Char In Text
                Me.Text &= c
                Await Task.Delay(Delay)
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


End Class
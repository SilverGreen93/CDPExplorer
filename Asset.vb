Public Class Asset

    Public AssetKuid As Kuid
    Public Username As String
    Public Kind As String
    Public TrainzBuild As Single
    Public CategoryRegion As String
    Public CategoryEra As String
    Public CategoryClass As String
    Public Description As String


    Sub New()
        AssetKuid = New Kuid()
        Username = "Untitled" 'This is the case for assets with tag "secret 1" which do not have username and asset-filename
        Kind = ""
        TrainzBuild = 1.3 'This is the value Trainz defaults assets which do not have a trainz-build tag
        CategoryRegion = ""
        CategoryEra = ""
        CategoryClass = ""
        Description = ""
    End Sub

    Public Sub SetTrainzBuild(ByVal build As Single)
        TrainzBuild = build
    End Sub

    Public Sub SetTrainzBuild(ByVal build As String)
        TrainzBuild = CSng(Val(build))
    End Sub

    Public Function GetTrainzBuildAsString() As String
        Return TrainzBuild.ToString("G", Globalization.CultureInfo.InvariantCulture)
    End Function

    Public Function GetTrainzBuild() As Single
        Return TrainzBuild
    End Function

End Class

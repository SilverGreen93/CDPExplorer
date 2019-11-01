Imports System.Globalization

Public Class Kuid

    Private kuidValue(7) As Byte

    Sub New()
        kuidValue = New Byte() {0, 0, 0, 0, 0, 0, 0, 0}
    End Sub

    ''' <summary>
    ''' Initialize class from byte array
    ''' </summary>
    ''' <param name="kuid">Kuid as 8 byte array</param>
    Sub New(ByRef kuid As Byte())
        SetKuid(kuid)
    End Sub

    ''' <summary>
    ''' Initialize class from kuid as string: &lt;kuid:474195:10010&gt;
    ''' </summary>
    ''' <param name="kuid">Kuid in standard string format</param>
    Sub New(ByRef kuid As String)
        SetKuid(kuid)
    End Sub

    ''' <summary>
    ''' Set kuid from byte array
    ''' </summary>
    ''' <param name="kuid">Kuid as 8 byte array</param>
    ''' <returns>True if kuid is set correctly</returns>
    Public Function SetKuid(ByRef kuid As Byte()) As Boolean
        If kuid.Length < 8 Then
            Return False
        End If

        Array.Copy(kuid, kuidValue, 8)
        Return True
    End Function

    ''' <summary>
    ''' Set kuid from explicit hex string: "53 3C 07 00 E9 86 01 00"
    ''' </summary>
    ''' <param name="hexKuid">Kuid as hex string</param>
    ''' <returns>True if kuid is parsed correctly</returns>
    Public Function SetKuidAsHexString(ByRef hexKuid As String) As Boolean
        Dim hexParts() As String = hexKuid.Split({" "c, "-"c, ":"c, ","c})
        Dim tempKuid(7) As Byte

        For i As Integer = 0 To hexParts.Length - 1
            If Not Byte.TryParse(hexParts(i), NumberStyles.HexNumber, CultureInfo.CurrentCulture, tempKuid(i)) Then
                Return False
            End If
        Next

        Array.Copy(tempKuid, kuidValue, 8)
        Return True
    End Function

    ''' <summary>
    ''' Set kuid from 3 individual values
    ''' </summary>
    ''' <param name="userID">User ID</param>
    ''' <param name="contentID">Content ID</param>
    ''' <param name="version">Version</param>
    ''' <returns>True if all 3 are valid</returns>
    Public Function SetKuid(ByVal userID As Integer, ByVal contentID As Integer, ByVal version As Byte) As Boolean
        Return SetUserID(userID) AndAlso SetContentID(contentID) AndAlso SetVersion(version)
    End Function

    ''' <summary>
    ''' Set kuid from human-readable string: &lt;kuid:474195:10010&gt;
    ''' </summary>
    ''' <param name="kuid">Kuid as human-readable string</param>
    ''' <returns>True if string is valid</returns>
    Public Function SetKuid(ByRef kuid As String) As Boolean
        Dim userBytes(3), contentBytes(3) As Byte
        Dim kuidParts() As String
        Dim kuidPartsInt(2) As Integer

        If kuid = "" Then 'null kuid
            Return False
        End If

        kuidParts = Text.RegularExpressions.Regex.Split(kuid, ":")

        If kuidParts.Length < 3 Then 'kuid too short
            Return False
        End If

        Try
            'convert the kuid parts to int (skip first part)
            'ex: <kuid2:1234:5678:9>
            'kuidParts(0) = "<kuid2"
            'kuidParts(1) = "1234"
            'kuidParts(2) = "5678"
            'kuidParts(3) = "9>"
            For i As Integer = 1 To kuidParts.Length - 1
                kuidPartsInt(i - 1) = Val(kuidParts(i))
            Next

            'convert integers to Byte arrays
            userBytes = BitConverter.GetBytes(kuidPartsInt(0))
            contentBytes = BitConverter.GetBytes(kuidPartsInt(1))

            'check version number and integrate into userBytes if userBytes is not negative
            If (kuidPartsInt(2) > 0 AndAlso kuidPartsInt(2) < 128 AndAlso kuidPartsInt(0) >= 0) Then
                'add the version number to byte 4 of userBytes and keep sign bit on bit 0
                userBytes(3) = userBytes(3) Xor (CByte(kuidPartsInt(2)) << 1)
            End If

            'copy bytes
            For i As Integer = 0 To 3
                kuidValue(i) = userBytes(i)
                kuidValue(i + 4) = contentBytes(i)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    ''' <summary>
    ''' Set kuid User ID from integer
    ''' </summary>
    ''' <param name="userID">User ID as integer</param>
    ''' <returns>True if userID is valid</returns>
    Public Function SetUserID(ByVal userID As Integer) As Boolean
        If userID > 16777215 OrElse userID < -16777216 Then
            Return False
        End If
        For i As Integer = 0 To 3
            kuidValue(i) = BitConverter.GetBytes(userID)(i)
        Next
        Return True
    End Function

    ''' <summary>
    ''' Set kuid User ID from byte array
    ''' </summary>
    ''' <param name="userID">User ID as 4 byte array</param>
    ''' <returns>True if userID is set correctly</returns>
    Public Function SetUserID(ByVal userID As Byte()) As Boolean
        If userID.Length < 4 Then
            Return False
        End If
        For i As Integer = 0 To 3
            kuidValue(i) = userID(i)
        Next
        Return True
    End Function

    ''' <summary>
    ''' Set kuid content ID from integer
    ''' </summary>
    ''' <param name="contentID">Content ID as integer</param>
    ''' <returns>Always True</returns>
    Public Function SetContentID(ByVal contentID As Integer) As Boolean
        For i As Integer = 0 To 3
            kuidValue(i + 4) = BitConverter.GetBytes(contentID)(i)
        Next
        Return True
    End Function

    ''' <summary>
    ''' Set kuid content ID from byte array
    ''' </summary>
    ''' <param name="contentID">Content ID as 4 byte array</param>
    ''' <returns>True if contentID is set correctly</returns>
    Public Function SetContentID(ByVal contentID As Byte()) As Boolean
        If contentID.Length < 4 Then
            Return False
        End If
        For i As Integer = 0 To 3
            kuidValue(i + 4) = contentID(i)
        Next
        Return True
    End Function

    ''' <summary>
    ''' Set kuid version
    ''' </summary>
    ''' <param name="ver">Version as single byte</param>
    ''' <returns>True if version is valid</returns>
    Public Function SetVersion(ByVal ver As Byte) As Boolean
        'check version number and integrate into userBytes if userBytes is not negative
        If (ver > 0 AndAlso ver < 128 AndAlso GetUserID() >= 0) Then
            'add the version number to byte 4 of userBytes and keep sign bit on bit 0
            kuidValue(3) = kuidValue(3) Xor (CByte(ver) << 1)
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Get kuid as human-readable string: &lt;kuid:474195:10010&gt;
    ''' </summary>
    ''' <returns>Kuid as human-readable string</returns>
    Public Function GetKuidAsString() As String
        Dim kuidString As String = "<kuid"
        Dim userBytes(3), contentBytes(3) As Byte
        Dim version As Integer

        Array.Copy(kuidValue, 0, userBytes, 0, 4)
        Array.Copy(kuidValue, 4, contentBytes, 0, 4)

        If (userBytes(3) And (1 << 0)) <> 0 Then
            'If the UserID is negative, ignore version
            version = 0
        Else
            version = Convert.ToInt32(userBytes(3) >> 1) 'version is upper 7 bits of userByte 4
            userBytes(3) = userBytes(3) And &H1 'keep only the sign bit (bit 0)
        End If

        'kuid or kuid2
        If version <> 0 Then
            kuidString &= "2:"
        Else
            kuidString &= ":"
        End If

        kuidString &= BitConverter.ToInt32(userBytes, 0)
        kuidString &= ":"
        kuidString &= BitConverter.ToInt32(contentBytes, 0)

        If version <> 0 Then
            kuidString = kuidString & ":" & version
        End If

        kuidString &= ">"
        Return kuidString
    End Function

    ''' <summary>
    ''' Get kuid as 8 byte array
    ''' </summary>
    ''' <returns>Kuid as 8 byte array</returns>
    Public Function GetKuidAsBytes() As Byte()
        Return kuidValue
    End Function

    ''' <summary>
    ''' Get User ID as integer
    ''' </summary>
    ''' <returns>User ID as integer</returns>
    Public Function GetUserID() As Integer
        Dim userBytes(3) As Byte
        Array.Copy(kuidValue, 0, userBytes, 0, 4)
        userBytes(3) = userBytes(3) And &H1 'keep only the sign bit (bit 0), strip version
        Return BitConverter.ToInt32(userBytes, 0)
    End Function

    ''' <summary>
    ''' Get User ID as 4 byte array
    ''' </summary>
    ''' <returns>User ID as 4 byte array</returns>
    Public Function GetUserIDAsBytes() As Byte()
        Dim userBytes(3) As Byte
        Array.Copy(kuidValue, 0, userBytes, 0, 4)
        Return userBytes
    End Function

    ''' <summary>
    ''' Get Content ID as integer
    ''' </summary>
    ''' <returns>Content ID as integer</returns>
    Public Function GetContentID() As Integer
        Dim contentBytes(3) As Byte
        Array.Copy(kuidValue, 4, contentBytes, 0, 4)
        Return BitConverter.ToInt32(contentBytes, 0)
    End Function

    ''' <summary>
    ''' Get Content ID as 4 byte array
    ''' </summary>
    ''' <returns>Content ID as 4 byte array</returns>
    Public Function GetContentIDAsBytes() As Byte()
        Dim contentBytes(3) As Byte
        Array.Copy(kuidValue, 4, contentBytes, 0, 4)
        Return contentBytes
    End Function

    ''' <summary>
    ''' Get kuid version as byte
    ''' </summary>
    ''' <returns>Kuid version as byte</returns>
    Public Function GetVersion() As Byte
        Return Convert.ToInt32(kuidValue(3) >> 1) 'version is upper 7 bits of userByte 4
    End Function

    ''' <summary>
    ''' Compute the hash of the kuid
    ''' </summary>
    ''' <returns>Kuid hash</returns>
    Public Function GetKuidHash() As Byte
        Dim hash As Byte = 0

        Try
            For i As Integer = 0 To 7
                hash = hash Xor kuidValue(i)
            Next
            'version does not affect the hash, so xor that out only if UserID is positive
            If (kuidValue(3) And (1 << 0)) = 0 Then
                hash = hash Xor kuidValue(3)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return hash
    End Function

    ''' <summary>
    ''' Get kuid as 8 byte array, but reverse UserID with ContentID
    ''' This is useful for searching kuids in map/session files
    ''' </summary>
    ''' <returns>Kuid as 8 byte array, reversed</returns>
    Public Function GetKuidReversedAsBytes() As Byte()
        Dim revKuid(7) As Byte

        Try
            'reverse bytes (a-b-c-d:e-f-g-h -> e-f-g-h:a-b-c-d)
            For i As Integer = 0 To 3
                revKuid(i) = kuidValue(i + 4)
                revKuid(i + 4) = kuidValue(i)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return revKuid
    End Function

    ''' <summary>
    ''' Get kuid as hex string: "53 3C 07 00 E9 86 01 00"
    ''' </summary>
    ''' <returns>Kuid as hex string</returns>
    Public Function GetKuidAsHexString() As String
        Return BitConverter.ToString(kuidValue).Replace("-"c, " "c)
    End Function

    ''' <summary>
    ''' Get kuid as hex string, but reversed: "E9 86 01 00 53 3C 07 00"
    ''' </summary>
    ''' <returns>Kuid as hex string, reversed</returns>
    Public Function GetKuidReversedAsHexString() As String
        Return BitConverter.ToString(GetKuidReversedAsBytes()).Replace("-"c, " "c)
    End Function

End Class

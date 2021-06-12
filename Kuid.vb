Imports System.Globalization

Public Class Kuid
    Public Const MAXINT As Integer = 2 ^ 31 - 1
    Public Const MININT As Integer = -2 ^ 31
    Public Const MAXUINT As UInteger = 2 ^ 32 - 1

    Private ReadOnly kuidValue(7) As Byte

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
    ''' Set kuid from User ID and Content ID
    ''' </summary>
    ''' <param name="userID">User ID</param>
    ''' <param name="contentID">Content ID</param>
    ''' <returns>True if all 2 are valid</returns>
    Public Function SetKuid(ByVal userID As Integer, ByVal contentID As Integer) As Boolean
        Return SetUserID(userID) AndAlso SetContentID(contentID)
    End Function

    ''' <summary>
    ''' Set kuid2 from User ID, Content ID and Version
    ''' </summary>
    ''' <param name="userID">User ID (should be positive)</param>
    ''' <param name="contentID">Content ID</param>
    ''' <param name="version">Version</param>
    ''' <returns>True if all 3 are valid</returns>
    Public Function SetKuid(ByVal userID As Integer, ByVal contentID As Integer, ByVal version As Byte) As Boolean
        Return SetUserID(userID) AndAlso SetContentID(contentID) AndAlso SetVersion(version)
    End Function

    ''' <summary>
    ''' Converts number to fit into an Integer type
    ''' </summary>
    ''' <param name="num">Number to be converted to Integer</param>
    ''' <returns>Integer representation or 0 on overflow</returns>
    Private Function toInt32(num As Double) As Integer
        If num > MAXUINT OrElse num < MININT Then
            Return 0
        End If

        If num > MAXINT Then
            'convert number to byte representation, then convert back to signed int32 to make sure we don't overflow
            'in this case the result will always be negative
            Return BitConverter.ToInt32(BitConverter.GetBytes(Convert.ToUInt32(num)), 0)
        End If

        Return Convert.ToInt32(num)
    End Function

    ''' <summary>
    ''' Set kuid from human-readable string: &lt;kuid:474195:10010&gt;
    ''' </summary>
    ''' <param name="kuid">Kuid as human-readable string</param>
    ''' <returns>True if string is valid</returns>
    Public Function SetKuid(ByRef kuid As String) As Boolean
        Dim userBytes(3), contentBytes(3) As Byte
        Dim kuidParts() As String

        If kuid = "" Then 'null kuid
            Return False
        End If

        kuidParts = Text.RegularExpressions.Regex.Split(kuid, ":")

        'kuid does not start with "kuid"
        If kuidParts(0).IndexOf("kuid", StringComparison.OrdinalIgnoreCase) = -1 Then

            'In some cases we can have a kuid without the "kuid" keyword
            '1234:12345
            If kuidParts.Length < 2 Then Return False 'kuid too short

            SetUserID(toInt32(Val(kuidParts(0))))
            SetContentID(toInt32(Val(kuidParts(1))))

            If kuidParts.Length > 2 Then
                SetVersion(toInt32(Val(kuidParts(2))))
            End If

        Else
            'kuid starts with "kuid"

            If kuidParts.Length < 3 Then Return False 'kuid too short

            SetUserID(toInt32(Val(kuidParts(1))))
            SetContentID(toInt32(Val(kuidParts(2))))

            If kuidParts.Length = 3 AndAlso kuidParts(0).IndexOf("kuid2", StringComparison.OrdinalIgnoreCase) <> -1 Then
                'In some cases we can have kuid2 with missing version number. In that case version is always 127
                '<kuid2:146087:27023>
                SetVersion(127)
            ElseIf kuidParts.Length > 3 Then
                SetVersion(toInt32(Val(kuidParts(3))))
            End If

        End If

        Return True
    End Function

    ''' <summary>
    ''' Set kuid User ID from a 25-bit integer
    ''' </summary>
    ''' <param name="userID">User ID as a 25-bit integer</param>
    ''' <returns>Always True</returns>
    Public Function SetUserID(ByVal userID As Integer) As Boolean
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
    Public Function SetVersion(ByVal ver As Integer) As Boolean
        'check version number and integrate into userBytes if userBytes is not negative
        If ver >= 0 AndAlso ver < 128 Then
            'add the version number to byte 4 of userBytes and keep sign bit on bit 0
            kuidValue(3) = kuidValue(3) And 1
            kuidValue(3) = kuidValue(3) Or (ver << 1)
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Get kuid as human-readable string: &lt;kuid:474195:10010&gt;
    ''' If version is > 0, return kuid2: &lt;kuid2:474195:10010:1&gt;
    ''' </summary>
    ''' <returns>Kuid as human-readable string</returns>
    Public Function GetKuidAsString() As String
        Dim kuidString As String = "<kuid"
        Dim version As Byte = GetVersion()
        Dim needKuid2 As Boolean =
            (version > 0 AndAlso GetUserID() > 0) OrElse
            (version = 0 AndAlso GetUserID() < 0)
        'otherwise if UserID < 0 version is always 127 and we don't show the version

        If needKuid2 Then
            kuidString &= "2:"
        Else
            kuidString &= ":"
        End If

        kuidString &= GetUserID()
        kuidString &= ":"
        kuidString &= GetContentID()

        If needKuid2 Then
            kuidString = kuidString & ":" & version
        End If

        kuidString &= ">"

        Return kuidString
    End Function

    ''' <summary>
    ''' Get kuid as kuid2 human-readable string even if version is 0: &lt;kuid2:474195:10010:0&gt;
    ''' </summary>
    ''' <returns>Kuid as human-readable string</returns>
    Public Function GetKuid2AsString() As String
        Dim kuidString As String = "<kuid2:"

        kuidString &= GetUserID()
        kuidString &= ":"
        kuidString &= GetContentID()
        kuidString &= ":"
        kuidString &= GetVersion()
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

        'from byte 3 keep only the sign bit (bit 24), strip version bits
        userBytes(3) = userBytes(3) And 1

        'if sign bit (bit 24) is set (is negative), sign extend to 32 bit
        If userBytes(3) And 1 = 1 Then
            userBytes(3) = &HFF
        End If

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
    Public Function GetVersion() As Integer
        'if the UserID is negative, version will be 127, or 0 if is the result of an overflow in UserID
        Return Convert.ToInt32(kuidValue(3) >> 1) 'version is upper 7 bits of userByte 3
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
            If (kuidValue(3) And 1) = 0 Then
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

    ''' <summary>
    ''' Debug kuid conversion at limits and overflow
    ''' </summary>
    Public Sub DebugKuid()
        SetKuidAsHexString("01 00 00 01 40 E2 01 00")
        Debug.Assert(GetKuidAsString() = "<kuid2:-16777215:123456:0>", "Kuid conversion failed!")

        SetKuidAsHexString("01 00 00 FF 40 E2 01 00")
        Debug.Assert(GetKuidAsString() = "<kuid:-16777215:123456>", "Kuid conversion failed!")

        SetKuid("<kuid:16777400:123456>")
        Debug.Assert(GetKuidAsHexString() = "B8 00 00 01 40 E2 01 00", "Kuid conversion failed!")
        Debug.Assert(GetKuidAsString() = "<kuid2:-16777032:123456:0>", "Kuid conversion failed!")
        Debug.Assert(GetKuid2AsString() = "<kuid2:-16777032:123456:0>", "Kuid conversion failed!")

        SetKuidAsHexString("01 00 00 01 0A B1 21 00")
        Debug.Assert(GetKuidAsString() = "<kuid2:-16777215:2208010:0>", "Kuid conversion failed!")

        SetKuid("<kuid2:-16777215:2208010:0>")
        Debug.Assert(GetKuidAsHexString() = "01 00 00 01 0A B1 21 00", "Kuid conversion failed!")

        SetKuidAsHexString("B8 00 00 FF 40 E2 01 00")
        Debug.Assert(GetKuidAsString() = "<kuid:-16777032:123456>", "Kuid conversion failed!")

        SetKuidAsHexString("B8 00 00 01 40 E2 01 00")
        Debug.Assert(GetKuidAsString() = "<kuid2:-16777032:123456:0>", "Kuid conversion failed!")

        SetKuid("<kuid:-16777032:123456>")
        Debug.Assert(GetKuidAsHexString() = "B8 00 00 FF 40 E2 01 00", "Kuid conversion failed!")

        SetKuid("<kuid:-16777400:123456>")
        Debug.Assert(GetKuidAsHexString() = "48 FF FF FE 40 E2 01 00", "Kuid conversion failed!")

        SetKuidAsHexString("48 FF FF FE 40 E2 01 00")
        Debug.Assert(GetKuidAsString() = "<kuid2:16777032:123456:127>", "Kuid conversion failed!")

        SetKuid("<kuid2:16777032:123456:127>")
        Debug.Assert(GetKuidAsHexString() = "48 FF FF FE 40 E2 01 00", "Kuid conversion failed!")

        SetKuid("<kuid:-1:100503>")
        Debug.Assert(GetKuidHash() = &H1E, "Kuid hashing failed!")
        Debug.Assert(GetKuidAsString() = "<kuid:-1:100503>", "Kuid conversion failed!")
        Debug.Assert(GetKuid2AsString() = "<kuid2:-1:100503:127>", "Kuid conversion failed!")
        SetVersion(50)
        Debug.Assert(GetVersion() = 50, "Kuid value failed!")
        Debug.Assert(GetKuidAsString() = "<kuid:-1:100503>", "Kuid conversion failed!")

        SetKuid("<kuid:-25:581>")
        Debug.Assert(GetKuidHash() = &H5F, "Kuid hashing failed!")
        Debug.Assert(GetUserID() = -25, "Kuid value failed!")
        Debug.Assert(GetContentID() = 581, "Kuid value failed!")
        Debug.Assert(GetVersion() = 127, "Kuid value failed!")

        SetKuid("<kuid:276266:100101>")
        Debug.Assert(GetKuidHash() = &H9A, "Kuid hashing failed!")

        SetKuid("<kuid2:132952:131304:10>")
        Debug.Assert(GetKuidHash() = &HB7, "Kuid hashing failed!")

        SetKuid("<kuid2:16777032:123456:127>")
        Debug.Assert(GetKuidHash() = &HEB, "Kuid hashing failed!")
        Debug.Assert(GetVersion() = 127, "Kuid value failed!")

        SetKuid("<kuid2:16777032:123456:13>")
        Debug.Assert(GetKuidHash() = &HEB, "Kuid hashing failed!")
        Debug.Assert(GetUserID() = 16777032, "Kuid value failed!")
        Debug.Assert(GetContentID() = 123456, "Kuid value failed!")
        Debug.Assert(GetVersion() = 13, "Kuid value failed!")

        SetKuid("<kuid:224567:2943774475>")
        Debug.Assert(GetKuidAsString() = "<kuid:224567:-1351192821>", "Kuid conversion failed!")
        SetKuid("<kuid:224567:-1351192821>")
        Debug.Assert(GetKuidAsString() = "<kuid:224567:-1351192821>", "Kuid conversion failed!")

        MsgBox("Kuid conversion verification completed!")
    End Sub

End Class

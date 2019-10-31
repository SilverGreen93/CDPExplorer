Imports System.IO

Public Class Form1

    Dim file As String
    Dim kuidList As String
    Dim Ckuid, Cusername, Cbuild, Cregion, Ckind, Cera, Cclass As String
    Dim totalAssets, lastIndex As Integer
    Dim expandedChump As String
    Dim bytesCopied(15) As Byte
    Dim bytes As Byte() 'the file contents is stored here

    ''' <summary>
    ''' Transforms string kuid to 8 byte kuid (for routes/sessions)
    ''' </summary>
    ''' <param name="kuid">The kuid as string</param>
    ''' <returns>The kuid as 8 bytes</returns>
    ''' <remarks></remarks>
    Function KuidToHex(ByVal kuid As String) As Byte()
        Dim num(2) As Integer                           'variable to store kuid parts
        Dim ubytes(3), cbytes(3) As Byte                'user bytes and content bytes
        Dim str() As String                             'split kuid to tokens
        Dim rbytes(7) As Byte                           'returned bytes

        If kuid = "" Then
            'MessageBox.Show("NULL kuid encountered!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'err = True
            Return {0, 0, 0, 0, 0, 0, 0, 0}
        End If

        str = System.Text.RegularExpressions.Regex.Split(kuid, ":")

        If str.Length < 3 Then
            'MessageBox.Show("Invalid kuid encountered!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return {0, 0, 0, 0, 0, 0, 0, 0}
        End If

        Try
            'parse the kuid parts (skip first) and convert to integers
            For i As Integer = 1 To str.Length - 1
                num(i - 1) = Val(str(i))
            Next

            'get the bytes from integers
            ubytes = BitConverter.GetBytes(num(0))
            cbytes = BitConverter.GetBytes(num(1))
            If (num(2) > 0 AndAlso num(2) < 128 AndAlso num(0) >= 0) Then 'check if uid is negative
                ubytes(3) = ubytes(3) Xor (CByte(num(2)) << 1)  'add the version number to byte 4 of uid
            End If

            'merge bytes
            For i As Integer = 0 To 3
                rbytes(i) = ubytes(i)
                rbytes(i + 4) = cbytes(i)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblProgress.Visible = False
        End Try

        Return rbytes

    End Function

    ''' <summary>
    ''' Reverses the UID with the CID (for config.chump)
    ''' </summary>
    ''' <param name="kuid">The kuid as 8 bytes</param>
    ''' <returns>The reversed kuid as 8 bytes</returns>
    ''' <remarks></remarks>
    Function revKuid(ByVal kuid As Byte()) As Byte()

        Dim rbytes(7) As Byte   'returned bytes

        Try
            'reverse bytes (a-b-c-d-e-f-g-h -> e-f-g-h-a-b-c-d)
            For i As Integer = 0 To 3
                rbytes(i) = kuid(i + 4)
                rbytes(i + 4) = kuid(i)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblProgress.Visible = False
        End Try

        Return rbytes

    End Function

    Function HexToKuid(ByVal kuid As Byte()) As String
        Dim rstring As String = "<kuid"
        Dim ubytes(3), cbytes(3) As Byte
        Dim version As Integer
        Dim uInt As Integer

        Array.Copy(kuid, 0, ubytes, 0, 4)
        Array.Copy(kuid, 4, cbytes, 0, 4)

        If (ubytes(3) And (1 << 0)) <> 0 Then
            'negative user id, ignore version
            version = 0
        Else
            version = Convert.ToInt32(ubytes(3) >> 1)
            ubytes(3) = ubytes(3) And &H1
        End If

        uInt = BitConverter.ToInt32(ubytes, 0)

        If version <> 0 Then
            rstring = rstring & "2:"
        Else
            rstring = rstring & ":"
        End If

        rstring = rstring & uInt
        rstring = rstring & ":"
        rstring = rstring & BitConverter.ToInt32(cbytes, 0)

        If version <> 0 Then
            rstring = rstring & ":" & version
        End If

        rstring = rstring & ">"
        Return rstring
    End Function

    Sub ParseSubTags(ByRef bytes As Byte(), ByRef fpointer As Integer, ByRef expandedChump As String, ByVal level As Integer, ByVal parentContainer As String)
        Dim tagLength As Integer = ParseInteger32(bytes, fpointer)
        fpointer = fpointer + 4
        Dim tagNameSize As Byte = bytes(fpointer)
        fpointer = fpointer + 1

        Dim tagName(tagNameSize - 2) As Byte
        Array.Copy(bytes, fpointer, tagName, 0, tagNameSize - 1)
        fpointer = fpointer + tagNameSize

        'expandedChump = expandedChump & Space(level * 2) & System.Text.Encoding.UTF8.GetString(tagName)

        Dim tagType As Byte = bytes(fpointer)
        fpointer = fpointer + 1

        Select Case tagType
            Case 0 'container

                Dim currentFilePointer As Integer = fpointer

                If parentContainer <> "assets" AndAlso parentContainer <> "" Then
                    fpointer = currentFilePointer + tagLength - tagNameSize - 2 'daca parintele nu e assets sau nu e fisierul insusi, nu mai parcurge subtaguri, treci peste
                End If

                'expandedChump = expandedChump & vbCrLf & Space(level * 2) & "{" & vbCrLf

                While Not fpointer = currentFilePointer + tagLength - tagNameSize - 2

                    ParseSubTags(bytes, fpointer, expandedChump, level + 1, System.Text.Encoding.UTF8.GetString(tagName))

                End While

                'expandedChump = expandedChump & Space(level * 2) & "}"
            Case 1 'integer
                'expandedChump = expandedChump &
                'Space(IIf(40 - tagNameSize - 1 - level * 2 < 2, 2, 40 - tagNameSize - 1 - level * 2))
                'Dim comma As Boolean = False
                'exceptie: daca kuid e -1 atunci e tinut ca int numai partea a doua si aici trebuie recompus!
                If System.Text.Encoding.UTF8.GetString(tagName) = "kuid" Then
                    'For i As Integer = 0 To tagLength - tagNameSize - 3 Step 4
                    'expandedChump = expandedChump & IIf(comma, ",", "") & ParseInteger32(bytes, fpointer + i).ToString("G", Globalization.CultureInfo.InvariantCulture)
                    Dim kuid As Byte() = {&HFF, &HFF, &HFF, &HFF, 0, 0, 0, 0}
                    'copiaza ultimii 4 octeti, primii 4 sint -1
                    Array.Copy(bytes, fpointer, kuid, 4, 4)
                    Ckuid = HexToKuid(kuid)
                    'comma = True
                    'Next
                End If
            Case 2 'float
                'expandedChump = expandedChump &
                'Space(IIf(40 - tagNameSize - 1 - level * 2 < 2, 2, 40 - tagNameSize - 1 - level * 2))
                Dim comma As Boolean = False
                For i As Integer = 0 To tagLength - tagNameSize - 3 Step 4
                    'expandedChump = expandedChump & IIf(comma, ",", "") & ParseFloat(bytes, fpointer + i).ToString("G", Globalization.CultureInfo.InvariantCulture)
                    If System.Text.Encoding.UTF8.GetString(tagName) = "trainz-build" Then
                        Cbuild = ParseFloat(bytes, fpointer + i).ToString("G", Globalization.CultureInfo.InvariantCulture)
                    End If
                    comma = True
                Next
            Case 3 'string
                If tagLength - tagNameSize - 4 > 0 Then 'otherwise it is a null string (not even a character)
                    Dim tagString(tagLength - tagNameSize - 4) As Byte
                    Array.Copy(bytes, fpointer, tagString, 0, tagLength - tagNameSize - 3)

                    If System.Text.Encoding.UTF8.GetString(tagName) = "username" Then
                        Cusername = System.Text.Encoding.UTF8.GetString(tagString)
                    ElseIf System.Text.Encoding.UTF8.GetString(tagName) = "kind" Then
                        Ckind = System.Text.Encoding.UTF8.GetString(tagString)
                    ElseIf System.Text.Encoding.UTF8.GetString(tagName) = "category-region" OrElse System.Text.Encoding.UTF8.GetString(tagName) = "category-region-0" Then
                        Cregion = System.Text.Encoding.UTF8.GetString(tagString)
                    ElseIf System.Text.Encoding.UTF8.GetString(tagName) = "category-era" OrElse System.Text.Encoding.UTF8.GetString(tagName) = "category-era-0" Then
                        Cera = System.Text.Encoding.UTF8.GetString(tagString)
                    ElseIf System.Text.Encoding.UTF8.GetString(tagName) = "category-class" Then
                        Cclass = System.Text.Encoding.UTF8.GetString(tagString)
                    ElseIf System.Text.Encoding.UTF8.GetString(tagName) = "kuid" Then
                        'exceptie: pentru unele asseturi vechi kuid poate fi si string.
                        Ckuid = "<kuid:" & System.Text.Encoding.UTF8.GetString(tagString) & ">"
                    ElseIf System.Text.Encoding.UTF8.GetString(tagName) = "asset-filename" Then
                        'This is the case for old assets which do not have username
                        If Cusername = "Untitled" Then
                            Cusername = System.Text.Encoding.UTF8.GetString(tagString)
                        End If
                    End If

                        'expandedChump = expandedChump &
                        'Space(IIf(40 - tagNameSize - 1 - level * 2 < 2, 2, 40 - tagNameSize - 1 - level * 2)) &
                        '"""" & System.Text.Encoding.UTF8.GetString(tagString) & """"
                    End If
            Case 4 'binary
                If tagLength - tagNameSize - 4 > 0 Then 'otherwise it is a null string (not even a character)
                    Dim tagString(tagLength - tagNameSize - 4) As Byte
                    Array.Copy(bytes, fpointer, tagString, 0, tagLength - tagNameSize - 3)
                    'expandedChump = expandedChump &
                    'Space(IIf(40 - tagNameSize - 1 - level * 2 < 2, 2, 40 - tagNameSize - 1 - level * 2)) &
                    'System.Text.Encoding.ASCII.GetString(tagString)
                End If
            Case 5 'null

            Case 13 'kuid
                Dim kuid(7) As Byte
                Array.Copy(bytes, fpointer, kuid, 0, 8)

                If System.Text.Encoding.UTF8.GetString(tagName) = "kuid" Then
                    Ckuid = HexToKuid(kuid)
                End If

                'expandedChump = expandedChump &
                'Space(IIf(40 - tagNameSize - 1 - level * 2 < 2, 2, 40 - tagNameSize - 1 - level * 2)) &
                'HexToKuid(kuid)
            Case Else
                Throw New Exception("Unknown tag type: " & tagType)
        End Select

        If tagType <> 0 Then fpointer = fpointer + tagLength - tagNameSize - 2

        'If Ckuid <> "" AndAlso Cusername <> "" AndAlso Ckind <> "" Then
        If parentContainer = "assets" Then
            expandedChump = expandedChump & Ckuid & ", " & Cusername & vbCrLf

            gridKUIDs.Rows.Add(New String() {Ckuid, Cusername, Ckind, Cclass, Cbuild, Cregion, Cera})

            kuidList = kuidList & Ckuid & ","

            Ckuid = ""
            Cusername = "Untitled" 'This is the case for assets with tag "secret 1" which do not have username and asset-filename
            Ckind = ""
            Cbuild = "1.3"
            Cregion = ""
            Cera = ""
            Cclass = ""
            totalAssets = totalAssets + 1
        End If

    End Sub

    Function CompareBytes(ByVal b1 As Byte(), ByVal b2 As Byte()) As Boolean
        If (b1 Is b2) Then
            Return True
        End If
        If (b1 Is Nothing OrElse b2 Is Nothing) Then
            Return False
        End If
        If (b1.Length <> b2.Length) Then
            Return False
        End If
        For i As Integer = 0 To b1.Length - 1
            If (b1(i) <> b2(i)) Then
                Return False
            End If
        Next i
        Return True
    End Function

    Function FormatHex(ByRef b As Byte(), ByVal start As Integer, ByVal len As Integer) As Byte()

        Dim rbytes(len - 1) As Byte   'returned bytes

        For i As Integer = 0 To len - 1
            rbytes(i) = b(start + i)
        Next

        Return rbytes

    End Function

    Function ParseInteger32(ByRef b As Byte(), ByVal pos As Integer) As Integer
        Return BitConverter.ToInt32(FormatHex(b, pos, 4), 0)
    End Function

    Function ParseInteger64(ByRef b As Byte(), ByVal pos As Integer) As Long
        Return BitConverter.ToInt64(FormatHex(b, pos, 8), 0)
    End Function

    Function ParseFloat(ByRef b As Byte(), ByVal pos As Integer) As Single
        Return BitConverter.ToSingle(FormatHex(b, pos, 4), 0)
    End Function

    Function ToInteger32(ByVal i As Integer) As Byte()
        Return FormatHex(BitConverter.GetBytes(i), 0, 4)
    End Function

    Function ToInteger64(ByVal i As Long) As Byte()
        Return FormatHex(BitConverter.GetBytes(i), 0, 8)
    End Function

    Function ToFloat(ByVal i As Single) As Byte()
        Return FormatHex(BitConverter.GetBytes(i), 0, 4)
    End Function


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initializeGrid()
        Dim files As String() = My.Application.CommandLineArgs.ToArray
        If files.Length > 0 Then
            file = files(0)
            lblProgress.Visible = True
            Application.DoEvents()
            ProcessCDP()
            lblProgress.Visible = False
        End If

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://vvmm.freeforums.org/")
    End Sub

    'a new instance of the application was started with new arguments
    Public Sub NewArgumentsReceived(args As String())
        If args.Length > 0 Then
            Dim files As String() = args.ToArray
            If files.Length > 0 Then
                file = files(0)
                lblProgress.Visible = True
                Application.DoEvents()
                ProcessCDP()
                lblProgress.Visible = False
            End If
        End If
    End Sub

    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        Dim files As String() = e.Data.GetData(DataFormats.FileDrop)
        If files.Length > 0 Then
            file = files(0)
            bytes = Nothing 'clears the previously read file
            lblProgress.Visible = True
            Application.DoEvents()
            ProcessCDP()
            lblProgress.Visible = False
        End If

    End Sub

    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        OpenFileDialog1.Title = "Select folder to extract to"
        OpenFileDialog1.FileName = "CDP Folder"

        If OpenFileDialog1.ShowDialog <> vbOK Then Exit Sub

        lblProgress.Visible = True
        Application.DoEvents()
        For row As Integer = 0 To gridKUIDs.SelectedRows.Count - 1
            ExtractContent(gridKUIDs.SelectedRows(row).Cells(0).Value, System.IO.Path.GetDirectoryName(OpenFileDialog1.FileName))
        Next
        lblProgress.Visible = False

        MessageBox.Show("Extracting assets to CDPs finished!", "Ready", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        OpenFileDialog1.Title = "Select folder to extract to"
        OpenFileDialog1.FileName = "CDP Folder"

        If OpenFileDialog1.ShowDialog <> vbOK Then Exit Sub

        lblProgress.Visible = True
        Application.DoEvents()
        For row As Integer = 0 To gridKUIDs.Rows.Count - 1
            ExtractContent(gridKUIDs.Rows(row).Cells(0).Value, System.IO.Path.GetDirectoryName(OpenFileDialog1.FileName))
        Next
        lblProgress.Visible = False

        MessageBox.Show("Extracting assets to CDPs finished!", "Ready", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub


    Sub ExtractContent(kuid As String, path As String)
        CopyCDP(kuid)

        'adauga containerul "assets"
        bytesCopied(&H14) = 7
        Array.Copy(System.Text.Encoding.ASCII.GetBytes("assets"), 0, bytesCopied, &H15, 6)
        bytesCopied(&H1B) = 0
        bytesCopied(&H1C) = 0

        'calculeaza lungimea containerului "assets" ca fiind lungimea pana la final fara header si fara lungimea insasi
        Array.Copy(ToInteger32(bytesCopied.Length - &H14), 0, bytesCopied, &H10, 4)

        'adauga tag-urile finale
        '    contents-table
        '    {
        '	     0   <kuid:0:0>
        '    }
        '    kuid-table
        '    {
        '    }
        '    obsolete-table
        '    {
        '    }
        '    kind "archive"
        '    package-version 1
        '    username "unknown"
        Dim currentLength As Integer = bytesCopied.Length
        Dim rawData As Byte() = {
                                    &H21, &H0, &H0, &H0, &HF, &H63, &H6F, &H6E, &H74, &H65, &H6E, &H74,
                                    &H73, &H2D, &H74, &H61, &H62, &H6C, &H65, &H0, &H0, &HC, &H0, &H0,
                                    &H0, &H2, &H30, &H0, &HD, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0,
                                    &HD, &H0, &H0, &H0, &HB, &H6B, &H75, &H69, &H64, &H2D, &H74, &H61,
                                    &H62, &H6C, &H65, &H0, &H0,
                                    &H11, &H0, &H0, &H0, &HF, &H6F, &H62, &H73, &H6F, &H6C, &H65, &H74,
                                    &H65, &H2D, &H74, &H61, &H62, &H6C, &H65, &H0, &H0, &HF, &H0, &H0,
                                    &H0, &H5, &H6B, &H69, &H6E, &H64, &H0, &H3, &H61, &H72, &H63, &H68,
                                    &H69, &H76, &H65, &H0, &H16, &H0, &H0, &H0, &H10, &H70, &H61, &H63,
                                    &H6B, &H61, &H67, &H65, &H2D, &H76, &H65, &H72, &H73, &H69, &H6F, &H6E,
                                    &H0, &H1, &H1, &H0, &H0, &H0, &H13, &H0, &H0, &H0, &H9, &H75,
                                    &H73, &H65, &H72, &H6E, &H61, &H6D, &H65, &H0, &H3, &H75, &H6E, &H6B,
                                    &H6E, &H6F, &H77, &H6E, &H0
                                }
        ReDim Preserve bytesCopied(UBound(bytesCopied) + rawData.Length)

        Array.Copy(rawData, 0, bytesCopied, currentLength, rawData.Length)
        Array.Copy(KuidToHex(kuid), 0, bytesCopied, currentLength + &H1D, 8)

        'calculeaza lungimea totala a fisierului
        Array.Copy(ToInteger32(bytesCopied.Length - &H10), 0, bytesCopied, &HC, 4)

        System.IO.File.WriteAllBytes(path & "\" & kuid.Replace("<", "").Replace(":", " ").Replace(">", "") & ".cdp", bytesCopied)

        ReDim bytesCopied(15)
    End Sub

    Sub ProcessCDP()
        If Not My.Computer.FileSystem.FileExists(file) Then Exit Sub

        'expand chump

        Try
            If bytes Is Nothing Then
                Dim fileStr As New FileStream(file, FileMode.Open, FileAccess.Read)
                If fileStr.Length > System.Int32.MaxValue Then
                    fileStr.Close()
                    Throw New Exception("Files greater than 2GB are not supported.")
                End If

                bytes = New Byte(fileStr.Length - 1) {}
                'MessageBox.Show(fileStr.Length) 'for debugging >2GB files
                fileStr.Seek(0, SeekOrigin.Begin)

                Dim bytesRead As Long = fileStr.Read(bytes, 0, bytes.Length)
                If bytesRead = 0 Then
                    fileStr.Close()
                    Throw New Exception("Error reading file: " & file)
                End If
                fileStr.Close()
            End If

            Dim b1(3) As Byte
            Array.Copy(bytes, b1, 4)
            Dim b2 As Byte() = System.Text.Encoding.ASCII.GetBytes("ACS$")

            If Not CompareBytes(b1, b2) Then
                Throw New Exception("File format not supported for " & file)
            End If

            Dim fpointer As Integer = 12
            Dim chumpLength As Integer = ParseInteger32(bytes, fpointer)
            fpointer = fpointer + 4

            expandedChump = ""
            totalAssets = 0
            Ckuid = ""
            Cusername = "Untitled" 'This is the case for assets with tag "secret 1" which do not have username and asset-filename
            Ckind = ""
            Cbuild = "1.3"
            Cregion = ""
            Cera = ""
            Cclass = ""
            kuidList = ""
            gridKUIDs.RowCount = 1

            Dim currentFilePointer As Integer = fpointer
            Dim level As Integer = 0
            While Not fpointer = currentFilePointer + chumpLength

                ParseSubTags(bytes, fpointer, expandedChump, level, "")

            End While

            lblCount.Text = totalAssets & " assets."
            gridKUIDs.Rows.RemoveAt(0)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblProgress.Visible = False
        End Try

        ' MessageBox.Show("Finished expanding chumps.", "Ready", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Sub CopyCDP(ByVal wkuid As String)
        Dim status As Boolean

        If Not My.Computer.FileSystem.FileExists(file) Then Exit Sub

        'expand chump

        Try

            If bytes Is Nothing Then
                bytes = My.Computer.FileSystem.ReadAllBytes(file)
            End If

            Dim b1(3) As Byte
            Array.Copy(bytes, b1, 4)
            Dim b2 As Byte() = System.Text.Encoding.ASCII.GetBytes("ACS$")

            If Not CompareBytes(b1, b2) Then
                Throw New Exception("File format not supported for " & file)
                Exit Sub
            End If

            Dim fpointer As Integer = 12
            Dim chumpLength As Integer = ParseInteger32(bytes, fpointer)
            fpointer = fpointer + 4

            Array.Copy(bytes, bytesCopied, 16)

            Dim currentFilePointer As Integer = fpointer
            Dim level As Integer = 0

            While Not fpointer = currentFilePointer + chumpLength
                status = CopySubTags(bytes, fpointer, expandedChump, level, "", wkuid)
                If status = True Then Exit While
            End While

            If status = False Then Throw New Exception("The asset " & wkuid & " could not be found in the CDP!")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblProgress.Visible = False
        End Try

        ' MessageBox.Show("Finished expanding chumps.", "Ready", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub


    Function CopySubTags(ByRef bytes As Byte(), ByRef fpointer As Integer, ByRef expandedChump As String, ByVal level As Integer, ByVal parentContainer As String, ByVal wkuid As String) As Boolean
        Dim tagLength As Integer = ParseInteger32(bytes, fpointer)
        fpointer = fpointer + 4
        Dim tagNameSize As Byte = bytes(fpointer)
        fpointer = fpointer + 1

        Dim status As Boolean

        Dim tagName(tagNameSize - 2) As Byte
        Array.Copy(bytes, fpointer, tagName, 0, tagNameSize - 1)

        If System.Text.Encoding.ASCII.GetString(tagName) = wkuid AndAlso parentContainer = "assets" Then
            ReDim Preserve bytesCopied(UBound(bytesCopied) + tagLength + 13 + 4) '13 este lungimea tag-ului assets
            Array.Copy(bytes, fpointer - 5, bytesCopied, 16 + 13, tagLength + 4) '16 este headerul fisierului cdp, 4 este lungimea lungimii
            Return True
        End If

        fpointer = fpointer + tagNameSize

        'expandedChump = expandedChump & Space(level * 2) & System.Text.Encoding.UTF8.GetString(tagName)

        Dim tagType As Byte = bytes(fpointer)
        fpointer = fpointer + 1

        Select Case tagType
            Case 0 'container

                Dim currentFilePointer As Integer = fpointer

                If parentContainer <> "assets" AndAlso parentContainer <> "" AndAlso parentContainer <> wkuid Then
                    fpointer = currentFilePointer + tagLength - tagNameSize - 2 'daca parintele nu e assets sau nu e fisierul insusi, nu mai parcurge subtaguri, treci peste
                End If

                'expandedChump = expandedChump & vbCrLf & Space(level * 2) & "{" & vbCrLf

                While Not fpointer = currentFilePointer + tagLength - tagNameSize - 2
                    status = CopySubTags(bytes, fpointer, expandedChump, level + 1, System.Text.Encoding.UTF8.GetString(tagName), wkuid)
                    If status = True Then Return True
                End While

                '    'expandedChump = expandedChump & Space(level * 2) & "}"
                'Case 1 'integer
                '    'expandedChump = expandedChump &
                '    'Space(IIf(40 - tagNameSize - 1 - level * 2 < 2, 2, 40 - tagNameSize - 1 - level * 2))
                '    Dim comma As Boolean = False
                '    For i As Integer = 0 To tagLength - tagNameSize - 3 Step 4
                '        'expandedChump = expandedChump & IIf(comma, ",", "") & ParseInteger32(bytes, fpointer + i).ToString("G", Globalization.CultureInfo.InvariantCulture)
                '        comma = True
                '    Next
                'Case 2 'float
                '    'expandedChump = expandedChump &
                '    'Space(IIf(40 - tagNameSize - 1 - level * 2 < 2, 2, 40 - tagNameSize - 1 - level * 2))
                '    Dim comma As Boolean = False
                '    For i As Integer = 0 To tagLength - tagNameSize - 3 Step 4
                '        'expandedChump = expandedChump & IIf(comma, ",", "") & ParseFloat(bytes, fpointer + i).ToString("G", Globalization.CultureInfo.InvariantCulture)
                '        If System.Text.Encoding.UTF8.GetString(tagName) = "trainz-build" Then
                '            Cbuild = ParseFloat(bytes, fpointer + i).ToString("G", Globalization.CultureInfo.InvariantCulture)
                '        End If
                '        comma = True
                '    Next
                'Case 3 'string
                '    If tagLength - tagNameSize - 4 > 0 Then 'otherwise it is a null string (not even a character)
                '        Dim tagString(tagLength - tagNameSize - 4) As Byte
                '        Array.Copy(bytes, fpointer, tagString, 0, tagLength - tagNameSize - 3)

                '        'If System.Text.Encoding.UTF8.GetString(tagName) = "username" Then
                '        '    Cusername = System.Text.Encoding.UTF8.GetString(tagString)
                '        'ElseIf System.Text.Encoding.UTF8.GetString(tagName) = "kind" Then
                '        '    Ckind = System.Text.Encoding.UTF8.GetString(tagString)
                '        'ElseIf System.Text.Encoding.UTF8.GetString(tagName) = "category-region" OrElse System.Text.Encoding.UTF8.GetString(tagName) = "category-region-0" Then
                '        '    Cregion = System.Text.Encoding.UTF8.GetString(tagString)
                '        'ElseIf System.Text.Encoding.UTF8.GetString(tagName) = "category-era" OrElse System.Text.Encoding.UTF8.GetString(tagName) = "category-era-0" Then
                '        '    Cera = System.Text.Encoding.UTF8.GetString(tagString)
                '        'ElseIf System.Text.Encoding.UTF8.GetString(tagName) = "category-class" Then
                '        '    Cclass = System.Text.Encoding.UTF8.GetString(tagString)
                '        'End If

                '        'expandedChump = expandedChump &
                '        'Space(IIf(40 - tagNameSize - 1 - level * 2 < 2, 2, 40 - tagNameSize - 1 - level * 2)) &
                '        '"""" & System.Text.Encoding.UTF8.GetString(tagString) & """"
                '    End If
                'Case 4 'binary
                '    If tagLength - tagNameSize - 4 > 0 Then 'otherwise it is a null string (not even a character)
                '        Dim tagString(tagLength - tagNameSize - 4) As Byte
                '        Array.Copy(bytes, fpointer, tagString, 0, tagLength - tagNameSize - 3)
                '        'expandedChump = expandedChump &
                '        'Space(IIf(40 - tagNameSize - 1 - level * 2 < 2, 2, 40 - tagNameSize - 1 - level * 2)) &
                '        'System.Text.Encoding.ASCII.GetString(tagString)
                '    End If
                'Case 5 'null

                'Case 13 'kuid
                '    Dim kuid(7) As Byte
                '    Array.Copy(bytes, fpointer, kuid, 0, 8)

                '    If System.Text.Encoding.UTF8.GetString(tagName) = "kuid" Then
                '        Ckuid = HexToKuid(kuid)
                '    End If

                '    'expandedChump = expandedChump &
                '    'Space(IIf(40 - tagNameSize - 1 - level * 2 < 2, 2, 40 - tagNameSize - 1 - level * 2)) &
                '    'HexToKuid(kuid)
                'Case Else
                '    Throw New Exception("Unknown tag type: " & tagType)
        End Select

        If tagType <> 0 Then fpointer = fpointer + tagLength - tagNameSize - 2

        Return False
    End Function


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        My.Computer.Clipboard.SetText(kuidList)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        My.Computer.Clipboard.SetText(expandedChump)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Dim index As Integer = txtKuids.Text.IndexOf(TextBox1.Text, lastIndex + 1)
        'lastIndex = index
        'If index >= 0 Then
        '    txtKuids.SelectionStart = index
        '    txtKuids.SelectionLength = TextBox1.Text.Length
        '    txtKuids.Select()
        '    txtKuids.ScrollToCaret()
        'End If
        Dim searchIndex = 0
        Dim found As Boolean = False
        gridKUIDs.ClearSelection()
        For Each row As DataGridViewRow In gridKUIDs.Rows
            For Each cell As DataGridViewCell In row.Cells
                If cell.Value = Nothing Then Continue For
                If CStr(cell.Value).Contains(TextBox1.Text) Then
                    If searchIndex = lastIndex Then
                        'This is the cell we want to select
                        cell.Selected = True
                        gridKUIDs.FirstDisplayedScrollingRowIndex = gridKUIDs.SelectedCells(0).RowIndex
                    End If
                    'Yellow background for all matches
                    'cell.Style.BackColor = Color.Yellow
                    searchIndex += 1
                    found = True
                End If
            Next
        Next
        If found = True Then
            lastIndex += 1
            If lastIndex = searchIndex Then lastIndex = 0
        Else
            lastIndex = 0
            MessageBox.Show("String not found!")
        End If

    End Sub

    Sub initializeGrid()
        gridKUIDs.ColumnCount = 7
        gridKUIDs.Columns(0).Name = "KUID"
        gridKUIDs.Columns(1).Name = "Username"
        gridKUIDs.Columns(2).Name = "Kind"
        gridKUIDs.Columns(3).Name = "Class"
        gridKUIDs.Columns(4).Name = "Build"
        gridKUIDs.Columns(5).Name = "Region"
        gridKUIDs.Columns(6).Name = "Era"


        gridKUIDs.Columns(0).ReadOnly = True
        gridKUIDs.Columns(1).ReadOnly = True
        gridKUIDs.Columns(2).ReadOnly = True
        gridKUIDs.Columns(3).ReadOnly = True
        gridKUIDs.Columns(4).ReadOnly = True
        gridKUIDs.Columns(5).ReadOnly = True
        gridKUIDs.Columns(6).ReadOnly = True

    End Sub

End Class

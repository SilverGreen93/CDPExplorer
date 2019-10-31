Imports System.IO

Public Class Form1

    Dim file As String
    Dim kuidList As String
    Dim Ckuid, Cusername, Cbuild, Cregion, Ckind, Cera, Cclass As String
    Dim totalAssets, lastIndex As Integer
    Dim expandedChump As String
    Dim bytesCopied(15) As Byte

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

    Sub ParseSubTags(ByRef FileStr As BinaryReader, ByVal Depth As Integer, ByVal ParentContainer As String)
        Dim tagLength As UInteger = FileStr.ReadUInt32()
        Dim tagNameSize As Byte = FileStr.ReadByte()
        Dim tagName As String

        tagName = System.Text.Encoding.UTF8.GetString(FileStr.ReadBytes(tagNameSize - 1))
        FileStr.ReadByte() 'null terminator

        Dim tagType As Byte = FileStr.ReadByte()

        Select Case tagType
            Case 0 'container
                Dim currentFilePointer As Long = FileStr.BaseStream.Position

                'We don't care of subcontainers that are not in the "assets" container, so skip them
                If ParentContainer <> "assets" AndAlso ParentContainer <> "" Then
                    FileStr.BaseStream.Seek(tagLength - tagNameSize - 2, SeekOrigin.Current)
                End If

                While Not FileStr.BaseStream.Position = currentFilePointer + tagLength - tagNameSize - 2
                    ParseSubTags(FileStr, Depth + 1, tagName)
                End While

            Case 1 'integer
                'If tagName is "kuid" it means that it is a legacy asset: user part is -1 and we only have the content part here
                If tagName = "kuid" Then
                    Dim kuid As Byte() = {&HFF, &HFF, &HFF, &HFF, 0, 0, 0, 0}
                    Dim IntVal(3) As Byte
                    IntVal = FileStr.ReadBytes(4)
                    'Copy only last 4 bytes. The first 4 are -1
                    Array.Copy(IntVal, 0, kuid, 4, 4)
                    Ckuid = HexToKuid(kuid)
                Else
                    'Just skip
                    Dim IntVal As Integer
                    'parse each int value if more than 1
                    For i As Integer = 0 To tagLength - tagNameSize - 3 Step 4
                        IntVal = FileStr.ReadInt32()
                    Next
                End If

            Case 2 'float
                Dim FloatVal As Single
                If tagName = "trainz-build" Then
                    FloatVal = FileStr.ReadSingle()
                    Cbuild = FloatVal.ToString("G", Globalization.CultureInfo.InvariantCulture)
                Else
                    'Just skip
                    'parse each float value if more than 1
                    For i As Integer = 0 To tagLength - tagNameSize - 3 Step 4
                        FloatVal = FileStr.ReadSingle()
                        If tagName = "trainz-build" Then
                            Cbuild = FloatVal.ToString("G", Globalization.CultureInfo.InvariantCulture)
                        End If
                    Next
                End If

            Case 3 'string
                If tagLength - tagNameSize - 4 > 0 Then 'otherwise it is a null string (not even a character)
                    Dim tagString As String

                    tagString = System.Text.Encoding.UTF8.GetString(FileStr.ReadBytes(tagLength - tagNameSize - 3))
                    FileStr.ReadByte() 'null terminator

                    If tagName = "username" Then
                        Cusername = tagString
                    ElseIf tagName = "kind" Then
                        Ckind = tagString
                    ElseIf tagName = "category-region" OrElse tagName = "category-region-0" Then
                        Cregion = tagString
                    ElseIf tagName = "category-era" OrElse tagName = "category-era-0" Then
                        Cera = tagString
                    ElseIf tagName = "category-class" Then
                        Cclass = tagString
                    ElseIf tagName = "kuid" Then
                        'For some legacy assets the kuid can be stored as string as 1234:12345
                        Ckuid = "<kuid:" & tagString & ">"
                    ElseIf tagName = "asset-filename" Then
                        'For legacy assets which do not have username
                        If Cusername = "Untitled" Then
                            Cusername = tagString
                        End If
                    End If
                End If

            Case 4 'binary
                'Just skip
                FileStr.ReadBytes(tagLength - tagNameSize - 2)

            Case 5 'null
                'Just skip
                FileStr.ReadBytes(tagLength - tagNameSize - 2)

            Case 13 'kuid
                Dim kuid(7) As Byte
                kuid = FileStr.ReadBytes(8)
                If tagName = "kuid" Then
                    Ckuid = HexToKuid(kuid)
                End If

            Case Else
                Throw New Exception("Unknown tag type: " & tagType)

        End Select

        If ParentContainer = "assets" Then
            expandedChump = expandedChump & Ckuid & ", " & Cusername & vbCrLf

            gridKUIDs.Rows.Add(New String() {Ckuid, Cusername, Ckind, Cclass, Cbuild, Cregion, Cera})

            kuidList = kuidList & Ckuid & ","

            'Reset the asset details
            Ckuid = ""
            Cusername = "Untitled" 'This is the case for assets with tag "secret 1" which do not have username and asset-filename
            Ckind = ""
            Cbuild = "1.3" 'This is the default trainz-build version.
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

    Function ParseInteger8(ByRef FileStr As FileStream) As Integer
        Dim IntBytes(0) As Byte
        FileStr.Read(IntBytes, FileStr.Position, IntBytes.Length)
        Return BitConverter.ToInt32(IntBytes, 0)
    End Function

    Function ParseInteger32(ByRef FileStr As FileStream) As Integer
        Dim IntBytes(3) As Byte
        FileStr.Read(IntBytes, FileStr.Position, IntBytes.Length)
        Return BitConverter.ToInt32(IntBytes, 0)
    End Function

    Function ParseInteger64(ByRef FileStr As FileStream) As Long
        Dim LongBytes(7) As Byte
        FileStr.Read(LongBytes, FileStr.Position, LongBytes.Length)
        Return BitConverter.ToInt64(LongBytes, 0)
    End Function

    Function ParseFloat(ByRef FileStr As FileStream) As Single
        Dim FloatBytes(3) As Byte
        FileStr.Read(FloatBytes, FileStr.Position, FloatBytes.Length)
        Return BitConverter.ToSingle(FloatBytes, 0)
    End Function


    'Function ParseInteger32(ByRef b As Byte(), ByVal pos As Integer) As Integer
    '    Return BitConverter.ToInt32(FormatHex(b, pos, 4), 0)
    'End Function

    'Function ParseInteger64(ByRef b As Byte(), ByVal pos As Integer) As Long
    '    Return BitConverter.ToInt64(FormatHex(b, pos, 8), 0)
    'End Function

    'Function ParseFloat(ByRef b As Byte(), ByVal pos As Integer) As Single
    '    Return BitConverter.ToSingle(FormatHex(b, pos, 4), 0)
    'End Function

    Function ToInteger32(ByVal i As Integer) As Byte()
        Return BitConverter.GetBytes(i)
    End Function

    Function ToInteger64(ByVal i As Long) As Byte()
        Return BitConverter.GetBytes(i)
    End Function

    Function ToFloat(ByVal i As Single) As Byte()
        Return BitConverter.GetBytes(i)
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
        Dim FileStr As BinaryReader

        If Not My.Computer.FileSystem.FileExists(file) Then Exit Sub

        Try
            FileStr = New BinaryReader(IO.File.Open(file, FileMode.Open))

            FileStr.BaseStream.Seek(0, SeekOrigin.Begin)

            'Check file signature to make sure it is a chump file type
            Dim ExpectedSignature As Byte() = System.Text.Encoding.ASCII.GetBytes("ACS$")
            Dim FileSignature(ExpectedSignature.Length - 1) As Byte

            FileSignature = FileStr.ReadBytes(ExpectedSignature.Length)

            If Not CompareBytes(FileSignature, ExpectedSignature) Then
                Throw New Exception("File format not supported for " & file)
            End If

            'Skip the next 8 bytes (version bytes)
            FileStr.BaseStream.Seek(8, SeekOrigin.Current)

            'Get the chump file length
            Dim ChumpLength As UInteger = FileStr.ReadUInt32

            'Reset the asset details
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

            Dim CurrentFilePointer As Long = FileStr.BaseStream.Position 'here we are right now
            Dim Depth As Integer = 0 'we are at the top level of the file

            While Not FileStr.BaseStream.Position = CurrentFilePointer + ChumpLength
                ParseSubTags(FileStr, Depth, "")
            End While

            lblCount.Text = totalAssets & " assets."
            gridKUIDs.Rows.RemoveAt(0)

            FileStr.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblProgress.Visible = False
        End Try

    End Sub

    Sub CopyCDP(ByVal wkuid As String)
        Dim FileStr As BinaryReader
        Dim status As Boolean

        If Not My.Computer.FileSystem.FileExists(file) Then Exit Sub

        Try
            FileStr = New BinaryReader(IO.File.Open(file, FileMode.Open))

            FileStr.BaseStream.Seek(0, SeekOrigin.Begin)

            'Check file signature to make sure it is a chump file type
            Dim ExpectedSignature As Byte() = System.Text.Encoding.ASCII.GetBytes("ACS$")
            Dim FileSignature(ExpectedSignature.Length - 1) As Byte

            FileSignature = FileStr.ReadBytes(ExpectedSignature.Length)

            If Not CompareBytes(FileSignature, ExpectedSignature) Then
                Throw New Exception("File format not supported for " & file)
            End If

            'Copy the signature and the next 8 bytes (version bytes)
            Array.Copy(FileSignature, bytesCopied, 4)
            Array.Copy(FileStr.ReadBytes(8), 0, bytesCopied, 4, 8)

            'Get the chump file length
            Dim ChumpLength As UInteger = FileStr.ReadUInt32
            Dim CurrentFilePointer As Long = FileStr.BaseStream.Position 'here we are right now
            Dim Depth As Integer = 0 'we are at the top level of the file

            While Not FileStr.BaseStream.Position = CurrentFilePointer + ChumpLength
                status = CopySubTags(FileStr, Depth, "", wkuid)
                If status = True Then Exit While 'we found the asset
            End While

            FileStr.Close()

            If status = False Then Throw New Exception("The asset " & wkuid & " could not be found in the CDP!")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblProgress.Visible = False
        End Try

    End Sub


    Function CopySubTags(ByRef FileStr As BinaryReader, ByVal Depth As Integer, ByVal ParentContainer As String, ByVal wkuid As String) As Boolean
        Dim tagLength As UInteger = FileStr.ReadUInt32()
        Dim tagNameSize As Byte = FileStr.ReadByte()
        Dim tagName As String

        Dim status As Boolean

        tagName = System.Text.Encoding.UTF8.GetString(FileStr.ReadBytes(tagNameSize - 1))
        FileStr.ReadByte() 'null terminator

        'we found the asset with the required kuid
        If tagName = wkuid AndAlso ParentContainer = "assets" Then
            Dim tempExtract(tagLength + 3) As Byte
            ReDim Preserve bytesCopied(UBound(bytesCopied) + tagLength + 13 + 4) '13 este lungimea tag-ului assets

            FileStr.BaseStream.Seek(-tagNameSize - 5, SeekOrigin.Current)
            tempExtract = FileStr.ReadBytes(tagLength + 4)

            Array.Copy(tempExtract, 0, bytesCopied, 16 + 13, tagLength + 4) '16 este headerul fisierului cdp, 4 este lungimea lungimii
            Return True
        End If

        Dim tagType As Byte = FileStr.ReadByte()

        If tagType = 0 Then 'container

            Dim currentFilePointer As Long = FileStr.BaseStream.Position

            'We don't care of subcontainers that are not in the "assets" container, so skip them
            If ParentContainer <> "assets" AndAlso ParentContainer <> "" AndAlso ParentContainer <> wkuid Then
                FileStr.BaseStream.Seek(tagLength - tagNameSize - 2, SeekOrigin.Current)
            End If

            While Not FileStr.BaseStream.Position = currentFilePointer + tagLength - tagNameSize - 2
                status = CopySubTags(FileStr, Depth + 1, tagName, wkuid)
                If status = True Then Return True
            End While

        Else
            'Skip other tag types
            FileStr.BaseStream.Seek(tagLength - tagNameSize - 2, SeekOrigin.Current)
        End If

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

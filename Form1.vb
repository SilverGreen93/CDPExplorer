Imports System.IO

Public Class Form1
    Dim fileName As String
    Dim kuidList As String
    Dim kuidNameList As String
    Dim totalAssets As Integer
    Dim lastIndex As Integer 'for search purposes (find box)
    Dim bytesCopied(15) As Byte 'for extracting assets from cdp
    Dim currentAsset As Asset

    Sub ResetData()
        kuidList = ""
        kuidNameList = ""
        totalAssets = 0
        gridKUIDs.RowCount = 1
    End Sub

    Sub InitializeGrid()
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

    ''' <summary>
    ''' Compare two Byte arrays
    ''' </summary>
    ''' <param name="b1">First Byte array</param>
    ''' <param name="b2">Second Byte array</param>
    ''' <returns>True if arrays match</returns>
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

    ''' <summary>
    ''' Start parsing the chump file (cdp) and extract asset info
    ''' </summary>
    ''' <param name="fileName">File to open for reading</param>
    Sub ParseChump(ByRef fileName As String)
        Dim fileStr As BinaryReader

        If Not My.Computer.FileSystem.FileExists(fileName) Then Exit Sub

        'Try
        fileStr = New BinaryReader(File.Open(fileName, FileMode.Open))

            fileStr.BaseStream.Seek(0, SeekOrigin.Begin)

            'Check file signature to make sure it is a chump file type
            Dim ExpectedSignature As Byte() = System.Text.Encoding.ASCII.GetBytes("ACS$")
            Dim FileSignature(ExpectedSignature.Length - 1) As Byte

            FileSignature = fileStr.ReadBytes(ExpectedSignature.Length)

            If Not CompareBytes(FileSignature, ExpectedSignature) Then
                Throw New Exception("File format not supported for " & fileName)
            End If

            'Skip the next 8 bytes (version bytes)
            fileStr.BaseStream.Seek(8, SeekOrigin.Current)

            'Get the chump file length
            Dim ChumpLength As UInteger = fileStr.ReadUInt32

            'Reset the asset details
            ResetData()
            currentAsset = New Asset()

            Dim CurrentFilePointer As Long = fileStr.BaseStream.Position 'here we are right now
            Dim Depth As Integer = 0 'we are at the top level of the file

            While Not fileStr.BaseStream.Position = CurrentFilePointer + ChumpLength
                ParseSubTags(fileStr, Depth, "")
            End While

            lblCount.Text = totalAssets & " assets."
            gridKUIDs.Rows.RemoveAt(0)

            fileStr.Close()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    lblProgress.Visible = False
        'End Try

    End Sub

    ''' <summary>
    ''' Parse the Tags and SubTags recursively
    ''' </summary>
    ''' <param name="FileStr">File stream</param>
    ''' <param name="Depth">Depth of the current container</param>
    ''' <param name="ParentContainer">Parent container name</param>
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
                    'Copy only last 4 bytes. The first 4 are -1
                    currentAsset.AssetKuid.SetUserID(-1)
                    currentAsset.AssetKuid.SetContentID(FileStr.ReadInt32())
                Else
                    'Just skip
                    'parse each int value if more than 1
                    For i As Integer = 0 To tagLength - tagNameSize - 3 Step 4
                        FileStr.ReadInt32()
                    Next
                End If

            Case 2 'float
                If tagName = "trainz-build" Then
                    currentAsset.SetTrainzBuild(FileStr.ReadSingle())
                Else
                    'Just skip
                    'parse each float value if more than 1
                    For i As Integer = 0 To tagLength - tagNameSize - 3 Step 4
                        FileStr.ReadSingle()
                    Next
                End If

            Case 3 'string
                If tagLength - tagNameSize - 3 > 0 Then 'otherwise it is a null string (not even a character)
                    Dim tagString As String

                    tagString = System.Text.Encoding.UTF8.GetString(FileStr.ReadBytes(tagLength - tagNameSize - 3))
                    FileStr.ReadByte() 'null terminator

                    If tagName = "username" Then
                        currentAsset.Username = tagString
                    ElseIf tagName = "kind" Then
                        currentAsset.Kind = tagString
                    ElseIf tagName = "category-region" OrElse tagName = "category-region-0" Then
                        currentAsset.CategoryRegion = tagString
                    ElseIf tagName = "category-era" OrElse tagName = "category-era-0" Then
                        currentAsset.CategoryEra = tagString
                    ElseIf tagName = "category-class" Then
                        currentAsset.CategoryClass = tagString
                    ElseIf tagName = "description" Then
                        currentAsset.Description = tagString
                    ElseIf tagName = "kuid" Then
                        'For some legacy assets the kuid can be stored as string as 1234:12345
                        currentAsset.AssetKuid.SetKuid("<kuid:" & tagString & ">")
                    ElseIf tagName = "asset-filename" Then
                        'For legacy assets which do not have username
                        If currentAsset.Username = "Untitled" Then
                            currentAsset.Username = tagString
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
                If tagName = "kuid" Then
                    currentAsset.AssetKuid.SetKuid(FileStr.ReadBytes(8))
                Else
                    'Just skip
                    FileStr.ReadBytes(8)
                End If

            Case Else
                Throw New Exception("Unknown tag type: " & tagType)

        End Select

        If ParentContainer = "assets" Then
            totalAssets += 1
            kuidList = kuidList & currentAsset.AssetKuid.GetKuidAsString() & ","
            kuidNameList = kuidNameList & currentAsset.Username & ", " & currentAsset.AssetKuid.GetKuidAsString() & vbCrLf
            gridKUIDs.Rows.Add(New String() {currentAsset.AssetKuid.GetKuidAsString(),
                                            currentAsset.Username,
                                            currentAsset.Kind,
                                            currentAsset.CategoryClass,
                                            currentAsset.TrainzBuild,
                                            currentAsset.CategoryRegion,
                                            currentAsset.CategoryEra})

            'Reset the asset details
            currentAsset = New Asset()
        End If

    End Sub

    ''' <summary>
    ''' Start extracting and copying an individual kuid from the CDP file
    ''' </summary>
    ''' <param name="assetKuid">Kuid to search for</param>
    ''' <param name="exportPath">Path to write the new asset to</param>
    Sub ExtractContent(ByRef assetKuid As Kuid, ByRef exportPath As String)
        CopyCDP(assetKuid)

        'add the "assets" container
        bytesCopied(&H14) = 7
        Array.Copy(System.Text.Encoding.ASCII.GetBytes("assets"), 0, bytesCopied, &H15, 6)
        bytesCopied(&H1B) = 0
        bytesCopied(&H1C) = 0

        'compute the length of the "assets" container:
        'this is the length to the end of the "assets" container without the header and the length field itself
        Array.Copy(BitConverter.GetBytes(bytesCopied.Length - &H14), 0, bytesCopied, &H10, 4)

        'this is how much we've extracted
        Dim currentLength As Integer = bytesCopied.Length

        'add the final tags and containers (rawData below)
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

        'make room for the final tags
        ReDim Preserve bytesCopied(UBound(bytesCopied) + rawData.Length)

        'copy the last tags to the end
        Array.Copy(rawData, 0, bytesCopied, currentLength, rawData.Length)

        'write the kuid of the asset in the contents-table
        Array.Copy(assetKuid.GetKuidAsBytes(), 0, bytesCopied, currentLength + &H1D, 8)

        'compute the total length of the file and write it at the beginning
        Array.Copy(BitConverter.GetBytes(bytesCopied.Length - &H10), 0, bytesCopied, &HC, 4)

        'write the file to disk
        File.WriteAllBytes(exportPath & "\" & assetKuid.GetKuidAsString().Replace("<", "").Replace(":", " ").Replace(">", "") & ".cdp", bytesCopied)

        'free memory and reset array
        ReDim bytesCopied(15)
    End Sub

    ''' <summary>
    ''' Start searching for the required kuid in the file
    ''' </summary>
    ''' <param name="assetKuid">The kuid to search for</param>
    Sub CopyCDP(ByRef assetKuid As Kuid)
        Dim FileStr As BinaryReader
        Dim status As Boolean

        If Not My.Computer.FileSystem.FileExists(fileName) Then Exit Sub

        Try
            FileStr = New BinaryReader(IO.File.Open(fileName, FileMode.Open))

            FileStr.BaseStream.Seek(0, SeekOrigin.Begin)

            'Check file signature to make sure it is a chump file type
            Dim ExpectedSignature As Byte() = System.Text.Encoding.ASCII.GetBytes("ACS$")
            Dim FileSignature(ExpectedSignature.Length - 1) As Byte

            FileSignature = FileStr.ReadBytes(ExpectedSignature.Length)

            If Not CompareBytes(FileSignature, ExpectedSignature) Then
                Throw New Exception("File format not supported for " & fileName)
            End If

            'Copy the signature and the next 8 bytes (version bytes)
            Array.Copy(FileSignature, bytesCopied, 4)
            Array.Copy(FileStr.ReadBytes(8), 0, bytesCopied, 4, 8)

            'Get the chump file length
            Dim ChumpLength As UInteger = FileStr.ReadUInt32
            Dim CurrentFilePointer As Long = FileStr.BaseStream.Position 'here we are right now
            Dim Depth As Integer = 0 'we are at the top level of the file

            While Not FileStr.BaseStream.Position = CurrentFilePointer + ChumpLength
                status = CopySubTags(FileStr, Depth, "", assetKuid)
                If status = True Then Exit While 'we found the asset
            End While

            FileStr.Close()

            If status = False Then Throw New Exception("The asset " & assetKuid.GetKuidAsString() & " could not be found in the CDP!")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblProgress.Visible = False
        End Try

    End Sub

    ''' <summary>
    ''' Copy all the SubTags after finding the required kuid recursively.
    ''' Parse containers until found.
    ''' </summary>
    ''' <param name="FileStr">File stream</param>
    ''' <param name="Depth">Depth of the current container</param>
    ''' <param name="ParentContainer">Parent container name</param>
    ''' <param name="assetKuid">The kuid to search for</param>
    ''' <returns></returns>
    Function CopySubTags(ByRef FileStr As BinaryReader, ByVal Depth As Integer, ByVal ParentContainer As String, ByRef assetKuid As Kuid) As Boolean
        Dim tagLength As UInteger = FileStr.ReadUInt32()
        Dim tagNameSize As Byte = FileStr.ReadByte()
        Dim tagName As String

        Dim status As Boolean

        tagName = System.Text.Encoding.UTF8.GetString(FileStr.ReadBytes(tagNameSize - 1))
        FileStr.ReadByte() 'null terminator

        'we found the asset with the required kuid
        If tagName = assetKuid.GetKuidAsString() AndAlso ParentContainer = "assets" Then
            'resize the bytesCopied array to make room for our asset
            ReDim Preserve bytesCopied(UBound(bytesCopied) + tagLength + 13 + 4) '13 is the length of the "assets" tag

            FileStr.BaseStream.Seek(-tagNameSize - 5, SeekOrigin.Current) 'go back a bit to copy the tagname itself

            Array.Copy(FileStr.ReadBytes(tagLength + 4), 0, bytesCopied, 16 + 13, tagLength + 4) '16 bytes reserved for the file header; 4 bytes is the length field
            Return True
        End If

        'if kuid is not found yet, continue to parse containers recursively until found

        Dim tagType As Byte = FileStr.ReadByte()

        If tagType = 0 Then 'container

            Dim currentFilePointer As Long = FileStr.BaseStream.Position

            'We don't care of subcontainers that are not in the "assets" container, so skip them
            If ParentContainer <> "assets" AndAlso ParentContainer <> "" AndAlso ParentContainer <> assetKuid.GetKuidAsString() Then
                FileStr.BaseStream.Seek(tagLength - tagNameSize - 2, SeekOrigin.Current)
            End If

            While Not FileStr.BaseStream.Position = currentFilePointer + tagLength - tagNameSize - 2
                status = CopySubTags(FileStr, Depth + 1, tagName, assetKuid)
                If status = True Then Return True
            End While

        Else
            'Skip other tag types
            FileStr.BaseStream.Seek(tagLength - tagNameSize - 2, SeekOrigin.Current)
        End If

        'we didn't find the required kuid
        Return False
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeGrid()
        Dim files As String() = My.Application.CommandLineArgs.ToArray
        If files.Length > 0 Then
            lblProgress.Visible = True
            fileName = files(0)
            Application.DoEvents()
            ParseChump(fileName)
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
                lblProgress.Visible = True
                fileName = files(0)
                Application.DoEvents()
                ParseChump(fileName)
                lblProgress.Visible = False
            End If
        End If
    End Sub

    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        Dim files As String() = e.Data.GetData(DataFormats.FileDrop)
        If files.Length > 0 Then
            lblProgress.Visible = True
            fileName = files(0)
            Application.DoEvents()
            ParseChump(fileName)
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
            ExtractContent(New Kuid(gridKUIDs.SelectedRows(row).Cells(0).Value.ToString), Path.GetDirectoryName(OpenFileDialog1.FileName))
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
            ExtractContent(New Kuid(gridKUIDs.Rows(row).Cells(0).Value.ToString), Path.GetDirectoryName(OpenFileDialog1.FileName))
        Next
        lblProgress.Visible = False

        MessageBox.Show("Extracting assets to CDPs finished!", "Ready", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        My.Computer.Clipboard.SetText(kuidList)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        My.Computer.Clipboard.SetText(kuidNameList)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
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

End Class

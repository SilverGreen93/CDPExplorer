Imports System.ComponentModel
Imports System.IO

Public Class frmMain

    Dim fileName As String
    Dim kuidList As String
    Dim kuidNameList As String
    Dim totalAssets As Integer
    Dim lastIndex As Integer 'for search purposes (find box)
    Dim bytesCopied(15) As Byte 'for extracting assets from cdp
    Dim currentAsset As Asset
    Public findString As String

    Enum ProcessingType
        EXTRACT_ALL
        EXTRACT_SELECTED
    End Enum

    Sub ResetData()
        kuidList = ""
        kuidNameList = ""
        totalAssets = 0
        gridKUIDs.RowCount = 1
    End Sub

    Sub InitializeGrid()
        gridKUIDs.ColumnCount = 8
        gridKUIDs.Columns(0).Name = "KUID"
        gridKUIDs.Columns(1).Name = "Username"
        gridKUIDs.Columns(2).Name = "Kind"
        gridKUIDs.Columns(3).Name = "Class"
        gridKUIDs.Columns(4).Name = "Build"
        gridKUIDs.Columns(5).Name = "Region"
        gridKUIDs.Columns(6).Name = "Era"
        gridKUIDs.Columns(7).Name = "Description"

        gridKUIDs.Columns(0).ReadOnly = True
        gridKUIDs.Columns(1).ReadOnly = True
        gridKUIDs.Columns(2).ReadOnly = True
        gridKUIDs.Columns(3).ReadOnly = True
        gridKUIDs.Columns(4).ReadOnly = True
        gridKUIDs.Columns(5).ReadOnly = True
        gridKUIDs.Columns(6).ReadOnly = True
        gridKUIDs.Columns(7).ReadOnly = True
    End Sub

    Sub ResizeGrid()
        gridKUIDs.Width = Width - gridKUIDs.Left - 24
        gridKUIDs.Height = Height - gridKUIDs.Top - StatusStrip.Height - 50
    End Sub

    Sub SetTitle()
        Text = My.Application.Info.Title & " v" & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor
        If fileName <> "" Then
            Text &= " - " & fileName
        End If
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

        Try
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
            kuidList = kuidList.Remove(kuidList.Length - 1) 'remove the trailing comma
            gridKUIDs.Rows.RemoveAt(0) 'remove the first row, it is empty
            gridKUIDs.AutoResizeColumns()

            fileStr.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & "KUID: " & currentAsset.AssetKuid.GetKuidAsString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

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
                        'For some legacy assets the kuid can be stored as string in 2 different formats
                        If (tagString.IndexOf("kuid", StringComparison.OrdinalIgnoreCase) <> -1) Then
                            'case <KUID2:1234:12345:03>
                            currentAsset.AssetKuid.SetKuid(tagString)
                        Else
                            'case 1234:12345
                            currentAsset.AssetKuid.SetKuid("<kuid2:" & tagString & ">")
                        End If
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
            kuidList &= currentAsset.AssetKuid.GetKuidAsString() & ","
            kuidNameList &= currentAsset.AssetKuid.GetKuidAsString() & " " & currentAsset.Username & vbCrLf
            gridKUIDs.Rows.Add(New String() {currentAsset.AssetKuid.GetKuidAsString(),
                                            currentAsset.Username,
                                            currentAsset.Kind,
                                            currentAsset.CategoryClass,
                                            currentAsset.TrainzBuild,
                                            currentAsset.CategoryRegion,
                                            currentAsset.CategoryEra,
                                            currentAsset.Description})

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
        If CopyCDP(assetKuid) = False Then Exit Sub

        Try
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

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & "KUID: " & assetKuid.GetKuidAsString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        'free memory and reset array
        ReDim bytesCopied(15)
    End Sub

    ''' <summary>
    ''' Start searching for the required kuid in the file
    ''' </summary>
    ''' <param name="assetKuid">The kuid to search for</param>
    ''' <returns>True if kuid is found</returns>
    Function CopyCDP(ByRef assetKuid As Kuid) As Boolean
        Dim FileStr As BinaryReader
        Dim status As Boolean

        If Not My.Computer.FileSystem.FileExists(fileName) Then Return False

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
        End Try

        Return status
    End Function

    ''' <summary>
    ''' Copy all the SubTags after finding the required kuid recursively.
    ''' Parse containers until found.
    ''' </summary>
    ''' <param name="FileStr">File stream</param>
    ''' <param name="Depth">Depth of the current container</param>
    ''' <param name="ParentContainer">Parent container name</param>
    ''' <param name="assetKuid">The kuid to search for</param>
    ''' <returns>True if kuid is found</returns>
    Function CopySubTags(ByRef FileStr As BinaryReader, ByVal Depth As Integer, ByVal ParentContainer As String, ByRef assetKuid As Kuid) As Boolean
        Dim tagLength As UInteger = FileStr.ReadUInt32()
        Dim tagNameSize As Byte = FileStr.ReadByte()
        Dim tagName As String

        Dim status As Boolean

        tagName = System.Text.Encoding.UTF8.GetString(FileStr.ReadBytes(tagNameSize - 1))
        FileStr.ReadByte() 'null terminator

        'Check if we found the asset with the required kuid
        If tagName = assetKuid.GetKuidAsString() AndAlso ParentContainer = "assets" Then
            'resize the bytesCopied array to make room for our asset
            ReDim Preserve bytesCopied(UBound(bytesCopied) + tagLength + 13 + 4) '13 is the length of the "assets" tag

            FileStr.BaseStream.Seek(-tagNameSize - 5, SeekOrigin.Current) 'go back a bit to copy the tagname itself

            Array.Copy(FileStr.ReadBytes(tagLength + 4), 0, bytesCopied, 16 + 13, tagLength + 4) '16 bytes reserved for the file header; 4 bytes is the length field
            Return True
        End If

        'If kuid is not found yet, continue to parse containers recursively until found

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

        'We didn't find the required kuid
        Return False
    End Function

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetTitle()
        InitializeGrid()
        resizeGrid()
        Dim files As String() = My.Application.CommandLineArgs.ToArray
        If files.Length > 0 Then
            fileName = files(0)
            SetTitle()
            lblStatus.Text = "Processing..."
            Application.DoEvents()
            ParseChump(fileName)
            lblStatus.Text = "Ready."
        End If
    End Sub

    'a new instance of the application was started with new arguments
    Public Sub NewArgumentsReceived(args As String())
        If args.Length > 0 Then
            Dim files As String() = args.ToArray
            If files.Length > 0 Then
                fileName = files(0)
                SetTitle()
                lblStatus.Text = "Processing..."
                Application.DoEvents()
                ParseChump(fileName)
                lblStatus.Text = "Ready."
            End If
        End If
    End Sub

    Private Sub frmMain_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        Dim files As String() = e.Data.GetData(DataFormats.FileDrop)
        If files.Length > 0 Then
            fileName = files(0)
            SetTitle()
            lblStatus.Text = "Processing..."
            Application.DoEvents()
            ParseChump(fileName)
            lblStatus.Text = "Ready."
        End If
    End Sub

    Private Sub frmMain_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Public Sub FindText(ByVal str As String)
        Dim searchIndex = 0
        Dim found As Boolean = False
        findString = str

        If findString = "" Then
            frmFind.Show()
            Exit Sub
        End If

        gridKUIDs.ClearSelection()
        For Each row As DataGridViewRow In gridKUIDs.Rows
            For Each cell As DataGridViewCell In row.Cells
                If cell.Value = Nothing Then Continue For
                If CStr(cell.Value).Contains(findString) Then
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
            If lastIndex = searchIndex Then 'wrap around
                lastIndex = 0
                Beep()
            End If
        Else 'not found
            lastIndex = 0
            Beep()
        End If
    End Sub

    Private Sub CopyKUIDListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyKUIDListToolStripMenuItem.Click
        My.Computer.Clipboard.SetText(kuidList)
    End Sub

    Private Sub CopyAssetListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyAssetListToolStripMenuItem.Click
        My.Computer.Clipboard.SetText(kuidNameList)
    End Sub

    Private Sub ExtractAsCDPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExtractAsCDPToolStripMenuItem.Click
        If FolderBrowserDialog.ShowDialog() <> vbOK Then Exit Sub
        BackgroundWorker.RunWorkerAsync(ProcessingType.EXTRACT_SELECTED)
        frmProgress.ShowDialog()
    End Sub

    Private Sub ExtractAllAsCDPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExtractAllAsCDPToolStripMenuItem.Click
        If FolderBrowserDialog.ShowDialog() <> vbOK Then Exit Sub
        BackgroundWorker.RunWorkerAsync(ProcessingType.EXTRACT_ALL)
        frmProgress.ShowDialog()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        frmAbout.ShowDialog()
    End Sub

    Private Sub BackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker.DoWork
        Dim extractionMethod As ProcessingType = e.Argument

        If extractionMethod = ProcessingType.EXTRACT_ALL Then
            For row As Integer = 0 To gridKUIDs.Rows.Count - 1
                ExtractContent(New Kuid(gridKUIDs.Rows(row).Cells(0).Value.ToString), FolderBrowserDialog.SelectedPath)
                BackgroundWorker.ReportProgress(row / gridKUIDs.Rows.Count * 100)
                If BackgroundWorker.CancellationPending Then Exit For
            Next
        ElseIf extractionMethod = ProcessingType.EXTRACT_SELECTED Then
            For row As Integer = 0 To gridKUIDs.SelectedRows.Count - 1
                ExtractContent(New Kuid(gridKUIDs.SelectedRows(row).Cells(0).Value.ToString), FolderBrowserDialog.SelectedPath)
                BackgroundWorker.ReportProgress(row / gridKUIDs.SelectedRows.Count * 100)
                If BackgroundWorker.CancellationPending Then Exit For
            Next
        End If
    End Sub

    Private Sub BackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker.RunWorkerCompleted
        frmProgress.Close()
    End Sub

    Private Sub BackgroundWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker.ProgressChanged
        frmProgress.ProgressBar.Value = e.ProgressPercentage
    End Sub

    Sub CopySelectedAssetsList()
        Dim assetList As String = ""
        For row As Integer = 0 To gridKUIDs.SelectedRows.Count - 1
            assetList &= gridKUIDs.SelectedRows(row).Cells(0).Value.ToString & " " & gridKUIDs.SelectedRows(row).Cells(1).Value.ToString & vbCrLf
        Next
        My.Computer.Clipboard.SetText(assetList)
    End Sub

    Sub CopySelectedKuidsList()
        Dim assetList As String = ""
        For row As Integer = 0 To gridKUIDs.SelectedRows.Count - 1
            assetList &= gridKUIDs.SelectedRows(row).Cells(0).Value.ToString & ","
        Next
        assetList = assetList.Remove(assetList.Length - 1) 'remove the trailing comma
        My.Computer.Clipboard.SetText(assetList)
    End Sub

    Private Sub CopyAssetInfoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyAssetInfoToolStripMenuItem.Click
        CopySelectedAssetsList()
    End Sub

    Private Sub CopyAssetInfoToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CopyAssetInfoToolStripMenuItem1.Click
        CopySelectedAssetsList()
    End Sub

    Private Sub CopyKUIDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyKUIDToolStripMenuItem.Click
        CopySelectedKuidsList()
    End Sub

    Private Sub CopyKUIDToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CopyKUIDToolStripMenuItem1.Click
        CopySelectedKuidsList()
    End Sub

    Private Sub ExtractSelectedAsCDPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExtractSelectedAsCDPToolStripMenuItem.Click
        If FolderBrowserDialog.ShowDialog() <> vbOK Then Exit Sub
        BackgroundWorker.RunWorkerAsync(ProcessingType.EXTRACT_SELECTED)
        frmProgress.ShowDialog()
    End Sub

    Private Sub OpenCDPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenCDPToolStripMenuItem.Click
        OpenFileDialog.Title = "Open CDP"
        OpenFileDialog.Filter = "CDP Files (*.cdp)|*.cdp"
        OpenFileDialog.FileName = ""

        If OpenFileDialog.ShowDialog() <> vbOK Then Exit Sub

        fileName = OpenFileDialog.FileName
        SetTitle()
        lblStatus.Text = "Processing..."
        Application.DoEvents()
        ParseChump(fileName)
        lblStatus.Text = "Ready."
    End Sub

    Private Sub frmMain_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        ResizeGrid()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub FindToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FindToolStripMenuItem.Click
        frmFind.Show()
    End Sub

    Private Sub FindNextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FindNextToolStripMenuItem.Click
        FindText(findString)
    End Sub

    Sub ExportCSV()
        Dim fName As String

        SaveFileDialog.Filter = "CSV files (*.csv)|*.csv"
        SaveFileDialog.FileName = ""

        If SaveFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            fName = SaveFileDialog.FileName
        Else
            Exit Sub
        End If

        Dim i As Integer
        Dim j As Integer
        Dim cellvalue As String
        Dim rowLine As String = ""

        Try
            Dim objWriter As New StreamWriter(fName, False)

            'get header row
            For i = 0 To gridKUIDs.Columns.Count - 1
                rowLine &= gridKUIDs.Columns(i).Name & ","
            Next

            rowLine = rowLine.Remove(rowLine.Length - 1) 'remove the trailing comma
            objWriter.WriteLine(rowLine)
            rowLine = ""

            'get rest of cells
            For j = 0 To gridKUIDs.Rows.Count - 1
                For i = 0 To gridKUIDs.Columns.Count - 1
                    If Not TypeOf gridKUIDs.CurrentRow.Cells.Item(i).Value Is DBNull Then
                        cellvalue = gridKUIDs.Item(i, j).Value
                    Else
                        cellvalue = ""
                    End If
                    rowLine &= cellvalue & ","
                Next

                rowLine = rowLine.Remove(rowLine.Length - 1) 'remove the trailing comma
                objWriter.WriteLine(rowLine)
                rowLine = ""
            Next

            objWriter.Close()

        Catch ex As Exception
            MessageBox.Show("Error occured while writing to file. " + ex.Message(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ExportAsCSVToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportAsCSVToolStripMenuItem.Click
        ExportCSV()
    End Sub

End Class

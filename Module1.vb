Imports System.Windows.Forms

Module Module1
    Private Function MD5This(ByVal Filename As String) As String

        Dim fail As String = "NOT VALID"
        Try
            Dim MD5 = System.Security.Cryptography.MD5.Create
            Dim Hash As Byte()
            Dim sb As New System.Text.StringBuilder
            Try
                Using st As New IO.FileStream(Filename, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
                    Hash = MD5.ComputeHash(st)
                End Using
                For Each b In Hash
                    sb.Append(b.ToString("X2"))
                Next
            Catch ex As Exception
            End Try
            Return sb.ToString
        Catch ex As Exception
            Return fail.ToString
        End Try
    End Function


    Private Function doesDirExist(ByVal directory As String)
        If My.Computer.FileSystem.DirectoryExists(directory) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private md5log As String = "XXX,"
    Private filelog As String = "XXX,"
    Private logToFile As Boolean = True
    Private cmd5 As String = Nothing
    Private Function determiner(ByVal refmd5log, ByVal reffilelog, ByVal MD5toCheck, ByVal newFileName)
        'Console.WriteLine("[determiner] called with md5tocheck: " & MD5toCheck & " new file name: " & newFileName)
        Dim md5log2 = refmd5log.Split(",")
        Dim flog2 = reffilelog.Split(",")

        Dim alrexist As Boolean = False
        Dim dupemd5 As String = ""
        Dim dupepat As String = ""


        For i = 0 To md5log2.Length - 1
            If md5log2(i).Contains(MD5toCheck) Then
                dupemd5 = dupemd5 & "," & md5log2(i)
                dupepat = dupepat & ", " & flog2(i)
                alrexist = True

            Else

                '  Console.WriteLine(md5log2(i) & " does not match " & MD5toCheck & ", check rest")
            End If
        Next

        If alrexist = True Then
            Console.WriteLine("We have a match: " & newFileName & " with a hash of " & MD5toCheck & " is identical to " & dupemd5 & " located at " & dupepat)
            If logToFile = True Then
                Try
                    My.Computer.FileSystem.WriteAllText("log.txt", Environment.NewLine & "We have a match: " & newFileName & " with a hash of " & MD5toCheck & " is identical to " & dupemd5 & " located at " & dupepat, True)
                    Console.WriteLine("[i] The log has been updated.")
                Catch ex As Exception
                    Console.WriteLine("[X] Failure to write log: " & ex.Message.ToString)
                End Try

            Else

            End If




        Else
            '   Console.WriteLine("No match, add to list for " & newFileName & " with a hash of " & MD5toCheck)

            filelog = filelog & newFileName & ","
            md5log = md5log & MD5toCheck & ","
        End If


        'existing file





        'new file:
        'file is new
        'Console.WriteLine("[>] This is a new file. The current log for MD5Log:" & md5log)
        'Console.WriteLine("[>] This is a new file. The current log for FileLog:" & filelog)
        'Console.WriteLine("[i] Proposed new MD5Log:" & md5log & MD5toCheck & ",")
        'Console.WriteLine("[i] Proposed new FileLog:" & filelog & newFileName & ",")
        'Console.WriteLine("Enter to confirm")
        'Console.ReadLine()


    End Function






    Sub Main()
        Dim als As New FolderBrowserDialog
        Console.WriteLine("dff - duplicate file finder")
        Console.WriteLine("===========================")
        Console.WriteLine("")
        Console.WriteLine("By Cobs // danos.cloud // v0.0.4")
        Console.WriteLine("")
        Console.WriteLine("[>] Enter directory to scan")
        als.ShowDialog()
        Dim als2 As MsgBoxResult = MsgBox("Do you want to log locally to log.txt?", vbYesNoCancel)
        If als2 = MsgBoxResult.Yes Then
            logToFile = True
        Else
            logToFile = False
        End If
        Console.WriteLine("[i] Thanks. Beginning search. You may not see any activity on this window for a while.")
        For Each d In My.Computer.FileSystem.GetDirectories(als.SelectedPath)

            For Each f In My.Computer.FileSystem.GetFiles(d)
                '  Console.WriteLine("[m] Call determiner for " & f & " located in " & d)
                determiner(md5log, filelog, MD5This(f), f)







            Next

        Next






        For Each f In My.Computer.FileSystem.GetFiles(als.SelectedPath)
            determiner(md5log, filelog, MD5This(f), f)
        Next

        Console.WriteLine("Finished folder and file discovery. Enter twice to dump log...")
        Console.ReadLine()
        Console.ReadLine()
        Dim als4 = md5log.Split(",")
        Dim als5 = filelog.Split(",")
        For i = 0 To als4.Length - 1
            Console.WriteLine("Entry " & i & ": " & als4(i).ToString & " --> " & als5(i).ToString)
        Next

        Console.WriteLine("Finished folder and file discovery. Enter twice to quit...")
        Console.ReadLine()
        Console.ReadLine()



    End Sub





End Module

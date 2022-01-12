﻿Imports System.Threading.Tasks
Imports Certes
Imports Certes.Acme
Imports Certes.Acme.Resource
Imports System.IO

'https://github.com/fszlin/certes/blob/main/docs/APIv2.md
'https://docs.certes.app/APIv2.html

Public Class SSL
    Private ReadOnly context As AcmeContext
    Private order As IOrderContext

    Public Sub New()

        Dim Key = PemKey
        If Key.Length > 0 Then
            ' use an existing ACME account:
            'Load the saved account key
            Dim accountKey = KeyFactory.FromPem(Key)
            context = New AcmeContext(WellKnownServers.LetsEncryptStagingV2, accountKey)
        Else
            context = New AcmeContext(WellKnownServers.LetsEncryptStagingV2)
            Dim result = context.NewAccount("fred@outworldz.com", True)
            ' Save the account key for later use
            PemKey = context.AccountKey.ToPem()
        End If

        Dim accountInfo = MyAccountAsync()
        Dim orderUri = NewOrderAsync()

    End Sub

    Private Property PemKey As String
        Get
            Dim d As String = ""
            Using Reader As New StreamReader(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles/PemKey.key"))
                While Not Reader.EndOfStream
                    d += Reader.ReadLine
                End While
            End Using
            Return d
        End Get
        Set(value As String)
            Dim f = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles/PemKey.key")
            DeleteFile(f)
            Using file As New System.IO.StreamWriter(f, False)
                file.WriteLine(value)
            End Using
        End Set

    End Property

    Private Async Function MyAccountAsync() As Task(Of Account)

        Dim account = Await context.Account()
        Dim res = Await account.Resource()
        Return res

    End Function

    Private Async Function NewOrderAsync() As Task(Of Object)

        order = Await context.NewOrder({Settings.DNSName})

        ' get the Get the token and key authorization string
        Dim Authz = (Await order.Authorizations()).First()
        Dim httpChallenge = Await Authz.Http()
        Dim keyAuthz = httpChallenge.KeyAuthz
        Dim token = httpChallenge.Token

        'Save the key authorization String In a text file, And upload it to http://your.domain.name/.well-known/acme-challenge/<token>
        If Not SaveCert(keyAuthz, "Outworldzfiles/Apache/htdocs/.well-known/acme-challenge/", token) Then Return False

        'Ask the ACME server to validate our domain ownership
        Await httpChallenge.Validate()

        Dim attempts = 10
        Dim result = Await httpChallenge.Resource

        ' Invalid      The invalid status.
        ' Pending      The pending status.
        ' Processing   The processing status.
        ' Valid        The valid status

        If result.Status = ChallengeStatus.Invalid Then
            ' debug
            Dim uri = result.Url
            Logger("SSL", result.Error.Detail, "SSL")
            Return True
        End If

        While attempts > 0 And (result.Status = ChallengeStatus.Pending Or result.Status = ChallengeStatus.Processing)
            If result.Status <> ChallengeStatus.Valid Then

                Dim retry = httpChallenge.RetryAfter

                Sleep(1000)
                attempts -= 1
                result = Await httpChallenge.Resource()
            Else
                Exit While
            End If

        End While

        If attempts = 0 Then Return False

        If result.Status <> ChallengeStatus.Valid Then
            Return False
        End If

        ''' TO DO  its stuck here with an error, dangit
        'Download the certificate once validation is done
        Dim privateKey = KeyFactory.NewKey(KeyAlgorithm.ES256)
        Dim cert = Await order.Generate(New CsrInfo With {
            .CountryName = "US",
            .State = "Texas",
            .Locality = "Allen",
            .Organization = "Outworldz, LLC.",
            .OrganizationUnit = "Outworldz",
            .CommonName = Settings.DNSName
        }, privateKey)

        'Export full chain certification
        Dim certPem = cert.ToPem()

        'Download the certificate for a finalized order.
        Dim certChain = Await order.Download()

        SaveCert(privateKey.ToString, "Outworldzfiles/Apache/conf/ssl/", "private.key")
        SaveCert(certChain.ToString, "Outworldzfiles/Apache/conf/ssl/", "freessl.key")

        'Export PFX
        Dim pfxBuilder = cert.ToPfx(privateKey)
        Dim pfx = pfxBuilder.Build("my-cert", "abcd1234")

        Return True

    End Function

    ''' <summary>
    ''' 'Save the key authorization String In a text file, And upload it to http://your.domain.name/.well-known/acme-challenge/<token>
    ''' </summary>
    ''' <param name="keyAuthz"></param>
    ''' <param name="folder"></param>
    ''' <param name="token"></param>
    ''' <returns></returns>
    Private Function SaveCert(keyAuthz As String, folder As String, token As String) As Boolean

        Dim file As String = ""
        If Debugger.IsAttached Then
            folder = "Y:/Inetpub/Secondlife/.well-known/acme-challenge"
            file = IO.Path.Combine(folder, token)
            Debug.Print($"http://.well-known/acme-challenge/{token}")
        Else
            file = IO.Path.Combine(Settings.CurrentDirectory, token)
        End If

        DeleteFolder(folder)
        MakeFolder(folder)

        Try
            Dim utf8WithoutBom = New System.Text.UTF8Encoding(False)
            Dim f = My.Computer.FileSystem.OpenTextFileWriter(file, False, utf8WithoutBom)
            f.WriteLine(keyAuthz)
            f.Close()
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Return False
        End Try

        Return True
    End Function

End Class

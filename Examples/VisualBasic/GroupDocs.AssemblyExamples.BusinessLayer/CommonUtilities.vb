﻿
Imports GroupDocs.Assembly
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Reflection

Namespace GroupDocs.AssemblyExamples.BusinessLayer
    'ExStart:CommonUtilities
    Public Class CommonUtilities

        Public Const sourceFolderPath As String = "../../../../Data/Source/"
        Public Const destinationFolderPath As String = "../../../../Data/Destination/"
        Public Const dataSourcesFolderPath As String = "../../../../Data/Data Sources/"

        'ExStart:LicenseFilePath
        Public Const licensePath As String = "D:/Aspose Projects/License/GroupDocs.Total.lic"
        'ExEnd:LicenseFilePath

#Region "DocumentDirectories"

        'ExStart:DocumentDirectories
        ''' <summary>
        ''' Takes source file name as argument. 
        ''' </summary>
        ''' <param name="sourceFileName">Source file name</param>
        ''' <returns>Returns explicit path by combining source folder path and source file name.</returns>

        Public Shared Function GetSourceDocument(sourceFileName As String) As String
            Return Path.Combine(Path.GetFullPath(sourceFolderPath), sourceFileName)
        End Function
        ''' <summary>
        ''' Takes output file name as argument. 
        ''' </summary>
        ''' <param name="outputFileName">output file name</param>
        ''' <returns>Returns explicit path by combining destination folder path and output file name.</returns>
        Public Shared Function SetDestinationDocument(outputFileName As String) As String
            Return Path.Combine(Path.GetFullPath(destinationFolderPath), outputFileName)
        End Function


        ''' <summary>
        ''' Takes source file name as argument. 
        ''' </summary>
        ''' <param name="sourceFileName">Source file name</param>
        ''' <returns>Returns explicit path by combining data source folder path and source file name.</returns>
        Public Shared Function GetDataSourceDocument(sourceFileName As String) As String
            Return Path.Combine(Path.GetFullPath(dataSourcesFolderPath), sourceFileName)
        End Function


        'ExEnd:DocumentDirectories
#End Region

#Region "ProductLicense"
        'ExStart:ApplyLicense
        ''' <summary>
        ''' Set product's license
        ''' </summary>

        Public Shared Sub ApplyLicense()
            Dim lic As New License()
            lic.SetLicense(licensePath)
        End Sub
        'ExEnd:ApplyLicense

        'ExStart:metered licensing 
        ''' <summary>
        ''' provide metered licensing
        ''' </summary>
        Public Shared Sub MeteredLicensing()

            ' Set metered license public and private keys.
            Dim metered As New Metered()
            metered.SetMeteredKey("PublicKey", "PrivateKey")

            ' Ensure that the product is licensed.
            Dim license As New License()
            Debug.Assert(license.IsLicensed)
        End Sub
        'ExEnd:metered licensing

#End Region

    End Class
    Module module2
#Region "ToADOTable"
        'ExStart:ConvertToDataTable
        ''' <summary>
        ''' It takes delegate and varlist IEnumberable as parameter
        ''' </summary>
        ''' <typeparam name="T">Template</typeparam>
        ''' <param name="varlist">IEnumerable varlist</param>
        ''' <param name="fn">Delegate as parameter</param>
        ''' <returns>It returns DataTable</returns>
        <System.Runtime.CompilerServices.Extension>
        Public Function ToADOTable(Of T)(varlist As IEnumerable(Of T), fn As ConvertDataTable.CreateRowDelegate(Of T)) As DataTable
            Dim dtReturn As New DataTable()
            Dim oProps As PropertyInfo() = Nothing
            For Each rec As T In varlist
                If oProps Is Nothing Then
                    oProps = DirectCast(rec.[GetType](), Type).GetProperties()
                    For Each pi As PropertyInfo In oProps
                        Dim colType As Type = pi.PropertyType
                        If (colType.IsGenericType) AndAlso (colType.GetGenericTypeDefinition() = GetType(Nullable(Of ))) Then
                            colType = colType.GetGenericArguments()(0)
                        End If
                        dtReturn.Columns.Add(New DataColumn(pi.Name, colType))
                    Next
                End If
                Dim dr As DataRow = dtReturn.NewRow()
                For Each pi As PropertyInfo In oProps
                    dr(pi.Name) = If(pi.GetValue(rec, Nothing) Is Nothing, DBNull.Value, pi.GetValue(rec, Nothing))
                Next
                dtReturn.Rows.Add(dr)
            Next
            Return (dtReturn)
        End Function
#End Region
        Public NotInheritable Class ConvertDataTable
            Private Sub New()
            End Sub
            Public Delegate Function CreateRowDelegate(Of T)(t As T) As Object()
        End Class
        'ExEnd:ConvertToDataTable
    End Module

    Public NotInheritable Class FileUtil
	Private Sub New()
	End Sub
	Public Shared Function GetBytesAsBase64(path As String) As String
		Return Convert.ToBase64String(File.ReadAllBytes(path))
	End Function
End Class
    'ExEnd:CommonUtilities
End Namespace



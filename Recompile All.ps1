# Calculate the project root from the invocation.
$projectRoot = $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path)

# Turn off verification for 64 bit applications.
&"$projectRoot\Client Compiler\bin\Development\Client Compiler.exe" -ns "GammaFour.UnderWriter.ClientDataModel" -i "C:\Source\Under Writer\GammaFour.UnderWriter.ClientDataModel\DataModel.xsd" -out "C:\Source\Under Writer\GammaFour.UnderWriter.ClientDataModel\DataModel.Designer.cs"
&"$projectRoot\Server Compiler\bin\Development\Server Compiler.exe" -ns "GammaFour.UnderWriter.ServerDataModel" -i "C:\Source\Under Writer\GammaFour.UnderWriter.ServerDataModel\DataModel.xsd" -out "C:\Source\Under Writer\GammaFour.UnderWriter.ServerDataModel\DataModel.Designer.cs"
&"$projectRoot\Data Service Compiler\bin\Development\Data Service Compiler.exe" -ns "GammaFour.UnderWriter.ServerDataModel" -i "C:\Source\Under Writer\GammaFour.UnderWriter.DataService\DataModel.xsd" -out "C:\Source\Under Writer\GammaFour.UnderWriter.DataService\DataModel.Designer.cs"
&"$projectRoot\Import Service Compiler\bin\Development\Import Service Compiler.exe" -ns "GammaFour.UnderWriter.ServerDataModel" -i "C:\Source\Under Writer\GammaFour.UnderWriter.RestService\DataModel.xsd" -out "C:\Source\Under Writer\GammaFour.UnderWriter.RestService\DataModel.Designer.cs"
&"$projectRoot\Persistent Store Compiler\bin\Development\Persistent Store Compiler.exe" -ns "GammaFour.UnderWriter.ServerDataModel" -i "C:\Source\Under Writer\GammaFour.UnderWriter.RestService\DataModel.xsd" -out "C:\Source\Under Writer\GammaFour.UnderWriter.PersistentStore\DataModel.Designer.cs"
&"$projectRoot\Database Compiler\bin\Development\Database Compiler.exe" -i "C:\Source\Under Writer\Database\DataModel.xsd" -out "C:\Source\Under Writer\Database\DataModel.Designer.sql"
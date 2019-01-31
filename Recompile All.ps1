# Calculate the project root from the invocation.
$projectRoot = $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path)

# Compile all the XSD files.
&"$projectRoot\Server Compiler\bin\Development\Server Compiler.exe" -ns "GammaFour.UnderWriter.ServerDataModel" -i "C:\Source\under-writer\GammaFour.UnderWriter.ServerDataModel\DataModel.xsd" -out "C:\Source\under-writer\GammaFour.UnderWriter.ServerDataModel\DataModel.Designer.cs"
&"$projectRoot\Rest Api Compiler\bin\Development\Rest Api Compiler.exe" -ns "GammaFour.UnderWriter.ServerDataModel" -i "C:\Source\under-writer\GammaFour.UnderWriter.DataService\DataModel.xsd" -out "C:\Source\under-writer\GammaFour.UnderWriter.DataService\DataModel.Designer.cs"

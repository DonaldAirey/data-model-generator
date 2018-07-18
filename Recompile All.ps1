# Calculate the project root from the invocation.
$projectRoot = $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path)

# Turn off verification for 64 bit applications.
&"$projectRoot\Client Compiler\bin\Development\Client Compiler.exe" -ns "DarkBond.SubscriptionManager" -i "C:\Source\Subscription Manager\DarkBond.SubscriptionManager.ClientDataModel\DataModel.xsd" -out "C:\Source\Subscription Manager\DarkBond.SubscriptionManager.ClientDataModel\DataModel.Designer.cs"
&"$projectRoot\Server Compiler\bin\Development\Server Compiler.exe" -ns "DarkBond.SubscriptionManager" -i "C:\Source\Subscription Manager\DarkBond.SubscriptionManager.ServerDataModel\DataModel.xsd" -out "C:\Source\Subscription Manager\DarkBond.SubscriptionManager.ServerDataModel\DataModel.Designer.cs"
&"$projectRoot\Data Service Compiler\bin\Development\Data Service Compiler.exe" -ns "DarkBond.SubscriptionManager" -i "C:\Source\Subscription Manager\DarkBond.SubscriptionManager.DataService\DataModel.xsd" -out "C:\Source\Subscription Manager\DarkBond.SubscriptionManager.DataService\DataModel.Designer.cs"
&"$projectRoot\Import Service Compiler\bin\Development\Import Service Compiler.exe" -ns "DarkBond.SubscriptionManager" -i "C:\Source\Subscription Manager\DarkBond.SubscriptionManager.ImportService\DataModel.xsd" -out "C:\Source\Subscription Manager\DarkBond.SubscriptionManager.ImportService\DataModel.Designer.cs"
&"$projectRoot\Persistent Store Compiler\bin\Development\Persistent Store Compiler.exe" -ns "DarkBond.SubscriptionManager" -i "C:\Source\Subscription Manager\DarkBond.SubscriptionManager.ImportService\DataModel.xsd" -out "C:\Source\Subscription Manager\DarkBond.SubscriptionManager.PersistentStore\DataModel.Designer.cs"
&"$projectRoot\SQL Compiler\bin\Development\SQL Compiler.exe" -i "C:\Source\Subscription Manager\Database\DataModel.xsd" -out "C:\Source\Subscription Manager\Database\DataModel.Designer.sql"

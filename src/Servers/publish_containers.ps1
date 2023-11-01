az login --tenant christofle.com
az acr login -n christofle
$version=(minver -t v -v e -d preview)
cd ./Hexalith.Server.Dynamics365Finance
dotnet publish --os linux --arch x64 /t:PublishContainer -c Release -p ContainerRegistry="christofle.azurecr.io" -p ContainerImageTag=$version
cd ../Hexalith.Server.ExternalSystems
dotnet publish --os linux --arch x64 /t:PublishContainer -c Release -p ContainerRegistry="christofle.azurecr.io" -p ContainerImageTag=$version
cd ../Hexalith.Server.Parties
dotnet publish --os linux --arch x64 /t:PublishContainer -c Release -p ContainerRegistry="christofle.azurecr.io" -p ContainerImageTag=$version
pause

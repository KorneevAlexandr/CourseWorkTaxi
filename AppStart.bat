dotnet build src\Taxi.DAL\Taxi.DAL.csproj
dotnet build src\Taxi.BLL\Taxi.BLL.csproj

start /d src\Taxi.UI\ dotnet run

FOR /F "tokens=2" %%x IN (src\Taxi.UI\hostsettings.json) DO start "" %%x
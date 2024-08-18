param(
    $action,
    [string[]] $services = $null)

$availableServices = @( "registry", "sqlserver" );
$actions = @("up", "down", "validate");

$runState = $action -in $actions;

if( $null -eq $runState) {
    Write-Host("Invalid state: ${action}");

    exit;
}

if( $null -eq $services) {
    $services = $availableServices;
} else {
    $services = $services | ForEach-Object { $_.ToLower() };
}

Write-Host("Starting ${services}");

$validate = @( $services | Where-Object { $_ -in $availableServices });

if( $validate.Count -ne $services.Count) {
    Write-Host("Invalid service name(s): ${services}");
    Write-Host("Must be one of (empty for all): ${availableServices}");

    exit;
}

foreach ($service in $services) {
    if( Test-Path "configs/${service}.json") {
        $config = (Get-Content "configs/${service}.json" | Out-String | ConvertFrom-Json);
    } else {
        $config = @{
            composePath = "docker-compose.yml";
            dataPath = "data/${service}"
        };
    }

    switch ( $action) {
        "up" {
            if( !(Test-Path("${root}/$($config.dataPath)"))) {
                New-Item "${root}/$($config.dataPath)" -ItemType Directory | out-null
            }

            if( $null -ne $config.extraPaths) {
                foreach ($extraPath in $config.extraPaths) {
                    if( !(Test-Path("${root}/$($extraPath)"))) {
                        New-Item "${root}/$($extraPath)" -ItemType Directory | out-null
                    }
                }
            }

            "docker compose -f ./${service}/$($config.composePath) --env-file ./services.env up -d" | cmd
        }
        "down" {
            "docker stop $($config.service ?? $service)" | cmd

            "docker compose -f ./${service}/$($config.composePath) --env-file ./services.env down" | cmd
        }
        "validate" {
            Write-Host("Testing ${service} in ${root}/${service}");
            Write-Host("Config: $($config.composePath)");
        }
    }
}
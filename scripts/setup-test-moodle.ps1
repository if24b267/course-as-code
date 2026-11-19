<#
.SYNOPSIS
  Richtet die lokale Moodle-Testinstanz für Course as Code ein.
.DESCRIPTION
  Startet docker compose, wartet bis Moodle erreichbar ist, schaltet das asynchrone
  Backup/Restore aus und legt einen Dummy-Kurs an (Workaround für den Known Issue
  "erster Import in eine leere Instanz schlägt fehl").
  Alle Moodle-CLI-Schritte laufen als Benutzer daemon, damit keine root-Dateien in moodledata entstehen.
#>
param(
    [int]$TimeoutSeconds = 600,
    [string]$DummyShortName = "DUMMY"
)

$ErrorActionPreference = "Continue"
Set-Location (Join-Path $PSScriptRoot "..")

function Invoke-Native {
    param([string]$Description, [scriptblock]$Command)
    Write-Host "==> $Description"
    & $Command
    if ($LASTEXITCODE -ne 0) {
        throw "Fehlgeschlagen: $Description (Exit-Code $LASTEXITCODE)"
    }
}

# 1) Docker vorhanden?
docker version *> $null
if ($LASTEXITCODE -ne 0) { throw "Docker ist nicht erreichbar. Bitte Docker Desktop starten." }

# 2) Container starten
Invoke-Native "Container starten (docker compose up -d)" { docker compose up -d }

$container = (docker compose ps -q moodle).Trim()
if (-not $container) { throw "Moodle-Container wurde nicht gefunden." }

# 3) Warten, bis Moodle antwortet
Write-Host "==> Warte auf Moodle (max. $TimeoutSeconds s) ..."
$deadline = (Get-Date).AddSeconds($TimeoutSeconds)
$ready = $false
while ((Get-Date) -lt $deadline) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost" -UseBasicParsing -TimeoutSec 5
        if ($response.StatusCode -eq 200) { $ready = $true; break }
    } catch { }
    Write-Host "    Moodle startet noch ..."
    Start-Sleep -Seconds 10
}
if (-not $ready) { throw "Moodle war nach $TimeoutSeconds s nicht erreichbar." }

# 4) Sind die CLI-Skripte da?
Invoke-Native "Moodle-CLI prüfen" { docker exec $container test -f /bitnami/moodle/admin/cli/cfg.php }

# 5) Asynchrones Backup/Restore ausschalten
Invoke-Native "Asynchrones Backup/Restore ausschalten" {
    docker exec -u daemon $container php /bitnami/moodle/admin/cli/cfg.php --name=enableasyncbackup --set=0
}

# 6) Dummy-Kurs anlegen (falls es ihn schon gibt, kommt eine Meldung, das ist kein Fehler)
Write-Host "==> Dummy-Kurs '$DummyShortName' anlegen"
docker exec -u daemon $container php /bitnami/moodle/admin/tool/generator/cli/maketestcourse.php --shortname=$DummyShortName --size=XS --bypasscheck
if ($LASTEXITCODE -ne 0) {
    Write-Warning "Dummy-Kurs konnte nicht angelegt werden (existiert er schon?). Ausgabe oben prüfen."
}

# 7) Zusammenfassung
Write-Host ""
Write-Host "Fertig. Moodle läuft unter http://localhost (Login: user / bitnami)."
Write-Host "Container-Name für den Upload: $((docker inspect -f '{{.Name}}' $container).TrimStart('/'))"
Write-Host 'Tipp: $env:COURSEASCODE_MOODLE_CONTAINER = "<Container-Name>"'

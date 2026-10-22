# Creates a GitHub repository and pushes the complete local solution.
# Requires: Git, GitHub CLI (https://cli.github.com/), and `gh auth login`.
# Usage: .\publish-to-github.ps1 -Visibility Public
#        .\publish-to-github.ps1 -Visibility Private

param(
    [ValidateSet('Public', 'Private')]
    [Parameter(Mandatory = $true)]
    [string]$Visibility
)

$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot

if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    throw 'Git is required: https://git-scm.com/download/win'
}
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    throw 'GitHub CLI is required: https://cli.github.com/'
}

gh auth status
if ($LASTEXITCODE -ne 0) {
    throw 'Sign in first: gh auth login'
}

if (-not (Test-Path '.git')) {
    git init -b main
    if ($LASTEXITCODE -ne 0) { throw 'git init failed' }
}

git add .
if ($LASTEXITCODE -ne 0) { throw 'git add failed' }

git diff --cached --quiet
$hasStagedChanges = ($LASTEXITCODE -eq 1)
if ($hasStagedChanges) {
    git commit -m 'Initial Emergence Atlas experiments and research documentation'
    if ($LASTEXITCODE -ne 0) {
        throw 'git commit failed. Configure Git author identity with git config user.name and user.email.'
    }
}

$repo = 'Agmash29a/EmergenceAtlas'
$repoExists = $false
$null = gh repo view $repo --json name --jq '.name' 2>$null
if ($LASTEXITCODE -eq 0) { $repoExists = $true }

if (-not $repoExists) {
    $visibilityFlag = if ($Visibility -eq 'Public') { '--public' } else { '--private' }
    gh repo create $repo $visibilityFlag --source . --remote origin --push --description 'Object-oriented C# experiments and a research atlas of emergent properties'
    if ($LASTEXITCODE -ne 0) { throw 'Repository creation or initial push failed' }
}
else {
    $origin = git remote get-url origin 2>$null
    if ($LASTEXITCODE -ne 0) {
        git remote add origin "https://github.com/$repo.git"
        if ($LASTEXITCODE -ne 0) { throw 'git remote add failed' }
    }
    elseif ($origin -notmatch 'github\.com[:/]Agmash29a/EmergenceAtlas(\.git)?$') {
        throw "Existing origin is '$origin', refusing to push to the wrong repository"
    }
    git push -u origin HEAD:main
    if ($LASTEXITCODE -ne 0) { throw 'git push failed. Verify your permissions and existing branch state.' }
}

Write-Host "Published: https://github.com/$repo" -ForegroundColor Green

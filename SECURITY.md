# Security Policy

## Supported Branches
| Branch | Supported | Notes |
|--------|-----------|-------|
| main   | Yes       | Latest stable / public branch |
| develop| Yes       | Active development (may be unstable) |

## Reporting a Vulnerability
If you discover a vulnerability:
1. DO NOT open a public issue with exploit details.
2. Email: security@example.com (replace with project email) including:
   - Affected component(s)
   - Steps to reproduce / PoC
   - Impact assessment (confidentiality, integrity, availability)
   - Suggested remediation (if known)
3. You will receive an acknowledgment within 3 business days.
4. Coordinated disclosure: We'll work with you on a fix & release timeline.

## Disclosure Timeline Guidelines
| Severity | Target Fix SLA | Public Advisory |
|----------|----------------|-----------------|
| Critical | 7 days         | After patch available |
| High     | 14 days        | After patch available |
| Medium   | 30 days        | Batched release |
| Low      | 60 days        | Batched release |

## Secret Management
Real secrets must never be committed. Use one of:
- dotnet user-secrets (local dev)
- Environment variables (CI / container runtime)
- External secret store (e.g., Azure Key Vault) (future)

Files that MAY contain only placeholders:
- `configuration/deployment/services.env` (sanitized)
- `configuration/deployment/services.env.example` (template)

## Pre-Commit / Pre-Push Checklist (Abbreviated)
Run: `pwsh ./scripts/security-scan.ps1`
- No gitleaks findings (or approved allowlist rationale)
- No HIGH/CRITICAL dependency vulnerabilities
- No plaintext secrets in config/appsettings

## Dependency Security
Use pinned central versions in `Directory.Packages.props`.
Upgrade path:
1. Identify vulnerable package via `dotnet list package --vulnerable --include-transitive`
2. Bump version centrally
3. Test build & runtime behavior
4. Document changes in PR

## Logging & Telemetry
- Avoid logging tokens, claims, connection strings
- EF Core SQL logging should be disabled / reduced outside development
- OpenTelemetry exporters should use secure endpoints (TLS) when remote

## Future Enhancements
- CodeQL analysis
- SBOM generation + signing
- Container image scanning (Trivy)
- Infrastructure as Code scanning (if IaC added)

## Incident Response (High-Level)
1. Triage report & assign severity
2. Reproduce & confirm scope
3. Create private security branch for fix
4. Write regression test (if applicable)
5. Prepare advisory (CVE if impactful & public scope)
6. Release patch & merge
7. Post-mortem (root cause + preventive controls)

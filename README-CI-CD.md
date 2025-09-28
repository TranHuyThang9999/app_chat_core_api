# CI/CD Pipeline Documentation

## Overview
Dự án này sử dụng GitHub Actions để thực hiện CI/CD pipeline tự động với các tính năng:

- ✅ Automated testing
- ✅ Code quality checks
- ✅ Security scanning
- ✅ Docker build và push
- ✅ Automated deployment
- ✅ Health monitoring

## Workflows

### 1. Main CI/CD Pipeline (`ci-cd.yml`)
**Trigger:** Push to `main` hoặc `develop`, Pull Request to `main`

**Jobs:**
- **Test:** Build, restore dependencies, và chạy tests với PostgreSQL
- **Build-and-Push:** Build Docker image và push lên GitHub Container Registry
- **Deploy-Staging:** Deploy lên staging environment (khi push to `develop`)
- **Deploy-Production:** Deploy lên production environment (khi push to `main`)

### 2. Pull Request Checks (`pr-check.yml`)
**Trigger:** Pull Request to `main` hoặc `develop`

**Jobs:**
- **Code Quality:** Kiểm tra code formatting và analysis
- **Security Scan:** Quét lỗ hổng bảo mật với Trivy
- **Test Coverage:** Chạy tests và tạo coverage report

### 3. Docker Build và Test (`docker-build.yml`)
**Trigger:** Push to `main` hoặc `develop`, Pull Request to `main`

**Jobs:**
- **Docker Build Test:** Build Docker image và chạy integration tests
- **Vulnerability Scan:** Quét lỗ hổng trong Docker image

## Setup Instructions

### 1. GitHub Repository Settings

#### Secrets cần thiết:
```
GITHUB_TOKEN (tự động có sẵn)
```

#### Environments:
- `staging`: Cho deployment staging
- `production`: Cho deployment production (có thể thêm protection rules)

### 2. Branch Protection Rules
Khuyến nghị setup cho branch `main`:
- ✅ Require pull request reviews
- ✅ Require status checks to pass before merging
- ✅ Require branches to be up to date before merging
- ✅ Include administrators

### 3. Container Registry
Images sẽ được push lên GitHub Container Registry:
```
ghcr.io/[username]/[repository-name]
```

## Health Checks
API có health check endpoint tại `/health` để monitoring:
- Database connection (PostgreSQL)
- MinIO service status
- Application health

## Local Development

### Chạy với Docker Compose:
```bash
# Development
docker-compose up -d

# CI environment
docker-compose -f docker-compose.ci.yml up -d
```

### Build và test local:
```bash
# Restore dependencies
dotnet restore

# Build
dotnet build

# Run tests
dotnet test

# Check formatting
dotnet format --verify-no-changes
```

## Deployment

### Staging Deployment
- Tự động trigger khi push to `develop`
- Environment: `staging`
- URL: [Configure your staging URL]

### Production Deployment
- Tự động trigger khi push to `main`
- Environment: `production`
- URL: [Configure your production URL]
- Có thể thêm manual approval

## Monitoring và Alerts

### Health Checks
```bash
# Check application health
curl https://your-app-url/health
```

### Logs
- GitHub Actions logs
- Application logs trong container
- Database logs

## Troubleshooting

### Common Issues:

1. **Build fails:**
   - Check dependencies trong `.csproj`
   - Verify .NET version compatibility

2. **Tests fail:**
   - Check database connection
   - Verify test data setup

3. **Docker build fails:**
   - Check Dockerfile syntax
   - Verify base image availability

4. **Deployment fails:**
   - Check environment variables
   - Verify deployment target accessibility

### Debug Commands:
```bash
# Check workflow status
gh workflow list

# View workflow run
gh run view [run-id]

# Check container logs
docker logs [container-id]
```

## Security Best Practices

1. **Secrets Management:**
   - Không commit secrets vào code
   - Sử dụng GitHub Secrets cho sensitive data
   - Rotate secrets định kỳ

2. **Container Security:**
   - Scan images với Trivy
   - Use non-root user trong container
   - Keep base images updated

3. **Access Control:**
   - Branch protection rules
   - Required reviews
   - Environment protection rules

## Performance Optimization

1. **Build Cache:**
   - Docker layer caching
   - NuGet package caching
   - GitHub Actions cache

2. **Parallel Jobs:**
   - Tests chạy parallel với build
   - Multiple environment deployments

3. **Resource Limits:**
   - Configure appropriate timeouts
   - Set resource limits cho containers
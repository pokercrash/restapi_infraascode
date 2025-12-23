# Notes API – AWS Terraform & GitLab CI

## Overview

Delivers a **small, production-ready REST API** built with **ASP.NET Core** and deployed on **AWS** using **Terraform**.  
Infrastructure provisioning and deployment are automated via **GitLab CI** with:

- Security (least-privilege IAM, private networking, encryption)
- Observability (centralised logging, retention)
- Safe CI/CD practices (manual apply, no hardcoded secrets)
- Cost-safe defaults (serverless / managed services)

The system exposes a single CRUD resource: **`note api`**.

---

## Architecture

### High-Level Diagram
Client (HTTPS) → Application Load Balancer (HTTP) within a private VPC → ECS Fargate (ASP.NET Core API) → DynamoDB (Notes table)

---

## AWS Services & Justification

### Amazon ECS (Fargate)
- Serverless container runtime (no EC2 management)
- Cost-efficient for small workloads
- Can be integrated with IAM, ALB, and CloudWatch
- Task-level IAM roles for least-privilege access

### API Exposure – Application Load Balancer
- HTTPS and health checks
- Integrates with ECS
- Security groups restrict inbound traffic

### Storage – Amazon DynamoDB
- On-demand billing
- Simple CRUD workloads
- Encrypted at rest by default

### Networking – Amazon VPC
- Public subnets for ALB
- Private subnets for ECS tasks
- Containers are not directly internet-accessible

### Logging – Amazon CloudWatch
- Application logs streamed from ECS
- Log retention configured (14 days)

---

## Terraform Design

### Modular Structure

Terraform is split into modules:

## Module Descriptions
- **network**: Contains VPC, subnets, and internet gateway configurations.
- **compute**: Defines ECS clusters, ALBs, and task definitions.
- **storage**: Manages DynamoDB tables.
- **iam**: Implements IAM roles and least-privilege policies.
- **logging**: Setup CloudWatch log groups.

### Key Principles
- No hardcoded ARNs or credentials
- All inputs passed via variables

---

## Terraform State Management

Terraform uses **remote state** stored in:
- **S3** (encrypted)
- **DynamoDB** for state locking

This prevents concurrent applies and supports team workflows.

---

## Security

### IAM
- ECS task role scoped to a single DynamoDB table
- No wildcard permissions
- Execution role uses AWS-managed policy only

### Networking
- ECS tasks run in **private subnets**
- Only the ALB is publicly accessible
- Security groups restrict traffic flow

### Data Protection
- DynamoDB encrypted at rest
- Terraform state encrypted in S3
- No secrets committed to the repository

---

## Logging & Observability

### Application Logs
- ASP.NET Core logs sent to CloudWatch
- Log group: `/ecs/notes-api`

### Retention
- Log retention set to **14 days** via Terraform
- Prevents unbounded log storage costs

### Where to Find Logs
- AWS Console → CloudWatch → Log Groups → `/ecs/notes-api`

---

## CI/CD – GitLab Pipeline

### Pipeline Stages

1. **Validate**
   - `terraform fmt`
   - `terraform validate`

2. **Build**
   - Build ASP.NET Core Docker image
   - Push to AWS ECR

3. **Cleanup**
   - Clean items from previous runs to avoid conflicts

4. **Plan**
   - Run `terraform plan`
   - Store plan file as an artifact

5. **Deploy**
   - Manual `terraform apply`
   - No automatic applies on merge

### Manual Apply
- Stop accidental infrastructure changes
- Aligns with production change control
- Explicitly required by the assignment scope

---

## Required Setup (GitLab CI Variables/AWS service)

Configure in **GitLab → Settings → CI/CD → Variables**.

> Configure 'aws configure' default region to `ap-southeast-1` for IAM user with the below access key

### AWS Credentials

| Variable | Description |
|------|-----------|
| `AWS_ACCESS_KEY_ID` | AWS access key |
| `AWS_SECRET_ACCESS_KEY` | AWS secret key |
| `AWS_REGION` | `ap-southeast-1` |
| `AWS_DEFAULT_REGION` | `ap-southeast-1` |
| `VPC_CIDR` | `10.0.0.0/16` |
| `AWS_ACCOUNT_ID` | AWS account id |

> Credentials must have permissions for ECS, ECR, ALB, DynamoDB, IAM, CloudWatch, S3, and DynamoDB (terraform state locking).

### AWS DynamoDB & S3 for Terraform State

| Service | Description |
|------|-----------|
| `note-testbucket` | AWS S3 bucket |
| `terraform-locks` | AWS DynamoDb terraform locks table |

### Container Registry

GitLab automatically provides:
- `CI_REGISTRY`
- `CI_REGISTRY_USER`
- `CI_REGISTRY_PASSWORD`
- `CI_REGISTRY_IMAGE`

---

## Deployment Flow

1. Push code or open a merge request
2. GitLab CI runs:
   - Terraform validation
   - Docker image build
   - Terraform plan
3. Review the **plan artifact**
4. Manual approval triggers `terraform apply`
5. Infrastructure is updated

---

## Verifying the System

1. Run the pipeline in GitLab
2. Inspect the **terraform plan artifact**
3. Approve the **manual deploy job**
4. After apply completes:
   - Retrieve the ALB DNS name from Terraform output
   - Call the API endpoints
5. Verify logs in CloudWatch

---

## CRUD Resource

**Resource:** `note`

| Field | Type |
|----|----|
| id | string |
| title | string |
| body | string |
| created_at | ISO 8601 timestamp |

---

## Cost Considerations

- ECS Fargate with minimal CPU/memory
- DynamoDB on-demand billing
- No always-on EC2 instances
- Manual deploy prevents accidental resource creation

---

## Summary

Solution consist of:
- Production-ready infrastructure design
- Secure AWS deployment
- Safe and auditable CI/CD pipeline
- Clear separation of application and infrastructure concerns

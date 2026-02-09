---
id: tec-node-001-web-application-server
name: Web Application Server
type: node
layer: technology
relationships:
- type: realization
  target: app-comp-001-customer-portal
  description: Hosts customer portal application
- type: realization
  target: tec-sysw-001-application-runtime
  description: Includes application runtime environment
- type: realization
  target: tec-sysw-002-linux-operating-system
  description: Runs on Linux operating system
properties:
  owner: Infrastructure Team
  status: production
  criticality: high
  version: Ubuntu 22.04 LTS
  lifecycle-phase: operate
  cost: $500/month
  last-updated: '2026-01-10'
  legacy-id: tec-node-web-application-server-001
tags:
- infrastructure
- web-server
- production
---

# Web Application Server

Primary server infrastructure hosting customer-facing web applications.

## Description

This technology node represents the server infrastructure that hosts and executes our customer-facing web applications. It provides the runtime environment and resources necessary for application operation.

## Technical Specifications

- **Hardware**: Virtual machine with 8 vCPUs, 16GB RAM
- **Operating System**: Ubuntu 22.04 LTS Server
- **Web Server**: Nginx 1.24
- **Application Server**: Node.js 20 LTS
- **Location**: Primary data center (US-East)

## Configuration

- Load balanced across 3 instances
- Auto-scaling enabled (min: 2, max: 10)
- Deployed in high-availability configuration
- Regular security patches applied

## Monitoring

- CPU utilization: Target < 70%
- Memory usage: Target < 80%
- Response time: < 200ms
- Uptime: 99.9% SLA

## Security

- Firewall rules configured
- SSL/TLS encryption enabled
- Regular vulnerability scanning
- Intrusion detection system active

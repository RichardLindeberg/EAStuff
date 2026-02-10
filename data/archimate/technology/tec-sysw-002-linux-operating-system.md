---
id: tec-sysw-002-linux-operating-system
owner: Infrastructure Team
status: production
version: '1.0'
last_updated: '2026-02-03'
review_cycle: annual
next_review: '2027-02-03'
relationships:
- type: serving
  target: tec-node-001-web-application-server
  description: Provides OS for web application server
- type: serving
  target: tec-sysw-001-application-runtime
  description: Hosts application runtime environment
name: Linux Operating System
tags:
- operating-system
- linux
- infrastructure
- ubuntu
archimate:
  type: system-software
  layer: technology
  criticality: critical
extensions:
  properties:
    distribution: Ubuntu Server 22.04 LTS
    kernel-version: 5.15 LTS
    hardening: CIS Benchmark Level 1
---
# Linux Operating System

Enterprise-grade Linux operating system providing secure, stable foundation for banking infrastructure.

## Description

The Linux Operating System serves as the foundational platform for the bank's application infrastructure. Running Ubuntu Server 22.04 LTS (Jammy Jellyfish), it provides a secure, reliable, and high-performance environment for containerized workloads, databases, and application servers across Nordic data centers.

## Distribution

### Ubuntu Server 22.04 LTS
- **Long-Term Support**: 5 years standard support (until 2027)
- **Extended Security Maintenance**: Available until 2032
- **Kernel**: Linux 5.15 LTS (Hardware Enablement Stack)
- **Architecture**: x86_64 (AMD64), ARM64 support
- **Package Management**: APT/dpkg with universe/multiverse repositories

### Selection Rationale
- **Stability**: LTS releases provide stable platform
- **Security**: Regular security updates and patching
- **Cloud Native**: Excellent support for containers and Kubernetes
- **Community**: Large community and extensive documentation
- **Compliance**: Security certifications (Common Criteria, FIPS)

## System Configuration

### Base Installation
- **Minimal Install**: Server installation without desktop
- **Partitioning**: LVM for flexible storage management
  - `/` (root): 50GB
  - `/var`: 100GB (logs and containers)
  - `/tmp`: 10GB (noexec, nosuid)
  - `/home`: 20GB
  - Swap: 16GB
- **File Systems**: ext4 for root, XFS for data volumes
- **Boot**: UEFI with Secure Boot enabled

### Security Hardening
- **CIS Benchmark**: Level 1 server profile implementation
- **SELinux/AppArmor**: AppArmor mandatory access control
- **Firewall**: ufw (Uncomplicated Firewall) with strict rules
- **Audit**: auditd for system call auditing
- **File Integrity**: AIDE for file integrity monitoring

### Access Control
- **User Management**: Centralized via LDAP/Active Directory
- **SSH**: Key-based authentication only, no password auth
- **sudo**: Restricted sudo access with logging
- **PAM**: Pluggable Authentication Modules configuration
- **2FA**: Two-factor authentication for privileged access

## Kernel & Performance

### Kernel Configuration
- **Version**: 5.15 LTS (Long-term support branch)
- **HWE**: Hardware Enablement Stack for newer hardware
- **Real-Time**: Optional real-time kernel patches
- **Parameters**: Tuned for container workloads
  - `vm.swappiness=10`: Minimize swapping
  - `net.ipv4.tcp_fastopen=3`: TCP Fast Open
  - `fs.file-max=2097152`: High file descriptor limits

### Performance Tuning
- **CPU Governor**: Performance governor for critical workloads
- **NUMA**: NUMA awareness for multi-socket systems
- **Huge Pages**: Transparent huge pages for memory-intensive apps
- **I/O Scheduler**: mq-deadline for SSD/NVMe
- **Network Stack**: TCP BBR congestion control

## Package Management

### System Packages
- **Updates**: Automated security updates (unattended-upgrades)
- **Repositories**: 
  - Ubuntu main (supported)
  - Ubuntu universe (community)
  - Docker/Kubernetes official repos
  - Custom internal repository
- **Package Pinning**: Version pinning for critical packages
- **Dependency Management**: Careful dependency resolution

### Container Runtime
- **containerd**: Container runtime (CRI-compatible)
- **Docker**: Docker CE for development environments
- **Buildah/Podman**: Daemonless container tools
- **CRI-O**: Lightweight CRI runtime for Kubernetes

## Networking

### Network Configuration
- **Interfaces**: Bonded network interfaces (LACP)
- **IPv4/IPv6**: Dual-stack networking
- **DNS**: systemd-resolved with internal DNS servers
- **NTP**: chrony for time synchronization
- **mDNS**: Avahi disabled for security

### Network Security
- **Firewall**: iptables/nftables with default deny
- **Port Security**: Only necessary ports exposed
- **Rate Limiting**: Connection rate limits
- **DDoS Protection**: Kernel-level DDoS mitigation
- **Encrypted Communication**: TLS everywhere

## Storage Management

### Volume Management
- **LVM**: Logical Volume Manager for flexibility
- **RAID**: Software RAID 10 for data volumes
- **Snapshots**: LVM snapshots for backups
- **Thin Provisioning**: Efficient storage allocation
- **Monitoring**: Volume space monitoring and alerts

### File Systems
- **Root**: ext4 with journaling
- **Data Volumes**: XFS for large files and high performance
- **Temporary**: tmpfs for `/run` and `/tmp`
- **Mount Options**: `noatime`, `nodiratime` for performance

## Monitoring & Logging

### System Monitoring
- **Node Exporter**: Prometheus metrics exporter
- **System Metrics**: CPU, memory, disk, network
- **Process Monitoring**: Process resource usage
- **Performance**: sysstat (sar, iostat, mpstat)
- **Health Checks**: Automated health monitoring

### Logging
- **Journald**: systemd journal for system logs
- **Rsyslog**: Remote syslog forwarding
- **Log Rotation**: logrotate for log management
- **Audit Logs**: auditd comprehensive auditing
- **Retention**: 30 days local, 1 year centralized

## Security

### Patch Management
- **Automatic Updates**: Unattended-upgrades for security patches
- **Patch Window**: Monthly patching schedule
- **Testing**: Patches tested in dev/test before production
- **Emergency Patches**: Process for critical vulnerabilities
- **Kernel Updates**: Live patching with kpatch/kGraft

### Vulnerability Management
- **Scanning**: Weekly vulnerability scans (OpenVAS, Nessus)
- **CVE Monitoring**: Subscription to security bulletins
- **Compliance Scanning**: CIS benchmark validation
- **Remediation**: Automated remediation where possible

### Malware Protection
- **ClamAV**: Antivirus scanning (periodic)
- **Rootkit Detection**: rkhunter and chkrootkit
- **HIDS**: Host-based intrusion detection (OSSEC)
- **File Integrity**: AIDE for integrity checking

## Backup & Recovery

### System Backups
- **Full System**: Quarterly full system images
- **Incremental**: Daily incremental backups
- **Configuration**: Ansible/Git for configuration management
- **Recovery Testing**: Quarterly recovery drills
- **Offsite Storage**: Backups replicated to DR site

### Disaster Recovery
- **Recovery Time**: <2 hours for system restoration
- **Bare Metal**: Automated bare-metal provisioning
- **Documentation**: Detailed recovery procedures
- **Failover**: Automated failover to standby systems

## Compliance

### Regulatory Requirements
- **GDPR**: Data protection compliance
- **DORA**: Digital operational resilience
- **PCI DSS**: Payment card industry standards
- **ISO 27001**: Information security controls

### Certifications
- **Common Criteria**: EAL4+ certified
- **FIPS 140-2**: Cryptographic module validation
- **CIS Benchmark**: Level 1 compliant
- **STIGs**: Security Technical Implementation Guides

## Automation

### Configuration Management
- **Ansible**: Infrastructure as code
- **Playbooks**: Automated provisioning and configuration
- **Version Control**: GitOps for configuration
- **Idempotency**: Repeatable, consistent deployments

### Orchestration
- **Kubernetes**: Container orchestration
- **systemd**: Service management
- **Cron/systemd timers**: Scheduled tasks
- **Scripts**: Automated maintenance scripts

## Future Enhancements

### 2026 Roadmap
- Ubuntu 24.04 LTS migration planning
- eBPF-based monitoring and security
- Kernel live patching across all systems
- Enhanced FIPS mode compliance

### 2027 Vision
- Immutable infrastructure with CoreOS/Flatcar
- Confidential computing with SEV/TDX
- Next-gen file systems (Btrfs, ZFS)
- Unikernel exploration for microservices

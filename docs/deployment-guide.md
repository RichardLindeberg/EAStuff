# Deployment Guide

## Overview

The `output/` folder contains a complete, self-contained static website that can be deployed anywhere. No server-side processing, databases, or build steps required!

## Quick Deployment

### 1. Generate the Website

```bash
python scripts/generators/generate_website.py
```

This creates everything in the `output/` folder.

### 2. Deploy

Simply upload the entire `output/` folder contents to your hosting provider.

## Deployment Options

### GitHub Pages (Free)

**Setup:**
1. Create a GitHub repository (if you don't have one)
2. Generate the website: `python scripts/generators/generate_website.py`
3. Commit and push the `output/` folder contents:
   ```bash
   cd output
   git init
   git add .
   git commit -m "Deploy EA documentation"
   git remote add origin https://github.com/username/repo.git
   git push -u origin main
   ```
4. In GitHub repo settings:
   - Go to Settings → Pages
   - Set source to "Deploy from a branch"
   - Select the `main` branch and `/ (root)` folder
   - Save

**Your site will be live at:** `https://username.github.io/repo/`

**Alternative (using gh-pages branch):**
```bash
cd output
git init
git checkout -b gh-pages
git add .
git commit -m "Deploy EA documentation"
git remote add origin https://github.com/username/repo.git
git push -f origin gh-pages
```

### Netlify (Free)

**Drag & Drop:**
1. Go to [netlify.com](https://www.netlify.com)
2. Sign in or create account
3. Drag the `output/` folder to the Netlify dashboard
4. Done! Your site is live

**Command Line:**
```bash
npm install -g netlify-cli
cd output
netlify deploy --prod
```

### Vercel (Free)

```bash
npm install -g vercel
cd output
vercel --prod
```

### Azure Static Web Apps (Free Tier Available)

```bash
# Install Azure CLI
az login

# Deploy
cd output
az staticwebapp create \
  --name my-ea-docs \
  --resource-group my-resource-group \
  --location eastus2 \
  --source .
```

### AWS S3 + CloudFront (Paid)

```bash
# Install AWS CLI and configure
aws configure

# Create S3 bucket
aws s3 mb s3://my-ea-documentation

# Upload files
cd output
aws s3 sync . s3://my-ea-documentation --acl public-read

# Enable static website hosting
aws s3 website s3://my-ea-documentation --index-document index.html
```

Your site: `http://my-ea-documentation.s3-website-us-east-1.amazonaws.com`

### Traditional Web Server

**Apache/Nginx:**
```bash
# SCP/SFTP the files
cd output
scp -r * user@yourserver.com:/var/www/html/ea-docs/

# Or use rsync
rsync -av . user@yourserver.com:/var/www/html/ea-docs/
```

**Server Configuration:**
No special configuration needed! Just serve the files as static HTML.

## Continuous Deployment

### GitHub Actions (Auto-deploy on commit)

Create `.github/workflows/deploy.yml`:

```yaml
name: Deploy EA Documentation

on:
  push:
    branches: [ main ]
    paths:
      - 'elements/**'
      - 'schemas/**'
      - 'scripts/**'

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Set up Python
        uses: actions/setup-python@v4
        with:
          python-version: '3.11'
      
      - name: Install dependencies
        run: pip install -r requirements.txt
      
      - name: Generate website
        run: python scripts/generators/generate_website.py
      
      - name: Deploy to GitHub Pages
        uses: peaceiris/actions-gh-pages@v3
        with:
          github_token: ${{ secrets.GITHUB_TOKEN }}
          publish_dir: ./output
          cname: ea.yourdomain.com  # Optional: custom domain
```

### GitLab CI/CD

Create `.gitlab-ci.yml`:

```yaml
pages:
  image: python:3.11
  stage: deploy
  script:
    - pip install -r requirements.txt
    - python scripts/generators/generate_website.py
    - mv output public
  artifacts:
    paths:
      - public
  only:
    - main
```

## Custom Domain

### GitHub Pages
1. Add a file `CNAME` in output folder with your domain:
   ```
   ea.yourdomain.com
   ```
2. Configure DNS:
   - Add CNAME record pointing to `username.github.io`

### Netlify
1. In Netlify dashboard: Site settings → Domain management
2. Add custom domain
3. Follow DNS configuration instructions

## Security

### HTTPS
- **GitHub Pages**: Automatic HTTPS
- **Netlify/Vercel**: Automatic HTTPS
- **Custom server**: Use Let's Encrypt for free HTTPS

### Access Control

For internal/private documentation:

**Option 1: Password Protection (Netlify)**
- Netlify Pro plan includes password protection
- Settings → Site settings → Access control

**Option 2: HTTP Basic Auth (Server)**
```apache
# .htaccess for Apache
AuthType Basic
AuthName "EA Documentation"
AuthUserFile /path/to/.htpasswd
Require valid-user
```

**Option 3: Deploy to Internal Network**
- Upload to internal web server
- Access via VPN only

## Performance Optimization

### Compress Files
```bash
# Gzip compression (most servers do this automatically)
find output -type f -name "*.html" -exec gzip -k {} \;
find output -type f -name "*.js" -exec gzip -k {} \;
```

### CDN Integration
- Netlify/Vercel provide CDN automatically
- For CloudFront: Create distribution pointing to S3 bucket

## Updating the Site

### Manual Updates
```bash
# 1. Update element markdown files
vim elements/application/new-component.md

# 2. Regenerate website
python scripts/generators/generate_website.py

# 3. Deploy updated files
cd output
git add .
git commit -m "Update architecture"
git push
```

### Scheduled Updates
Use cron or Task Scheduler to regenerate regularly:
```bash
# Linux cron: Regenerate daily at 2am
0 2 * * * cd /path/to/EA\ Stuff && python scripts/generators/generate_website.py
```

## Monitoring

### Check Deployment
After deploying, verify:
- ✓ Index page loads: `https://yoursite.com/`
- ✓ Diagrams render: Check full-architecture-mermaid.html
- ✓ Element pages work: Click elements in diagram
- ✓ Navigation works: Back buttons, related element links
- ✓ All layers accessible: Test each layer diagram link

### Analytics
Add Google Analytics by editing `index.html`:
```html
<!-- Add before </head> -->
<script async src="https://www.googletagmanager.com/gtag/js?id=GA_MEASUREMENT_ID"></script>
<script>
  window.dataLayer = window.dataLayer || [];
  function gtag(){dataLayer.push(arguments);}
  gtag('js', new Date());
  gtag('config', 'GA_MEASUREMENT_ID');
</script>
```

## Troubleshooting

### Links Don't Work
- Check file paths are relative
- Ensure all files were uploaded
- Verify case sensitivity (Unix systems)

### Diagrams Don't Render
- Check browser console for errors
- Ensure CDN access (Mermaid.js from CDN)
- Try different browser

### 404 Errors
- Check that `index.html` is in the root
- Verify server is configured for static sites
- Check `.htaccess` or server config

## Best Practices

1. **Version Control**: Keep generated site in git (separate from source)
2. **Automation**: Use CI/CD for automatic updates
3. **Testing**: Test locally before deploying
4. **Backup**: Keep deployment credentials secure
5. **Documentation**: Document custom deployment steps

## Example Deployment Scripts

### deploy.sh (Generic)
```bash
#!/bin/bash
echo "Generating website..."
python scripts/generators/generate_website.py

echo "Deploying to server..."
rsync -av --delete output/ user@server:/var/www/ea-docs/

echo "✅ Deployment complete!"
echo "🌐 https://ea-docs.yourcompany.com"
```

### deploy.ps1 (Windows)
```powershell
Write-Host "Generating website..."
python scripts/generators/generate_website.py

Write-Host "Deploying to server..."
# Use your preferred deployment method
# Example: WinSCP, Azure CLI, etc.

Write-Host "✅ Deployment complete!"
```

## Support

For deployment issues:
1. Check hosting provider documentation
2. Verify all files are uploaded
3. Test locally first: Open `output/index.html`
4. Check browser console for errors

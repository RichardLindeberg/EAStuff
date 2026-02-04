function reloadFile(filePath) {
    const encodedPath = encodeURIComponent(filePath);
    fetch(`/api/validation/revalidate/${encodedPath}`, { method: 'POST' })
        .then(r => r.json())
        .then(data => {
            alert(`File reloaded: ${data.errorCount} errors found`);
            location.reload();
        })
        .catch(err => alert(`Error: ${err.message}`));
}

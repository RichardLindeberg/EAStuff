document.addEventListener('DOMContentLoaded', () => {
    if (window.mermaid) {
        window.mermaid.initialize({
            startOnLoad: true,
            theme: 'default',
            securityLevel: 'loose',
            flowchart: { htmlLabels: true }
        });
        window.mermaid.init(undefined, document.querySelectorAll('.mermaid'));
    }

    let panZoomInstance = null;

    function initPanZoom() {
        const svg = document.querySelector('.mermaid svg');
        if (!svg) {
            setTimeout(initPanZoom, 100);
            return;
        }

        if (panZoomInstance) {
            panZoomInstance.destroy();
            panZoomInstance = null;
        }

        if (!svg.getAttribute('viewBox')) {
            try {
                const bbox = svg.getBBox();
                svg.setAttribute('viewBox', `${bbox.x} ${bbox.y} ${bbox.width} ${bbox.height}`);
            } catch (e) {
                console.warn('Could not set viewBox:', e);
            }
        }

        svg.removeAttribute('height');
        svg.removeAttribute('width');
        svg.style.width = '100%';
        svg.style.height = '100%';

        try {
            panZoomInstance = svgPanZoom(svg, {
                zoomEnabled: true,
                controlIconsEnabled: false,
                fit: true,
                center: true,
                minZoom: 0.1,
                maxZoom: 20,
                zoomScaleSensitivity: 0.3
            });
        } catch (e) {
            console.error('Error initializing pan-zoom:', e);
        }
    }

    setTimeout(initPanZoom, 500);

    document.getElementById('zoom-in')?.addEventListener('click', () => {
        if (panZoomInstance) panZoomInstance.zoomIn();
    });

    document.getElementById('zoom-out')?.addEventListener('click', () => {
        if (panZoomInstance) panZoomInstance.zoomOut();
    });

    document.getElementById('zoom-reset')?.addEventListener('click', () => {
        if (panZoomInstance) {
            panZoomInstance.reset();
            panZoomInstance.fit();
            panZoomInstance.center();
        }
    });
});

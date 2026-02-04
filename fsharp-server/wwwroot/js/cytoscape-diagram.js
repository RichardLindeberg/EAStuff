document.addEventListener('DOMContentLoaded', () => {
    const dataElement = document.getElementById('graph-data');
    if (!dataElement) {
        return;
    }

    let graphData = { nodes: [], edges: [] };
    try {
        graphData = JSON.parse(dataElement.textContent || '{}');
    } catch (err) {
        console.error('Failed to parse graph data', err);
    }

    const cy = cytoscape({
        container: document.getElementById('cy'),
        elements: {
            nodes: graphData.nodes,
            edges: graphData.edges
        },
        style: [
            {
                selector: 'node',
                style: {
                    'label': 'data(label)',
                    'text-valign': 'center',
                    'text-halign': 'center',
                    'text-wrap': 'wrap',
                    'text-max-width': '90px',
                    'font-size': '10px',
                    'font-weight': 'normal',
                    'font-family': 'Segoe UI, Tahoma, Geneva, Verdana, sans-serif',
                    'background-color': 'data(color)',
                    'background-image': 'none',
                    'border-width': 2,
                    'border-color': '#333',
                    'shape': 'data(shape)',
                    'width': 110,
                    'height': 80,
                    'padding': '4px',
                    'text-margin-y': 0,
                    'text-margin-x': 0
                }
            },
            {
                selector: 'node:selected',
                style: {
                    'border-width': 3,
                    'border-color': '#0066cc',
                    'overlay-opacity': 0.2,
                    'overlay-color': '#0066cc'
                }
            },
            {
                selector: 'edge',
                style: {
                    'width': 'data(lineWidth)',
                    'line-color': 'data(color)',
                    'target-arrow-color': 'data(color)',
                    'target-arrow-shape': 'data(arrowType)',
                    'curve-style': 'bezier',
                    'label': 'data(label)',
                    'font-size': '9px',
                    'text-rotation': 'autorotate',
                    'text-margin-y': -10,
                    'line-style': 'data(lineStyle)',
                    'line-dash-pattern': [5, 5],
                    'arrow-scale': 1.0
                }
            },
            {
                selector: 'edge:selected',
                style: {
                    'line-color': '#0066cc',
                    'target-arrow-color': '#0066cc',
                    'width': 3
                }
            },
            {
                selector: 'node.badge-label',
                style: {
                    'font-size': '11px',
                    'line-height': 1.3
                }
            }
        ],
        layout: {
            name: 'dagre',
            rankDir: 'TB',
            nodeSep: 40,
            rankSep: 60,
            animate: true,
            animationDuration: 500
        },
        minZoom: 0.2,
        maxZoom: 3,
        wheelSensitivity: 0.2
    });

    cy.on('tap', 'node', function (evt) {
        const nodeId = evt.target.id();
        window.location.href = '/elements/' + nodeId;
    });

    document.getElementById('fitView')?.addEventListener('click', () => {
        cy.fit(null, 50);
    });

    document.getElementById('zoomIn')?.addEventListener('click', () => {
        cy.zoom(cy.zoom() * 1.2);
    });

    document.getElementById('zoomOut')?.addEventListener('click', () => {
        cy.zoom(cy.zoom() * 0.8);
    });

    document.getElementById('exportPNG')?.addEventListener('click', () => {
        const png = cy.png({ full: true, scale: 2 });
        const link = document.createElement('a');
        link.download = 'diagram.png';
        link.href = png;
        link.click();
    });

    const enableSave = document.body?.dataset.enableSave === 'true';
    if (enableSave) {
        const savePositions = () => {
            const positions = {};
            cy.nodes().forEach(node => {
                positions[node.id()] = node.position();
            });
            localStorage.setItem('cytoscape_positions_' + document.title, JSON.stringify(positions));
        };

        const savedPositions = localStorage.getItem('cytoscape_positions_' + document.title);
        if (savedPositions) {
            const positions = JSON.parse(savedPositions);
            cy.nodes().forEach(node => {
                if (positions[node.id()]) {
                    node.position(positions[node.id()]);
                }
            });
        }

        cy.on('position', 'node', _.debounce(savePositions, 500));

        document.getElementById('resetLayout')?.addEventListener('click', () => {
            localStorage.removeItem('cytoscape_positions_' + document.title);
            cy.layout({ name: 'dagre', rankDir: 'TB', nodeSep: 80, rankSep: 100 }).run();
        });
    }

    cy.on('mouseover', 'node', function (evt) {
        const node = evt.target;
        const connectedEdges = node.connectedEdges();
        const connectedNodes = connectedEdges.connectedNodes();

        cy.elements().style('opacity', 0.3);
        node.style('opacity', 1);
        connectedNodes.style('opacity', 1);
        connectedEdges.style('opacity', 1);
    });

    cy.on('mouseout', 'node', function () {
        cy.elements().style('opacity', 1);
    });
});

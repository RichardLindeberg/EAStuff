document.addEventListener('DOMContentLoaded', () => {
    // Check if graphData is already defined in the page (from wrapCytoscapeHtml)
    if (typeof graphData === 'undefined') {
        console.error('graphData is not defined');
        return;
    }

    if (!graphData || !graphData.nodes || !graphData.edges) {
        console.error('Invalid graphData structure', graphData);
        return;
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

    const menu = document.getElementById('cy-context-menu');
    const menuContainer = cy.container();
    let contextNode = null;

    const hideMenu = () => {
        if (menu) {
            menu.style.display = 'none';
        }
        contextNode = null;
    };

    const getGroupVisibility = (node) => {
        if (node.hasClass('arch-node') || node.hasClass('arch-edge')) {
            return document.getElementById('toggle-architecture')?.checked ?? true;
        }
        if (node.hasClass('governance-node') || node.hasClass('governance-edge')) {
            return document.getElementById('toggle-governance')?.checked ?? true;
        }
        return true;
    };

    const hideElement = (element) => {
        element.addClass('hidden-by-user');
        element.style('display', 'none');
    };

    const showElement = (element) => {
        if (!getGroupVisibility(element)) {
            return;
        }
        element.removeClass('hidden-by-user');
        element.style('display', 'element');
    };

    const showContextMenu = (evt) => {
        if (!menu || !menuContainer) {
            return;
        }

        const rendered = evt.renderedPosition || evt.position;
        const containerRect = menuContainer.getBoundingClientRect();

        menu.style.display = 'block';
        menu.style.visibility = 'hidden';

        const menuWidth = menu.offsetWidth;
        const menuHeight = menu.offsetHeight;
        const maxLeft = containerRect.width - menuWidth - 8;
        const maxTop = containerRect.height - menuHeight - 8;
        const left = Math.max(8, Math.min(rendered.x, maxLeft));
        const top = Math.max(8, Math.min(rendered.y, maxTop));

        menu.style.left = `${containerRect.left + left}px`;
        menu.style.top = `${containerRect.top + top}px`;
        menu.style.visibility = 'visible';
    };

    const applyGroupVisibility = (element) => {
        if (element.hasClass('hidden-by-user')) {
            element.style('display', 'none');
            return;
        }
        element.style('display', getGroupVisibility(element) ? 'element' : 'none');
    };

    const mergeGraphData = (graph) => {
        if (!graph || !graph.nodes || !graph.edges) {
            return;
        }

        const added = [];

        graph.nodes.forEach(node => {
            if (cy.getElementById(node.data.id).length === 0) {
                added.push(node);
            }
        });

        graph.edges.forEach(edge => {
            if (cy.getElementById(edge.data.id).length === 0) {
                added.push(edge);
            }
        });

        if (added.length > 0) {
            cy.add(added);
            added.forEach(item => {
                const element = cy.getElementById(item.data.id);
                if (element.isNode && element.isNode()) {
                    applyGroupVisibility(element);
                }
                if (element.isEdge && element.isEdge()) {
                    applyGroupVisibility(element);
                }
            });
        }
    };

    cy.on('tap', 'node', function (evt) {
        const nodeId = evt.target.id();
        const kind = evt.target.data('kind');
        if (kind === 'governance') {
            const slug = evt.target.data('slug');
            window.location.href = '/governance/' + slug;
        } else {
            window.location.href = '/elements/' + nodeId;
        }
    });

    cy.on('cxttap', 'node', function (evt) {
        contextNode = evt.target;
        showContextMenu(evt);
    });

    cy.on('cxttap', function (evt) {
        if (evt.target === cy) {
            hideMenu();
        }
    });

    cy.on('tap', function () {
        hideMenu();
    });

    cy.on('pan zoom', function () {
        hideMenu();
    });

    if (menuContainer) {
        menuContainer.addEventListener('contextmenu', (event) => {
            event.preventDefault();
        });
    }

    if (menu) {
        menu.addEventListener('click', (event) => {
            const action = event.target?.dataset?.action;
            if (!action || !contextNode) {
                return;
            }

            if (action === 'remove') {
                hideElement(contextNode);
                contextNode.connectedEdges().forEach(edge => hideElement(edge));
                hideMenu();
                return;
            }

            if (action === 'expand') {
                const nodeId = contextNode.id();
                fetch(`/api/diagrams/expand/${encodeURIComponent(nodeId)}`)
                    .then(response => response.ok ? response.json() : null)
                    .then(data => {
                        if (data) {
                            mergeGraphData(data);
                        }
                        const neighbors = contextNode.neighborhood('node');
                        neighbors.forEach(node => {
                            if (node.hasClass('hidden-by-user')) {
                                showElement(node);
                            }
                        });

                        const edges = contextNode.connectedEdges();
                        edges.forEach(edge => {
                            if (!edge.hasClass('hidden-by-user')) {
                                return;
                            }

                            const source = edge.source();
                            const target = edge.target();
                            if (source.style('display') !== 'none' && target.style('display') !== 'none') {
                                showElement(edge);
                            }
                        });
                    })
                    .finally(() => {
                        hideMenu();
                    });
            }
        });
    }

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

    const toggleElements = (selector, isVisible) => {
        const elements = cy.elements(selector);
        if (!isVisible) {
            elements.style('display', 'none');
            return;
        }

        elements
            .filter(element => !element.hasClass('hidden-by-user'))
            .style('display', 'element');
    };

    const archToggle = document.getElementById('toggle-architecture');
    const govToggle = document.getElementById('toggle-governance');

    if (archToggle) {
        archToggle.addEventListener('change', (e) => {
            toggleElements('.arch-node, .arch-edge', e.target.checked);
        });
    }

    if (govToggle) {
        govToggle.addEventListener('change', (e) => {
            toggleElements('.governance-node, .governance-edge', e.target.checked);
        });
    }
});

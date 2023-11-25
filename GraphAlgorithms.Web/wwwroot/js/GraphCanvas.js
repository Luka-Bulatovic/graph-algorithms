var GraphCanvas = new function () {
    this.Initialize = function (viewDataObj) {
        $('body').on('click', function () { viewDataObj.contextMenu.removeClass('visible'); });

        // Context Menu Item click
        viewDataObj.contextMenu.find('.item').on('click', function (e) {
            GraphCanvas.onContextMenuItemClick(viewDataObj, $(e.target));
        });

        // Opening Context Menu
        viewDataObj.graphContainer.contextmenu(function (event) {
            event.preventDefault();
            GraphCanvas.openContextMenu(viewDataObj, event.clientY, event.clientX);
            return false;
        });

        // Save Button click
        viewDataObj.btnSave.on('click', function (e) { GraphCanvas.onBtnSaveClick(viewDataObj); });

        // Full Screen click
        viewDataObj.btnFullScreen.on('click', function (e) { GraphCanvas.onFullScreenBtnClick(viewDataObj); });

        // Calculate what max node ID is, in order to be able to assign distinct new IDs for new nodes
        var maxNodeID = -1;
        viewDataObj.nodes.forEach((n) => {
            if (n.id > maxNodeID)
                maxNodeID = n.id;
        });
        viewDataObj.newNodeIDCounter = maxNodeID;

        // Initial network
        GraphCanvas.createInitialNetwork(viewDataObj);
    }

    this.onContextMenuItemClick = function (viewDataObj, itemObj) {
        if (itemObj.hasClass('context-item-add-node'))
            GraphCanvas.onAddNode(viewDataObj);
        else if (itemObj.hasClass('context-item-delete-node'))
            GraphCanvas.onDeleteNode(viewDataObj);
        else if (itemObj.hasClass('context-item-delete-edge'))
            GraphCanvas.onDeleteEdge(viewDataObj);
        else if (itemObj.hasClass('context-item-start-adding-edge'))
            GraphCanvas.onStartAddingEdge(viewDataObj);
        else if (itemObj.hasClass('context-item-finish-adding-edge'))
            GraphCanvas.onFinishAddingEdge(viewDataObj);

        viewDataObj.contextMenu.removeClass('visible');
    }

    this.onAddNode = function (viewDataObj) {
        if (viewDataObj.network == null)
            return;

        viewDataObj.newNodeIDCounter++;

        viewDataObj.nodes.push({ id: viewDataObj.newNodeIDCounter, label: viewDataObj.newNodeIDCounter + '' });
        viewDataObj.network.body.data.nodes.add({ id: viewDataObj.newNodeIDCounter, label: viewDataObj.newNodeIDCounter + '' });
        viewDataObj.network.stabilize();
    }

    this.onDeleteNode = function (viewDataObj) {
        if (viewDataObj.network == null)
            return;

        let selectedNodes = viewDataObj.network.getSelectedNodes();
        if (selectedNodes == null || selectedNodes.length == 0) {
            alert("Error: You must select node.");
            return;
        }

        let selectedNodeID = selectedNodes[0];

        for (let i = 0; i < viewDataObj.nodes.length; i++) {
            let node = viewDataObj.nodes[i];

            if (node.id == selectedNodeID) {
                GraphCanvas.setStatus(viewDataObj, `Deleted node ${node.label}`);

                viewDataObj.nodes.splice(i, 1);
                viewDataObj.network.body.data.nodes.remove(selectedNodeID);
            }
        }
    }

    this.onDeleteEdge = function (viewDataObj) {
        if (viewDataObj.network == null)
            return;

        var selectedEdges = viewDataObj.network.getSelectedEdges();

        if (selectedEdges == null || selectedEdges.length == 0) {
            alert("Error: You must select edge.");
            return;
        }

        var edgeId = selectedEdges[0];
        var isDeleted = false;

        for (let i = 0; i < viewDataObj.edges.length; i++) {
            if (viewDataObj.edges[i].id == edgeId) {
                isDeleted = true;

                let fromNode = GraphCanvas.getNodeByID(viewDataObj, viewDataObj.edges[i].from);
                let toNode = GraphCanvas.getNodeByID(viewDataObj, viewDataObj.edges[i].to);

                GraphCanvas.setStatus(viewDataObj, `Deleted edge ${fromNode.label} - ${toNode.label}`);

                viewDataObj.edges.splice(i, 1);
                viewDataObj.network.body.data.edges.remove(edgeId);
                break;
            }
        }

        if (!isDeleted)
            alert("Error deleting edge!");
    }

    this.onStartAddingEdge = function (viewDataObj) {
        if (viewDataObj.network == null)
            return;

        let selectedStartNodes = viewDataObj.network.getSelectedNodes();
        if (selectedStartNodes != null && selectedStartNodes.length > 0) {
            viewDataObj.graphEdit.newEdgeStartNode = selectedStartNodes[0];

            var node = GraphCanvas.getNodeByID(viewDataObj, viewDataObj.graphEdit.newEdgeStartNode);

            GraphCanvas.setStatus(viewDataObj, `Adding edge with starting node ${node.label}`);
        }
    }

    this.onFinishAddingEdge = function (viewDataObj) {
        if (viewDataObj.network == null)
            return;

        let selectedEndNodes = viewDataObj.network.getSelectedNodes();
        if (selectedEndNodes == null || selectedEndNodes.length == 0) {
            alert("Error: You must select node.");
            return;
        }

        viewDataObj.graphEdit.newEdgeEndNode = selectedEndNodes[0];

        if (viewDataObj.graphEdit.newEdgeStartNode != null) {
            var edgeId = viewDataObj.network.body.data.edges.add({ from: viewDataObj.graphEdit.newEdgeStartNode, to: viewDataObj.graphEdit.newEdgeEndNode });
            edgeId = edgeId[0];

            viewDataObj.edges.push({
                from: viewDataObj.graphEdit.newEdgeStartNode,
                to: viewDataObj.graphEdit.newEdgeEndNode,
                id: edgeId
            });

            let fromNode = GraphCanvas.getNodeByID(viewDataObj, viewDataObj.graphEdit.newEdgeStartNode);
            let toNode = GraphCanvas.getNodeByID(viewDataObj, viewDataObj.graphEdit.newEdgeEndNode);

            GraphCanvas.setStatus(viewDataObj, `Added edge ${fromNode.label} - ${toNode.label}`);
        }

        viewDataObj.graphEdit.newEdgeStartNode = null;
        viewDataObj.graphEdit.newEdgeEndNode = null;
    }

    this.openContextMenu = function (viewDataObj, posY, posX) {
        viewDataObj.contextMenu.removeClass("visible");
        viewDataObj.contextMenu.css({ top: posY, left: posX });

        //setTimeout(() => {
        viewDataObj.contextMenu.addClass("visible");
        //}, 10);
    }

    this.createInitialNetwork = function (viewDataObj) {
        var options = {
            physics: {
                barnesHut: {
                    gravitationalConstant: -2000,
                    centralGravity: 0.3,
                    springLength: 95
                },
                stabilization: { iterations: 150 } // Stabilize the network after initial layout
            }
        };

        viewDataObj.network = new vis.Network(
            viewDataObj.graphContainer[0],
            {
                nodes: new vis.DataSet(viewDataObj.nodes),
                edges: new vis.DataSet(viewDataObj.edges)
            },
            options // {}
        );
    }

    this.onBtnSaveClick = function (viewDataObj) {
        console.log("Custom nodes", viewDataObj.nodes);
        console.log("Lib nodes", viewDataObj.network.body.data.nodes.get());
        console.log("Custom edges", viewDataObj.edges);
        console.log("Lib edges", viewDataObj.network.body.data.edges.get());

        $.post("/GraphDrawing/Store", {
            id: viewDataObj.graphID,
            nodes: viewDataObj.nodes,
            edges: viewDataObj.edges,
            score: 0
        }).done(function (data) {
            alert("Saved");
        });
    }

    this.onFullScreenBtnClick = function (viewDataObj) {
        if (
            document.fullscreenElement ||
            document.webkitFullscreenElement ||
            document.mozFullScreenElement ||
            document.msFullscreenElement
        ) {
            if (document.exitFullscreen) {
                document.exitFullscreen();
            } else if (document.mozCancelFullScreen) {
                document.mozCancelFullScreen();
            } else if (document.webkitExitFullscreen) {
                document.webkitExitFullscreen();
            } else if (document.msExitFullscreen) {
                document.msExitFullscreen();
            }
        }
        else {
            let container = viewDataObj.canvasContainer.get(0);

            if (container.requestFullscreen) {
                container.requestFullscreen();
            } else if (container.mozRequestFullScreen) {
                container.mozRequestFullScreen();
            } else if (container.webkitRequestFullscreen) {
                container.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
            } else if (container.msRequestFullscreen) {
                container.msRequestFullscreen();
            }
        }
    }

    this.setStatus = function (viewDataObj, status) {
        viewDataObj.statusMsg.html(`<b>Status</b>: ${status}`);
    }

    this.getNodeByID = function (viewDataObj, id) {
        var nodes = viewDataObj.nodes.filter(n => n.id == id);

        if (nodes.length > 0)
            return nodes[0];
        else
            return null;
    }
};
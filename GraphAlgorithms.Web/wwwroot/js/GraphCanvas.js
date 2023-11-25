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

        viewDataObj.nodes.add({ id: viewDataObj.newNodeIDCounter, label: viewDataObj.newNodeIDCounter + '' });
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
        let node = viewDataObj.nodes.get(selectedNodeID);
        GraphCanvas.setStatus(viewDataObj, `Deleted node ${node.label}`);
        viewDataObj.nodes.remove(selectedNodeID);

        console.log("Edges after deleting node:", viewDataObj.edges.get());
    }

    this.onDeleteEdge = function (viewDataObj) {
        if (viewDataObj.network == null)
            return;

        var selectedEdges = viewDataObj.network.getSelectedEdges();

        if (selectedEdges == null || selectedEdges.length == 0) {
            alert("Error: You must select edge.");
            return;
        }

        var edgeID = selectedEdges[0];

        let fromNode = viewDataObj.nodes.get(viewDataObj.edges.get(edgeID).from);
        let toNode = viewDataObj.nodes.get(viewDataObj.edges.get(edgeID).to);

        viewDataObj.edges.remove(edgeID);
        GraphCanvas.setStatus(viewDataObj, `Deleted edge ${fromNode.label} - ${toNode.label}`);
    }

    this.onStartAddingEdge = function (viewDataObj) {
        if (viewDataObj.network == null)
            return;

        let selectedStartNodes = viewDataObj.network.getSelectedNodes();
        if (selectedStartNodes != null && selectedStartNodes.length > 0) {
            viewDataObj.graphEdit.newEdgeStartNode = selectedStartNodes[0];

            var node = viewDataObj.nodes.get(viewDataObj.graphEdit.newEdgeStartNode);

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
            var edgeId = viewDataObj.edges.add({ from: viewDataObj.graphEdit.newEdgeStartNode, to: viewDataObj.graphEdit.newEdgeEndNode });
            edgeId = edgeId[0];

            let fromNode = viewDataObj.nodes.get(viewDataObj.graphEdit.newEdgeStartNode);
            let toNode = viewDataObj.nodes.get(viewDataObj.graphEdit.newEdgeEndNode);

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
            },
            manipulation: {
                enabled: true
            }
        };

        // This modifies initial nodes/edges JSONs to represent vis.js DataSets and should only be called once!
        viewDataObj.nodes = new vis.DataSet(viewDataObj.nodes);
        viewDataObj.edges = new vis.DataSet(viewDataObj.edges);

        viewDataObj.network = new vis.Network(
            viewDataObj.graphContainer[0],
            {
                nodes: viewDataObj.nodes,
                edges: viewDataObj.edges
            },
            options // {}
        );

        // Handle removal of nodes to also remove their adjacent edges
        viewDataObj.network.on("remove", function (params) {
            console.log("Triggered remove:", params);
            //if (params.nodes.length > 0) {
            //    params.nodes.forEach(nodeID => {
            //        const connectedEdges = viewDataObj.edges.get({
            //            filter: function (edge) {
            //                return edge.from === nodeID || edge.to === nodeID;
            //            }
            //        });
            //        viewDataObj.edges.remove(connectedEdges.map(edge => edge.id));
            //    });
            //}
        });
    }

    this.onBtnSaveClick = function (viewDataObj) {
        var nodes = viewDataObj.nodes.get();
        var edges = viewDataObj.edges.get();

        GraphCanvas.reassignNodeIDs(viewDataObj);
        return;

        $.post("/GraphDrawing/Store", {
            id: viewDataObj.graphID,
            nodes: nodes,
            edges: edges,
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

    this.reassignNodeIDs = function (viewDataObj) {
        let nodesCount = viewDataObj.nodes.get().length;
        let edgesCount = viewDataObj.edges.get().length;
        let newNodeIDs = [...Array(nodesCount).keys()];

        //console.log("Edges before: ", JSON.stringify(viewDataObj.edges.get()));
        viewDataObj.edges.get().forEach((e) => {
            let fromNodeIdx = GraphCanvas.getNodeIndexByID(viewDataObj, e.from);
            let toNodeIdx = GraphCanvas.getNodeIndexByID(viewDataObj, e.to);

            e.from = newNodeIDs[fromNodeIdx];
            e.to = newNodeIDs[toNodeIdx];
        });

        viewDataObj.nodes.get().forEach((n, i) => {
            n.id = newNodeIDs[i];
        });

        console.log(viewDataObj.nodes.get());
        console.log(viewDataObj.edges.get());
    }

    this.getNodeIndexByID = function (viewDataObj, id) {
        var idx = -1;

        viewDataObj.nodes.get().forEach((n, i) => {
            if (n.id == id)
                idx = i;
        });

        return idx;
    }
};
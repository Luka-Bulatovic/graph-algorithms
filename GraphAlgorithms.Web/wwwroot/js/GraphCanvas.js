var GraphCanvas = new function () {
    this.Initialize = function (viewDataObj) {
        $('body').on('click', function () { viewDataObj.contextMenu.removeClass('visible'); });

        // Context Menu Item click
        viewDataObj.contextMenu.find('.item').on('click', function (e) {
            GraphCanvas.onContextMenuItemClick(viewDataObj, $(e.target));
        });

        // Opening Context Menu
        /*
        viewDataObj.graphContainer.contextmenu(function (event) {
            event.preventDefault();
            GraphCanvas.openContextMenu(viewDataObj, event.clientY, event.clientX);
            return false;
        });
        */

        // Save Button click
        viewDataObj.btnSave.on('click', function (e) { GraphCanvas.onBtnSaveClick(viewDataObj); });

        // Full Screen click
        viewDataObj.btnFullScreen.on('click', function (e) { GraphCanvas.onFullScreenBtnClick(viewDataObj); });

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

    /*
    GraphCanvas.setStatus(viewDataObj, `Added edge ${fromNode.label} - ${toNode.label}`);
    GraphCanvas.setStatus(viewDataObj, `Adding edge with starting node ${node.label}`);
    GraphCanvas.setStatus(viewDataObj, `Deleted edge ${fromNode.label} - ${toNode.label}`);
    GraphCanvas.setStatus(viewDataObj, `Deleted node ${node.label}`);

            viewDataObj.nodes.add({ id: viewDataObj.newNodeIDCounter, label: viewDataObj.newNodeIDCounter + '' });
        viewDataObj.network.stabilize();
    */
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
                enabled: true,
                deleteNode: GraphCanvas.onDeleteNode.bind(null, viewDataObj),
                deleteEdge: GraphCanvas.onDeleteEdge.bind(null, viewDataObj),
                addNode: GraphCanvas.onAddNode.bind(null, viewDataObj),
                addEdge: GraphCanvas.onAddEdge.bind(null, viewDataObj)
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
    }

    this.onAddNode = function (viewDataObj, nodeData, callback) {
        let nodeLabel = prompt("Please enter node label:");
        if (nodeLabel != null && nodeLabel != "")
            nodeData.label = nodeLabel;

        GraphCanvas.setStatus(viewDataObj, `Added node ${nodeData.label}`);
        callback(nodeData);
    }
    this.onAddEdge = function (viewDataObj, edgeData, callback) {
        let fromNode = viewDataObj.nodes.get(edgeData.from);
        let toNode = viewDataObj.nodes.get(edgeData.to);

        GraphCanvas.setStatus(viewDataObj, `Added edge ${fromNode.label} - ${toNode.label}`);
        callback(edgeData);
    }
    this.onDeleteNode = function (viewDataObj, nodeData, callback) {
        if (nodeData.nodes == null || nodeData.length == 0) {
            alert("Error: You must select node.");
            return;
        }

        let node = viewDataObj.nodes.get(nodeData.nodes[0]);
        GraphCanvas.setStatus(viewDataObj, `Deleted node ${node.label}`);

        callback(nodeData);
    }
    this.onDeleteEdge = function (viewDataObj, edgeData, callback) {
        if (edgeData.edges == null || edgeData.length == 0) {
            alert("Error: You must select edge.");
            return;
        }

        let edgeID = edgeData.edges[0];
        let edge = viewDataObj.edges.get(edgeID);

        let fromNode = viewDataObj.nodes.get(edge.from);
        let toNode = viewDataObj.nodes.get(edge.to);

        GraphCanvas.setStatus(viewDataObj, `Deleted edge ${fromNode.label} - ${toNode.label}`);

        callback(edgeData);
    }

    this.onBtnSaveClick = function (viewDataObj) {
        var nodes = viewDataObj.nodes.get();
        var edges = viewDataObj.edges.get();

        GraphCanvas.reassignNodeIDs(viewDataObj);

        $.post("/GraphDrawing/Store", {
            graph: {
                id: viewDataObj.graphID,
                nodes: nodes,
                edges: edges
            }
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

            viewDataObj.network.fit();
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

            viewDataObj.network.fit();
        }
    }

    this.setStatus = function (viewDataObj, status) {
        viewDataObj.statusMsg.html(`<b>Status</b>: ${status}`);
    }

    this.reassignNodeIDs = function (viewDataObj) {
        let nodesCount = viewDataObj.nodes.get().length;
        let newNodeIDs = [...Array(nodesCount).keys()];

        viewDataObj.edges.get().forEach((e) => {
            let fromNodeIdx = GraphCanvas.getNodeIndexByID(viewDataObj, e.from);
            let toNodeIdx = GraphCanvas.getNodeIndexByID(viewDataObj, e.to);

            e.from = newNodeIDs[fromNodeIdx];
            e.to = newNodeIDs[toNodeIdx];
        });

        viewDataObj.nodes.get().forEach((n, i) => {
            n.id = newNodeIDs[i];
        });
    }

    this.getNodeIndexByID = function (viewDataObj, id) {
        var idx = -1;

        viewDataObj.nodes.get().forEach((n, i) => {
            if (n.id === id)
                idx = i;
        });

        return idx;
    }
};